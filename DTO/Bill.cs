using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoMartApplication.DTO
{
   public class Bill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(50)]
        public string Bill_ID { get; set; }

        [ForeignKey("Seller")]
        public string SellerID { get; set; }
        public Seller Seller { get; set; }

        public DateTime SellDate { get; set; }

        [Column(TypeName = "decimal")]
        public decimal TotalAmt { get; set; }
        public ICollection<BillDetail> BillDetails { get; set; }
        public Bill()
        {
            BillDetails = new HashSet<BillDetail>();
        }
    }
}
