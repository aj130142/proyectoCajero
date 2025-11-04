    using System;
using System.Security.Cryptography;
using System.Text;

namespace proyectoCajero
{
    public static class HashHelper
    {
        public static string ComputeSha256Hash(string rawData)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            var sb = new StringBuilder();
            foreach (var b in bytes) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        public static bool VerifySha256Hash(string rawData, string hash)
        {
            var computed = ComputeSha256Hash(rawData);
            return string.Equals(computed, hash, StringComparison.OrdinalIgnoreCase);
        }
    }
}