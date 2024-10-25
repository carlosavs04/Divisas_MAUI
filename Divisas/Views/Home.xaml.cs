using System.Collections.ObjectModel;
using Syncfusion.Maui.Charts;

namespace Divisas.Views;

public partial class Home : ContentPage
{
	public ObservableCollection<CurrencyData> Data { get; set; }

	public Home()
	{
		InitializeComponent();
		 Data = new ObservableCollection<CurrencyData>
            {
                new CurrencyData { Rate = 19.58, Date = DateTime.Now.AddDays(-5) },
                new CurrencyData { Rate = 19.60, Date = DateTime.Now.AddDays(-4) },
                new CurrencyData { Rate = 19.55, Date = DateTime.Now.AddDays(-3) },
                new CurrencyData { Rate = 19.50, Date = DateTime.Now.AddDays(-2) },
                new CurrencyData { Rate = 19.45, Date = DateTime.Now.AddDays(-1) },
                new CurrencyData { Rate = 19.40, Date = DateTime.Now }
            };

            BindingContext = this;
	}

    private void ShowAvailableCurrencies(object sender, TappedEventArgs e)
    {
		Console.WriteLine("Tapped");
		Navigation.PushModalAsync(new Divisas());
    }

	public class CurrencyData
	{
		public double Rate { get; set; }
		public DateTime Date { get; set; }
	}
}