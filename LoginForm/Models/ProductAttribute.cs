using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ERP.Models
{
    [Table("ProductAttribute", Schema = "Manufacture")]
    public class ProductAttribute
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int productAttributeid { get; set; }
        [ForeignKey("Categories")]
        public int CategoryID { get; set; }
        [ForeignKey("Fabrics")]
        public int FabricID { get; set; }
        [ForeignKey("Designs")]
        public int DesignID { get; set; }
        [ForeignKey("Weaving_process")]
        public int WeaveID { get; set; }
        [ForeignKey("ProductColor")]
        public int ColorID { get; set; }
    }
}
