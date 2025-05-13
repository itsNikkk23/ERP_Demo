using Microsoft.AspNetCore.Mvc;
using ERP.Repository;
using ERP.Models;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ERP.Controllers
{
    [Route("Manufacture")]
    public class ManufactureController : Controller
    {
        private readonly IERPRepository _repository;

        public ManufactureController(IERPRepository repository)
        {
            _repository = repository;
        }

        // ---------------------- RAW MATERIALS CRUD ---------------------- //

        [HttpGet("AddRawMaterials")]
        public IActionResult AddRawMaterials()
        {
            return View();
        }

        [HttpPost("AddRawMaterials")]
        public IActionResult AddRawMaterials(string MaterialName, string MaterialType, string UnitOfMeasure, string Description)
        {
            _repository.InsertRawMaterials(MaterialName, MaterialType, UnitOfMeasure, Description);
            return RedirectToAction("ViewRawMaterials");
        }

        [HttpGet("ViewRawMaterials")]
        public IActionResult ViewRawMaterials()
        {
            List<RawMaterials> rawMaterialsList = _repository.GetAllRawMaterials();
            return View(rawMaterialsList);
        }

        [HttpGet("EditRawMaterials/{id}")]
        public IActionResult EditRawMaterials(int id)
        {
            RawMaterials rawMaterial = _repository.GetRawMaterialById(id);
            if (rawMaterial == null)
            {
                return NotFound();
            }
            return View(rawMaterial);
        }

        [HttpPost("EditRawMaterials/{id}")]
        public IActionResult EditRawMaterials(int id, string MaterialName, string MaterialType, string UnitOfMeasure, string Description)
        {
            _repository.UpdateRawMaterials(id, MaterialName, MaterialType, UnitOfMeasure, Description);
            return RedirectToAction("ViewRawMaterials");
        }

        [HttpPost("DeleteRawMaterials/{id}")]
        public IActionResult DeleteRawMaterials(int id)
        {
            _repository.DeleteRawMaterials(id);
            return RedirectToAction("ViewRawMaterials");
        }

        // ---------------------- PURCHASE ORDERS CRUD ---------------------- //

        [HttpGet("AddPurchaseOrder")]
        public IActionResult AddPurchaseOrder()
        {
            var suppliersList = _repository.GetAllSuppliers();

            // Ensure the Supplier List is not null
            ViewBag.SupplierList = suppliersList != null
                ? new SelectList(suppliersList, "SupplierID", "SupplierName")
                : new SelectList(new List<SelectListItem>());

            return View(new PurchaseOrders());
        }

        [HttpPost("AddPurchaseOrder")]
        public IActionResult AddPurchaseOrder(int SupplierID, DateTime OrderDate, DateTime? DeliveryDate, decimal TotalAmount, string Status)
        {
            _repository.InsertPurchaseOrder(SupplierID, OrderDate, DeliveryDate ?? DateTime.MinValue, TotalAmount, Status);
            return RedirectToAction("ViewPurchaseOrders");
        }

        [HttpGet("ViewPurchaseOrders")]
        public IActionResult ViewPurchaseOrders()
        {
            List<PurchaseOrders> orderList = _repository.GetAllPurchaseOrders();
            return View(orderList);
        }

        [HttpGet("EditPurchaseOrder/{id}")]
        public IActionResult EditPurchaseOrder(int id)
        {
            PurchaseOrders order = _repository.GetPurchaseOrderById(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewBag.SupplierList = new SelectList(_repository.GetAllSuppliers(), "SupplierID", "SupplierName", order.SupplierID);
            return View(order);
        }

        [HttpPost("EditPurchaseOrder/{id}")]
        public IActionResult EditPurchaseOrder(int id, int SupplierID, DateTime OrderDate, DateTime? DeliveryDate, decimal TotalAmount, string Status)
        {
            _repository.UpdatePurchaseOrder(id, SupplierID, OrderDate, DeliveryDate ?? DateTime.MinValue, TotalAmount, Status);
            return RedirectToAction("ViewPurchaseOrders");
        }

        [HttpPost("DeletePurchaseOrder/{id}")]
        public IActionResult DeletePurchaseOrder(int id)
        {
            _repository.DeletePurchaseOrder(id);
            return RedirectToAction("ViewPurchaseOrders");
        }

        // ---------------------- WEAVING CRUD ---------------------- //

        [HttpGet("AddWeaving")]
        public IActionResult AddWeaving()
        {
            return View();
        }

        [HttpPost("AddWeaving")]
        public IActionResult AddWeaving(string weave_type, string loomtype)
        {
            _repository.AddWeaving(weave_type, loomtype);
            return RedirectToAction("WeaveProcess");
        }

        [HttpGet("WeaveProcess")]
        public IActionResult WeaveProcess()
        {
            List<Weaving_process> weavingList = _repository.GetWeavingProcess();
            return View(weavingList);
        }

        [HttpGet("EditWeave/{id}")]
        public IActionResult EditWeave(int id)
        {
            Weaving_process weaving = _repository.GetWeavingById(id);
            if (weaving == null)
            {
                return NotFound();
            }
            return View(weaving);
        }

        [HttpPost("EditWeave/{id}")]
        public IActionResult EditWeave(int id, string weave_type, string loomtype)
        {
            _repository.UpdateWeaving(id, weave_type, loomtype);
            return RedirectToAction("WeaveProcess");
        }

        [HttpPost("DeleteWeave/{id}")]
        public IActionResult DeleteWeave(int id)
        {
            _repository.DeleteWeaving(id);
            return RedirectToAction("WeaveProcess");
        }

        // ---------------------- PRODUCT COLOR CRUD ---------------------- //

        [HttpGet("AddProductColor")]
        public IActionResult AddProductColor()
        {
            return View();
        }

        [HttpPost("AddProductColor")]
        public IActionResult AddProductColor(string ColorName)
        {
            _repository.AddProductColor(ColorName);
            return RedirectToAction("ViewProductColor");
        }

        [HttpGet("ViewProductColor")]
        public IActionResult ViewProductColor()
        {
            List<ProductColor> productColorList = _repository.GetProductColor();
            return View(productColorList);
        }

        [HttpGet("EditColor/{id}")]
        public IActionResult EditColor(int id)
        {
            ProductColor productColor = _repository.GetProductColorById(id);
            if (productColor == null)
            {
                return NotFound();
            }
            return View(productColor);
        }

        [HttpPost("EditColor/{id}")]
        public IActionResult EditColor(int id, string ColorName)
        {
            _repository.UpdateProductColor(id, ColorName);
            return RedirectToAction("ViewProductColor");
        }

        [HttpPost("DeleteColor/{id}")]
        public IActionResult DeleteColor(int id)
        {
            _repository.DelProductColor(id);
            return RedirectToAction("ViewProductColor");
        }

        // ---------------------- FABRICS CRUD ---------------------- //

        [HttpGet("AddFabrics")]
        public IActionResult AddFabrics()
        {
            return View();
        }

        [HttpPost("AddFabrics")]
        public IActionResult AddFabrics(string FabricName)
        {
            _repository.AddFabric(FabricName);
            return RedirectToAction("ViewFabrics");
        }

        [HttpGet("ViewFabrics")]
        public IActionResult ViewFabrics()
        {
            List<Fabrics> fabricList = _repository.GetFabric();
            return View(fabricList);
        }

        [HttpGet("EditFabric/{id}")]
        public IActionResult EditFabric(int id)
        {
            Fabrics fabric = _repository.GetFabricById(id);
            if (fabric == null)
            {
                return NotFound();
            }
            return View(fabric);
        }

        [HttpPost("EditFabric/{id}")]
        public IActionResult EditFabric(int id, string FabricName)
        {
            _repository.UpdateFabric(id, FabricName);
            return RedirectToAction("ViewFabrics");
        }

        [HttpPost("DeleteFabric/{id}")]
        public IActionResult DeleteFabric(int id)
        {
            _repository.DeleteFabric(id);
            return RedirectToAction("ViewFabrics");
        }
        [HttpGet("AddProductAttribute")]
        public IActionResult AddProductAttribute()
        {
            // Fetching data for dropdowns
            ViewBag.categoryList = new SelectList(_repository.GetCategory(), "CategoryID", "CategoryName");
            ViewBag.fabricList = new SelectList(_repository.GetFabric(), "FabricID", "FabricName");
            ViewBag.designList = new SelectList(_repository.GetDesign(), "DesignID", "DesignType");
            ViewBag.weavingList = new SelectList(_repository.GetWeavingProcess(), "WeaveID", "weave_type");
            ViewBag.colorList = new SelectList(_repository.GetProductColor(), "ColorID", "ColorName");
            return View();
        }
        [HttpPost("AddProductAttribute")]
        public IActionResult AddProductAttribute(int CategoryID, int FabricID, int DesignID, int WeaveID, int ColorID)
        {
            _repository.AddProductAttribute(CategoryID, FabricID, DesignID, WeaveID, ColorID);
            return RedirectToAction("ViewProductAttribute");
        }
        [HttpGet("ViewProductAttribute")]
        public IActionResult ViewProductAttribute()
        {
            List<ProductAttribute> productAttributeList = _repository.GetProductAttribute();
            return View(productAttributeList);
        }
        [HttpGet("EditProductAttribute/{id}")]
        public IActionResult EditAttribute(int id)
        {
            var productAttribute = _repository.GetProductAttributeByid(id);
            if (productAttribute == null)
            {
                return NotFound();
            }

            // Always set dropdown lists if model is valid
            ViewBag.categoryList = new SelectList(_repository.GetCategory(), "CategoryID", "CategoryName");
            ViewBag.fabricList = new SelectList(_repository.GetFabric(), "FabricID", "FabricName");
            ViewBag.designList = new SelectList(_repository.GetDesign(), "DesignID", "DesignType");
            ViewBag.weavingList = new SelectList(_repository.GetWeavingProcess(), "WeaveID", "weave_type");
            ViewBag.colorList = new SelectList(_repository.GetProductColor(), "ColorID", "ColorName");

            return View(productAttribute);
        }

   
        [HttpPost("EditProductAttribute/{id}")]
        public IActionResult EditAttribute(int id, int CategoryID, int FabricID, int DesignID, int WeaveID, int ColorID)
        {
            _repository.UpdateProductAttribute(id, CategoryID, FabricID, DesignID, WeaveID, ColorID);
            return RedirectToAction("ViewProductAttribute");
        }
        [HttpPost("DeleteProductAttribute/{id}")]
        public IActionResult DeleteProductAttribute(int id)
        {
            _repository.DeleteProductAttribute(id);
            return RedirectToAction("ViewProductAttribute");
        }
        [HttpGet("AddProduction")]
        public IActionResult AddProduction()
        {
            // Fetching data for dropdowns
            ViewBag.productAttributeList = new SelectList(_repository.GetProductAttribute(), "productAttributeid", "productAttributeid");
            ViewBag.rawMaterialsList = new SelectList(_repository.GetAllRawMaterials(), "RawMaterialID", "MaterialName");
            ViewBag.employeeList = new SelectList(_repository.GetEmployees(), "EmployeeID", "Name");
            return View();
        }
        [HttpPost("AddProduction")]
        public IActionResult AddProduction(int productAttributeid, DateTime ProductionDate, string MachineUsed, int EmployeeID, int RawMaterialID, int Quantity)
        {
            _repository.AddProduction(productAttributeid, ProductionDate, MachineUsed, EmployeeID, RawMaterialID, Quantity);
            return RedirectToAction("ViewProduction");
        }
        [HttpGet("ViewProduction")]
        public IActionResult ViewProduction()
        {
            List<Production> productionList = _repository.GetProduction();
            return View(productionList);
        }
        [HttpGet("EditProduction/{id}")]
        public IActionResult EditProduction(int id)
        {
            var production = _repository.GetProductionById(id);
            if (production == null)
            {
                return NotFound();
            }
            // Always set dropdown lists if model is valid
            ViewBag.productAttributeList = new SelectList(_repository.GetProductAttribute(), "productAttributeid", "productAttributeid");
            ViewBag.rawMaterialsList = new SelectList(_repository.GetAllRawMaterials(), "RawMaterialID", "MaterialName");
            ViewBag.employeeList = new SelectList(_repository.GetEmployees(), "EmployeeID", "Name");
            return View(production);
        }
        [HttpPost("EditProduction/{id}")]
        public IActionResult EditProduction(int id, int productAttributeid, DateTime ProductionDate, string MachineUsed, int EmployeeID, int RawMaterialID, int Quantity)
        {
            _repository.UpdateProduction(id, productAttributeid, ProductionDate, MachineUsed, EmployeeID, RawMaterialID, Quantity);
            return RedirectToAction("ViewProduction");
        }
        [HttpPost("DeleteProduction/{id}")]
        public IActionResult DeleteProduction(int id)
        {
           // _repository.DeleteProduction(id);
            return RedirectToAction("ViewProduction");
        }
        

    }
}
