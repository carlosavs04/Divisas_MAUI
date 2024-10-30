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

    protected override void OnAppearing()
    {
        base.OnAppearing();

        DateLabel.Text = _homeViewModel.CurrentDate;
        TimeLabel.Text = _homeViewModel.CurrentTime;
    }
}