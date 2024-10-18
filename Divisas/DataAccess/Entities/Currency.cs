using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divisas.DataAccess.Entities
{
    [Table("Currency")]
    public class Currency
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(3)]
        public required string Code { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Flag { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public bool IsBase { get; set; }

        [Required]
        public bool IsDefault { get; set; }

        public virtual ICollection<ExchangeRateHistory>? Rates { get; set; }
    }
}
