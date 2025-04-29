using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ERP.Models
{
    [Table("ProductColor", Schema = "Manufacture")]
    public class ProductColor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ColorID { get; set; }
        [Required]
        [StringLength(20)]
        public string ColorName { get; set; }
      
    }
}
