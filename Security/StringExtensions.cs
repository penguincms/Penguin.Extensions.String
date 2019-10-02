using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Penguin.Extensions.Strings.Security
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public static class StringExtensions
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        /// <summary>
        /// Computes an SHA256 hash
        /// </summary>
        /// <param name="input">The string to hash</param>
        /// <returns>The hashed string</returns>
        public static string ComputeSha256Hash(this string input)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2", CultureInfo.InvariantCulture));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Computes an SHA1 hash
        /// </summary>
        /// <param name="input">The string to hash</param>
        /// <returns>The hashed string</returns>
        public static string ComputeSha1Hash(this string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("x2", CultureInfo.InvariantCulture));
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// Generates a salted MD5 hash of the string
        /// </summary>
        /// <param name="plainText">The input string</param>
        /// <param name="saltBytes">Optional bytes to override the default salt</param>
        /// <returns>The hashed string</returns>
        [Obsolete("Use ComputeSha512Hash")]
        public static string SHA512(this string plainText, byte[] saltBytes = null) => plainText.ComputeSha512Hash(saltBytes);

        /// <summary>
        /// Generates a salted MD5 hash of the string
        /// </summary>
        /// <param name="input">The input string</param>
        /// <param name="saltBytes">Optional bytes to override the default salt</param>
        /// <returns>The hashed string</returns>
        public static string ComputeSha512Hash(this string input, byte[] saltBytes = null)
        {
            if (input is null)
            {
                return null;
            }

            saltBytes = saltBytes ?? new byte[6] { 0, 7, 2, 6, 9, 5 };

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(input);
            byte[] plainTextWithSaltBytes = new byte[plainTextBytes.Length + saltBytes.Length];

            for (int i = 0; i < plainTextBytes.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainTextBytes[i];
            }

            for (int i = 0; i < saltBytes.Length; i++)
            {
                plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];
            }

            using (HashAlgorithm hash = new SHA512Managed())
            {

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

                return Convert.ToBase64String(hashWithSaltBytes);
            }
        }
    }
}