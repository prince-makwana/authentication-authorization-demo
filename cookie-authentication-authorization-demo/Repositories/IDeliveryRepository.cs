using System.Collections.Generic;
using System.Threading.Tasks;
using cookie_authentication_authorization_demo.Models;

namespace cookie_authentication_authorization_demo.Repositories;

public interface IDeliveryRepository
{
    Task<Delivery> GetByIdAsync(int id);
    Task<IEnumerable<Delivery>> GetAllAsync();
    Task<IEnumerable<Delivery>> GetByOrderIdAsync(int orderId);
    Task<Delivery> AddAsync(Delivery delivery);
    Task<Delivery> UpdateAsync(Delivery delivery);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
} 