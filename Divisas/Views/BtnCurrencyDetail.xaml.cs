using Divisas.Utils;
using Divisas.ViewModels;

namespace Divisas.Views;

public partial class BtnCurrencyDetail : ContentPage
{
	private UpdateCurrencyViewModel _viewModel;
    private int _currencyId;
    private IServiceProvider _serviceProvider;

    public BtnCurrencyDetail(IServiceProvider serviceProvider, int currencyId)
	{
		InitializeComponent();
		var locator = serviceProvider.GetRequiredService<ViewModelLocator>();
        _viewModel = locator.UpdateCurrencyViewModel;
        BindingContext = _viewModel;
        _viewModel.IsLoading = false;
        _serviceProvider = serviceProvider;
        _currencyId = currencyId;
        LoadCurencyDetails(currencyId);
    }

    private void LoadCurencyDetails(int currencyId)
    {
        if (!_viewModel.IsLoading)
        {
            _viewModel.Initialize(currencyId);
            _viewModel.LoadCurrencyCommand.Execute(null);
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        _viewModel.UpdateCurrencyCommand.Execute(null);
        await Navigation.PushAsync(new CurrencyDetail(_serviceProvider, _currencyId));
    }


}