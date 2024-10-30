
using System.Collections.ObjectModel;
using Divisas.Utils;
using Divisas.ViewModels;
using Syncfusion.Maui.Charts;

namespace Divisas.Views;

public partial class Home : ContentPage
{
    private HomeViewModel _viewModel;
    private IServiceProvider _serviceProvider;

	public Home(IServiceProvider serviceProvider)
	{
		InitializeComponent();
        _serviceProvider = serviceProvider;
        var locator = serviceProvider.GetRequiredService<ViewModelLocator>();
        _viewModel = locator.HomeViewModel;
        BindingContext = _viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.IsLoading = false;

        if (!_viewModel.IsLoading)
        {
            if (!_viewModel.IsInitialized)
            {
                _viewModel.InitCurrenciesCommand.Execute(null);
            } 
            else
            {
                _viewModel.UpdateCurrenciesRatesCommand.Execute(null);
            }
        }
    }

    private void ShowAvailableCurrencies(object sender, TappedEventArgs e)
    {
        var isFromCurrency = sender == FromCurrencyImage;

        var currenciesSelector = new Currencies(_serviceProvider, _viewModel, isFromCurrency);
        Navigation.PushModalAsync(currenciesSelector);
    }

    private void UnfocusAmount(object sender, FocusEventArgs e)
    {
        if (e.IsFocused)
            return;

        _viewModel.ConvertCurrencyCommand.Execute(null);
    }

	private void GenerateTicket(object sender, EventArgs e)
	{
		   Navigation.PushModalAsync(new Ticket(_viewModel));
	}
}