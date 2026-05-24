using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWIFTCARGOAPI.Models;
using SWIFTCARGOAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using SWIFTCARGOAPI.Documents;

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
        

        [HttpGet("report/pdf")]
        public async Task<IActionResult> GetShipmentReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var allShipments = await _shipmentService.GetAllShipmentsAsync();

            // Filter by date range if provided
            var filteredShipments = allShipments
                .Where(s => (!startDate.HasValue || s.EstimatedDelivery.Date >= startDate.Value.Date))
                .Where(s => (!endDate.HasValue || s.EstimatedDelivery.Date <= endDate.Value.Date))
                .ToList();

            if (!filteredShipments.Any())
            {
                return NotFound("No shipments found for the selected date range.");
            }

            var reportTitle = $"Shipment Report ({startDate:yyyy-MM-dd} - {endDate:yyyy-MM-dd})";
            if (!startDate.HasValue && !endDate.HasValue)
            {
                reportTitle = "Full Shipment Report";
            }

            var document = new ShipmentReport(filteredShipments, reportTitle);
            var pdfBytes = document.GeneratePdf();

            var fileName = $"ShipmentReport_{DateTime.Now:yyyyMMddHHmm}.pdf";
            return File(pdfBytes, "application/pdf", fileName);
        }
    }
}