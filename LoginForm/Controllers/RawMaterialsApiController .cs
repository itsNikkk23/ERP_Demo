using Microsoft.AspNetCore.Mvc;
using ERP.Repository;
using ERP.Models;

namespace LoginForm.Controllers.Api
{
    [Route("api/rawmaterials")]
    [ApiController]
    public class RawMaterialsApiController : ControllerBase
    {
        private readonly IERPRepository _repository;

        public RawMaterialsApiController(IERPRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_repository.GetAllRawMaterials());

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var material = _repository.GetRawMaterialById(id);
            return material != null ? Ok(material) : NotFound();
        }

        [HttpPost]
        public IActionResult Create([FromBody] RawMaterials material)
        {
            if (material == null) return BadRequest("Invalid data.");
            _repository.InsertRawMaterials(material.MaterialName, material.MaterialType, material.UnitOfMeasure, material.Description);
            return Ok(new { message = "Raw Material Added Successfully." });
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] RawMaterials material)
        {
            if (material == null) return BadRequest("Invalid data.");
            _repository.UpdateRawMaterials(id, material.MaterialName, material.MaterialType, material.UnitOfMeasure, material.Description);
            return Ok(new { message = "Raw Material Updated Successfully." });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _repository.DeleteRawMaterials(id);
            return Ok(new { message = "Raw Material Deleted Successfully." });
        }
    }
}
