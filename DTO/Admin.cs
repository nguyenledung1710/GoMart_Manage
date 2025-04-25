using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoMartApplication.DTO
{
    public class Admin
    {
        [Key]
        [StringLength(50)]
        public string AdminId { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        // Navigation properties
        public ICollection<Bill> Bills { get; set; }
    }
}
