
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace GoMartApplication.DTO
{
    public class BillDetail
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Bill")]
        [StringLength(50)]
        public string Bill_ID { get; set; }
        public Bill Bill { get; set; }

        [ForeignKey("Product")]
        public int ProdID { get; set; }
        public Product Product { get; set; }
        public decimal Price { get; set; }

        public int Qty { get; set; }

        public decimal Total { get; set; }
        }
    }
