using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ERP.Models
{
    [Table("Products",Schema ="Supply")]
    public class Products
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int productid { get; set; }
        public string ProductName { get; set; }
        [ForeignKey("Production")]
        public int ProductionID { get; set; }
        public decimal ProductRatings { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public List<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
    }
}
