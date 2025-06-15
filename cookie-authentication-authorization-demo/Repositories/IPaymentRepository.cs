using System.Collections.Generic;
using System.Threading.Tasks;
using cookie_authentication_authorization_demo.Models;
using Microsoft.EntityFrameworkCore;

namespace cookie_authentication_authorization_demo.Repositories;

public interface IPaymentRepository
{
    Task<Payment> GetByIdAsync(int id);
    Task<Payment> GetByIdWithUserAsync(int id);
    Task<IEnumerable<Payment>> GetByUserIdAsync(string userId);
    Task<Payment> AddAsync(Payment payment);
    Task<Payment> UpdateAsync(Payment payment);
    Task<bool> ExistsAsync(int id);
} 