using ECom_API.Data;
using ECom_API.Models;
using ECom_API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ECom_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult PlaceOrder([FromBody] PlaceOrderDTO orderDto)
        {
            var order = orderDto.Order;
            var orderItems = orderDto.OrderItems;

            // Set Order date and save Order
            order.OrderDate = DateTime.UtcNow;
            _context.Orders.Add(order);
            _context.SaveChanges();

            // Save Order Items
            foreach (var item in orderItems)
            {
                item.OrderId = order.Id;
                _context.OrderItems.Add(item);
            }

            _context.SaveChanges();
            return Ok("Order placed successfully.");
        }

        [HttpGet("{userId}")]
        public IActionResult GetOrdersByUser(int userId)
        {
            var orders = _context.Orders.Where(o => o.UserId == userId).ToList();
            return Ok(orders);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateOrderStatus(int id, [FromBody] string status)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
                return NotFound("Order not found.");

            order.Status = status;
            _context.SaveChanges();
            return Ok("Order status updated successfully.");
        }
    }
}
