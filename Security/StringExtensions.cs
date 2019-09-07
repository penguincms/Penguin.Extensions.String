using System.Security.Cryptography;
using System.Text;

namespace Penguin.Extensions.String.Security
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public static class StringExtensions
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        /// <summary>
        /// Generates a salted MD5 hash of the string
        /// </summary>
        /// <param name="plainText">The input string</param>
        /// <param name="saltBytes">Optional bytes to override the default salt</param>
        /// <returns></returns>
        public static string SHA512(this string plainText, byte[] saltBytes = null)
        {
            if (plainText is null)
            {
                return null;
            }

            saltBytes = saltBytes ?? new byte[6] { 0, 7, 2, 6, 9, 5 };

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] plainTextWithSaltBytes = new byte[plainTextBytes.Length + saltBytes.Length];

            for (int i = 0; i < plainTextBytes.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainTextBytes[i];
            }

            for (int i = 0; i < saltBytes.Length; i++)
            {
                plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];
            }

            HashAlgorithm hash;
            hash = new SHA512Managed();

            byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);
            byte[] hashWithSaltBytes = new byte[hashBytes.Length + saltBytes.Length];

            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashWithSaltBytes[i] = hashBytes[i];
            }

            for (int i = 0; i < saltBytes.Length; i++)
            {
                hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];
            }

            string hashValue = System.Convert.ToBase64String(hashWithSaltBytes);

            return hashValue;
        }
    }
}