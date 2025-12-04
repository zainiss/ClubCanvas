using System.Collections.Generic;
using System.Threading.Tasks;
using ClubCanvas.Core.Models;

namespace ClubCanvas.Core
{
    public interface IUserRepository
    {
        Task<List<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser?> GetUserByEmailAsync(string email);
        Task AddUserAsync(ApplicationUser user);
        Task UpdateUserAsync(ApplicationUser user);
        Task DeleteUserAsync(ApplicationUser user);
    }
}