using Divisas.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Divisas.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private CurrencyService _currencyService;
        private RateHistoryService _rateHistoryService;
        private Models.Currency? _fromCurrency;
        private Models.Currency? _toCurrency;
        private Models.ConversionResult? _conversionResult;
        private bool _isLoading;
        private bool _isInitialized;
        private decimal _actualRate;
        private decimal _amountToConvert;
        private decimal _retailPrice;

        public ObservableCollection<Models.ExchangeRateHistory> FromRates { get; private set; }
        public ObservableCollection<Models.ExchangeRateHistory> ToRates { get; private set; }
        public ICommand InitCurrenciesCommand { get; }
        public ICommand UpdateCurrenciesRatesCommand { get; }
        public ICommand ConvertCurrencyCommand { get; }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public bool IsInitialized
        {
            get => _isInitialized;
            set => SetProperty(ref _isInitialized, value);
        }

        public decimal ActualRate
        {
            get => _actualRate;
            set => SetProperty(ref _actualRate, value);
        }

        public decimal AmountToConvert
        {
            get => _amountToConvert;
            set => SetProperty(ref _amountToConvert, value);
        }

        public decimal SuggestedRetailPrice
        {
            get => _retailPrice;
            set => SetProperty(ref _retailPrice, value);
        }

        public Models.Currency? FromCurrency
        {
            get => _fromCurrency;
            set
            {
                SetProperty(ref _fromCurrency, value);
                OnPropertyChanged(nameof(FromFlagImagePath));
                OnPropertyChanged(nameof(FromCode));
                OnPropertyChanged(nameof(ActualRate));
            }
        }

        public Models.Currency? ToCurrency
        {
            get => _toCurrency;
            set
            {
                SetProperty(ref _toCurrency, value);
                OnPropertyChanged(nameof(ToFlagImagePath));
                OnPropertyChanged(nameof(ToCode));
                OnPropertyChanged(nameof(ActualRate));
            }
        }

        public Models.ConversionResult? ConversionResult
        {
            get => _conversionResult;
            set => SetProperty(ref _conversionResult, value);
        }

        public string FromFlagImagePath => FromCurrency != null ? FromCurrency.Flag != null && FromCurrency.Flag.EndsWith(".png") ? FromCurrency.Flag : $"{FromCurrency.Flag}.png" : "default_flag.png";
        public string ToFlagImagePath => ToCurrency != null ? ToCurrency.Flag != null && ToCurrency.Flag.EndsWith(".png") ? ToCurrency.Flag : $"{ToCurrency.Flag}.png" : "default_flag.png";
        public string FromCode => FromCurrency?.Code ?? "Unknown";
        public string ToCode => ToCurrency?.Code ?? "Unknown";
        public string ConversionText => $"$1 {FromCode} = {ActualRate} {ToCode}";
        public string ConversionResultText => ConversionResult != null ? ConversionResult.ConvertedAmount.ToString("C3") : "$0.00";
        public string RetailPriceText => ConversionResult != null ? ConversionResult.SuggestedRetailPrice.ToString("C3") : "$0.00";

        public HomeViewModel(CurrencyService currencyService, RateHistoryService rateHistoryService, ConfigurationService configService) : base(configService)
        {
            _currencyService = currencyService;
            _rateHistoryService = rateHistoryService;
            _amountToConvert = 0;
            _retailPrice = 0;
            FromRates = new ObservableCollection<Models.ExchangeRateHistory>();
            ToRates = new ObservableCollection<Models.ExchangeRateHistory>();
            InitCurrenciesCommand = new Command(async () => await InitializeDefaultCurrencies());
            UpdateCurrenciesRatesCommand = new Command(async () => await UpdateCurrencyRates());
            ConvertCurrencyCommand = new Command(async () => await ConvertCurrencyAmount());
        }

        private async Task InitializeDefaultCurrencies()
        {
            if (IsLoading || _isInitialized)
                return;

            try
            {
                IsLoading = true;
                await Task.Delay(100);

                var defaultCurrencies = await _currencyService.GetDefaultCurrenciesToConvertAsync();

                if (defaultCurrencies.Count() == 2)
                {
                    FromCurrency = defaultCurrencies.ElementAt(0);
                    ToCurrency = defaultCurrencies.ElementAt(1);
                }

                _isInitialized = true;
                await UpdateCurrencyRates();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task UpdateCurrencyRates()
        {
            if (FromCurrency == null || ToCurrency == null)
                return;

            var fromRate = await _currencyService.GetActualRateAsync(FromCurrency.Id);
            var toRate = await _currencyService.GetActualRateAsync(ToCurrency.Id);

            var result = Math.Round(toRate / fromRate, 3);

            //ConversionResult = null;
            //AmountToConvert = 0;
            ActualRate = result;

            OnPropertyChanged(nameof(ConversionText));
            OnPropertyChanged(nameof(AmountToConvert));
            OnPropertyChanged(nameof(ConversionResultText));
            OnPropertyChanged(nameof(RetailPriceText));
        }

        private async Task ConvertCurrencyAmount()
        {
            if (FromCurrency == null || ToCurrency == null || AmountToConvert <= 0 || IsLoading)
                return;

            try
            {
                IsLoading = true;
                await Task.Delay(100);

                var result = await _currencyService.ConvertCurrencyAsync(FromCurrency.Id, ToCurrency.Id, AmountToConvert);
                ConversionResult = result;

                OnPropertyChanged(nameof(ConversionResultText));
                OnPropertyChanged(nameof(RetailPriceText));

                await GetCurrencyLastRates();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task GetCurrencyLastRates()
        {
            if (FromCurrency == null || ToCurrency == null || AmountToConvert <= 0)
                return;

            await Task.Delay(100);

            var fromRates = await _rateHistoryService.GetLastRatesByCurrencyAsync(FromCurrency.Id);
            var toRates = await _rateHistoryService.GetLastRatesByCurrencyAsync(ToCurrency.Id);

            FromRates.Clear();
            ToRates.Clear();

            foreach (var rate in fromRates)
            {
                FromRates.Add(rate);
            }

            foreach (var rate in toRates)
            {
                ToRates.Add(rate);
            }
        }
    }
}
