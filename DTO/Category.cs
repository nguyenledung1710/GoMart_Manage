
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoMartApplication.DTO
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CatID { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }

        public string CategoryDesc { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
