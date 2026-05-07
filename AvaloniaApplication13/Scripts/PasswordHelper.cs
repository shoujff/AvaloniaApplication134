using System;
using System.Security.Cryptography;
using System.Text;

namespace AvaloniaApplication13.Scripts
{
    public static class PasswordHelper
    {
        public static string Hash(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

      
    }
}