using ERP.Models;
using ERP.Repositories;
using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;
using System.Security.Cryptography;
using System.Text;

namespace ERP.Controllers
{
    public class CartController : Controller
    {
        private readonly ICustomersRepositories _cartrepository;
        private const string RazorpayKey = "rzp_test_ff2f2hHJPr8s6m";
        private const string RazorpaySecret = "LnpNKv31vq47pbOwbmZ5BmgR";
        public CartController(ICustomersRepositories cartrepository)
        {
            _cartrepository = cartrepository;
        }
        /*[HttpPost]
        public IActionResult CreateOrder(decimal amount)
        {
            RazorpayClient client = new RazorpayClient(RazorpayKey, RazorpaySecret);

            Dictionary<string, object> options = new Dictionary<string, object>
    {
        { "amount", (int)(amount * 100) }, // Convert to paise
        { "currency", "INR" },
        { "receipt", "order_rcptid_" + Guid.NewGuid() },
        { "payment_capture", 1 }
    };

            Order order = client.Order.Create(options);

            ViewBag.RazorpayOrderId = order["id"].ToString();
            ViewBag.Amount = amount;
            ViewBag.RazorpayKey = RazorpayKey;

            return View("PaymentPage");
        }*/
        [HttpPost]
        public IActionResult ShowAddressForm()
        {
            return PartialView("_DeliveryAddressForm");
        }
        [HttpPost]
        public JsonResult CreateRazorpayOrderWithAddress([FromBody] RazorAmountDto model)
        {
           // HttpContext.Session.SetString("DeliveryAddress", model.Address);

            RazorpayClient client = new RazorpayClient("rzp_test_ff2f2hHJPr8s6m", "LnpNKv31vq47pbOwbmZ5BmgR");

            Dictionary<string, object> options = new Dictionary<string, object>
    {
        { "amount", model.Amount * 100 },
        { "currency", "INR" },
        { "receipt", "receipt_cart_" + DateTime.Now.Ticks },
        { "payment_capture", 1 }
    };

            var order = client.Order.Create(options);
            return Json(new
            {
                orderId = order["id"].ToString(),
                amount = order["amount"]
            });
        }
        public IActionResult ConfirmPayment(string paymentId, string orderId)
        {
            int userId = Convert.ToInt32(HttpContext.Session.GetString("customerid"));
            string address = HttpContext.Session.GetString("DeliveryAddress");

            string actualSignature = Request.Query["razorpay_signature"];
            string payload = orderId + "|" + paymentId;

            string generatedSignature;
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes("LnpNKv31vq47pbOwbmZ5BmgR")))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
                generatedSignature = BitConverter.ToString(hash).Replace("-", "").ToLower();
            }

            if (generatedSignature != actualSignature)
            {
                TempData["Error"] = "Payment signature mismatch!";
                return RedirectToAction("Index", "Home");
            }

            var success = _cartrepository.PlaceOrder(userId, address);
            if (success)
            {
                TempData["Success"] = "Order placed successfully!";
                return RedirectToAction("OrderConfirmation");
            }

            TempData["Error"] = "Order could not be placed.";
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Index()
        {
            List<DisplayProduct> products;
            int userId = Convert.ToInt32(HttpContext.Session.GetString("customerid"));
            if (userId == 0)
            {
                return RedirectToAction("Login", "Users");
            }

            products = _cartrepository.CheckWishlist(userId);
            var cartItems = _cartrepository.GetCartItemsByUserId(userId);
            foreach (var product in products)
            {
                product.ProductImages = _cartrepository.GetImagesByProductId(product.productid);
            }
            //ViewBag.CartItemCount = _cartrepository.GetCartItemCount(userId);
            //TempData["CartItemCount"] = _cartrepository.GetCartItemCount(userId);
            return View(cartItems);
        }
       
       
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            int userId = Convert.ToInt32(HttpContext.Session.GetString("customerid"));
            if (userId == 0)
            {
                return RedirectToAction("Login", "Users");
            }

            _cartrepository.AddItemToCart(userId, productId, quantity);
            TempData["ShowCartSidebar"] = true;
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public IActionResult UpdateQuantity(int cartId, string operation)
        {
            int userId = Convert.ToInt32(HttpContext.Session.GetString("customerid"));
            if (userId == 0)
            {
                return RedirectToAction("Login", "Users");
            }

            var cartItem = _cartrepository.GetCartItemById(cartId, userId);
            if (cartItem != null)
            {
                if (operation == "increase")
                    cartItem.Quantity += 1;
                else if (operation == "decrease" && cartItem.Quantity > 1)
                    cartItem.Quantity -= 1;

                _cartrepository.UpdateCartItemQuantity(cartItem.CartId, cartItem.Quantity);
            }

            return RedirectToAction("Index","Home");
        }
        [HttpPost]
        public JsonResult CreateRazorpayOrder([FromBody] RazorAmountDto model)
        {
            RazorpayClient client = new RazorpayClient("rzp_test_ff2f2hHJPr8s6m", "LnpNKv31vq47pbOwbmZ5BmgR");

            Dictionary<string, object> options = new Dictionary<string, object>
    {
        { "amount", model.Amount * 100 }, // paise
        { "currency", "INR" },
        { "receipt", "receipt_cart_" + DateTime.Now.Ticks },
        { "payment_capture", 1 }
    };

            var order = client.Order.Create(options);
            return Json(new
            {
                orderId = order["id"].ToString(),
                amount = order["amount"]
            });
        }

        public class RazorAmountDto
        {
            public decimal Amount { get; set; }
        }


        /*[HttpPost]
        public IActionResult PlaceOrderCart()
        {
            int userId = Convert.ToInt32(HttpContext.Session.GetString("customerid"));
            var cartItems = _cartrepository.GetCartItemsByUserId(userId);
            var totalAmount = cartItems.Sum(item => item.Quantity * item.Price); // Assuming `Price` is available

            RazorpayClient client = new RazorpayClient("rzp_test_ff2f2hHJPr8s6m", "LnpNKv31vq47pbOwbmZ5BmgR");

            Dictionary<string, object> options = new Dictionary<string, object>
    {
        { "amount", totalAmount * 100 }, // in paise
        { "currency", "INR" },
        { "receipt", "order_rcptid_" + userId },
        { "payment_capture", 1 }
    };

            Order order = client.Order.Create(options);

            ViewBag.RazorpayOrderId = order["id"].ToString();
            ViewBag.Amount = totalAmount * 100;
            ViewBag.RazorpayKey = "rzp_test_ff2f2hHJPr8s6m";
            ViewBag.UserName = "Test User"; // Optional
            ViewBag.Email = "test@example.com"; // Optional
            ViewBag.Contact = "9999999999"; // Optional

            return View("Cart");
        }*/

        /*  public IActionResult PlaceOrderCart()
          {
              int userId = Convert.ToInt32(HttpContext.Session.GetString("customerid"));
              var cartItems = _cartrepository.GetCartItemsByUserId(userId);
              var totalAmount = cartItems.Sum(item => item.Quantity * item.Price); // Ensure `Price` is in `CartItem`

              RazorpayClient client = new RazorpayClient(RazorpayKey, RazorpaySecret);
              Dictionary<string, object> options = new Dictionary<string, object>
              {
                  { "amount", totalAmount * 100 }, // amount in paise
                  { "currency", "INR" },
                  { "receipt", "order_rcptid_" + userId },
                  { "payment_capture", 1 }
              };

              Order order = client.Order.Create(options);

              TempData["RazorpayOrderId"] = order["id"].ToString();
              TempData["Amount"] = totalAmount;

              return View("PaymentPage");
          }*/

        /*  public IActionResult ConfirmPayment(string paymentId, string orderId)
          {
              int userId = Convert.ToInt32(HttpContext.Session.GetString("customerid"));

              // OPTIONAL: Add server-side verification with Razorpay API

              var success = _cartrepository.PlaceOrder(userId);
              if (success)
              {
                  TempData["Success"] = "Order placed successfully!";
                  return RedirectToAction("OrderConfirmation");
              }

              TempData["Error"] = "Order could not be placed.";
              return RedirectToAction("Index", "Home");
          }*/
        /*[HttpPost]
        public IActionResult PlaceOrderCart()
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("customerid"));
                var success = _cartrepository.PlaceOrder(userId);

                if (success)
                {
                    TempData["Success"] = "Order placed successfully!";
                    return RedirectToAction("OrderConfirmation");
                }
                else
                {
                    TempData["Error"] = "Order could not be placed.";
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message; // Insufficient stock
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Unexpected error: {ex.Message}";
                return RedirectToAction("Index","Home");
            }
        }*/

        [HttpPost]
        public IActionResult Remove(int cartId)
        {
            _cartrepository.RemoveCartItem(cartId);
            return RedirectToAction("Index","Home");
        }
    }
}
