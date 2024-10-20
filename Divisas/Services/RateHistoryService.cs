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

        public RateHistoryService(CurrencyDbContext ctx)
        {
            _ctx = ctx;
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
    }
}
