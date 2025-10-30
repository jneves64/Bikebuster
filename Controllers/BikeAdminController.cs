using BikeBuster.Models;

using Microsoft.AspNetCore.Mvc;

namespace BikeBuster.Controllers
{
    [ApiController]
    [Route("motos")]
    public class MotosController : ControllerBase
    {
        // POST /motos
        [HttpPost]
        public IActionResult Create([FromBody] BikeModel moto)
        {
            return Created("", moto); // 201 Created
        }

        // GET /motos
        [HttpGet]
        public IActionResult GetAll([FromQuery] string? placa)
        {
            return Ok(new[] { new BikeModel { Id = "moto123", Year = 2020, Model = "Mottu Sport", Plate = "CDX-0101" } });
        }

        // GET /motos/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            return Ok(new BikeModel { Id = id, Year = 2020, Model = "Mottu Sport", Plate = "CDX-0101" });
        }

        // PUT /motos/{id}/placa
        [HttpPut("{id}/placa")]
        public IActionResult UpdatePlate(string id, [FromBody] string novaPlaca)
        {
            return NoContent(); // 204
        }

        // DELETE /motos/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            return NoContent();
        }
    }
}
