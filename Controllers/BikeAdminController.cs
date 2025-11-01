using System.Diagnostics;
using BikeBuster.Models;
using BikeBuster.Services;
using Microsoft.AspNetCore.Mvc;

namespace BikeBuster.Controllers
{
    [ApiController]
    [Route("motos")]
    public class BikeAdminController : ControllerBase
    {

        private readonly BikeService _bikeService;

        public BikeAdminController(BikeService bikeService)
        {
            _bikeService = bikeService;
        }

        // POST /motos

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BikeModel bike)
        {
            //check if
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var result = await _bikeService.Create(bike);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }

            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Erro = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Erro = ex.Message });
            }

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
