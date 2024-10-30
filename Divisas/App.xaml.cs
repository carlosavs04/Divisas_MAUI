using Divisas.Services;
using Divisas.Utils;
using Divisas.Utils.Test;

namespace Divisas
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            SeedDatabase.Initialize(serviceProvider);

            var configService = serviceProvider.GetRequiredService<ConfigurationService>();
            bool isDarkMode = configService.GetDarkMode();

            if (App.Current != null)
                App.Current.UserAppTheme = isDarkMode ? AppTheme.Dark : AppTheme.Light;

            MainPage = new AppShell(_serviceProvider);
        }

        protected override async void OnStart()
        {
            var updateService = _serviceProvider.GetRequiredService<CurrencyRateUpdateService>();
            await updateService.UpdateRatesAsync();

            //TEST
            /*var viewer = _serviceProvider.GetRequiredService<DatabaseViewer>();
            viewer.ShowDatabaseContents();*/
            base.OnStart();
        }
    }
}