using Divisas.Services;
using Divisas.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Divisas.ViewModels
{
    public class ConfigurationViewModel : BaseViewModel
    {
        private readonly ConfigurationService _configService;
        private readonly CurrencyService _currencyService;
        private readonly RateHistoryService _rateHistoryService;
        private bool _isLoading;
        private bool _isDarkMode;
        private Models.Currency? _defaultCurrency;
        private Models.Currency? _selectedCurrency;

        public ObservableCollection<Models.Currency> Currencies { get; private set; }
        public ICommand LoadCurrenciesCommand { get; }
        public ICommand ChangeBaseCurrencyCommand { get; }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public bool IsDarkMode
        {
            get => _isDarkMode;
            set 
            {
                if (SetProperty(ref _isDarkMode, value))
                {
                    _configService.SetDarkMode(value);

                    if (App.Current != null)
                        App.Current.UserAppTheme = value ? AppTheme.Dark : AppTheme.Light;
                }
            }
        }

        public Models.Currency? DefaultCurrency
        {
            get => _defaultCurrency;
            set => SetProperty(ref _defaultCurrency, value);
        }

        public Models.Currency? SelectedCurrency
        {
            get => _selectedCurrency;
            set => SetProperty(ref _selectedCurrency, value);
        }

        public ConfigurationViewModel(ConfigurationService configService, CurrencyService currencyService, RateHistoryService rateHistoryService) : base(configService)
        {
            _configService = configService;
            _currencyService = currencyService;
            _rateHistoryService = rateHistoryService;
            _isDarkMode = _configService.GetDarkMode();
            Currencies = new ObservableCollection<Models.Currency>();
            LoadCurrenciesCommand = new Command(async () => await LoadCurrenciesAsync());
            ChangeBaseCurrencyCommand = new Command(async () => await ChangeBaseCurrency());
        }

        private async Task LoadCurrenciesAsync()
        {
            if (IsLoading)
                return;

            try
            {
                IsLoading = true;
                await Task.Delay(100);

                Currencies.Clear();

                var currencies = await _currencyService.GetCurrenciesAsync();

                foreach (var currency in currencies)
                {
                    Currencies.Add(currency);
                }

                DefaultCurrency = Currencies.FirstOrDefault(i => i.IsBase);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task ChangeBaseCurrency()
        {
            if (IsLoading)
                return;

            if (SelectedCurrency == null || DefaultCurrency == null || DefaultCurrency.Id == SelectedCurrency.Id)
                return;

            bool confirm = await DialogsHelper.ShowWarningMessage(
                "¿Estás seguro de cambiar la divisa base?",
                "Esta acción recalculará los valores de las divisas así como gran parte de su historial. Esto puede tomar un momento."
            );

            if (!confirm)
                return;

            try
            {
                await _rateHistoryService.SetBaseCurrencyAsync(SelectedCurrency.Id);
                _configService.SetBaseCurrency(SelectedCurrency.Code);
            }

            finally
            {
                IsLoading = false;
            }
        }
    }
}
