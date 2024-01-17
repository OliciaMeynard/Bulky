using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

//namespace BulkyBookWeb.Models
namespace BulkyBook.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Category Name")]
        [MaxLength(40)]
        [MinLength(6)]
        public string Name { get; set; }


        [Required]
        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Must be between 1-100")]
        public int DisplayOrder { get; set; }
    }
}
