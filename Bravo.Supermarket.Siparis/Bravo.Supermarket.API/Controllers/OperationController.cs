using Bravo.Supermarket.API.Data;
using Bravo.Supermarket.API.Dto.Xml;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Bravo.Supermarket.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OperationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OperationController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("ReceiveOrder")]
        public IActionResult ReceiveOrder([FromBody] OrderDto orderDto)
        {
            Console.WriteLine($"Alınan sifariş: {JsonSerializer.Serialize(orderDto)}");

            return Ok();
        }
        [HttpGet]
        public IActionResult GetOrders()
        {
            var barkodTanimlari = _context.barkodTanimlari.ToList();
            return Ok(barkodTanimlari);
        }
    }
}