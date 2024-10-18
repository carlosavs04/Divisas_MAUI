using Divisas.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divisas.Services
{
    public class ExchangeRateApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public ExchangeRateApiService(HttpClient httpClient, IOptions<ExchangeRateApiConfig> options)
        {
            _httpClient = httpClient;
            _apiKey = options.Value.ApiKey;
        }

        public async Task<decimal?> GetExchangeRateAsync(string baseCurrency, string targetCurrency) 
        {
            string url = $"https://v6.exchangerate-api.com/v6/{_apiKey}/pair/{baseCurrency}/{targetCurrency}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);
            
            decimal rate = json["conversion_rate"].Value<decimal>();

            return rate;
        }
    }
}
