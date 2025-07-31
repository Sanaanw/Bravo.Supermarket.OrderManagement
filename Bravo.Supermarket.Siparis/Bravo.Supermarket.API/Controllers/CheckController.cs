using Bravo.Supermarket.API.Data;
using Microsoft.AspNetCore.Mvc;

namespace Bravo.Supermarket.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CheckController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("Barkodlar")]
        public IActionResult GetBarkodlar()
        {
          var data = _context.barkodTanimlari.ToList();
            if (data == null || !data.Any())
                return NotFound("Barkodlar taplmadi.");

            return Ok(data);
        }
    }
}
