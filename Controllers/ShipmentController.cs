using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using SWIFTCARGOAPI.Documents;
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

        /// <summary>
        /// Retrieves a list of all shipments.
        /// </summary>
        /// <returns>A list of shipments.</returns>
        /// <response code="200">Returns the list of shipments.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Shipment>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Shipment>>> GetShipments()
        {
            var shipments = await _shipmentService.GetAllShipmentsAsync();
            return Ok(shipments);
        }

        /// <summary>
        /// Retrieves a specific shipment by its ID.
        /// </summary>
        /// <param name="id">The ID of the shipment to retrieve.</param>
        /// <returns>The requested shipment.</returns>
        /// <response code="200">Returns the requested shipment.</response>
        /// <response code="404">If the shipment is not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Shipment), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Shipment>> GetShipment(int id)
        {
            var shipment = await _shipmentService.GetShipmentByIdAsync(id);
            if (shipment == null)
            {
                return NotFound();
            }
            return Ok(shipment);
        }

        /// <summary>
        /// Creates a new shipment.
        /// </summary>
        /// <param name="shipment">The shipment details to create.</param>
        /// <returns>The newly created shipment.</returns>
        /// <response code="201">Returns the newly created shipment.</response>
        /// <response code="400">If the shipment data is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Shipment), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Shipment>> PostShipment(Shipment shipment)
        {
            var createdShipment = await _shipmentService.CreateShipmentAsync(shipment);
            return CreatedAtAction(nameof(GetShipment), new { id = createdShipment.Id }, createdShipment);
        }

        /// <summary>
        /// Replaces an entire shipment's details.
        /// </summary>
        /// <param name="id">The ID of the shipment to replace.</param>
        /// <param name="shipment">The full new shipment object.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">If the update was successful.</response>
        /// <response code="400">If the ID in the URL does not match the ID in the body.</response>
        /// <response code="404">If the shipment is not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Partially updates a shipment's details.
        /// </summary>
        /// <param name="id">The ID of the shipment to update.</param>
        /// <param name="patchDoc">The JSON Patch document with update operations.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">If the update was successful.</response>
        /// <response code="400">If the patch document is invalid.</response>
        /// <response code="404">If the shipment is not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PatchShipment(int id, [FromBody] JsonPatchDocument<Shipment> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var shipmentFromDb = await _shipmentService.GetShipmentByIdAsync(id);
            if (shipmentFromDb == null)
            {
                return NotFound();
            }

            // On applique le patch et on ajoute manuellement les erreurs au ModelState si nécessaire.
            patchDoc.ApplyTo(shipmentFromDb);

            // On re-valide l'objet après l'application du patch.
            if (!TryValidateModel(shipmentFromDb))
            {
                return ValidationProblem(ModelState);
            }

            await _shipmentService.UpdateShipmentAsync(id, shipmentFromDb);

            return NoContent();
        }

        /// <summary>
        /// Deletes a shipment.
        /// </summary>
        /// <param name="id">The ID of the shipment to delete.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">If the deletion was successful.</response>
        /// <response code="404">If the shipment is not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteShipment(int id)
        {
            var result = await _shipmentService.DeleteShipmentAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        /// <summary>
        /// Updates the status of a specific shipment (Admin/Driver only).
        /// </summary>
        /// <param name="id">The ID of the shipment to update.</param>
        /// <param name="statusUpdate">The new status.</param>
        /// <returns>A confirmation message.</returns>
        /// <response code="200">If the status was updated successfully.</response>
        /// <response code="404">If the shipment is not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not an Admin or Driver.</response>
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin,Driver")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] StatusUpdateDto statusUpdate)
        {
            var result = await _shipmentService.UpdateShipmentStatusAsync(id, statusUpdate.Status);
            if (!result)
            {
                return NotFound();
            }
            return Ok("Status updated successfully.");
        }

        /// <summary>
        /// Generates a PDF report of shipments for a given date range.
        /// </summary>
        /// <param name="startDate">The start date of the report period (optional).</param>
        /// <param name="endDate">The end date of the report period (optional).</param>
        /// <returns>A PDF file with the shipment report.</returns>
        /// <response code="200">Returns the PDF file.</response>
        /// <response code="404">If no shipments are found in the given date range.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpGet("report/pdf")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetShipmentReport([FromQuery] System.DateTime? startDate, [FromQuery] System.DateTime? endDate)
        {
            var allShipments = await _shipmentService.GetAllShipmentsAsync();

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

            var fileName = $"ShipmentReport_{System.DateTime.Now:yyyyMMddHHmm}.pdf";
            return File(pdfBytes, "application/pdf", fileName);
        }
    }
}