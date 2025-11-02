using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using BikeBuster.Models;
using BikeBuster.Models.DTOs;
using BikeBuster.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BikeBuster.Controllers
{
    [Route("locacao")]
    [ApiController]
    public class RentalsController : ControllerBase
    {


        private readonly RentalService _rentalService;

        public RentalsController(RentalService rentalService)
        {
            _rentalService = rentalService;
        }

        [HttpPost]
        public async Task<IActionResult> Rent([FromBody][Required] RentalModel rent)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var result = await _rentalService.Create(rent);
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([Required] string id)
        {
            var bike = await _rentalService.GetByIdAsync(id);
            if (bike == null)
                return NotFound(); // 404 - esse ID específico não existe

            return Ok(bike); // 200 - encontrou a moto
        }


        [HttpPut("{id}/devolucao")]
        public async Task<IActionResult> ReturnAsync([Required] string id, [FromBody][Required] RentalReturnRequest body)
        {
            try
            {
                var (rental, total) = await _rentalService.ReturnAsync(id, body.ReturnDate);

                return Ok(new
                {
                    mensagem = "Data de devolução informada com sucesso",
                    entregador_id = rental.UserId,
                    moto_id = rental.BikeId,
                    data_inicio = rental.ContractStartDate.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    data_devolucao = rental.ContractEndDate?.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    data_prevista = rental.ContractExpectedEndDate.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    valor_total = total,
                    aluguel_id = rental.Id,
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { mensagem = "Locação não encontrada" });
            }

        }
    }
}
