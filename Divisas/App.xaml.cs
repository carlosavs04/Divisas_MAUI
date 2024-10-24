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