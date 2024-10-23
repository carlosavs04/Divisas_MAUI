using System.Collections.ObjectModel;
using Divisas.Models;

namespace Divisas.Views;

public partial class CurrenciesList : ContentPage
{
    public ObservableCollection<Currency> Currencies { get; set; }
    public CurrenciesList()
    {
        InitializeComponent();

        Currencies = new ObservableCollection<Currency>{
            new Currency { Code = "USD", Name = "Dolar" },
            new Currency { Code = "EUR", Name = "Euro" },
            new Currency { Code = "JPY", Name = "Yen" }
        };
        
        BindingContext = this;
    }

    void NavigateToCurrencyDetail(object sender, TappedEventArgs args)
    {
        Navigation.PushAsync(new CurrencyDetail());
    }
}