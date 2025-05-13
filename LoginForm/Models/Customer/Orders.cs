namespace ERP.Models
{
    public class Orders
    {
        public int OrderId { get; set; }
        public int customer_id { get; set; }
        public int ProductId { get; set; }
        public string OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public int handloomID { get; set; }
        public int staffID { get; set; }
        public string RazorOrder_Id { get; set; }
        public List<Order_Item> Items { get; set; }=new List<Order_Item>();
       // public List<Product> products { get; set; } = new List<Product>();
    }
}
