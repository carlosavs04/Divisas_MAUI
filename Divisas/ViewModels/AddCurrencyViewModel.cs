using Divisas.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Divisas.ViewModels
{
    public class AddCurrencyViewModel : BaseViewModel
    {
        private readonly CurrencyService _currencyService;
        private string _code;
        private string _name;
        private decimal? _actualRate;
        private bool _isLoading;
        private bool _canSave;

        public ICommand CreateCurrencyCommand { get; }

        public string Code
        {
            get => _code;
            set
            {
                SetProperty(ref _code, value);
                Validate();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
                Validate();
            }
        }

        public decimal? ActualRate
        {
            get => _actualRate;
            set
            {
                SetProperty(ref _actualRate, value);
                Validate();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public bool CanSave
        {
            get => _canSave;
            private set => SetProperty(ref _canSave, value);
        }

        public AddCurrencyViewModel(CurrencyService currencyService, ConfigurationService configService) : base(configService)
        {
            _currencyService = currencyService;
            _code = string.Empty;
            _name = string.Empty;
            CreateCurrencyCommand = new Command(async () => await AddCurrencyAsync(), () => CanSave);
        }

        private async Task AddCurrencyAsync()
        {
            if (IsLoading)
                return;

            try
            {
                IsLoading = true;
                await Task.Delay(100);

                var newCurrency = new Models.Currency
                {
                    Code = Code,
                    Name = Name,
                    ActualRate = ActualRate
                };

                await _currencyService.CreateCurrencyAsync(newCurrency);

                Code = string.Empty;
                Name = string.Empty;
                ActualRate = null;
            }

            finally
            {
                IsLoading = false;
            }
        }

        private void Validate()
        {
            CanSave = !string.IsNullOrWhiteSpace(Code) && !string.IsNullOrWhiteSpace(Name) && ActualRate.HasValue;
            ((Command)CreateCurrencyCommand).ChangeCanExecute();
        }
    }
}
