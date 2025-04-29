namespace ERP.Models
{
    public class Order_Item
    {
        public int itemID { get; set; }
        public int OrderID { get; set; }
        public int productid { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
