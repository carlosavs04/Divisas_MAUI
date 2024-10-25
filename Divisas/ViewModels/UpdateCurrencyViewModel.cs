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
        private bool _canSave;

        public ICommand LoadCurrencyCommand { get; }
        public ICommand UpdateCurrencyCommand { get; }

        public Models.Currency? Currency
        {
            get => _currency;
            set => SetProperty(ref _currency, value);
        }

        public string Code
        {
            get => Currency?.Code ?? string.Empty;
            set 
            {
                if (Currency != null)
                {
                    Currency.Code = value;
                    OnPropertyChanged(nameof(Code));
                    Validate();
                }
            }

        }

        public string Name
        {
            get => Currency?.Name ?? string.Empty;
            set
            {
                if (Currency != null)
                {
                    Currency.Name = value;
                    OnPropertyChanged(nameof(Name));
                    Validate();
                }
            }
        }

        public decimal? ActualRate
        {
            get => Currency?.ActualRate ?? 0;
            set
            {
                if (Currency != null)
                {
                    Currency.ActualRate = value;
                    OnPropertyChanged(nameof(ActualRate));
                    Validate();
                }
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

        public UpdateCurrencyViewModel(CurrencyService currencyService, ConfigurationService configService) : base(configService)
        {
            _currencyService = currencyService;
            LoadCurrencyCommand = new Command(async () => await LoadCurrencyAsync());
            UpdateCurrencyCommand = new Command(async () => await UpdateCurrencyAsync(), () => CanSave);
        }

        public void Initialize(int currencyId)
        {
            _currencyId = currencyId;
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

                await _currencyService.UpdateCurrencyAsync(Currency);
            }

            finally
            {
                IsLoading = false;
            }
        }

        private void Validate()
        {
            CanSave = !string.IsNullOrWhiteSpace(Code) && !string.IsNullOrWhiteSpace(Name) && ActualRate.HasValue;
            ((Command)UpdateCurrencyCommand).ChangeCanExecute();
        }
    }
}
