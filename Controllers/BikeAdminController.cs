using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using BikeBuster.Models;
using BikeBuster.Models.DTOs;
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
        public async Task<IActionResult> GetAll([FromQuery] string? placa)
        {
            var bikes = await _bikeService.GetAllAsync(placa);
            return Ok(bikes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([Required] string id)
        {
            var bike = await _bikeService.GetByIdAsync(id);
            if (bike == null)
                return NotFound(); // 404 - esse ID específico não existe

            return Ok(bike); // 200 - encontrou a moto
        }

        [HttpPut("{id}/placa")]
        public async Task<IActionResult> UpdatePlate([Required] string id, [Required][FromBody] PlateUpdateRequest body)
        {
            if (string.IsNullOrWhiteSpace(body.NewPlate))
                return BadRequest("Placa inválida.");

            try
            {
                var ok = await _bikeService.UpdatePlateAsync(id, body.NewPlate);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([Required] string id)
        {
            var ok = await _bikeService.DeleteAsync(id);
            if (!ok) return NotFound("Moto não encontrada.");
            return Ok();


        }
    }
}