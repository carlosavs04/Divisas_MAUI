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
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = DbConection.GetDbPath("Currency.db");
            optionsBuilder.UseSqlite($"Filename={dbPath}");
        }
    }
}
