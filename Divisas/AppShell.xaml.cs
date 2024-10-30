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
            CurrentItem = homeTab;
        }

        private void InitTabs()
        {
            var currenciesListPage = _serviceProvider.GetRequiredService<CurrenciesList>();
            var homePage = _serviceProvider.GetRequiredService<Home>();
            var configPage = _serviceProvider.GetRequiredService<Config>();

            currenciesTab.Items.Add(new ShellContent { Content = currenciesListPage });
            homeTab.Items.Add(new ShellContent { Content = homePage });
            configTab.Items.Add(new ShellContent { Content = configPage });
        }
    }
}
