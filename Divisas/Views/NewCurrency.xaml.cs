using Divisas.Utils;
using Divisas.ViewModels;

namespace Divisas.Views;

public partial class NewCurrency : ContentPage
{
	private AddCurrencyViewModel _viewModel;
	private IServiceProvider _serviceProvider;

    public NewCurrency(IServiceProvider serviceProvider)
	{
		InitializeComponent();
        _serviceProvider = serviceProvider;
        var locator = serviceProvider.GetRequiredService<ViewModelLocator>();
        _viewModel = locator.AddCurrencyViewModel;
        BindingContext = _viewModel;
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        _viewModel.CreateCurrencyCommand.Execute(null);
        await Navigation.PushAsync(new CurrenciesList(_serviceProvider));
    }
}