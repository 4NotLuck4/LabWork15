using AuthLibrary.Contexts;
using AuthLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
namespace AuthLibrary.Services
{
    public class AuthService
    {
        private readonly DatabaseLibrary _context;
        public AuthService(DatabaseLibrary context)
        {
            _context = context;
        }
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
        public bool Register(string login, string password)
        { 
        if (_context.CinemaUsers.Any(u =>u.Login == login)) 
                return false;
            var user = new CinemaUser
            {
                Login = login,
                PasswordHash = HashPassword(password),
                RoleId = 3
            };
            _context.CinemaUsers.Add(user);
            _context.SaveChanges();
            return true;
        }
        public CinemaUser Authenticate(string login, string password) {
            var user = _context.CinemaUsers
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Login == login);
            if (user == null) return null;
            if (user.UnlockDate.HasValue && user.UnlockDate > DateTime.Now)
                throw new Exception("Аккаунт заблокирован.Попробуйте позже.");

            if (user.PasswordHash != HashPassword(password))
            {
                user.FailedLoginAttempts++;

                if (user.FailedLoginAttempts >= 3)
                {
                    user.UnlockDate = DateTime.Now;
                }

    }
}
