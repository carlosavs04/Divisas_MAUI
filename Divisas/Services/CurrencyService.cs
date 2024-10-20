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
    }
}
