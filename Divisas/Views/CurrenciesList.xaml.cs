using System.Collections.ObjectModel;
using Divisas.Models;
using Divisas.Utils;
using Divisas.ViewModels;

namespace Divisas.Views;

public partial class CurrenciesList : ContentPage
{
    private CurrenciesListViewModel _viewModel;
    private IServiceProvider _serviceProvider;

    public CurrenciesList(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
        var locator = serviceProvider.GetRequiredService<ViewModelLocator>();
        _viewModel = locator.CurrenciesListViewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        _viewModel.IsLoading = false;
        base.OnAppearing();

        if (!_viewModel.IsLoading && _viewModel.Currencies.Count == 0)
            _viewModel.LoadCurrenciesCommand.Execute(null);
    }

    private void NavigateToCurrencyDetail(object sender, TappedEventArgs args)
    {
        if (args.Parameter is Models.Currency selected)
        {
            var currencyId = selected.Id;
            Navigation.PushAsync(new CurrencyDetail(_serviceProvider, currencyId));
        }
    }

    private void NavigateToNewCurrency(object sender, EventArgs e)
    {
        Navigation.PushAsync(new NewCurrency(_serviceProvider));
    }
}