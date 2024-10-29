using Divisas.DataAccess;
using Entities = Divisas.DataAccess.Entities;
using Models = Divisas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Divisas.Services
{
    public class CurrencyService
    {
        private readonly CurrencyDbContext _ctx;
        private readonly IMapper _mapper;
        private const string DEFAULT_FLAG = "default_flag";

        public CurrencyService(CurrencyDbContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Models.Currency>> GetCurrenciesAsync()
        {
            var currencies = await _ctx.Currencies.Where(i => i.IsActive).OrderByDescending(i => i.IsBase).ThenByDescending(i => i.IsDefault).ThenBy(i => i.Name).ToListAsync();

            return currencies.Select(i => new Models.Currency
            {
                Id = i.Id,
                Code = i.Code,
                Name = i.Name,
                Flag = i.Flag,
                IsBase = i.IsBase
            });
        }

        public async Task<Models.Currency?> GetCurrencyAsync(int id)
        {
            var currency = await _ctx.Currencies.FindAsync(id);

            if (currency == null)
                return null;

            var m = _mapper.Map<Models.Currency>(currency);
            m.ActualRate = await GetActualRateAsync(currency.Id);
            m.SuggestedRetailPrice = GetSuggestedRetailPrice(m.ActualRate.Value);

            return m;
        }

        public async Task<decimal> GetActualRateAsync(int currencyId)
        {
            var actualRate = await _ctx.ExchangeRateHistory.Where(i => i.CurrencyId == currencyId).OrderByDescending(i => i.Date).FirstOrDefaultAsync();

            if (actualRate == null)
                return 0;

            return actualRate.Rate;
        }

        public static decimal GetSuggestedRetailPrice(decimal actualRate)
        {
            if (actualRate == 0)
                return 0;

            return actualRate * (1 + 0.3m);
        }

        public async Task CreateCurrencyAsync(Models.Currency model)
        {
            var currency = new Entities.Currency
            {
                Code = model.Code,
                Name = model.Name,
                Flag = DEFAULT_FLAG,
                IsActive = true,
                IsBase = false,
                IsDefault = false
            };

            _ctx.Currencies.Add(currency);
            await _ctx.SaveChangesAsync();

            if (model.ActualRate.HasValue)
            {
                var rate = new Entities.ExchangeRateHistory
                {
                    CurrencyId = currency.Id,
                    Rate = model.ActualRate.Value,
                    Date = DateTime.Now
                };

                _ctx.ExchangeRateHistory.Add(rate);
                await _ctx.SaveChangesAsync();
            }       
        }

        public async Task UpdateCurrencyAsync(Models.Currency model)
        {
            var currency = await _ctx.Currencies.FindAsync(model.Id);

            if (currency == null || currency.IsBase || currency.IsDefault)
                return;

            currency.Code = model.Code;
            currency.Name = model.Name;

            if (model.ActualRate.HasValue)
            {
                var rate = new Entities.ExchangeRateHistory
                {
                    CurrencyId = currency.Id,
                    Rate = model.ActualRate.Value,
                    Date = DateTime.Now
                };

                _ctx.ExchangeRateHistory.Add(rate);
            }

            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteCurrencyAsync(int id)
        {
            var currency = await _ctx.Currencies.FindAsync(id);

            if (currency == null || currency.IsBase || currency.IsDefault)
                return;

            currency.IsActive = false;
            await _ctx.SaveChangesAsync();
        }

        public async Task<Models.ConversionResult> ConvertCurrencyAsync(int fromCurrencyId, int toCurrencyId, decimal amount)
        {
            var fromRate = await GetActualRateAsync(fromCurrencyId);
            var toRate = await GetActualRateAsync(toCurrencyId);

            if (fromRate == 0 || toRate == 0)
                throw new InvalidOperationException();

            decimal convertedAmount = amount * (toRate / fromRate);
            decimal suggestedRetailPrice = GetSuggestedRetailPrice(convertedAmount);

            return new Models.ConversionResult
            {
                ConvertedAmount = convertedAmount,
                SuggestedRetailPrice = suggestedRetailPrice
            };
        }

        public async Task<IEnumerable<Models.Currency>> GetDefaultCurrenciesToConvertAsync()
        {
            string otherCurrencyCode = "MXN";
            var baseCurrency = await _ctx.Currencies.FirstOrDefaultAsync(i => i.IsBase);

            if (baseCurrency == null)
                throw new InvalidOperationException();

            if (baseCurrency.IsDefault && baseCurrency.Code == "MXN")
                otherCurrencyCode = "USD";

            var otherCurrency = await _ctx.Currencies.FirstOrDefaultAsync(i => i.Code == otherCurrencyCode && i.IsDefault);

            if (otherCurrency == null)
                throw new InvalidOperationException();

            return new List<Models.Currency>
            {
                _mapper.Map<Models.Currency>(baseCurrency),
                _mapper.Map<Models.Currency>(otherCurrency)
            };
        }
    }
}
