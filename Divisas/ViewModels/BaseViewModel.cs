using Divisas.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Divisas.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private ConfigurationService _configService;

        public event PropertyChangedEventHandler? PropertyChanged;
        public string _baseCurrency;

        public BaseViewModel(ConfigurationService configurationService)
        {
            _configService = configurationService;
            _baseCurrency = _configService.GetDefaultCurrency();
        }

        public string BaseCurrency
        {
            get => _baseCurrency;
            set => SetProperty(ref _baseCurrency, value, () => _configService.SetBaseCurrency(value));
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingStore, T value, Action onChanged = null!, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
