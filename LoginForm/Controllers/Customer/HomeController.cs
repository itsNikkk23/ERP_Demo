using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ERP.Models;
using ERP.Repositories;
using Microsoft.AspNetCore.Hosting.Server;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace ERP.Controllers;

public class HomeController : Controller
{
    private readonly ICustomersRepositories _repository;

    public HomeController(ICustomersRepositories repository)
    {
        _repository = repository;
    }

    public IActionResult Index()
    {
        int userId = Convert.ToInt32(HttpContext.Session.GetString("customerid"));
        List<DisplayProduct> products;
        if (userId == 0)
        {
            products = _repository.GetAllProducts();
            //return RedirectToAction("Login", "Users");
        }

         products = _repository.CheckWishlist(userId);
        var cartItems = _repository.GetCartItemsByUserId(userId);

        foreach (var product in products)
        {
            product.ProductImages = _repository.GetImagesByProductId(product.productid);
        }

        var viewModel = new ShopViewModel
        {
            Products = products,
            CartItems = cartItems
        };

        return View(viewModel);
    }
    //fing categories
    public IActionResult GetCategories()
    {
        var categories = _repository.Getcategories();
        Console.WriteLine("Fetched categories count: " + categories.Count());
        return PartialView("~/Views/Home/CategoryPartial.cshtml", categories);
        
    }
    //Categpry Wise
    public ActionResult CategoryWiseProduct(int id)
    {
        var products = _repository.CategoryById(id);
      /*  foreach (var product in products)
        {
            product.ProductImages = _repository.GetImagesByProductId(product.productid);
        }*/
        return View(products);
    }
    // Add Product Page
    public ActionResult AddProduct()
    {
      
        return View();
    }

    [HttpPost]
    public ActionResult AddProduct(DisplayProduct product, List<IFormFile> images)
    {
        if (ModelState.IsValid)
        {
            int productId = _repository.AddProduct(product);

            if (images != null && images.Count > 0)
            {
                foreach (var image in images)
                {
                    if (image.Length > 0)
                    {
                        string fileName = Path.GetFileName(image.FileName);
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                             image.CopyTo(stream);
                        }

                        ProductImage img = new ProductImage
                        {
                            productid = productId,
                            ImagePath = "/uploads/" + fileName,
                            //contenttype = image.ContentType
                        };

                        _repository.AddProductImage(img);
                    }
                }
            }
            return RedirectToAction("Index","Home");
        }
        return View(product);
    }
    /* public IActionResult Privacy()
     {
         return View();
     }*/

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
