using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BikeBuster.Controllers
{
    [Route("locacao")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        [HttpPost]
        public IActionResult Rent()
        {
            return Ok(new { Message = "rented sucessfully." });
        }


        [HttpGet("{id}")]
        public IActionResult GetRental()
        {
            return Ok(new { Message = "rental info accessed successfully." });
        }

       
        [HttpPut("{id}/devolucao")]
        public IActionResult Return()
        {
            return Ok(new { Message = "bike was returned" });
        }




    }
}
