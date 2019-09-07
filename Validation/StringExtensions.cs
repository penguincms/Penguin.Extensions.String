using System.Text.RegularExpressions;

namespace Penguin.Extensions.String.Validation
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public static class StringExtensions
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        /// <summary>
        /// Attempts to validate whether or not a string represents a valid email address
        /// </summary>
        /// <param name="str">The string to validate</param>
        /// <returns>A bool representing whether or not the email is valid</returns>
        public static bool IsValidEmail(this string str) => Regex.IsMatch(str, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

        /// <summary>
        /// Attempts to validate whether or not a string represents a valid Url
        /// </summary>
        /// <param name="toTest">The string to validate</param>
        /// <returns>A bool representing whether or not the Url is valid</returns>
        public static bool IsValidUrl(this string toTest) => Regex.IsMatch(toTest, @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,; .]+$");
    }
}