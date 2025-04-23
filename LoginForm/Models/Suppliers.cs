using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LoginForm.Models
{
    [Table("Suppliers",Schema ="Supply")]
    public class Suppliers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string SupplierContact { get; set; }
    }
}
