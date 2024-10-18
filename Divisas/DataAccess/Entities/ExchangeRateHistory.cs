using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divisas.DataAccess.Entities
{
    [Table("ExchangeRateHistory")]
    public class ExchangeRateHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int CurrencyId { get; set; }

        [Required]
        public decimal Rate { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
