using System.ComponentModel.DataAnnotations;

namespace SWIFTCARGOAPI.Models
{
    public class Shipment
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string TrackingNumber { get; set; }

        [Required]
        [StringLength(100)]
        public required string Origin { get; set; }

        [Required]
        [StringLength(100)]
        public required string Destination { get; set; }

        public decimal Weight { get; set; } // in kg

        [Required]
        [StringLength(50)]
        public required string Status { get; set; } // e.g., "Pending", "In Transit", "Delivered"

        public DateTime EstimatedDelivery { get; set; }
    }
}