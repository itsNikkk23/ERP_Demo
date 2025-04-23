using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LoginForm.Models
{
   // [Table("ProductView", Schema = "Supply")]
    public class ProductView
    {
        public Products Product { get; set; }
        public ProductImage ProductImage { get; set; }
        public string ImagePath { get; set; } // This comes from ProductImage
    }
}
