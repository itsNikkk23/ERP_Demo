using ERP.Data;
using Microsoft.AspNetCore.Mvc;
using ERP.Models;
using ERP.Repositories;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Data.SqlClient;
using System.Reflection;
using Razorpay.Api;
using System.Net;


namespace ERP.Controllers
{
    public class UsersController : Controller
    {
        private readonly ICustomersRepositories _repository;

        public UsersController(ICustomersRepositories repository)
        {
            _repository = repository;
        }
        //Register
        public IActionResult Index()
        {
            return View(new Customers());
        }
        [HttpPost]
        public IActionResult Index(Customers customer)
        {
            
            bool result = _repository.Register(customer);
                if (result)
                {
                int cust = customer.customer_id;
                HttpContext.Session.SetString("Email", customer.email);
                HttpContext.Session.SetString("customerid", cust.ToString());
                return RedirectToAction("Index","Home");
                }
            TempData["UniqEmail"] = "This email is already registered.";
            //return View("Index");
            return RedirectToAction("Login","Users");
        }
        //Login
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string email,string password)
        {
            if (_repository.Login(email, password))
            {
                var customer = _repository.GetSessionDataByEmail(email);
                
                int cust = customer.customer_id;
                HttpContext.Session.SetString("Email", email);
                HttpContext.Session.SetString("customerid", cust.ToString());
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["Error"] = "Invalid username or password.";
                //ViewBag.ErrorMessage = "Invalid username or password.";
                return RedirectToAction("Login","Users");
            }
        }

        public IActionResult EditProfile()
        {
            string email = HttpContext.Session.GetString("Email");
            var customer = _repository.GetSessionDataByEmail(email);
            return View(customer);
        }
        [HttpPost]
        public IActionResult EditProfile(Customers customer)
        {
            if (ModelState.IsValid)
            {
                bool updated = _repository.UpdateProfile(customer);
                if (updated)
                {
                    ViewBag.Message = "Profile updated successfully.";
                }
            }
            return View(customer);
        }
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(string oldPassword, string newPassword)
        {
            string email = HttpContext.Session.GetString("Email");
            bool result = _repository.ChangePassword(email, oldPassword, newPassword);
            if (result)
            {
                ViewBag.Message = "Password changed successfully.";
            }
            else
            {
                ViewBag.Message = "Old password incorrect.";
            }
            return View();
        }

        public ActionResult AddToWishlist()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddToWishlist(int productId)
        {
            //int uid = Convert.ToInt32(HttpContext.Session.GetString("user_id"));
            var uid = HttpContext.Session.GetString("customerid");
            if (string.IsNullOrEmpty(uid))
            {
                //TempData["Error"] = "Please log in First to add Wishlist!!";
                return RedirectToAction("Login", "Users");
            }
            int customerIdInt = Convert.ToInt32(uid);
            bool isInwishlist = _repository.IsInWishlist(productId, customerIdInt);

            //bool isAdded = _repository.AddToWishlist(customerIdInt, productId);
            //ViewBag.IsAdded = isAdded;
            if (isInwishlist)
            {
                _repository.RemoveFromWishlist(customerIdInt, productId);
            }
            else
            {
                _repository.AddToWishlist(customerIdInt, productId);
            }
            return RedirectToAction("Index","Home"); // Redirect back to product listing
        }
      
        
       
        public IActionResult WishList()
        {
            List<DisplayProduct> wishList = null;
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("customerid")))
            {
                //TempData["Error"] = "Please log in First to add Wishlist!!";
                return RedirectToAction("Login", "Users");
            }
            int customerIdInt = Convert.ToInt32(HttpContext.Session.GetString("customerid"));
             wishList = _repository.GetWishlist(customerIdInt);
            foreach (var product in wishList)
            {
                product.ProductImages = _repository.GetImagesByProductId(product.productid);
            }
            return View(wishList);
        }

       
        public IActionResult BuyNow()
        {
            return View();
        }

        /*[HttpPost]
        public IActionResult BuyNow(int pid, int quantity)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("customerid")))
                return RedirectToAction("Login", "Users");

            int custId = Convert.ToInt32(HttpContext.Session.GetString("customerid"));
            var product = _repository.ProductById(pid);
            int stock = _repository.GetAvailableStock(pid);

            if (stock < quantity)
            {
                TempData["stockError"] = "Not enough stock available!";
                return RedirectToAction("ProductDetails", "Products", new { id = pid });
            }

            decimal totalAmount = quantity * product.Price;

            // Razorpay Setup
            string key = "rzp_test_ff2f2hHJPr8s6m";
            string secret = "LnpNKv31vq47pbOwbmZ5BmgR";
            Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient(key, secret);

            Dictionary<string, object> options = new Dictionary<string, object>
    {
        { "amount", (int)(totalAmount * 100) },  // in paise
        { "currency", "INR" },
         { "receipt", "rcpt_" + Guid.NewGuid().ToString("N").Substring(0, 30) },
        { "payment_capture", 1 }
    };

            Razorpay.Api.Order razorOrder = client.Order.Create(options);

            // Send data to RazorpayCheckout view
            ViewBag.RazorpayOrderId = razorOrder["id"].ToString();
          
            ViewBag.RazorpayKey = key;
            ViewBag.Amount = totalAmount;
            ViewBag.CustomerName = "Demo User";
            ViewBag.Email = "demo@email.com";
            ViewBag.Phone = "9999999999";

            ViewBag.ProductId = pid;
            ViewBag.Quantity = quantity;
            ViewBag.OrderAmount = totalAmount;

            return View("RazorpayCheckout");
        }*/

        [HttpPost]
        public IActionResult BuyNow(int pid, int quantity)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("customerid")))
                return RedirectToAction("Login", "Users");

            int stock = _repository.GetAvailableStock(pid);
            if (stock < quantity)
            {
                TempData["stockError"] = "Not enough stock available!";
                return RedirectToAction("ProductDetails", "Products", new { id = pid });
            }

            // Store in Session instead of TempData
            HttpContext.Session.SetInt32("pid", pid);
            HttpContext.Session.SetInt32("quantity", quantity);

            return RedirectToAction("EnterAddress");
        }

        [HttpGet]
        public IActionResult EnterAddress()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("customerid")))
                return RedirectToAction("Login", "Users");

            return View();
        }

        [HttpPost]
        public IActionResult SubmitAddress(AddressModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Address");

            int custId = Convert.ToInt32(HttpContext.Session.GetString("customerid"));
            int pid = HttpContext.Session.GetInt32("pid") ?? 0;
            int quantity = HttpContext.Session.GetInt32("quantity") ?? 0;

            var product = _repository.ProductById(pid);
            decimal totalAmount = quantity * product.Price;

            _repository.SaveAddress(model, custId);

            var order = new Orders
            {
                customer_id = custId,
                TotalAmount = totalAmount,
                Status = "Pending"
            };

            var items = new List<Order_Item>
    {
        new Order_Item
        {
            productid = pid,
            Quantity = quantity,
            Price = product.Price
        }
    };

            int orderId = _repository.CreateOrder(order, items);

            string key = "rzp_test_ff2f2hHJPr8s6m";
            string secret = "LnpNKv31vq47pbOwbmZ5BmgR";
            Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient(key, secret);

            Dictionary<string, object> options = new Dictionary<string, object>
    {
        { "amount", (int)(totalAmount * 100) },
        { "currency", "INR" },
        { "receipt", "rcpt_" + orderId },
        { "payment_capture", 1 }
    };

            Razorpay.Api.Order razorOrder = client.Order.Create(options);

            return Json(new
            {
                razorpayOrderId = razorOrder["id"].ToString(),
                razorpayKey = key,
                amount = totalAmount,
                orderId = orderId,
                pid = pid,
                quantity = quantity
            });
        }


        public IActionResult PaymentSuccess(string razorpay_payment_id, string razorpay_order_id, string razorpay_signature, int orderId, int pid, int quantity)
        {
            try
            {
                Dictionary<string, string> attributes = new Dictionary<string, string>
        {
            { "razorpay_payment_id", razorpay_payment_id },
            { "razorpay_order_id", razorpay_order_id },
            { "razorpay_signature", razorpay_signature }
        };

                Razorpay.Api.Utils.verifyPaymentSignature(attributes);

                decimal amount = _repository.GetOrderAmount(orderId);

                // Save payment details in the database
                var payment = new PaymentDetails
                {
                    OrderId = orderId,
                    RazorpayOrderId = razorpay_order_id,
                    RazorpayPaymentId = razorpay_payment_id,
                    RazorpaySignature = razorpay_signature,
                    Status = "Success",
                    Amount = amount,
                    PaymentDate = DateTime.Now
                };
                _repository.SavePaymentDetails(payment);

                // Update order status to "Paid"
                _repository.UpdateOrderStatus(orderId, "Paid");

                ViewBag.PaymentId = razorpay_payment_id;
                ViewBag.Amount = amount;
                ViewBag.OrderId = orderId;

                return View("~/Views/Users/PaymentSuccess.cshtml");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Payment verification failed: " + ex.Message;
                return RedirectToAction("PaymentFailed", "Users", new { orderId });
            }
        }

        public IActionResult PaymentFailed(int orderId)
        {
            decimal amount = _repository.GetOrderAmount(orderId);

            var payment = new PaymentDetails
            {
                OrderId = orderId,
                RazorpayOrderId = "",
                RazorpayPaymentId = "",
                RazorpaySignature = "",
                Status = "Fail",
                Amount = amount,
                PaymentDate = DateTime.Now
            };
            _repository.SavePaymentDetails(payment);
            _repository.UpdateOrderStatus(orderId, "Failed");

            return View("~/Views/Users/PaymentFailed.cshtml");
        }



        /* [HttpGet]
         public IActionResult PaymentSuccess(string razorpay_payment_id, string razorpay_order_id, string razorpay_signature, int pid, int quantity)
         {
             try
             {
                 // Razorpay Signature Validation
                 Dictionary<string, string> attributes = new Dictionary<string, string>
         {
             { "razorpay_payment_id", razorpay_payment_id },
             { "razorpay_order_id", razorpay_order_id },
             { "razorpay_signature", razorpay_signature }
         };

                 Razorpay.Api.Utils.verifyPaymentSignature(attributes); // Corrected

                 // If valid, create order
                 int custId = Convert.ToInt32(HttpContext.Session.GetString("customerid"));
                 var product = _repository.ProductById(pid);
                 decimal totalAmount = quantity * product.Price;

                 var order = new Orders
                 {
                     customer_id = custId,
                     TotalAmount = totalAmount,
                     Status = "Success"
                 };

                 var items = new List<Order_Item>
         {
             new Order_Item
             {
                 productid = product.productid,
                 Quantity = quantity,
                 Price = product.Price
             }
         };

                 int newOrderId = _repository.CreateOrder(order, items);

                 _repository.SavePaymentDetails(new PaymentDetails
                 {
                     OrderId = newOrderId,
                     RazorpayOrderId = razorpay_order_id,
                     RazorpayPaymentId = razorpay_payment_id,
                     RazorpaySignature = razorpay_signature,
                     Status = "Success",
                     Amount = totalAmount,
                     PaymentDate = DateTime.Now
                 });

                 ViewBag.PaymentId = razorpay_payment_id;
                 ViewBag.Amount = totalAmount;
                 ViewBag.OrderId = order.OrderId;

                 return View("PaymentSuccess");
             }
             catch (Exception ex)
             {
                 TempData["Error"] = "Payment verification failed: " + ex.Message;
                 return RedirectToAction("PaymentFailed");
             }
         }
         */

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Users");
        }
        /* public IActionResult AddToCart(Product product)
       {
           if (Session["cart"] == null)
           {
               List<Product> li = new List<Product>();
               li.Add(product);
               Session["cart"] = li;
               ViewBag.cart = li.Count();
               Session["count"] = 1;
           }
           else
           {
               List<Product> li = (List<Product>)Session["cart"];
               li.Add(product);
               Session["cart"] = li;
               ViewBag.cart = li.Count();
               Session["count"] = Convert.ToInt32(Session["count"]) + 1;
           }
           return RedirectToAction("Index", "Home");
       }*/
        // [HttpPost]
    }
}
