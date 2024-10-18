using Divisas.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divisas.Utils.Test
{
    public class DatabaseViewer
    {
        private readonly CurrencyDbContext _ctx;

        public DatabaseViewer(CurrencyDbContext context)
        {
            _ctx = context;
        }

        public void ShowDatabaseContents()
        {
            var currencies = _ctx.Currencies.ToList();
            var exchangeRateHistories = _ctx.ExchangeRateHistory.ToList();

            var currencyMsg = "Currencies:\n";
            foreach (var currency in currencies)
            {
                currencyMsg += $"Id: {currency.Id}, Code: {currency.Code}, Name: {currency.Name}, Flag: {currency.Flag}, IsActive: {currency.IsActive}, IsBase: {currency.IsBase}, IsDefault: {currency.IsDefault}\n";
            }

            var exchangeRateHistoryMsg = "ExchangeRateHistories:\n";
            foreach (var exchangeRateHistory in exchangeRateHistories)
            {
                exchangeRateHistoryMsg += $"Id: {exchangeRateHistory.Id}, CurrencyId: {exchangeRateHistory.CurrencyId}, Rate: {exchangeRateHistory.Rate}, Date: {exchangeRateHistory.Date}\n";
            }

            var fullMsg = $"{currencyMsg}\n{exchangeRateHistoryMsg}";

            Console.WriteLine(fullMsg);
        }
    }
}
