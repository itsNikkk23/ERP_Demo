using Microsoft.AspNetCore.Mvc;
using ERP.Repository;
using ERP.Models;

namespace LoginForm.Controllers.Api
{
    [Route("api/suppliers")]
    [ApiController]
    public class SuppliersApiController : ControllerBase
    {
        private readonly IERPRepository _repository;

        public SuppliersApiController(IERPRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_repository.GetAllSuppliers());

        [HttpPost]
        public IActionResult Create([FromBody] Suppliers supplier)
        {
            if (supplier == null) return BadRequest("Invalid data.");
            _repository.InsertSuppliers(supplier.SupplierName, supplier.SupplierContact);
            return Ok(new { message = "Supplier Added Successfully." });
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Suppliers supplier)
        {
            if (supplier == null) return BadRequest("Invalid data.");
            _repository.UpdateSuppliers(id, supplier.SupplierName, supplier.SupplierContact);
            return Ok(new { message = "Supplier Updated Successfully." });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _repository.DeleteSuppliers(id);
            return Ok(new { message = "Supplier Deleted Successfully." });
        }
    }
}
