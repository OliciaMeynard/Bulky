using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyWeb.Models
{
    public class Category
    {
        /// <summary>
        /// /[Key] will make next line of code primary key
        /// /if you have like Category___ID
        /// </summary>
        /// 

        //// If you name it Id
        ////// no need to put [key] Id will be automatically treated as primary key

        //// also if the name is same like the model Category and followed by Id like CategoryId
        ///No need to put the [Key]

        [Key]
        public int Id { get; set; }


        /// <summary>
        ///  [Required] data annotation will make the column non nullable
        /// </summary>
        [Required]
        [DisplayName("Category Name")]
        [MaxLength(40)]
        [MinLength(6)]
        public string Name { get; set; }


        [Required]
        [DisplayName("Display Order")]
        [Range(1,100, ErrorMessage ="Must be between 1-100")]
        //[Range(1,100)]
        public int DisplayOrder { get; set; }
    }
}
