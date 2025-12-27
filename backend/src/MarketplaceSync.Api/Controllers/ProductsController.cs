using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MarketplaceSync.Service.Product;
using MarketplaceSync.Api.DTOs.Product;
using MarketplaceSync.Domain.Entities;

namespace MarketplaceSync.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        private Guid GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return Guid.Parse(userIdClaim?.Value ?? Guid.Empty.ToString());
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
        {
            var userId = GetUserId();
            var products = await _productService.GetProductsByUserAsync(userId);

            var dtos = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Sku = p.Sku,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Cost = p.Cost,
                Category = p.Category,
                ImageUrl = p.ImageUrl,
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt
            }).ToList();

            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetById(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
                return NotFound();

            var dto = new ProductDto
            {
                Id = product.Id,
                Sku = product.Sku,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Cost = product.Cost,
                Category = product.Category,
                ImageUrl = product.ImageUrl,
                IsActive = product.IsActive,
                CreatedAt = product.CreatedAt
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductDto request)
        {
            var userId = GetUserId();

            var product = new Product
            {
                UserId = userId,
                Sku = request.Sku,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Cost = request.Cost,
                Category = request.Category,
                ImageUrl = request.ImageUrl
            };

            var created = await _productService.CreateProductAsync(product);

            var dto = new ProductDto
            {
                Id = created.Id,
                Sku = created.Sku,
                Name = created.Name,
                Description = created.Description,
                Price = created.Price,
                Cost = created.Cost,
                Category = created.Category,
                ImageUrl = created.ImageUrl,
                IsActive = created.IsActive,
                CreatedAt = created.CreatedAt
            };

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductDto request)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
                return NotFound();

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.Cost = request.Cost;
            product.Category = request.Category;
            product.ImageUrl = request.ImageUrl;

            var success = await _productService.UpdateProductAsync(product);

            if (!success)
                return BadRequest("Erro ao atualizar produto");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _productService.DeleteProductAsync(id);

            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}