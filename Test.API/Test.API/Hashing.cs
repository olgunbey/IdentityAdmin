using System.Security.Cryptography;
using System.Text;

namespace Test.API
{
    public class Hashing
    {
        public static byte[] Hash(string secretKey)
        {
            byte[] byteSecretKey;
            using (var hash = SHA512.Create())
            {
                byteSecretKey = hash.ComputeHash(Encoding.UTF8.GetBytes(secretKey));
            }
            return byteSecretKey;
        }
    }
}
