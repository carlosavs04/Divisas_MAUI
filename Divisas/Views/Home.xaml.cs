namespace Divisas.Views;

public partial class Home : ContentPage
{
	public Home()
	{
		InitializeComponent();
	}

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
		Console.WriteLine("Tapped");
		Navigation.PushModalAsync(new Divisas());
    }
}