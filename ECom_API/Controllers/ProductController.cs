using ECom_API.Data;
using ECom_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECom_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var products = _context.products.ToList();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = _context.products.Find(id);
            if (product == null) 
                return NotFound("Product not found.");

            return Ok(product);
        }

        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            _context.products.Add(product);
            _context.SaveChanges();
            return Ok("Product added successfully.");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, Product updateProduct)
        {
            var product = _context.products.Find(id);
            if (product == null)
                return NotFound("Product not found");

            product.Name = updateProduct.Name;
            product.Description = updateProduct.Description;
            product.Price = updateProduct.Price;
            product.Stock = updateProduct.Stock;
            product.Category = updateProduct.Category;
            product.ImageUrl = updateProduct.ImageUrl;

            _context.SaveChanges();
            return Ok("Product updated successfully.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.products.Find(id);
            if (product == null)
                return NotFound("Product not found.");

            _context.products.Remove(product);
            _context.SaveChanges();
            return Ok("Product deleted successfully.");
        }
    }
}
