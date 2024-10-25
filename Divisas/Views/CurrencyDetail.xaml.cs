using Divisas.Utils;
using Divisas.ViewModels;

namespace Divisas.Views;

public partial class CurrencyDetail : ContentPage
{
	private CurrencyDetailViewModel _viewModel;
    private int _currencyId;
    private IServiceProvider _serviceProvider;

    public CurrencyDetail(IServiceProvider serviceProvider, int currencyId)
	{
		InitializeComponent();
		var locator = serviceProvider.GetRequiredService<ViewModelLocator>();
        _viewModel = locator.CurrencyDetailViewModel;
        BindingContext = _viewModel;
        _viewModel.IsLoading = false;
        _serviceProvider = serviceProvider;
        _currencyId = currencyId;
        LoadCurencyDetails(currencyId);

        _viewModel.PropertyChanged += async (sender, args) =>
        {
            if (args.PropertyName == nameof(_viewModel.Currency))
            {
                if (_viewModel.Currency == null)
                {
                    await Navigation.PushAsync(new CurrenciesList(serviceProvider));
                    await DisplayAlert("Divisa eliminada", "La divisa ha sido eliminada correctamente", "Aceptar");
                }
            }
        };
    }

    private void LoadCurencyDetails(int currencyId)
    {
        if (!_viewModel.IsLoading) 
        {
            _viewModel.Initialize(currencyId);
            _viewModel.LoadCurrencyCommand.Execute(null);
        }
    }

    private void NavigateToEditionMode(object sender, TappedEventArgs args)
    {
        Navigation.PushAsync(new BtnCurrencyDetail(_serviceProvider, _currencyId));
    }
}