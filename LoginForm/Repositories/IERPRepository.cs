using ERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;

namespace ERP.Repository
{
    public interface IERPRepository
    {
        bool ValidateCredentials(string UserName, string Passcode);
        void LogLogin(string UserName, string Passcode);
        List<RawMaterials> GetAllRawMaterials();
        RawMaterials GetRawMaterialById(int RawMaterialID);

        void InsertRawMaterials(string MaterialName, string MaterialType, string UnitOfMeasure, string Description);
        void UpdateRawMaterials(int RawMaterialID, string MaterialName, string MaterialType, string UnitOfMeasure, string Description);
        void DeleteRawMaterials(int RawMaterialID);
        void InsertSuppliers(string SupplierName,string SupplierContact);
       // void GetSupplierById(int SupplierID);
       List<Suppliers> GetAllSuppliers();
        void UpdateSuppliers(int SupplierId, string SupplierName, string SupplierContact);
        Suppliers GetSupplierById(int SupplierID);
        void DeleteSuppliers(int SupplierId);
        void InsertPurchaseOrder(int SupplierID, DateTime OrderDate,DateTime DeliveryDate,Decimal TotalAmount,string Status);
        List<PurchaseOrders> GetAllPurchaseOrders();
         PurchaseOrders GetPurchaseOrderById(int PurchaseOrderID);
        void UpdatePurchaseOrder(int OrderID, int SupplierID, DateTime OrderDate, DateTime? DeliveryDate, decimal TotalAmount, string Status);
        void DeletePurchaseOrder(int OrderID);
        void AddWeaving(string weave_type, string loomtype);
        List<Weaving_process>GetWeavingProcess();
        Weaving_process GetWeavingById(int WeaveID);
        void UpdateWeaving(int WeaveID, string weave_type, string loomtype);
        void DeleteWeaving(int WeaveID);
        void AddProductColor(string colorName);
        List<ProductColor> GetProductColor();
        ProductColor GetProductColorById(int colorID);
        void UpdateProductColor(int colorID, string colorName);
        void DelProductColor(int colorID);
        void AddFabric(string FabricType);
        public List<Fabrics> GetFabric();
        void UpdateFabric(int FabricID, string FabricName);
        Fabrics GetFabricById(int id);
        void DeleteFabric(int FabricID);
        //void InsertDesigns(string DesignType);
        //public List<Designs> GetDesign();
        public List<Designs> GetDesign();
        void AddCategory(string CategoryName,string Description);
        List<Categories> GetCategory();
        Categories GetCategoryById(int CategoryID);
        void UpdateCategory(int CategoryID, string CategoryName, string Description);
        void DeleteCategory(int CategoryID);
        //Dictionary<string, List<SelectListItem>> GetDropdownData();
        void AddProductAttribute(int CategoryID, int FabricID, int DesignID, int WeaveID, int ColorID);

        List<ProductAttribute> GetProductAttribute();
        ProductAttribute GetProductAttributeByid(int productAttributeid);
        void UpdateProductAttribute(int productAttributeid, int CategoryID, int FabricID, int DesignID, int WeaveID, int ColorID);
        void DeleteProductAttribute(int productAttributeid);
        void AddProduction(int productAttributeid, DateTime ProductionDate, string MachineUsed, int EmployeeID, int RawMaterialID, int Quantity);
        public  List<Production> GetProduction();
        public List<employees> GetEmployees();
        //employees GetEmployeesById(int EmployeeID);
        Production GetProductionById(int ProductionID);
        void UpdateProduction(int ProductionID, int productAttributeid, DateTime ProductionDate, string MachineUsed, int EmployeeID, int RawMaterialID, int Quantity);
        int AddProducts(string ProductName,int ProductionID, decimal ProductRatings,decimal Price, string Description);
        public List<Products> GetProducts();
        void UpdateProduct(int productid, string ProductName, int ProductionID, decimal ProductRatings, decimal Price, string Description);

        int AddProductImage(int productid, string ImagePath);
        string? GetImagePathByProductId(int productId);
        public List<ProductImage> GetProductImages();
        public List<ProductView> GetProductsWithImages();
        ProductImage GetProductImageById(int ImageID);
        Products GetProductsbyid(int productid);
    }
}
