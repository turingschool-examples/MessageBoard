using System.Security.Cryptography;
using System.Text;

namespace MessageBoard.Services
{
    public class HashService
    {
        public static string Digest(string password)
        {
            var sha = SHA256.Create();

            byte[] bytesDigested = sha.ComputeHash(Encoding.ASCII.GetBytes(password));
            StringBuilder digest = new StringBuilder();
            foreach (byte b in bytesDigested)
            {
                digest.Append(b.ToString("x2"));
            }

            return digest.ToString();
        }
    }
}
