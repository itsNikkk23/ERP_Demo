using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ERP.Models
{
    [Table("ProductImage",Schema ="Supply")]
    public class ProductImage
    {
        [Key]
        public int Imageid { get; set; }
        [ForeignKey("Products")]
        public int productid { get; set; }
        public string ImagePath { get; set; }
    }
}
