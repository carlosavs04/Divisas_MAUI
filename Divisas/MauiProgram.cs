using Divisas.DataAccess;
using Divisas.Models;
using Divisas.Services;
using Divisas.Utils;
using Divisas.Utils.Test;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;

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

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            var a = Assembly.GetExecutingAssembly();
            using var stream = a.GetManifestResourceStream("Divisas.appsettings.json");

            if (stream != null)
            {
                var config = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();

                builder.Configuration.AddConfiguration(config);
            }

            builder.Services.Configure<ExchangeRateApiConfig>(builder.Configuration.GetSection("ExchangeRateApi"));
            builder.Services.AddHttpClient<ExchangeRateApiService>();
            builder.Services.AddTransient<CurrencyRateUpdateService>();

            //TEST
            //builder.Services.AddTransient<DatabaseViewer>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
