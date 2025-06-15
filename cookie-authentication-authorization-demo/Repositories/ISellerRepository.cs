using System.Collections.Generic;
using System.Threading.Tasks;
using cookie_authentication_authorization_demo.Models;

namespace cookie_authentication_authorization_demo.Repositories;

public interface ISellerRepository
{
    Task<Seller> GetByIdAsync(int id);
    Task<IEnumerable<Seller>> GetAllAsync();
    Task<Seller> AddAsync(Seller seller);
    Task<Seller> UpdateAsync(Seller seller);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
} 