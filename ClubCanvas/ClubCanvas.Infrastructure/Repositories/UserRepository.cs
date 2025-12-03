using System.Collections.Generic;
using System.Linq;
using ClubCanvas.Core;
using ClubCanvas.Core.Models;

namespace ClubCanvas.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly List<ApplicationUser> _users;

        public UserRepository()
        {
            _users = new List<ApplicationUser>
            {
                new ApplicationUser { Email = "h@g.com", UserName = "H" },
                new ApplicationUser { Email = "admin@example.com", UserName = "a" }
            };
        }

        public List<ApplicationUser> GetAllUsers()
        {
            return _users;
        }
        public ApplicationUser GetUserByEmail(string email)
        {
            return _users.FirstOrDefault(u => u.Email == email);
        }
        public void AddUser(ApplicationUser user)
        {
            _users.Add(user);
        }
        public void UpdateUser(ApplicationUser updatedUser)
        {
            if (updatedUser != null)
            {
                ApplicationUser userToUpdate = _users.FirstOrDefault(u => u.Email == updatedUser.Email);

                if (userToUpdate != null)
                {
                    userToUpdate = updatedUser;
                }
            }
        }
        public void DeleteUser(ApplicationUser user)
        {
            var userToRemove = _users.FirstOrDefault(u => u.Email == user.Email);
            if (userToRemove != null)
            {
                _users.Remove(userToRemove);
            }
        }
    }
}

