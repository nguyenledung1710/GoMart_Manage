using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoMartApplication.DTO
{
    public class Category
    {
        [Key]
        public int CatID { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }

        public string CategoryDesc { get; set; }

        // Navigation properties
        public ICollection<Product> Products { get; set; }
    }
}
