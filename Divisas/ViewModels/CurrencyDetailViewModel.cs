using Divisas.Services;
using Divisas.Utils;
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
            set
            {
                SetProperty(ref _currency, value);
                OnPropertyChanged(nameof(FlagImagePath));
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Code));
                OnPropertyChanged(nameof(ActualRate));
                OnPropertyChanged(nameof(SuggestedRetailPrice));
                OnPropertyChanged(nameof(IsEditable));
            }

        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string FlagImagePath => Currency != null ? $"{Currency.Flag}.png" : "default_flag.png";
        public string Name => Currency?.Name ?? "Unknown";
        public string Code => Currency?.Code ?? "Unknown";
        public string ActualRate => Currency != null ? $"{Currency.ActualRate:C3}" : "$0.00";
        public string SuggestedRetailPrice => Currency != null ? $"{Currency.SuggestedRetailPrice:C3}" : "$0.00";
        public bool IsEditable => Currency != null && !Currency.IsBase && !Currency.IsDefault;

        public CurrencyDetailViewModel(CurrencyService currencyService, ConfigurationService configService) : base(configService)
        {
            _currencyService = currencyService;
            LoadCurrencyCommand = new Command(async () => await LoadCurrencyAsync());
            DeleteCurrencyCommand = new Command(async () => await DeleteCurrencyAsync());
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

            bool confirm = await DialogsHelper.ShowWarningMessage(
                "Confirmar eliminación", 
                "¿Estás seguro que deseas eliminar esta divisa? Una vez que lo hagas no volverás a poder utilizarla ni ver su información"
            );

            if (!confirm)
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
