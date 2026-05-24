using Microsoft.EntityFrameworkCore;
using SWIFTCARGOAPI.Data;
using SWIFTCARGOAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWIFTCARGOAPI.Services
{
    public class ShipmentService : IShipmentService
    {
        private readonly ApplicationDbContext _context;

        public ShipmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Shipment>> GetAllShipmentsAsync()
        {
            return await _context.Shipments.ToListAsync();
        }

        public async Task<Shipment?> GetShipmentByIdAsync(int id)
        {
            return await _context.Shipments.FindAsync(id);
        }

        public async Task<Shipment> CreateShipmentAsync(Shipment shipment)
        {
            _context.Shipments.Add(shipment);
            await _context.SaveChangesAsync();
            return shipment;
        }

        public async Task<bool> UpdateShipmentAsync(int id, Shipment shipment)
        {
            if (id != shipment.Id)
            {
                return false;
            }

            _context.Entry(shipment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ShipmentExists(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
            return true;
        }

        public async Task<bool> DeleteShipmentAsync(int id)
        {
            var shipment = await _context.Shipments.FindAsync(id);
            if (shipment == null)
            {
                return false;
            }

            _context.Shipments.Remove(shipment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateShipmentStatusAsync(int id, string status)
        {
            var shipment = await _context.Shipments.FindAsync(id);
            if (shipment == null)
            {
                return false;
            }

            shipment.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<bool> ShipmentExists(int id)
        {
            return await _context.Shipments.AnyAsync(e => e.Id == id);
        }
    }
}