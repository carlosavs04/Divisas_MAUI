using Divisas.Utils;
using Divisas.Views;

namespace Divisas
{
    public partial class AppShell : Shell
    {
        private readonly IServiceProvider _serviceProvider;

        public AppShell(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            InitTabs();
            Routing.RegisterRoute(nameof(CurrenciesList), typeof(CurrenciesList));
            Routing.RegisterRoute(nameof(CurrencyDetail), typeof(CurrencyDetail));
        }

        private void InitTabs()
        {
            var currenciesListPagee = _serviceProvider.GetRequiredService<CurrenciesList>();

            currenciesTab.Items.Add(new ShellContent { Content = currenciesListPagee });
        }
    }
}
