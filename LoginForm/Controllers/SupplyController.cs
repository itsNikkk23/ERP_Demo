using Microsoft.AspNetCore.Mvc;
using ERP.Repository;
using System.Collections.Generic;
using ERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ERP.Controllers
{
    // Use conventional attribute routing with token replacement for simplicity.
    [Route("[controller]/[action]")]
    public class SupplyController : Controller
    {
        private readonly IERPRepository _repository;

        public SupplyController(IERPRepository repository)
        {
            _repository = repository;
        }

        // Default index page
        //[HttpGet]
        //public IActionResult Index()
        //{
        //    return View();
        //}

        // ------------------ Suppliers ------------------

        [HttpGet]
        public IActionResult Suppliers()
        {
            List<Suppliers> suppliersList = _repository.GetAllSuppliers();
            return View(suppliersList);
        }

        [HttpGet]
        public IActionResult AddSuppliers()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddSuppliers(string SupplierName, string SupplierContact)
        {
            _repository.InsertSuppliers(SupplierName, SupplierContact);
            return RedirectToAction("Suppliers");
        }

        [HttpGet]
        public IActionResult EditSuppliers(int id)
        {
            Suppliers supplier = _repository.GetSupplierById(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        [HttpPost]
        public IActionResult EditSuppliers(Suppliers model)
        {
            if (ModelState.IsValid)
            {
                _repository.UpdateSuppliers(model.SupplierID, model.SupplierName, model.SupplierContact);
                return RedirectToAction("Suppliers");
            }
            return View(model);
        }

        [HttpPost("{id}")]
        public IActionResult DeleteSuppliers(int id)
        {
            _repository.DeleteSuppliers(id);
            return RedirectToAction("Suppliers");
        }

        // ------------------ Categories ------------------

        [HttpGet]
        public IActionResult DisplayCategory()
        {
            List<Categories> categoriesList = _repository.GetCategory();
            return View(categoriesList);
        }

        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCategory(string CategoryName, string Description)
        {
            _repository.AddCategory(CategoryName, Description);
            return RedirectToAction("DisplayCategory");
        }

        [HttpGet("{id}")]
        public IActionResult EditCategory(int id)
        {
            Categories category = _repository.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult EditCategory(Categories model)
        {
            if (ModelState.IsValid)
            {
                _repository.UpdateCategory(model.CategoryID, model.CategoryName, model.Description);
                return RedirectToAction("DisplayCategory");
            }
            return View(model);
        }

        [HttpPost("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            _repository.DeleteCategory(id);
            return RedirectToAction("DisplayCategory");
        }

        // ------------------ Products ------------------
        [HttpGet]
        public IActionResult AddImage()
        {
            ViewBag.Products = _repository.GetProducts();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddImage(ProductImage model, IFormFile ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                // Ensure upload directory exists
                string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Image");
                if (!Directory.Exists(uploadDir))
                    Directory.CreateDirectory(uploadDir);

                // Save file
                string fileName = Path.GetFileName(ImageFile.FileName);
                string filePath = Path.Combine(uploadDir, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                // Relative path to store in DB
                string imagePath = "/Image/" + fileName;

                // ✅ Save using your repository method
                _repository.AddProductImage(model.productid, imagePath);
            }
            // Save model to database here...

            return RedirectToAction("ViewImage");
        }
        [HttpGet]
        public IActionResult ViewImage()
        {
            List<ProductImage> images = _repository.GetProductImages();
            return View(images);
        }
        [HttpGet]
        public IActionResult EditImage(int id)
        {
            var image = _repository.GetProductImageById(id);
            if (image == null)
            {
                return NotFound();
            }

            ViewBag.Products = _repository.GetProducts(); // So you can rebind dropdown
            return View(image);
        }
        [HttpPost]
        public async Task<IActionResult> EditImage(ProductImage model, IFormFile NewImageFile)
        {
            if (NewImageFile != null && NewImageFile.Length > 0)
            {
                // Define image path
                string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Image");
                if (!Directory.Exists(uploadDir))
                    Directory.CreateDirectory(uploadDir);

                string fileName = Path.GetFileName(NewImageFile.FileName);
                string filePath = Path.Combine(uploadDir, fileName);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await NewImageFile.CopyToAsync(stream);
                }

                // Relative path for DB
                string newImagePath = "/Image/" + fileName;

                // ✅ Call UpdateProductImage from repository
                _repository.UpdateProductImage(model.productid, newImagePath);
            }

            return RedirectToAction("ViewImage");
        }



        [HttpGet]
        public IActionResult Products()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AddProducts()
        {
            ViewBag.productionList = new SelectList(_repository.GetProduction(), "ProductionID", "ProductionID");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProducts(
            string ProductName,
            int ProductionID,
            decimal ProductRatings,
            decimal Price,
            string Description,
            IFormFile ProductImageFile)
        {
            int productId = _repository.AddProducts(ProductName, ProductionID, ProductRatings, Price, Description);

            // Then handle image upload if provided
            if (ProductImageFile != null && ProductImageFile.Length > 0)
            {
                string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Image");
                if (!Directory.Exists(uploadFolder))
                    Directory.CreateDirectory(uploadFolder);

                string fileName = Path.GetFileName(ProductImageFile.FileName);
                string filePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProductImageFile.CopyToAsync(stream);
                }

                string imagePath = "/Image/" + fileName;

                // Use the stored procedure via repository method
                try
                {
                    _repository.AddProductImage(productId, imagePath);
                }
                catch (Exception ex)
                {
                    // Optionally, log or handle error
                    ViewBag.Error = ex.Message;
                }
            }

            return RedirectToAction("ViewProducts");
        }
        [HttpGet]
        public IActionResult ViewProducts()
        {
            var productViews = _repository.GetProductsWithImages();
            return View(productViews);
        }
        [HttpGet]
        public IActionResult SearchProducts(string searchTerm)
        {
            ViewBag.SearchTerm = searchTerm; // Pass it to the view
            var products = _repository.SearchProducts(searchTerm);
            return View("ViewProducts", products);
        }


        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            var product = _repository.GetProductsbyid(id);
            var imagePath = _repository.GetImagePathByProductId(id);

            var viewModel = new ProductView
            {
                Product = product,
                ImagePath = imagePath
            };

            ViewBag.productionList = new SelectList(_repository.GetProduction(), "ProductionID", "ProductionID", product.ProductionID);
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductView model, IFormFile ProductImageFile)
        {
            _repository.UpdateProduct(
                model.Product.productid,
                model.Product.ProductName,
                model.Product.ProductionID,
                model.Product.ProductRatings,
                model.Product.Price,
                model.Product.Description);

            if (ProductImageFile != null && ProductImageFile.Length > 0)
            {
                string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Image");
                if (!Directory.Exists(uploadFolder)) Directory.CreateDirectory(uploadFolder);

                string fileName = Path.GetFileName(ProductImageFile.FileName);
                string filePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProductImageFile.CopyToAsync(stream);
                }

                string imagePath = "/Image/" + fileName;

                // Insert new image using stored procedure logic
                try
                {
                    _repository.AddProductImage(model.Product.productid, imagePath);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                }
            }

            return RedirectToAction("ViewProducts");
        }





    }
}
