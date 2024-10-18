using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divisas.Models
{
    public class ExchangeRateHistory
    {
        public int Id { get; set; }
        public int CurrencyId { get; set; }
        public decimal Rate { get; set; }
        public DateTime Date { get; set; }
    }
}
