using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoMartApplication.DTO
{
    public class Product
    {
        [Key]
        public int ProdID { get; set; }

        [Required]
        [StringLength(200)]
        public string ProdName { get; set; }

        [ForeignKey("Category")]
        public int ProdCatID { get; set; }
        public Category Category { get; set; }

        public decimal ProdPrice { get; set; }

        public int ProdQty { get; set; }

        public ICollection<BillDetail> BillDetails { get; set; }
    }
}
