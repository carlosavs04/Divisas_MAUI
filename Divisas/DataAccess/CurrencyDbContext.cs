using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Divisas.Utils;

namespace Divisas.DataAccess
{
    public class CurrencyDbContext : DbContext
    {

        public CurrencyDbContext(DbContextOptions<CurrencyDbContext> options) : base(options) { }

        public virtual DbSet<Entities.Currency> Currencies { get; set; }
        public virtual DbSet<Entities.ExchangeRateHistory> ExchangeRateHistories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string dbPath = DbConnection.GetDbPath("Currency.db");
                optionsBuilder.UseSqlite($"Filename={dbPath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Entities.Currency>().HasData(
                new Entities.Currency { Id = 1, Code = "USD", Name = "Dólar estadounidense", Flag = "us_flag", IsActive = true, IsBase = true, IsDefault = true },
                new Entities.Currency { Id = 2, Code = "MXN", Name = "Peso mexicano", Flag = "mx_flag", IsActive = true, IsBase = false, IsDefault = true },
                new Entities.Currency { Id = 3, Code = "EUR", Name = "Euro", Flag = "eu_flag", IsActive = true, IsBase = false, IsDefault = true },
                new Entities.Currency { Id = 4, Code = "GBP", Name = "Libra esterlina", Flag = "uk_flag", IsActive = true, IsBase = false, IsDefault = true },
                new Entities.Currency { Id = 5, Code = "JPY", Name = "Yen japonés", Flag = "jp_flag", IsActive = true, IsBase = false, IsDefault = true }
            );

            modelBuilder.Entity<Entities.ExchangeRateHistory>().HasData(
                new Entities.ExchangeRateHistory { Id = 1, CurrencyId = 1, Rate = 1.0m, Date = new DateTime(2024, 10, 17) },
                new Entities.ExchangeRateHistory { Id = 2, CurrencyId = 2, Rate = 19.83m, Date = new DateTime(2024, 10, 17) },
                new Entities.ExchangeRateHistory { Id = 3, CurrencyId = 3, Rate = 0.92m, Date = new DateTime(2024, 10, 17) },
                new Entities.ExchangeRateHistory { Id = 4, CurrencyId = 4, Rate = 0.77m, Date = new DateTime(2024, 10, 17) },
                new Entities.ExchangeRateHistory { Id = 5, CurrencyId = 5, Rate = 150.12m, Date = new DateTime(2024, 10, 17) }
            );
        }
    }
}
