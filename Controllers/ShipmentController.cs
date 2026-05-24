using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWIFTCARGOAPI.Models;
using SWIFTCARGOAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWIFTCARGOAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentController : ControllerBase
    {
        private readonly IShipmentService _shipmentService;

        public ShipmentController(IShipmentService shipmentService)
        {
            _shipmentService = shipmentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shipment>>> GetShipments()
        {
            var shipments = await _shipmentService.GetAllShipmentsAsync();
            return Ok(shipments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Shipment>> GetShipment(int id)
        {
            var shipment = await _shipmentService.GetShipmentByIdAsync(id);
            if (shipment == null)
            {
                return NotFound();
            }
            return Ok(shipment);
        }

        [HttpPost]
        public async Task<ActionResult<Shipment>> PostShipment(Shipment shipment)
        {
            var createdShipment = await _shipmentService.CreateShipmentAsync(shipment);
            return CreatedAtAction(nameof(GetShipment), new { id = createdShipment.Id }, createdShipment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutShipment(int id, Shipment shipment)
        {
            if (id != shipment.Id)
            {
                return BadRequest();
            }

            var result = await _shipmentService.UpdateShipmentAsync(id, shipment);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShipment(int id)
        {
            var result = await _shipmentService.DeleteShipmentAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin,Driver")] // Restrict this endpoint
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] StatusUpdateDto statusUpdate)
        {
            var result = await _shipmentService.UpdateShipmentStatusAsync(id, statusUpdate.Status);
            if (!result)
            {
                return NotFound();
            }
            return Ok("Status updated successfully.");
        }
    }
}