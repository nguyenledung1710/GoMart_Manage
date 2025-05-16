
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace GoMartApplication.DTO
{
    public class Seller
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string SellerId { get; set; }

        [Required]
        [StringLength(100)]
        public string SellerName { get; set; }

        public int SellerAge { get; set; }

        [StringLength(20)]
        public string SellerPhone { get; set; }

        [Required]
        [StringLength(100)]
        public string SellerPass { get; set; }
        public ICollection<Bill> Bills { get; set; }
    }
}
