using Microsoft.EntityFrameworkCore;
using cookie_authentication_authorization_demo.Data;
using cookie_authentication_authorization_demo.Models;
using Microsoft.AspNetCore.Identity;
using cookie_authentication_authorization_demo.Enums;

namespace cookie_authentication_authorization_demo.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuditService _auditService;

        public DeliveryService(ApplicationDbContext context, IAuditService auditService)
        {
            _context = context;
            _auditService = auditService;
        }

        public async Task<IEnumerable<Delivery>> GetAllDeliveriesAsync()
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.DeliveryPerson)
                .ToListAsync();
        }

        public async Task<Delivery?> GetDeliveryByIdAsync(int id)
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.DeliveryPerson)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<Delivery>> GetDeliveriesByUserIdAsync(string userId)
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.DeliveryPerson)
                .Where(d => d.Order.UserId == userId)
                .ToListAsync();
        }

        public async Task<Delivery> CreateDeliveryAsync(Delivery delivery)
        {
            delivery.CreatedAt = DateTime.UtcNow;
            delivery.Status = DeliveryStatus.Pending;
            _context.Deliveries.Add(delivery);
            await _context.SaveChangesAsync();
            return delivery;
        }

        public async Task<Delivery> UpdateDeliveryAsync(Delivery delivery)
        {
            _context.Deliveries.Update(delivery);
            await _context.SaveChangesAsync();
            return delivery;
        }

        public async Task<bool> DeleteDeliveryAsync(int id)
        {
            var delivery = await _context.Deliveries.FindAsync(id);
            if (delivery == null)
                return false;

            _context.Deliveries.Remove(delivery);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Delivery>> GetDeliveriesByOrderIdAsync(int orderId)
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.DeliveryPerson)
                .Where(d => d.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Delivery>> GetDeliveriesByDeliveryPersonIdAsync(string deliveryPersonId)
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.DeliveryPerson)
                .Where(d => d.DeliveryPersonId == deliveryPersonId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Delivery>> GetDeliveriesByStatusAsync(DeliveryStatus status)
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.DeliveryPerson)
                .Where(d => d.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Delivery>> GetDeliveriesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.DeliveryPerson)
                .Where(d => d.CreatedAt >= startDate && d.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Delivery>> GetDeliveriesByAddressAsync(string address)
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.DeliveryPerson)
                .Where(d => d.Address.Contains(address))
                .ToListAsync();
        }

        public async Task<IEnumerable<Delivery>> GetDeliveriesByTrackingNumberAsync(string trackingNumber)
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.DeliveryPerson)
                .Where(d => d.TrackingNumber == trackingNumber)
                .ToListAsync();
        }

        public async Task<IEnumerable<Delivery>> GetDeliveriesByDeliveryPersonAndStatusAsync(string deliveryPersonId, DeliveryStatus status)
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.DeliveryPerson)
                .Where(d => d.DeliveryPersonId == deliveryPersonId && d.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Delivery>> GetDeliveriesByUserAndStatusAsync(string userId, DeliveryStatus status)
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.DeliveryPerson)
                .Where(d => d.Order.UserId == userId && d.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Delivery>> GetDeliveriesByOrderAndStatusAsync(int orderId, DeliveryStatus status)
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.DeliveryPerson)
                .Where(d => d.OrderId == orderId && d.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Delivery>> GetDeliveriesByDateRangeAndStatusAsync(DateTime startDate, DateTime endDate, DeliveryStatus status)
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.DeliveryPerson)
                .Where(d => d.CreatedAt >= startDate && d.CreatedAt <= endDate && d.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Delivery>> GetDeliveriesByAddressAndStatusAsync(string address, DeliveryStatus status)
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.DeliveryPerson)
                .Where(d => d.Address.Contains(address) && d.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Delivery>> GetDeliveriesByTrackingNumberAndStatusAsync(string trackingNumber, DeliveryStatus status)
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.DeliveryPerson)
                .Where(d => d.TrackingNumber == trackingNumber && d.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Delivery>> GetDeliveriesByDeliveryPersonAndDateRangeAsync(string deliveryPersonId, DateTime startDate, DateTime endDate)
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.DeliveryPerson)
                .Where(d => d.DeliveryPersonId == deliveryPersonId && d.CreatedAt >= startDate && d.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Delivery>> GetDeliveriesByUserAndDateRangeAsync(string userId, DateTime startDate, DateTime endDate)
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.DeliveryPerson)
                .Where(d => d.Order.UserId == userId && d.CreatedAt >= startDate && d.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Delivery>> GetDeliveriesByOrderAndDateRangeAsync(int orderId, DateTime startDate, DateTime endDate)
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.DeliveryPerson)
                .Where(d => d.OrderId == orderId && d.CreatedAt >= startDate && d.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Delivery>> GetDeliveriesByAddressAndDateRangeAsync(string address, DateTime startDate, DateTime endDate)
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.DeliveryPerson)
                .Where(d => d.Address.Contains(address) && d.CreatedAt >= startDate && d.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Delivery>> GetDeliveriesByTrackingNumberAndDateRangeAsync(string trackingNumber, DateTime startDate, DateTime endDate)
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.DeliveryPerson)
                .Where(d => d.TrackingNumber == trackingNumber && d.CreatedAt >= startDate && d.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Delivery>> GetDeliveriesByDeliveryPersonStatusAndDateRangeAsync(string deliveryPersonId, DeliveryStatus status, DateTime startDate, DateTime endDate)
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.DeliveryPerson)
                .Where(d => d.DeliveryPersonId == deliveryPersonId && d.Status == status && d.CreatedAt >= startDate && d.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Delivery>> GetDeliveriesByUserStatusAndDateRangeAsync(string userId, DeliveryStatus status, DateTime startDate, DateTime endDate)
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.DeliveryPerson)
                .Where(d => d.Order.UserId == userId && d.Status == status && d.CreatedAt >= startDate && d.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Delivery>> GetDeliveriesByOrderStatusAndDateRangeAsync(int orderId, DeliveryStatus status, DateTime startDate, DateTime endDate)
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.DeliveryPerson)
                .Where(d => d.OrderId == orderId && d.Status == status && d.CreatedAt >= startDate && d.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Delivery>> GetDeliveriesByAddressStatusAndDateRangeAsync(string address, DeliveryStatus status, DateTime startDate, DateTime endDate)
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.DeliveryPerson)
                .Where(d => d.Address.Contains(address) && d.Status == status && d.CreatedAt >= startDate && d.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Delivery>> GetDeliveriesByTrackingNumberStatusAndDateRangeAsync(string trackingNumber, DeliveryStatus status, DateTime startDate, DateTime endDate)
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                .Include(d => d.DeliveryPerson)
                .Where(d => d.TrackingNumber == trackingNumber && d.Status == status && d.CreatedAt >= startDate && d.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<Delivery> UpdateDeliveryStatusAsync(int id, DeliveryStatus status)
        {
            var delivery = await _context.Deliveries.FindAsync(id);
            if (delivery == null)
            {
                throw new KeyNotFoundException($"Delivery with ID {id} not found");
            }

            delivery.Status = status;
            delivery.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return delivery;
        }

        public async Task<bool> CancelDeliveryAsync(int id)
        {
            var delivery = await _context.Deliveries.FindAsync(id);
            if (delivery == null)
            {
                return false;
            }

            if (delivery.Status == DeliveryStatus.Delivered)
            {
                throw new InvalidOperationException("Cannot cancel a completed delivery");
            }

            delivery.Status = DeliveryStatus.Cancelled;
            delivery.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CompleteDeliveryAsync(int id)
        {
            var delivery = await _context.Deliveries.FindAsync(id);
            if (delivery == null)
            {
                return false;
            }

            if (delivery.Status != DeliveryStatus.InTransit)
            {
                throw new InvalidOperationException("Delivery is not in transit");
            }

            delivery.Status = DeliveryStatus.Delivered;
            delivery.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> TrackDeliveryAsync(int id)
        {
            var delivery = await _context.Deliveries.FindAsync(id);
            if (delivery == null)
            {
                return false;
            }

            // Here you would typically integrate with a delivery tracking service
            // For demo purposes, we'll just return true if the delivery exists
            return true;
        }
    }
} 