using Divisas.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Divisas.ViewModels
{
    public class UpdateCurrencyViewModel : BaseViewModel
    {
        private readonly CurrencyService _currencyService;
        private Models.Currency? _currency;
        private bool _isLoading;
        private int _currencyId;
        private string _code;
        private string _name;
        private decimal? _actualRate;

        public ICommand LoadCurrencyCommand { get; }
        public ICommand UpdateCurrencyCommand { get; }

        public Models.Currency? Currency
        {
            get => _currency;
            set => SetProperty(ref _currency, value);
        }

        public string Code
        {
            get => _code;
            set => SetProperty(ref _code, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public decimal? ActualRate
        {
            get => _actualRate;
            set => SetProperty(ref _actualRate, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public UpdateCurrencyViewModel(CurrencyService currencyService, int currencyId)
        {
            _currencyService = currencyService;
            _currencyId = currencyId;
            _code = string.Empty;
            _name = string.Empty;
            LoadCurrencyCommand = new Command(async () => await LoadCurrencyAsync());
            UpdateCurrencyCommand = new Command(async () => await UpdateCurrencyAsync());
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

                var currency = await _currencyService.GetCurrencyAsync(_currencyId);

                if (currency != null)
                {
                    Currency = currency;
                    Code = currency.Code;
                    Name = currency.Name;
                    ActualRate = currency.ActualRate;
                }
            }

            finally
            {
                IsLoading = false;
            }
        }

        private async Task UpdateCurrencyAsync()
        {
            if (IsLoading || Currency == null)
                return;

            try
            {
                IsLoading = true;
                await Task.Delay(100);

                var updatedCurrency = new Models.Currency
                {
                    Id = _currencyId,
                    Code = Code,
                    Name = Name,
                    ActualRate = ActualRate
                };

                await _currencyService.UpdateCurrencyAsync(updatedCurrency);
            }

            finally
            {
                IsLoading = false;
            }
        }
    }
}
