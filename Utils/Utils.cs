using System.Security.Cryptography;
using System.Text;

namespace Utils
{
    public static class Utils
    {
        public static string PasswordHash(string password)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = sha256.ComputeHash(bytes);

            StringBuilder hashString = new();

            foreach (byte b in hashBytes)
            {
                hashString.Append(b.ToString("x2"));
            }

            return hashString.ToString();
        }
    }
}
