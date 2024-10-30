using System.Globalization;
using Divisas.Models;
using Divisas.Utils;
using Divisas.ViewModels;

namespace Divisas.Views;

public partial class Config : ContentPage
{
	private ConfigurationViewModel _viewModel;
	private IServiceProvider _serviceProvider;

    public Config(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
        var locator = serviceProvider.GetRequiredService<ViewModelLocator>();
        _viewModel = locator.ConfigurationViewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        _viewModel.IsLoading = false;
        base.OnAppearing();

        if (!_viewModel.IsLoading && _viewModel.Currencies.Count == 0)
            _viewModel.LoadCurrenciesCommand.Execute(null);
    }

    private void OnCurrencyChanged(object sender, EventArgs e)
    {
        if (_viewModel.ChangeBaseCurrencyCommand.CanExecute(null))
        {
            _viewModel.ChangeBaseCurrencyCommand.Execute(null);
        }
    }

}
