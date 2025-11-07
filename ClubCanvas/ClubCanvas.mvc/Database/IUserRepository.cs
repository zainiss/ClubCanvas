using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClubCanvas.mvc.Models;

namespace ClubCanvas.mvc.Database
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