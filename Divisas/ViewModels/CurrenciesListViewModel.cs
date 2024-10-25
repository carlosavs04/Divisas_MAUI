using Divisas.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Divisas.ViewModels
{
    public class CurrenciesListViewModel : BaseViewModel
    {
        private readonly CurrencyService _currencyService;
        private bool _isLoading;
        private string _searchText;

        public ObservableCollection<Models.Currency> Currencies { get; private set; }
        public ObservableCollection<Models.Currency> FilteredCurrencies { get; private set; }
        public ICommand LoadCurrenciesCommand { get; }
        public ICommand SearchCommand { get; }


        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    if (!IsLoading)
                        ApplySearch();
                }
            }
        }

        public CurrenciesListViewModel(CurrencyService currencyService, ConfigurationService configService) : base(configService)
        {
            _currencyService = currencyService;
            _searchText = string.Empty;
            Currencies = new ObservableCollection<Models.Currency>();
            FilteredCurrencies = new ObservableCollection<Models.Currency>();
            LoadCurrenciesCommand = new Command(async () => await LoadCurrenciesAsync());
            SearchCommand = new Command(ApplySearch);
        }

        private async Task LoadCurrenciesAsync()
        {
            if (IsLoading)
                return;

            try
            {
                IsLoading = true;
                await Task.Delay(100);

                var currenciesList = await _currencyService.GetCurrenciesAsync();
                Currencies.Clear();

                foreach (var currency in currenciesList)
                {
                    Currencies.Add(currency);
                }

                ApplySearch();
            }

            finally
            {
                IsLoading = false;
            }

        }

        private string RemoveDiacritics(string text)
        {
            return string.Concat(
                text.Normalize(NormalizationForm.FormD)
                .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
            ).Normalize(NormalizationForm.FormC);
        }

        private void ApplySearch()
        {
            FilteredCurrencies.Clear();

            if (string.IsNullOrWhiteSpace(SearchText))
            {
                foreach (var currency in Currencies)
                {
                    FilteredCurrencies.Add(currency);
                }
            }

            else
            {
                var normalizedSearchText = RemoveDiacritics(SearchText.ToLower().Trim());

                var filtered = Currencies.Where(c => c.Code.Contains(normalizedSearchText.ToUpper()) ||
                    RemoveDiacritics(c.Name.ToLower()).Contains(normalizedSearchText)).ToList();

                FilteredCurrencies.Clear();
                foreach (var currency in filtered)
                {
                    FilteredCurrencies.Add(currency);
                }
            }
        }
    }
}
