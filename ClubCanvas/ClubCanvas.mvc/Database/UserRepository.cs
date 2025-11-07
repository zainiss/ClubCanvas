using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClubCanvas.mvc.Models;

namespace ClubCanvas.mvc.Database
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users;

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