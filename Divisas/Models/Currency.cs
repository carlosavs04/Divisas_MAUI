using System;
using System.Collections.Generic;
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
        public string Code { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Flag { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
