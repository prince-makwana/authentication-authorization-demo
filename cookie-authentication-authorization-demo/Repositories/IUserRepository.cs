using System.Collections.Generic;
using System.Threading.Tasks;
using cookie_authentication_authorization_demo.Models;

namespace cookie_authentication_authorization_demo.Repositories;

public interface IUserRepository
{
    Task<ApplicationUser> GetByIdAsync(string id);
    Task<ApplicationUser> GetByUserNameAsync(string userName);
    Task<IEnumerable<ApplicationUser>> GetAllAsync();
    Task<ApplicationUser> AddAsync(ApplicationUser user);
    Task<ApplicationUser> UpdateAsync(ApplicationUser user);
    Task<bool> DeleteAsync(string id);
    Task<bool> ExistsAsync(string id);
    Task<bool> ExistsByUserNameAsync(string userName);
} 