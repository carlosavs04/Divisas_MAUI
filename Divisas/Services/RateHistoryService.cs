using Divisas.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divisas.Services
{
    public class RateHistoryService
    {
        private readonly CurrencyDbContext _ctx;
        private readonly CurrencyService _currencyService;

        public RateHistoryService(CurrencyDbContext ctx, CurrencyService currencyService)
        {
            _ctx = ctx;
            _currencyService = currencyService;
        }

        public async Task<IEnumerable<Models.ExchangeRateHistory>> GetLastRatesByCurrencyAsync(int currencyId)
        {
            var rates = await _ctx.ExchangeRateHistory.Where(i => i.CurrencyId == currencyId).OrderByDescending(i => i.Date).Take(30).ToListAsync();

            return rates.Select(i => new Models.ExchangeRateHistory
            {
                Id = i.Id,
                Rate = i.Rate,
                Date = i.Date
            });
        }

        public async Task SetBaseCurrencyAsync(int newCurrencyId)
        {
            var currentBaseCurrency = await _ctx.Currencies.FirstOrDefaultAsync(i => i.IsBase);

            if (currentBaseCurrency != null)
                currentBaseCurrency.IsBase = false;

            var newBaseCurrency = await _ctx.Currencies.FindAsync(newCurrencyId);

            if (newBaseCurrency == null)
                throw new InvalidOperationException();

            newBaseCurrency.IsBase = true;

            var baseRateHistory = new DataAccess.Entities.ExchangeRateHistory
            {
                CurrencyId = newBaseCurrency.Id,
                Rate = 1,
                Date = DateTime.Now
            };

            _ctx.ExchangeRateHistory.Add(baseRateHistory);
            await _ctx.SaveChangesAsync();
            await RecalculateRatesForBaseCurrencyChangeAsync(newCurrencyId);
        }

        public async Task RecalculateRatesForBaseCurrencyChangeAsync(int newCurrencyId)
        {
            var newBaseRate = await _currencyService.GetActualRateAsync(newCurrencyId);

            if (newBaseRate == 0)
                return;

            var currencies = await _ctx.Currencies.Where(i => i.IsActive && i.Id != newCurrencyId).ToListAsync();

            foreach (var currency in currencies)
            {
                var lastRates = await _ctx.ExchangeRateHistory.Where(i => i.CurrencyId == currency.Id).OrderByDescending(i => i.Date).Take(30).ToListAsync();

                if (lastRates.Count == 0)
                    continue;

                foreach (var rate in lastRates)
                {
                    rate.Rate = rate.Rate / newBaseRate;
                }

                await _ctx.SaveChangesAsync();
            }
        }
    }
}
