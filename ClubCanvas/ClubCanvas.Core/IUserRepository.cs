using System.Collections.Generic;
using ClubCanvas.Core.Models;

namespace ClubCanvas.Core
{
    public interface IUserRepository
    {
        List<User> GetAllUsers();
        User GetUserByEmail(string email);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
    }
}