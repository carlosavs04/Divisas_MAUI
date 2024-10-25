using Divisas.Utils;
using Divisas.ViewModels;

namespace Divisas.Views;

public partial class CurrencyDetail : ContentPage
{
	private CurrencyDetailViewModel _viewModel;

    public CurrencyDetail(IServiceProvider serviceProvider, int currencyId)
	{
		InitializeComponent();
		var locator = serviceProvider.GetRequiredService<ViewModelLocator>();
        _viewModel = locator.CurrencyDetailViewModel;
        BindingContext = _viewModel;
        _viewModel.IsLoading = false;
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
}