using Divisas.Utils;
using Divisas.Utils.Test;

namespace Divisas
{
    public partial class App : Application
    {
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            if (serviceProvider == null)
                throw new ArgumentNullException();

            SeedDatabase.Initialize(serviceProvider);
            
            //TEST
            /*var viewer = serviceProvider.GetRequiredService<DatabaseViewer>();
            viewer.ShowDatabaseContents();*/

            MainPage = new AppShell();
        }
    }
}
