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
        }

        private void InitTabs()
        {
            var currenciesListPage = _serviceProvider.GetRequiredService<CurrenciesList>();

            currenciesTab.Items.Add(new ShellContent { Content = currenciesListPage });
        }
    }
}
