using Divisas.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Divisas.ViewModels
{
    public class CurrencyDetailViewModel : BaseViewModel
    {
        private readonly CurrencyService _currencyService;
        private Models.Currency? _currency;
        private bool _isLoading;
        private int _currencyId;

        public ICommand LoadCurrencyCommand { get; }
        public ICommand DeleteCurrencyCommand { get; }

        public Models.Currency? Currency
        {
            get => _currency;
            set => SetProperty(ref _currency, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public CurrencyDetailViewModel(CurrencyService currencyService, int currencyId, ConfigurationService configService) : base(configService)
        {
            _currencyService = currencyService;
            _currencyId = currencyId;
            LoadCurrencyCommand = new Command(async () => await LoadCurrencyAsync());
            DeleteCurrencyCommand = new Command(async () => await DeleteCurrencyAsync());
            LoadCurrencyCommand.Execute(null);
        }

        private async Task LoadCurrencyAsync()
        {
            if (IsLoading)
                return;

            try
            {
                IsLoading = true;
                await Task.Delay(100);

                Currency = await _currencyService.GetCurrencyAsync(_currencyId);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task DeleteCurrencyAsync()
        {
            if (IsLoading || Currency == null)
                return;

            try
            {
                IsLoading = true;
                await Task.Delay(100);

                await _currencyService.DeleteCurrencyAsync(_currencyId);
                Currency = null;
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
