using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ERP.Models
{
    [Table("Designs", Schema = "Manufacture")]
    public class Designs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DesignID { get; set; }
        [Required]
        public string DesignType { get; set; }
    }
}
