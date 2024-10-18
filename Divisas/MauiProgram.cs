using Divisas.DataAccess;
using Divisas.Utils;
using Divisas.Utils.Test;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Divisas
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddDbContext<CurrencyDbContext>(options =>
            {
                string dbPath = DbConnection.GetDbPath("Currency.db");
                options.UseSqlite($"Filename={dbPath}");
            });

            //TEST
            //builder.Services.AddTransient<DatabaseViewer>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
