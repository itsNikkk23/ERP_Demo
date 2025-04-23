using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LoginForm.Models
{

    [Table("Fabrics", Schema = "Manufacture")]
    public class Fabrics
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FabricID { get; set; }
        public string FabricName { get; set; }
    }
}
