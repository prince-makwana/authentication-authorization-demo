using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cookie_authentication_authorization_demo.Data;
using cookie_authentication_authorization_demo.Models;
using Microsoft.EntityFrameworkCore;

namespace cookie_authentication_authorization_demo.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly ApplicationDbContext _context;

    public PaymentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Payment> GetByIdAsync(int id)
    {
        return await _context.Payments.FindAsync(id);
    }

    public async Task<Payment> GetByIdWithUserAsync(int id)
    {
        return await _context.Payments
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Payment>> GetByUserIdAsync(string userId)
    {
        return await _context.Payments
            .Include(p => p.User)
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<Payment> AddAsync(Payment payment)
    {
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();
        return payment;
    }

    public async Task<Payment> UpdateAsync(Payment payment)
    {
        _context.Entry(payment).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return payment;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Payments.AnyAsync(p => p.Id == id);
    }
} 