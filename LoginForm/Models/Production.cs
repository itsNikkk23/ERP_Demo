using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
namespace LoginForm.Models
{
    [Table("Production", Schema = "Manufacture")]
    public class Production
    {
        [Key]
        public int ProductionID { get; set; }
        [ForeignKey("ProductAttribute")]
        public int productattributeid { get; set; }
        public DateTime ProductionDate { get; set; }
        [AllowNull]
        public string  MachineUsed { get; set; }
        [ForeignKey("employees")]
        public int EmployeeID { get; set; }
        [ForeignKey("RawMaterials")]
        public int RawMaterialID { get; set; }
        public int Quantity { get; set; }

    }
}
