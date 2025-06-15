using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cookie_authentication_authorization_demo.Data;
using cookie_authentication_authorization_demo.Models;

namespace cookie_authentication_authorization_demo.Repositories;

public class SellerRepository : ISellerRepository
{
    private readonly ApplicationDbContext _context;

    public SellerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Seller> GetByIdAsync(int id)
    {
        return await _context.Sellers.FindAsync(id);
    }

    public async Task<IEnumerable<Seller>> GetAllAsync()
    {
        return await _context.Sellers.ToListAsync();
    }

    public async Task<Seller> AddAsync(Seller seller)
    {
        _context.Sellers.Add(seller);
        await _context.SaveChangesAsync();
        return seller;
    }

    public async Task<Seller> UpdateAsync(Seller seller)
    {
        _context.Entry(seller).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return seller;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var seller = await _context.Sellers.FindAsync(id);
        if (seller == null)
        {
            return false;
        }
        _context.Sellers.Remove(seller);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Sellers.AnyAsync(s => s.Id == id);
    }
} 