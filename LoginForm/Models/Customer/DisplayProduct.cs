namespace ERP.Models
{
    public class DisplayProduct
    {
        public int productid { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int ProductRatings { get; set; }
        public int customer_id { get; set; }
        public int wishlist_id { get; set; }
        public bool IsInWishlist { get; set; }
        public List<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
       public string category_name { get; set; }
       public string FabricName { get; set; }
        public string DesignType { get; set; }
        public string weave_type { get; set; }
        public string ColorName { get; set; }

        public List<CartItemViewModel> cartItemViewModels { get; set; }=new List<CartItemViewModel>();
    }
}
