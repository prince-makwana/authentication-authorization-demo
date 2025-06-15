using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cookie_authentication_authorization_demo.Data;
using cookie_authentication_authorization_demo.Models;

namespace cookie_authentication_authorization_demo.Repositories;

public class DeliveryRepository : IDeliveryRepository
{
    private readonly ApplicationDbContext _context;

    public DeliveryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Delivery> GetByIdAsync(int id)
    {
        return await _context.Deliveries.FindAsync(id);
    }

    public async Task<IEnumerable<Delivery>> GetAllAsync()
    {
        return await _context.Deliveries.ToListAsync();
    }

    public async Task<IEnumerable<Delivery>> GetByOrderIdAsync(int orderId)
    {
        return await _context.Deliveries.Where(d => d.OrderId == orderId).ToListAsync();
    }

    public async Task<Delivery> AddAsync(Delivery delivery)
    {
        _context.Deliveries.Add(delivery);
        await _context.SaveChangesAsync();
        return delivery;
    }

    public async Task<Delivery> UpdateAsync(Delivery delivery)
    {
        _context.Entry(delivery).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return delivery;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var delivery = await _context.Deliveries.FindAsync(id);
        if (delivery == null)
        {
            return false;
        }
        _context.Deliveries.Remove(delivery);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Deliveries.AnyAsync(d => d.Id == id);
    }
} 