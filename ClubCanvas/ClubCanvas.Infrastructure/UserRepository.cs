using System.Collections.Generic;
using System.Linq;
using ClubCanvas.Core;
using ClubCanvas.Core.Models;

namespace ClubCanvas.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users;

        public UserRepository()
        {
            _users = new List<User>
            {
                new User { Email = "h@g.com", Username = "H" , Password = "g" },
                new User { Email = "admin@example.com", Username = "a", Password = "123" }
            };
        }

        public List<User> GetAllUsers()
        {
            return _users;
        }
        public User GetUserByEmail(string email)
        {
            return _users.FirstOrDefault(u => u.Email == email);
        }
        public void AddUser(User user)
        {
            _users.Add(user);
        }
        public void UpdateUser(User updatedUser)
        {
            if (updatedUser != null)
            {
                User userToUpdate = _users.FirstOrDefault(u => u.Email == updatedUser.Email);

                if (userToUpdate != null)
                {
                    userToUpdate = updatedUser;
                }
            }
        }
        public void DeleteUser(User user)
        {
            var userToRemove = _users.FirstOrDefault(u => u.Email == user.Email);
            if (userToRemove != null)
            {
                _users.Remove(userToRemove);
            }
        }
    }
}