using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Penguin.Extensions.String.Html
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public static class StringExtensions
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        /// <summary>
        /// Returns a list of strings representing HTML elements with a certain attribute value
        /// </summary>
        /// <param name="input">The HTML string to search</param>
        /// <param name="attr">The Attribute to search for</param>
        /// <param name="value">The value of the attribute to be searched for</param>
        /// <returns></returns>
        public static IEnumerable<string> GetElementsWithAttributeValue(this string input, string attr, string value)
        {
            MatchCollection matches = Regex.Matches(input, "<.*\\s" + attr + "=.*([^a-z]|\")" + value + "([^a-z]|\").*?>.*?>");

            List<string> output = new();

            foreach (Match thisMatch in matches)
            {
                output.Add(thisMatch.Value);
            }

            return output.Distinct();
        }

        /// <summary>
        /// Returns the first matching HTML attribute value in the source string
        /// </summary>
        /// <param name="input">The HTML string to search</param>
        /// <param name="attr">The Attribute to search for</param>
        /// <returns>The value of the attribute being searched for</returns>
        public static string GetFirstAttribute(this string input, string attr)
        {
            return input is null ? null : !input.Contains(attr + "=") ? string.Empty : input.From(attr).Enclose("\"", false);
        }

        /// <summary>
        /// Attempts to find HTML elements that are not properly closed, for validation
        /// </summary>
        /// <param name="input">The input string to search</param>
        /// <param name="element">The element to search for</param>
        /// <returns></returns>
        public static IList<string> GetUnclosedElements(this string input, string element)
        {
            string[] starts = input.Split("<" + element, true);

            IEnumerable<string> fixedE = starts.Select(s => s.To("</" + element + ">", true));

            IEnumerable<string> filteredE = fixedE.Where(s => s.Contains("</" + element + ">"));

            return filteredE.ToList();
        }

        /// <summary>
        /// Returns the InnerHtml of the element (text between &gt; and lt;)
        /// </summary>
        /// <param name="input">The HTML string representing the element</param>
        /// <returns></returns>
        public static string InnerHtml(this string input)
        {
            return string.IsNullOrWhiteSpace(input) ? string.Empty : input.From(">").To("<");
        }
    }
}