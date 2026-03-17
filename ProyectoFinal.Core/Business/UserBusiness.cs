using System;
using System.Security.Cryptography;
using System.Text;
using ProyectoFinal.Data;
using ProyectoFinal.Data.Repositories;

namespace ProyectoFinal.Core.Business
{
    public class UserBusiness : IUserBusiness
    {
        private readonly IUserRepository _userRepository;

        public UserBusiness()
        {
            _userRepository = new UserRepository();
        }

        public Users ValidateUser(string username, string password)
        {
            var user = _userRepository.GetByUsername(username);

            if (user == null)
                return null;

            var passwordHash = HashPassword(password);

            if (user.PasswordHash != passwordHash)
                return null;

            return user;
        }

        public bool RegisterUser(string username, string password, string email)
        {
            var existingUser = _userRepository.GetByUsername(username);

            if (existingUser != null)
                return false;

            var newUser = new Users
            {
                Username = username,
                PasswordHash = HashPassword(password),
                Email = email,
                Role = "Player",
                MaxScore = 0,
                TotalGames = 0,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _userRepository.Add(newUser);
            _userRepository.Save();

            return true;
        }

        public Users GetUserById(int userId)
        {
            return _userRepository.GetById(userId);
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}
