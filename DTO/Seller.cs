using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoMartApplication.DTO
{
    public class Seller
    {
        [Key]
        public int SellerId { get; set; }

        [Required]
        [StringLength(100)]
        public string SellerName { get; set; }

        public int SellerAge { get; set; }

        [StringLength(20)]
        public string SellerPhone { get; set; }

        [Required]
        [StringLength(100)]
        public string SellerPass { get; set; }

        // Navigation properties
        public ICollection<Bill> Bills { get; set; }
    }
}
