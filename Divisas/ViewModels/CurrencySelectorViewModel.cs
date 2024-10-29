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
    public class CurrencySelectorViewModel : BaseViewModel
    {
        private readonly CurrencyService _currencyService;
        private bool _isLoading;

        public ObservableCollection<Models.Currency> Currencies { get; private set; }
        public ICommand LoadCurrenciesCommand { get; }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public CurrencySelectorViewModel(CurrencyService currencyService, ConfigurationService configService) : base(configService)
        {
            _currencyService = currencyService;
            Currencies = new ObservableCollection<Models.Currency>();
            LoadCurrenciesCommand = new Command(async () => await LoadCurrenciesAsync());
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
                    currency.Flag += ".png";
                    Currencies.Add(currency);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
