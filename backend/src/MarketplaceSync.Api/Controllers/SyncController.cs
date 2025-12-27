using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MarketplaceSync.Service.Marketplace;
using MarketplaceSync.Api.DTOs.Marketplace;

namespace MarketplaceSync.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SyncController : ControllerBase
    {
        private readonly ISyncService _syncService;

        public SyncController(ISyncService syncService)
        {
            _syncService = syncService;
        }

        private Guid GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return Guid.Parse(userIdClaim?.Value ?? Guid.Empty.ToString());
        }

        [HttpPost("stock/{marketplace}")]
        public async Task<IActionResult> SyncStock(string marketplace)
        {
            var userId = GetUserId();
            var success = await _syncService.SyncStockAsync(userId, marketplace);

            if (!success)
                return BadRequest("Erro ao sincronizar estoque");

            return Ok(new { message = "Sincronização de estoque iniciada" });
        }

        [HttpPost("orders/{marketplace}")]
        public async Task<IActionResult> SyncOrders(string marketplace)
        {
            var userId = GetUserId();
            var success = await _syncService.SyncOrdersAsync(userId, marketplace);

            if (!success)
                return BadRequest("Erro ao sincronizar pedidos");

            return Ok(new { message = "Sincronização de pedidos iniciada" });
        }

        [HttpGet("history/{marketplace}/{syncType}")]
        public async Task<IActionResult> GetLastSync(string marketplace, string syncType)
        {
            var userId = GetUserId();
            var lastSync = await _syncService.GetLastSyncAsync(userId, marketplace, syncType);

            if (lastSync == null)
                return NotFound();

            return Ok(lastSync);
        }
    }
}