using Divisas.ViewModels;

namespace Divisas.Views;

public partial class Ticket : ContentPage
{
	private HomeViewModel _homeViewModel;
	public Ticket(HomeViewModel homeViewModel)
	{
		InitializeComponent();
		_homeViewModel = homeViewModel;
        BindingContext = _homeViewModel;
    }
}