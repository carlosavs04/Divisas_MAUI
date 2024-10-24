using System.Globalization;
using Divisas.Models;

namespace Divisas.Views;

public partial class Config : ContentPage
{
	public Config()
	{
		InitializeComponent();

		List<Currency> currencies = new List<Currency>
		{
				new Currency { Code = "USD", Name = "United States Dollar" },
				new Currency { Code = "EUR", Name = "Euro" },
				new Currency { Code = "MXN", Name = "Mexican Peso" },
		};

		currencyPicker.ItemsSource = currencies;
	}
}
