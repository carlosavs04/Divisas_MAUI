using System.Collections.ObjectModel;
using Divisas.Models;
using Divisas.Utils;
using Divisas.ViewModels;

namespace Divisas.Views;

public partial class CurrenciesList : ContentPage
{
    private CurrenciesListViewModel _viewModel;

    public CurrenciesList(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        var locator = serviceProvider.GetRequiredService<ViewModelLocator>();
        _viewModel = locator.CurrenciesListViewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (!_viewModel.IsLoading && _viewModel.Currencies.Count == 0)
            _viewModel.LoadCurrenciesCommand.Execute(null);
    }

    void NavigateToCurrencyDetail(object sender, TappedEventArgs args)
    {
        Shell.Current.GoToAsync(nameof(CurrencyDetail));
    }
}