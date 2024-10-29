using Divisas.Utils;
using Divisas.ViewModels;

namespace Divisas.Views;

public partial class Currencies : ContentPage
{
	private CurrencySelectorViewModel _viewModel;
    private HomeViewModel _homeViewModel;
    private IServiceProvider _serviceProvider;
    private bool _isFromCurrency;

    public Currencies(IServiceProvider serviceProvider, HomeViewModel home, bool isFromCurrency)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
        var locator = serviceProvider.GetRequiredService<ViewModelLocator>();
        _viewModel = locator.CurrencySelectorViewModel;
        BindingContext = _viewModel;
        _homeViewModel = home;
        _isFromCurrency = isFromCurrency;
    }

    protected override void OnAppearing()
    {
        _viewModel.IsLoading = false;
        base.OnAppearing();

        if (!_viewModel.IsLoading && _viewModel.Currencies.Count == 0)
            _viewModel.LoadCurrenciesCommand.Execute(null);
    }

    private async void OnCurrencySelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Models.Currency selected)
        {
            if (_isFromCurrency)
                _homeViewModel.FromCurrency = selected;
            
            else
                _homeViewModel.ToCurrency = selected;
        }

        await Navigation.PopModalAsync();
    }
}