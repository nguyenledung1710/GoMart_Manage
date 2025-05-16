
using System.ComponentModel.DataAnnotations;


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

        [Required]
        [StringLength(100)]
        public string address { get; set; }
    }
}
