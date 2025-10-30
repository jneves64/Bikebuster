using BikeBuster.Models;

using Microsoft.AspNetCore.Mvc;

namespace BikeBuster.Controllers
{
    [ApiController]
    [Route("entregadores")]
    public class RidersController : ControllerBase
    {
        // POST /entregadores
        [HttpPost]
        public IActionResult RegisterRider([FromBody] UserModel rider)
        {
            // lógica de cadastro
            return Created("", rider); // 201 Created
        }

        // PUT /entregadores/{id}/cnh
        [HttpPost("{id}/cnh")]
        public IActionResult UpdateLicense(string id, [FromBody] UserModel rider)
        {
            // lógica de atualização da imagem CNH
            return NoContent(); // 204 No Content
        }
    }
}
