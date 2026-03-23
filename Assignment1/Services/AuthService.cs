using System;
using System.Collections.Generic;
using Assignment1.Models;

namespace Assignment1.Services
{
    public class AuthService
    {
        private static List<User> _users = new List<User>
        {
            new User { Id = 1, Username = "admin", Password = "admin123", Email = "admin@lms.com", Role = "Admin", CreatedDate = DateTime.UtcNow },
            new User { Id = 2, Username = "staff", Password = "staff123", Email = "staff@lms.com", Role = "Staff", CreatedDate = DateTime.UtcNow }
        };

        private static int _nextId = 3;

        public List<User> GetAll() => _users;

        public User? GetById(int id) => _users.Find(u => u.Id == id);

        public User? GetByUsername(string username) => _users.Find(u => u.Username == username);

        public (bool Success, string Message, User? User) Register(string username, string email, string password, string role = "Staff")
        {
            if (_users.Exists(u => u.Username == username))
            {
                return (false, "Username already exists", null);
            }

            if (_users.Exists(u => u.Email == email))
            {
                return (false, "Email already exists", null);
            }

            var user = new User
            {
                Id = _nextId++,
                Username = username,
                Email = email,
                Password = password,
                Role = role,
                CreatedDate = DateTime.UtcNow
            };

            _users.Add(user);
            return (true, "Registration successful", user);
        }

        public (bool Success, string Message, User? User) Login(string username, string password)
        {
            var user = _users.Find(u => u.Username == username && u.Password == password);
            
            if (user == null)
            {
                return (false, "Invalid username or password", null);
            }

            return (true, "Login successful", user);
        }

        public bool Update(int id, User updatedUser)
        {
            int index = _users.FindIndex(u => u.Id == id);
            if (index == -1) return false;
            updatedUser.Id = id;
            _users[index] = updatedUser;
            return true;
        }

        public bool Delete(int id)
        {
            return _users.RemoveAll(u => u.Id == id) > 0;
        }
    }
}
