using ERP.Data;
using ERP.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Security.AccessControl;
using static System.Net.Mime.MediaTypeNames;

namespace ERP.Repositories
{
    public class CustomersRepositories : ICustomersRepositories
    {
        private readonly DbHelper _dbHelper;
        public CustomersRepositories(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public int AddProduct(DisplayProduct product)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                string query = "INSERT INTO Supply.Products (ProductName, Price, ProductRatings) OUTPUT INSERTED.productid VALUES (@ProductName, @Price, @Rating)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@Rating", product.ProductRatings);

                con.Open();
                int productId = (int)cmd.ExecuteScalar();
                con.Close();

                return productId; // Return newly added ProductId
            }
        }



        public bool AddProductImage(ProductImage productImage)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                string query = "INSERT INTO Supply.ProductImage (ProductId, ImagePath) VALUES (@ProductId, @ImagePath)";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@ProductId", productImage.productid);
                cmd.Parameters.AddWithValue("@ImagePath", productImage.ImagePath);
                //cmd.Parameters.AddWithValue("@ContentType", productImage.contenttype);

                con.Open();
                int rows = cmd.ExecuteNonQuery();
                con.Close();

                return rows > 0;
            }
        }

        public int GetAvailableStock(int productId)
        {
            int stock = 0;
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT Quantity FROM inventory.Stock WHERE productid = @pid", con);
                cmd.Parameters.AddWithValue("@pid", productId);
                con.Open();
                var result = cmd.ExecuteScalar();
                if (result != null)
                    stock = Convert.ToInt32(result);
            }
            return stock;
        }
        public int CreateOrder(Orders order, List<Order_Item> items)
        {
            int orderId;
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sales.sp_CreateOrders", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Customer_Id", order.customer_id);
                cmd.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                cmd.Parameters.AddWithValue("@Status", order.Status);

                SqlParameter outputId = new SqlParameter("@OrderID", SqlDbType.Int)
                { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(outputId);

                con.Open();
                cmd.ExecuteNonQuery();
                orderId = Convert.ToInt32(outputId.Value);
                // con.Close();

            }
            foreach (var i in items)
            {
                using (SqlConnection con = _dbHelper.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("sales.sp_OrderItems", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@OrderID", orderId);
                    cmd.Parameters.AddWithValue("@productid", i.productid);
                    cmd.Parameters.AddWithValue("@Quantity", i.Quantity);
                    cmd.Parameters.AddWithValue("@Price", i.Price);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return orderId; // Return the newly created OrderId
        }
        public void UpdateOrderStatus(int orderId, string status)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("UPDATE sales.orders SET Status = @Status WHERE OrderId = @OrderId", con);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@OrderId", orderId);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        //for buy now

        public decimal GetOrderAmount(int orderId)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                string query = "SELECT TotalAmount FROM sales.orders WHERE OrderId = @orderId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@orderId", orderId);
                con.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToDecimal(result) : 0;
            }
        }
        public decimal GetOrderTotalAmount(int orderId)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                string query = "SELECT TotalAmount FROM sales.orders WHERE OrderId = @orderId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@orderId", orderId);
                con.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToDecimal(result) : 0;
            }
        }
        public void SavePaymentDetails(PaymentDetails payment)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO sales.PaymentDetails (OrderId, RazorpayOrderId, RazorpayPaymentId, RazorpaySignature, Status, Amount, PaymentDate) VALUES (@OrderId, @ROrderId, @RPaymentId, @RSignature, @Status, @Amount, @Date)", con);
                cmd.Parameters.AddWithValue("@OrderId", payment.OrderId);
                cmd.Parameters.AddWithValue("@ROrderId", payment.RazorpayOrderId);
                cmd.Parameters.AddWithValue("@RPaymentId", payment.RazorpayPaymentId);
                cmd.Parameters.AddWithValue("@RSignature", payment.RazorpaySignature);
                cmd.Parameters.AddWithValue("@Status", payment.Status);
                cmd.Parameters.AddWithValue("@Amount", payment.Amount);
                cmd.Parameters.AddWithValue("@Date", payment.PaymentDate);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public bool AddToWishlist(int customer_id, int productid)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string query = @"INSERT INTO crm.wishlist (customer_id, productid) VALUES (@customer_id, @productid)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@customer_id", customer_id);
                cmd.Parameters.AddWithValue("@productid", productid);

                conn.Open();
                int cnt = cmd.ExecuteNonQuery();
                conn.Close();
                return cnt > 0; 
            }
        }
        public List<DisplayProduct> GetWishlist(int userId)
        {
            List<DisplayProduct> wishlists = new List<DisplayProduct>();
            using (SqlConnection cn = _dbHelper.GetConnection())
            {
                //string query = "select p.*,w.wishlist_id, w.customer_id from crm.wishlist w right join Supply.Products p on w.productid=p.productid where customer_id=@customer_id";
                string query = @"select p.*, w.wishlist_id, w.customer_id                
            from Supply.Products p
            INNER join crm.wishlist w on p.productid = w.productid WHERE w.customer_id = @customer_id";
               SqlCommand cmd = new SqlCommand(query, cn);
               // SqlCommand cmd = new SqlCommand("sp_GetWishlist", cn);

                //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@customer_id", userId);
                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DisplayProduct wish = new DisplayProduct
                    {
                        productid = reader["productid"] != DBNull.Value ? Convert.ToInt32(reader["productid"]) : 0,
                        ProductName = reader["ProductName"] != DBNull.Value ? reader["ProductName"].ToString() : string.Empty,
                        Price = reader["Price"] != DBNull.Value ? Convert.ToDecimal(reader["Price"]) : 0m,
                        ProductRatings = reader["ProductRatings"] != DBNull.Value ? Convert.ToInt32(reader["ProductRatings"]) : 0,

                        wishlist_id = reader["wishlist_id"] != DBNull.Value ? Convert.ToInt32(reader["wishlist_id"]) : 0,
                        customer_id = reader["customer_id"] != DBNull.Value ? Convert.ToInt32(reader["customer_id"]) : 0,

                        //IsInWishlist = reader["IsInWishlist"] != DBNull.Value ? Convert.ToBoolean(reader["IsInWishlist"]) : false
                    };
                    wishlists.Add(wish);
                }
                cn.Close();
            }
            return wishlists;
        }
        public bool IsInWishlist(int productId, int userId)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM crm.wishlist WHERE productid = @ProductId AND customer_id = @Userid";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ProductId", productId);
                cmd.Parameters.AddWithValue("@Userid", userId);

                con.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }
        public void RemoveFromWishlist(int userId, int productId)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string query = @"DELETE FROM crm.wishlist WHERE customer_id=@uid AND productid=@prodid ";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@uid", userId);
                cmd.Parameters.AddWithValue("@prodid", productId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public List<DisplayProduct> CheckWishlist(int userId)
        {
            List<DisplayProduct> wishlists = new List<DisplayProduct>();
            using (SqlConnection cn = _dbHelper.GetConnection())
            {
                //string query = "select p.*,w.wishlist_id, w.customer_id from crm.wishlist w right join Supply.Products p on w.productid=p.productid";
                string query = @"select p.*, w.wishlist_id, w.customer_id, 
                (case when w.wishlist_id is not null then 1 else 0 end) as IsInWishlist
            from Supply.Products p
            left join crm.wishlist w on p.productid = w.productid and w.customer_id = @customer_id";
                SqlCommand
                    
                   cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@customer_id", userId);
                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DisplayProduct wish = new DisplayProduct
                    {
                        productid = reader["productid"] != DBNull.Value ? Convert.ToInt32(reader["productid"]) : 0,
                        ProductName = reader["ProductName"] != DBNull.Value ? reader["ProductName"].ToString() : string.Empty,
                        Price = reader["Price"] != DBNull.Value ? Convert.ToDecimal(reader["Price"]) : 0m,
                        ProductRatings = reader["ProductRatings"] != DBNull.Value ? Convert.ToInt32(reader["ProductRatings"]) : 0,

                        wishlist_id = reader["wishlist_id"] != DBNull.Value ? Convert.ToInt32(reader["wishlist_id"]) : 0,
                        customer_id = reader["customer_id"] != DBNull.Value ? Convert.ToInt32(reader["customer_id"]) : 0,

                        IsInWishlist = reader["IsInWishlist"] != DBNull.Value ? Convert.ToBoolean(reader["IsInWishlist"]) : false
                    };
                    wishlists.Add(wish);
                }
                cn.Close();
            }
            return wishlists;
        }
       /* public WishList CheckWishlist(int userId, int productId,int wishId)
        {
            WishList wish = null;
            using (SqlConnection cn = _dbHelper.GetConnection())
            {

                string query = "SELECT * FROM crm.wishlist WHERE wishlist_id = @wishId AND customer_id = @customer_id AND productid = @productid";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@wishId", wishId);
                cmd.Parameters.AddWithValue("@customer_id", userId);
                cmd.Parameters.AddWithValue("@productid", productId);
                cn.Open();
               SqlDataReader rd= cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    rd.Read();
                    wish= new WishList
                    {
                        wishlist_id = Convert.ToInt32(rd["wishlist_id"]),
                        customer_id = Convert.ToInt32(rd["customer_id"]),
                        productid = Convert.ToInt32(rd["productid"])
                        //added_at = Convert.ToDateTime(rd["added_at"])
                    };
                }
                cn.Close();
                return wish;
            }
        }*/


        public List<DisplayProduct> GetAllProducts()
        {
            List<DisplayProduct> products = new List<DisplayProduct>();

            using (SqlConnection con = _dbHelper.GetConnection())
            {
                string query = "SELECT * FROM Supply.Products";
                SqlCommand cmd = new SqlCommand(query, con);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    /*products.Add(new Product
                    {
                        productid = Convert.ToInt32(reader["productid"]),
                        ProductName = reader["ProductName"].ToString(),
                        Price = Convert.ToDecimal(reader["Price"]),
                        ProductRatings = Convert.ToInt32(reader["ProductRatings"])
                    });*/
                    products.Add(new DisplayProduct
                    {
                        productid = reader["productid"] != DBNull.Value ? Convert.ToInt32(reader["productid"]) : 0,
                        ProductName = reader["ProductName"] != DBNull.Value ? reader["ProductName"].ToString() : string.Empty,
                        Price = reader["Price"] != DBNull.Value ? Convert.ToDecimal(reader["Price"]) : 0m,
                        ProductRatings = reader["ProductRatings"] != DBNull.Value ? Convert.ToInt32(reader["ProductRatings"]) : 0
                    });
                }
                con.Close();
            }
            return products;
        }

        public List<DisplayProduct> GetRelativeProducts(string category, string designId,string fabricId,string weaveId)
        {
            List<DisplayProduct> products = new List<DisplayProduct>();

            using (SqlConnection con = _dbHelper.GetConnection())
            {
               
                SqlCommand cmd = new SqlCommand("Supply.sp_AllRelativeProd", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@category", category);
                cmd.Parameters.AddWithValue("@design", designId);
                cmd.Parameters.AddWithValue("@fabric", fabricId);
                cmd.Parameters.AddWithValue("@weave", weaveId);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    /*products.Add(new Product
                    {
                        productid = Convert.ToInt32(reader["productid"]),
                        ProductName = reader["ProductName"].ToString(),
                        Price = Convert.ToDecimal(reader["Price"]),
                        ProductRatings = Convert.ToInt32(reader["ProductRatings"])
                    });*/
                    products.Add(new DisplayProduct
                    {
                        productid = reader["productid"] != DBNull.Value ? Convert.ToInt32(reader["productid"]) : 0,
                        ProductName = reader["ProductName"] != DBNull.Value ? reader["ProductName"].ToString() : string.Empty,
                        Price = reader["Price"] != DBNull.Value ? Convert.ToDecimal(reader["Price"]) : 0m,
                        ProductRatings = reader["ProductRatings"] != DBNull.Value ? Convert.ToInt32(reader["ProductRatings"]) : 0,
                        ProductImages = GetImagesByProductId(Convert.ToInt32(reader["productid"]))
                    });
                }
                con.Close();
            }
            return products;
        }

        public List<ProductImage> GetImagesByProductId(int productId)
        {
            List<ProductImage> images = new List<ProductImage>();

            using (SqlConnection con = _dbHelper.GetConnection())
            {
                string query = "SELECT * FROM Supply.ProductImage WHERE productid = @productid";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@productid", productId);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    images.Add(new ProductImage
                    {
                        Imageid = Convert.ToInt32(reader["ImageId"]),
                        productid = Convert.ToInt32(reader["productid"]),
                        ImagePath = reader["ImagePath"].ToString(),
                        //contenttype = reader["contenttype"].ToString()
                    });
                }
                con.Close();
            }
            return images;
        }

        public Customers GetSessionDataByEmail(string email)
        {
            Customers customer = null;
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                string query = "SELECT * FROM crm.customers WHERE email = @Email";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", email);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    customer = new Customers
                    {
                        customer_id = Convert.ToInt32(reader["customer_id"]),
                        email = reader["email"].ToString()
                        /*firstName = reader["firstName"].ToString(),
                        lastName = reader["lastName"].ToString(),
                        ,
                        phone = reader["phone"].ToString(),
                        password = reader["password"].ToString()*/
                    };
                }
                con.Close();

                return customer; // Login successful if count > 0
            }
        }

      
       




        public bool Login(string email, string password)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                string query = "SELECT count(*) FROM crm.customers WHERE email = @Email AND password = @Password";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                con.Open();
                int count = (int)cmd.ExecuteScalar(); // Returns the number of matching rows
                con.Close();

                return count > 0; 
            }
        }

        public bool ChangePassword(string email, string oldPassword, string newPassword)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                // Check old password
                string checkQuery = "SELECT count(*) FROM crm.customers WHERE email = @Email AND password = @OldPassword";
                SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                checkCmd.Parameters.AddWithValue("@Email", email);
                checkCmd.Parameters.AddWithValue("@OldPassword", oldPassword);

                con.Open();
                int count = (int)checkCmd.ExecuteScalar();
                if (count == 0)
                {
                    con.Close();
                    return false; // Old password does not match
                }

                // Update new password
                string updateQuery = "UPDATE crm.customers SET password = @NewPassword WHERE email = @Email";
                SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                updateCmd.Parameters.AddWithValue("@NewPassword", newPassword);
                updateCmd.Parameters.AddWithValue("@Email", email);

                int rows = updateCmd.ExecuteNonQuery();
                con.Close();

                return rows > 0;
            }
        }
        public bool UpdateProfile(Customers customer)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                string query = @"UPDATE crm.customers 
                         SET firstName = @firstName, lastName = @lastName, phone = @phone 
                         WHERE email = @Email";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@firstName", customer.firstName);
                cmd.Parameters.AddWithValue("@lastName", customer.lastName);
                cmd.Parameters.AddWithValue("@phone", customer.phone);
                cmd.Parameters.AddWithValue("@Email", customer.email);

                con.Open();
                int rows = cmd.ExecuteNonQuery();
                con.Close();

                return rows > 0;
            }
        }

        public DisplayProduct ProductById(int id)
        {
            DisplayProduct products = null;
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                //string query = "SELECT productid, ProductName, Price,ProductRatings FROM Supply.Products WHERE productid = @productid";
                SqlCommand cmd = new SqlCommand("Supply.sp_AllProdDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@prodId", id);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    products = new DisplayProduct
                    {
                        productid = Convert.ToInt32(reader["productid"]),
                        ProductName = reader["ProductName"].ToString(),
                        Price = Convert.ToDecimal(reader["Price"]),
                        ProductRatings = Convert.ToInt32(reader["ProductRatings"]),
                        category_name = reader["CategoryName"].ToString(),
                        FabricName = reader["FabricName"].ToString(),
                        DesignType = reader["DesignType"].ToString(),
                        weave_type = reader["weave_type"].ToString(),
                        ColorName = reader["ColorName"].ToString(),
                        ProductImages = GetImagesByProductId(Convert.ToInt32(reader["productid"]))

                    };
                    /*{
                        ImageId = Convert.ToInt32(reader["ImageId"]),
                        productid = Convert.ToInt32(reader["productid"]),
                        ImagePath = reader["ImagePath"].ToString(),
                        contenttype = reader["contenttype"].ToString()
                    });*/
                }
                //con.Close();
            }
            return products;
        }

        public List<DisplayProduct> CategoryById(int id)
        {
            List<DisplayProduct> products = new List<DisplayProduct>();
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                //string query = "select p.productid,p.ProductName,p.Price,c.CategoryName,p.ProductRatings  from Supply.Products p inner join Manufacture.Production po on p.ProductionID=po.ProductionID inner join Manufacture.ProductAttribute pa on po.productAttributeid=pa.productAttributeid inner join Supply.Categories c on pa.CategoryID=c.CategoryID where c.CategoryID = @catid";
                SqlCommand cmd = new SqlCommand("Supply.sp_CategoryWiseProd", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@categoryId", id);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new DisplayProduct
                    {
                        productid = Convert.ToInt32(reader["productid"]),
                        ProductName = reader["ProductName"].ToString(),
                        Price = Convert.ToDecimal(reader["Price"]),
                        category_name = reader["CategoryName"].ToString(),
                        ProductRatings = Convert.ToInt32(reader["ProductRatings"])

                    });
                }
                //con.Close();
            }
            return products;
        }
        public bool Register(Customers customer)
        {
            using(SqlConnection con = _dbHelper.GetConnection())
            {
                string checkQuery = "SELECT COUNT(1) FROM crm.customers WHERE email = @Email";
                SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                checkCmd.Parameters.AddWithValue("@Email", customer.email);

                con.Open();
                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    con.Close();
                    return false; // Email already exists
                }

                string query = "insert into crm.customers(firstName,lastName,email,phone,password) values(@firstName,@lastName,@Email,@phone,@password)";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@firstName", customer.firstName);
                cmd.Parameters.AddWithValue("@lastName", customer.lastName);
                cmd.Parameters.AddWithValue("@Email", customer.email);
                cmd.Parameters.AddWithValue("@phone", customer.phone);
                cmd.Parameters.AddWithValue("@password", customer.password);

                //con.Open();
                int rows = cmd.ExecuteNonQuery();
                con.Close();

                return rows > 0;
            }
        }
        //Cart
        public void AddItemToCart(int userId, int productId, int quantity)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("InsertOrUpdateCart", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void RemoveCartItem(int cartId)
        {
            using (SqlConnection conn =_dbHelper.GetConnection())
            {
                string query = "DELETE FROM crm.cart WHERE cartId = @CartId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CartId", cartId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public int GetCartItemCount(int userId)
        {
            int count = 0;
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT SUM(Quantity) FROM crm.cart WHERE customer_id = @UserId", conn);
                cmd.Parameters.AddWithValue("@UserId", userId);

                var result = cmd.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    count = Convert.ToInt32(result);
                }
            }
            return count;
        }
        public List<CartItemViewModel> GetCartItemsByUserId(int userId)
        {
            var items = new List<CartItemViewModel>();

            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                /* string query = @"SELECT c.cartId AS CartId, p.productid, p.ProductName, p.Price, pi.ImagePath, c.Quantity
                         FROM crm.cart c
                         JOIN supply.Products p ON c.ProductId = p.productid
                         INNER JOIN supply.ProductImage pi
                         ON p.productid=pi.productid
                         WHERE c.customer_id = @UserId";*/
                string query = @"
            SELECT 
                c.cartid AS CartId,
                p.productid,
                p.ProductName,
                p.Price,
                pi.ImagePath,
                c.Quantity
            FROM crm.cart c
            INNER JOIN supply.Products p ON c.productid = p.productid
            OUTER APPLY (
                SELECT TOP 1 ImagePath 
                FROM supply.ProductImage 
                WHERE productid = p.productid
            ) pi
            WHERE c.customer_id = @userId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(new CartItemViewModel
                            {
                                CartId = Convert.ToInt32(reader["CartId"]),
                                ProductId = Convert.ToInt32(reader["productid"]),
                                ProductName = reader["ProductName"].ToString(),
                                Price = Convert.ToDecimal(reader["Price"]),
                                ImagePath = reader["ImagePath"].ToString(),
                                Quantity = Convert.ToInt32(reader["Quantity"])
                            });
                        }
                    }
                }
            }

            return items;
        }
        public CartItemViewModel GetCartItemById(int cartId, int userId)
        {
            CartItemViewModel item = null;

            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT c.CartId, c.ProductId, c.Quantity, p.ProductName, p.Price, pi.ImagePath " +
                                                "FROM crm.cart c " +
                                                "INNER JOIN supply.Products p ON c.productid = p.productid " +
                                                "LEFT JOIN supply.ProductImage pi ON pi.productid = p.productid " +
                                                "WHERE c.CartId = @CartId AND c.customer_id = @CustomerId", conn);
                cmd.Parameters.AddWithValue("@CartId", cartId);
                cmd.Parameters.AddWithValue("@CustomerId", userId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        item = new CartItemViewModel
                        {
                            CartId = Convert.ToInt32(reader["CartId"]),
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            ProductName = reader["ProductName"].ToString(),
                            Price = Convert.ToDecimal(reader["Price"]),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            ImagePath = reader["ImagePath"] != DBNull.Value ? reader["ImagePath"].ToString() : null
                        };
                    }
                }
            }

            return item;
        }

        public void UpdateCartItemQuantity(int cartId, int quantity)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE crm.cart SET Quantity = @Quantity WHERE CartId = @CartId", conn);
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.Parameters.AddWithValue("@CartId", cartId);
                cmd.ExecuteNonQuery();
            }
        }
        //Category
        public IEnumerable<Category> Getcategories()
        {
            var categories = new List<Category>();

            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string query = "SELECT CategoryID, CategoryName FROM Supply.Categories";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    categories.Add(new Category
                    {
                        CategoryId = Convert.ToInt32(reader["CategoryID"]),
                        CategoryName = reader["CategoryName"].ToString()
                    });
                }
            }
          
            return categories;
        }

      
       /* public bool PlaceOrder(int userId)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sales.PlaceOrderWithItems", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CustomerId", userId);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Order error: " + ex.Message);
                        return false;
                    }
                }
            }
        }*/

        public bool PlaceOrder(int userId, string address)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sales.PlaceOrderWithItemsAndAddress", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CustomerId", userId);
                    cmd.Parameters.AddWithValue("@DeliveryAddress", address);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Order error: " + ex.Message);
                        return false;
                    }
                }
            }
        }
        public int SaveCustomerAddress(DeliveryAddress orderAddress)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand("crm.sp_InsertCustomerAddress", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CustomerID", orderAddress.customer_id);
                cmd.Parameters.AddWithValue("@HouseNo", orderAddress.houseNo);
                cmd.Parameters.AddWithValue("@Street", orderAddress.street);
                cmd.Parameters.AddWithValue("@CityID", orderAddress.cityID);
                cmd.Parameters.AddWithValue("@StateID", orderAddress.street);
                cmd.Parameters.AddWithValue("@CountryID", orderAddress.CountryID);
                cmd.Parameters.AddWithValue("@Pincode", orderAddress.pincode);

                SqlParameter outputParam = new SqlParameter("@NewAddressID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);

                con.Open();
                cmd.ExecuteNonQuery();

                return outputParam.Value != DBNull.Value ? Convert.ToInt32(outputParam.Value) : 0;
            }
        }
        public void SaveAddress(AddressModel address, int customerId)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                string query = @"
            INSERT INTO crm.Address_details
            (customer_id, houseNo, street, cityID, StateID, CountryID, pincode)
            VALUES
            (@CustomerId, @HouseNo, @Street, @CityID, @StateID, @CountryID, @Pincode)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerId", customerId);
                    cmd.Parameters.AddWithValue("@HouseNo", address.HouseNo);
                    cmd.Parameters.AddWithValue("@Street", address.Street);
                    cmd.Parameters.AddWithValue("@CityID", address.CityID);
                    cmd.Parameters.AddWithValue("@StateID", address.StateID);
                    cmd.Parameters.AddWithValue("@CountryID", address.CountryID);
                    cmd.Parameters.AddWithValue("@Pincode", address.Pincode);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
       
    }
}
