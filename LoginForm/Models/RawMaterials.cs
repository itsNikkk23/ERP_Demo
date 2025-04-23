using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginForm.Models
{
    [Table("RawMaterials", Schema = "Manufacture")]

    public class RawMaterials
    {
        [Key]
        public int RawMaterialID { get; set; }

        [Required]
        public string MaterialName { get; set; }

        [Required]
        public string MaterialType { get; set; }

        [Required]
        public string UnitOfMeasure { get; set; }

        public string  Description { get; set; }
    }
}
