using ERP.Models;
using ERP.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ICustomersRepositories _repository;

        public ProductsController(ICustomersRepositories repository)
        {
            _repository = repository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AllProducts()
        {
            return View();
        }
        [Route("Products/ProductDetails/{id}")]
        public IActionResult ProductsDetails(int id)
        {
            DisplayProduct product = _repository.ProductById(id);

            if (product == null)
            {
                //TempData["StockError"] = "Product not found.";
                return RedirectToAction("Index", "Home");
            }
            
            
            // Load related images for the single product
            List<DisplayProduct> relativeProducts = _repository.GetRelativeProducts(
                product.category_name,
                product.DesignType,
                product.FabricName,
                product.weave_type
            );

            ViewBag.RelativeProducts = relativeProducts;

            return View(product);
        }
        public IActionResult Search()
        {
            return View();
        }
       
    }
}
