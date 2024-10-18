using Divisas.DataAccess;
using Divisas.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divisas.Services
{
    public class CurrencyRateUpdateService
    {
        private readonly CurrencyDbContext _ctx;
        private readonly ExchangeRateApiService _rateApi;

        public CurrencyRateUpdateService(CurrencyDbContext ctx, ExchangeRateApiService rateApi)
        {
            _ctx = ctx;
            _rateApi = rateApi;
        }

        public async Task UpdateRatesAsync()
        {
            var today = DateTime.UtcNow.Date;
            var existingRates = await _ctx.ExchangeRateHistory.Where(i => i.Date == today).ToListAsync();
            var baseCurrency = await _ctx.Currencies.FirstOrDefaultAsync(i => i.IsBase);

            if (existingRates.Count != 0 || baseCurrency == null)
                return;

            var defaultCurrencies = await _ctx.Currencies.Where(i => i.IsDefault && !i.IsBase).ToListAsync();

            foreach (var currency in defaultCurrencies)
            {
                var rate = await _rateApi.GetExchangeRateAsync(baseCurrency.Code, currency.Code);

                if (rate.HasValue)
                {
                    var exchangeRateHistory = new ExchangeRateHistory
                    {
                        CurrencyId = currency.Id,
                        Rate = rate.Value,
                        Date = today
                    };

                    _ctx.ExchangeRateHistory.Add(exchangeRateHistory);
                }
            }

            await _ctx.SaveChangesAsync();
        }
    }
}
