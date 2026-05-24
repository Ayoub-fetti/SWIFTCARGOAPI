using SWIFTCARGOAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWIFTCARGOAPI.Services
{
    public interface IShipmentService
    {
        Task<IEnumerable<Shipment>> GetAllShipmentsAsync();
        Task<Shipment?> GetShipmentByIdAsync(int id);
        Task<Shipment> CreateShipmentAsync(Shipment shipment);
        Task<bool> UpdateShipmentAsync(int id, Shipment shipment);
        Task<bool> DeleteShipmentAsync(int id);
        Task<bool> UpdateShipmentStatusAsync(int id, string status);
    }
}