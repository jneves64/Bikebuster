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

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var result = await _bikeService.Create(bike);
                return Created("", result);
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


        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? placa, CancellationToken cancellationToken = default)
        {
            var bikes = await _bikeService.GetAllAsync(placa, cancellationToken);
            return Ok(bikes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken = default)
        {
            var bike = await _bikeService.GetByIdAsync(id, cancellationToken);
            if (bike == null)
                return NotFound(); // 404 - esse ID específico não existe

            return Ok(bike); // 200 - encontrou a moto
        }


        // PUT /motos/{id}/placa
        [HttpPut("{id}/placa")]
        public async Task<IActionResult> UpdatePlate(string id, [FromBody] string novaPlaca)
        {
            if (string.IsNullOrWhiteSpace(novaPlaca))
                return BadRequest("Placa inválida.");

            try
            {
                var ok = await _bikeService.UpdatePlateAsync(id, novaPlaca);
                if (!ok) return NotFound("Moto não encontrada.");
                return NoContent(); // 204
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

        // DELETE /motos/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var ok = await _bikeService.DeleteAsync(id);
            if (!ok) return NotFound("Moto não encontrada.");
            return Ok();


        }
    }
}