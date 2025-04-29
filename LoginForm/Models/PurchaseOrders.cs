using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ERP.Models
{
    [Table("PurchaseOrders", Schema = "Manufacture")]
    public class PurchaseOrders
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderID { get; set; }
        [ForeignKey("Suppliers")]
        public int SupplierID { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public DateTime? DeliveryDate { get; set; }
        public Decimal TotalAmount { get; set; }
        public string Status { get; set; }

    }
}
