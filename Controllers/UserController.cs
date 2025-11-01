using System.Text.Json;
using BikeBuster.Models;
using BikeBuster.Services;
using Microsoft.AspNetCore.Mvc;

namespace BikeBuster.Controllers
{
    [ApiController]
    [Route("entregadores")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }
        // POST /entregadores
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserModel user)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var result = await _userService.Create(user);
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

        // PUT /entregadores/{id}/cnh
        [HttpPost("{id}/cnh")]
        public async Task<IActionResult> UpdateLicense(string id, [FromBody] JsonElement body, CancellationToken cancellationToken = default)
        {
            // Extrai imagem do JSON
            if (!body.TryGetProperty("imagem_cnh", out var imgProp) || string.IsNullOrEmpty(imgProp.GetString()))
                return BadRequest(new { mensagem = "Imagem vazia" });

            try
            {
                var updated = await _userService.UpdateLicenseImageAsync(id, imgProp.GetString()!, cancellationToken);
                if (!updated)
                    return NotFound();

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }
    }
}
