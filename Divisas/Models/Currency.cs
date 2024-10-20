using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divisas.Models
{
    public class Currency
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(3)]
        public required string Code { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        [MaxLength(50)]
        public string? Flag { get; set; }

        public bool IsActive { get; set; }

        public bool IsBase { get; set; }

        public bool IsDefault { get; set; }

        public decimal? ActualRate { get; set; }

        public decimal? SuggestedRetailPrice { get; set; }
    }
}
