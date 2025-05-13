using ERP.Models;
using System.Web;

namespace ERP.Repositories
{
    public interface ICustomersRepositories
    {
        int AddProduct(DisplayProduct product);
        bool AddProductImage(ProductImage productImage);
        List<DisplayProduct> GetAllProducts();
        DisplayProduct ProductById(int id);
        List<ProductImage> GetImagesByProductId(int productId);
        List<DisplayProduct> GetRelativeProducts(string cateId, string designId, string fabricId, string weaveId);

        bool Register(Customers customer);
        //bool InsertWishList(WishList wishList);

        bool Login(string email, string password);
        bool UpdateProfile(Customers customer);
        bool ChangePassword(string email, string oldPassword, string newPassword);

        Customers GetSessionDataByEmail(string email);
        bool AddToWishlist(int userId, int productId);
        List<DisplayProduct> GetWishlist(int userId);
        List<DisplayProduct> CheckWishlist(int userId);
        bool IsInWishlist(int productId, int userId);
        void RemoveFromWishlist(int userId, int productId);

        int CreateOrder(Orders order,List<Order_Item> items);
        void SaveAddress(AddressModel address, int customerId);

        void UpdateOrderStatus(int orderId, string status);
       
        
        decimal GetOrderAmount(int orderId);
        decimal GetOrderTotalAmount(int orderId);
        int GetAvailableStock(int productId);
        void SavePaymentDetails(PaymentDetails payment);
        //--

        // Cart GetCart();
        //void AddToCart(int productId, string name, decimal price, int quantity);
        void AddItemToCart(int userId, int productId, int quantity);
        int GetCartItemCount(int userId);
        CartItemViewModel GetCartItemById(int cartId, int userId);
        void UpdateCartItemQuantity(int cartId, int quantity);
        List<CartItemViewModel> GetCartItemsByUserId(int userId);
        void RemoveCartItem(int cartId);

        IEnumerable<Category> Getcategories();
         List<DisplayProduct> CategoryById(int id);
        //void PlaceOrder(Orders order);
        //bool PlaceOrder(int userId);
        bool PlaceOrder(int userId, string address);
    }
}
