using System.Collections.Generic;
using ClubCanvas.Core.Models;

namespace ClubCanvas.Core
{
    public interface IUserRepository
    {
        List<ApplicationUser> GetAllUsers();
        ApplicationUser GetUserByEmail(string email);
        void AddUser(ApplicationUser user);
        void UpdateUser(ApplicationUser user);
        void DeleteUser(ApplicationUser user);
    }
}