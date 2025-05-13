namespace ERP.Models
{
    public class WishList
    {
        public int wishlist_id{ get; set; }
        public int customer_id{ get; set; }
        public int productid{ get; set; }
        public DateTime added_at { get; set; }

    }
}
