namespace Penguin.Extensions.String.Compatibility
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public static class StringExtensions
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        /// <summary>
        /// Adds the ability to call IsNullOrWhiteSpace as an extension
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <returns>A bool representing the return of String.IsNullOrWhitespace(value)</returns>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            if (value is null)
            {
                return true;
            }

            return string.IsNullOrEmpty(value.Trim());
        }
    }
}