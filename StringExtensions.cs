using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Penguin.Extensions.Strings
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public static class StringExtensions
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        private const string EMPTY_STRING_MESSAGE = "The string to find may not be empty";

        /// <summary>
        /// Strips non-numeric characters from a string
        /// </summary>
        /// <param name="input">The original value</param>
        /// <returns>The value without non-numeric characters</returns>
        public static string ToNumeric(this string input)
        {
            if (input is null)
            {
                return null;
            }

            char[] result = new char[input.Length];

            int index = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsDigit(input[i]))
                {
                    result[index++] = input[i];
                }
            }

            return new string(result, 0, index);
        }

        /// <summary>
        /// Returns a list of indexes for the specified string
        /// </summary>
        /// <param name="str">The source string</param>
        /// <param name="value">The string to find</param>
        /// <param name="comparisonType">The comparison type to pass into the index function</param>
        /// <returns>A list of indexes where the value is found</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "<Pending>")]
        public static IEnumerable<int> AllIndexesOf(this string str, string value, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException(EMPTY_STRING_MESSAGE, nameof(value));
            }

            for (int index = 0; ; index++)
            {
                index = str.IndexOf(value, index, comparisonType);
                if (index == -1)
                {
                    yield break;
                }

                yield return index;
            }
        }

        /// <summary>
        /// Checks if the parent string contains the substring, with optional case inequality
        /// </summary>
        /// <param name="s">The source string</param>
        /// <param name="search">The substring to search for</param>
        /// <param name="comparisonType">The comparison type to pass into the index function</param>
        /// <returns>A value indicating if the substring was found</returns>
        public static bool Contains(this string s, string search, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (s is null)
            {
                return false;
            }

            return s.IndexOf(search, comparisonType) >= 0;
        }

        /// <summary>
        /// Finds a substring between two anchor characters. Allows for nested
        /// </summary>
        /// <param name="input">The string to search</param>
        /// <param name="openingclosing">The opening and closing character</param>
        /// <param name="inclusive">Bool indicating whether or not the returned string should include the enclosing characters</param>
        /// <returns>The substring between the nested characters</returns>
        public static string Enclose(this string input, string openingclosing, bool inclusive = true)
        {
            return input.Enclose(openingclosing, openingclosing, inclusive);
        }

        /// <summary>
        /// Finds a substring between two anchor characters. Allows for nested
        /// </summary>
        /// <param name="input">The string to search</param>
        /// <param name="opening">The opening character</param>
        /// <param name="closing">The closing character</param>
        /// <param name="inclusive"></param>
        /// <returns>The substring between the nested characters</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "<Pending>")]
        public static string Enclose(this string input, string opening, string closing, bool inclusive = true)
        {
            if (input is null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(opening))
            {
                throw new ArgumentException(EMPTY_STRING_MESSAGE, nameof(opening));
            }

            if (string.IsNullOrEmpty(closing))
            {
                throw new ArgumentException(EMPTY_STRING_MESSAGE, nameof(closing));
            }

            int count = 0;
            bool started = false;

            int openingIndex = 0;
            string result = string.Empty;

            for (int i = 0; i < input.Length; i++)
            {
                if (input.Substring(i, opening.Length) == opening && (!started || closing != opening))
                {
                    if (started == false)
                    {
                        openingIndex = i + (inclusive ? 0 : opening.Length);
                        started = true;
                    }

                    count++;
                }
                else if (input.Substring(i, closing.Length) == closing)
                {
                    count--;
                }

                if (started && count == 0)
                {
                    int closingIndex = i + (inclusive ? closing.Length : 0);
                    result = input.Substring(openingIndex, closingIndex - openingIndex);
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Returns the portion of the source string after the first instance of the delimiter
        /// </summary>
        /// <param name="s">The source string</param>
        /// <param name="fromText">The delimiter</param>
        /// <param name="inclusive">A bool indicating whether or not the delimiter should be returned with the result</param>
        /// <param name="comparison">The string comparison to use when searching for a match</param>
        /// <returns>The substring found after the first instance of the delimiter</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "<Pending>")]
        public static string From(this string s, string fromText, bool inclusive = false, StringComparison comparison = StringComparison.Ordinal)
        {
            if (s is null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(fromText))
            {
                throw new ArgumentException(EMPTY_STRING_MESSAGE, nameof(fromText));
            }

            int i = s.IndexOf(fromText, comparison);

            if (i >= 0)
            {
                if (inclusive)
                {
                    return s.Substring(i);
                }
                else
                {
                    return s.Substring(i + fromText.Length);
                }
            }

            return s;
        }

        /// <summary>
        /// Splits a string on \r and \n\ (individually) and returns any "lines" in trimmed form, that are not null or whitespace
        /// </summary>
        /// <param name="s">The string to split</param>
        /// <returns>Any trimmed lines that are not null or whitespace</returns>
        public static IEnumerable<string> TrimLines(this string s)
        {
            if (s is null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            foreach (string r in s.Split('\r'))
            {
                foreach (string n in r.Split('\n'))
                {
                    string t = n.Trim();

                    if (!string.IsNullOrWhiteSpace(t))
                    {
                        yield return t;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the portion of the source string after the last instance of the delimiter
        /// </summary>
        /// <param name="s">The source string</param>
        /// <param name="fromText">The delimiter</param>
        /// <returns>The substring found after the last instance of the delimiter</returns>
        public static string FromLast(this string s, string fromText)
        {
            if (s is null)
            {
                return null;
            }

            if (fromText is null)
            {
                throw new ArgumentNullException(nameof(fromText));
            }

            int i = s.LastIndexOf(fromText, StringComparison.OrdinalIgnoreCase);

            if (i >= 0)
            {
                return s.Substring(i + fromText.Length);
            }

            return s;
        }

        /// <summary>
        /// Returns the portion of the source string after the last instance of the delimiter
        /// </summary>
        /// <param name="s">The source string</param>
        /// <param name="fromText">The delimiter</param>
        /// <returns>The substring found after the last instance of the delimiter</returns>
        public static string FromLast(this string s, char fromText)
        {
            if (s is null)
            {
                return null;
            }

            return s.Substring(s.LastIndexOf(fromText) + 1);
        }

        /// <summary>
        /// Returns the leftmost portion of a string of a specified length
        /// </summary>
        /// <param name="str">The source string</param>
        /// <param name="count">The number of characters to return</param>
        /// <returns>A substring of the specified length from the source string</returns>
        public static string Left(this string str, int count)
        {
            return str?.Substring(0, count);
        }

        /// <summary>
        /// Removes all instances of the specified string, from the source (using Replace)
        /// </summary>
        /// <param name="input">The source string</param>
        /// <param name="toRemove">The substring to remove</param>
        /// <returns>The source string without any instances of the specified substring</returns>
        public static string Remove(this string input, string toRemove)
        {
            if (input is null)
            {
                return input;
            }

            return input.Replace(toRemove, string.Empty);
        }

        /// <summary>
        /// Replaces a character at the specified index with another character
        /// </summary>
        /// <param name="input">The source string</param>
        /// <param name="index">The index at which to replace the character</param>
        /// <param name="newChar">The new character to insert</param>
        /// <returns>A new string with the character replaced</returns>
        public static string ReplaceAt(this string input, int index, char newChar)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            char[] chars = input.ToCharArray();
            chars[index] = newChar;
            return new string(chars);
        }

        /// <summary>
        /// Returns the rightmost portion of a string of a specified length
        /// </summary>
        /// <param name="str">The source string</param>
        /// <param name="count">The number of characters to return</param>
        /// <returns>The rightmost portion of the source string of a specified length</returns>
        public static string Right(this string str, int count)
        {
            if (str is null)
            {
                return null;
            }

            return str.Substring(str.Length - count);
        }

        /// <summary>
        /// Splits a string on a delimiter
        /// </summary>
        /// <param name="input">The source string</param>
        /// <param name="spliton">The delimiter to split on</param>
        /// <param name="preserveSplit">A bool indicating whether or not to append the delimiter back to the results</param>
        /// <param name="options">String split options for the initial split</param>
        /// <returns>An array of strings</returns>
        public static string[] Split(this string input, string spliton, bool preserveSplit = false, StringSplitOptions options = StringSplitOptions.None)
        {
            if (input is null)
            {
                return null;
            }

            string[] output = input.Split(new string[] { spliton }, options);

            if (preserveSplit)
            {
                output = output.Select(s => spliton + s).ToArray();
            }

            return output;
        }

        /// <summary>
        /// Splits a CamelCase string on each uppercase letter by adding a space in front of each
        /// </summary>
        /// <param name="str">The string to split</param>
        /// <returns>A string where each uppercase letter is preceded by a space</returns>
        public static string SplitCamelCase(this string str)
        {
            if (str is null)
            {
                return null;
            }

            int uppers = 0;

            foreach (char c in str.Skip(1))
            {
                if (char.IsUpper(c))
                {
                    uppers++;
                }
            }

            char[] toReturn = new char[str.Length + uppers];

            toReturn[0] = str[0];

            int index = 1;
            for (int i = 1; i < str.Length; i++)
            {
                if (char.IsUpper(str[i]))
                {
                    toReturn[index++] = ' ';
                }
                toReturn[index++] = str[i];
            }

            return new string(toReturn);
        }

        [Obsolete("Switch to SplitQuotedString")]
        public static IEnumerable<string> SplitCSVRow(this string row, char delimiter = ',')
        {
            return row.SplitQuotedString(delimiter);
        }

        /// <summary>
        /// Splits a CSV row on the specified delimeter. Supports quoted
        /// </summary>
        /// <param name="toSplit">The string to split</param>
        /// <param name="delimiter">The column delimiter</param>
        /// <param name="stripQuotes"></param>
        /// <returns>An IEnumerable used to obtain the split values</returns>
        public static IEnumerable<string> SplitQuotedString(this IEnumerable<char> toSplit, char delimiter = ',', bool stripQuotes = true)
        {
            StringBuilder currentString = new StringBuilder();
            bool inQuotes = false;
            bool quoteIsEscaped = false; //Store when a quote has been escaped.
            toSplit = toSplit.Concat(new List<char>() { delimiter }); //We add new cells at the delimiter, so append one for the parser.

            int index = -1;

            IEnumerator<char> CharEnumerator = toSplit.GetEnumerator();

            bool hasNextChar = CharEnumerator.MoveNext();

            while (hasNextChar)
            {
                char c = CharEnumerator.Current;
                hasNextChar = CharEnumerator.MoveNext();
                char nextChar = CharEnumerator.Current;

                index++;

                if (c == delimiter) //We hit a delimiter character...
                {
                    if (!inQuotes) //Are we inside quotes? If not, we've hit the end of a cell value.
                    {
                        yield return currentString.ToString();
                        currentString.Clear();
                    }
                    else
                    {
                        currentString.Append(c);
                    }
                }
                else
                {
                    if (c != ' ')
                    {
                        if (c == '"') //If we've hit a quote character...
                        {
                            if (inQuotes) //Does it appear to be a closing quote? //How does this even work? How can both of these be true? I didn't write this.. I dont know...
                            {
                                if (nextChar == c && !quoteIsEscaped) //If the character afterwards is also a quote, this is to escape that (not a closing quote).
                                {
                                    quoteIsEscaped = true; //Flag that we are escaped for the next character. Don't add the escaping quote.

                                    if (!stripQuotes) //unless we want to
                                    {
                                        currentString.Append(c);
                                    }
                                }
                                else if (quoteIsEscaped)
                                {
                                    quoteIsEscaped = false; //This is an escaped quote. Add it and revert quoteIsEscaped to false.
                                    currentString.Append(c);
                                }
                                else
                                {
                                    inQuotes = false;

                                    if (!stripQuotes)
                                    {
                                        currentString.Append(c);
                                    }
                                }
                            }
                            else
                            {
                                if (!inQuotes)
                                {
                                    inQuotes = true;

                                    if (!stripQuotes)
                                    {
                                        currentString.Append(c);
                                    }
                                }
                                else
                                {
                                    currentString.Append(c); //...It's a quote inside a quote.
                                }
                            }
                        }
                        else
                        {
                            currentString.Append(c);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(currentString.ToString())) //Append only if not new cell
                        {
                            currentString.Append(c);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns the portion of a string up until the first instance of a delimiter
        /// </summary>
        /// <param name="s">The source</param>
        /// <param name="toText">The delimeter</param>
        /// <param name="inclusive">A bool indicating whether or not the delimiter should be included in the return</param>
        /// <param name="comparison">String comparison options</param>
        /// <returns>The portion of a string up until the first instance of a delimiter</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "<Pending>")]
        public static string To(this string s, string toText, bool inclusive = false, StringComparison comparison = StringComparison.Ordinal)
        {
            if (s is null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(toText))
            {
                throw new ArgumentException(EMPTY_STRING_MESSAGE, nameof(toText));
            }

            int i = s.IndexOf(toText, comparison);

            if (i >= 0)
            {
                if (inclusive)
                {
                    return s.Substring(0, i + toText.Length);
                }
                else
                {
                    return s.Substring(0, i);
                }
            }

            return s;
        }

        /// <summary>
        /// Strips non-alphanumeric characters from the string
        /// </summary>
        /// <param name="str">The source string</param>
        /// <returns>The source string in AlphaNumeric</returns>
        public static string ToAlphaNumeric(this string str)
        {
            if (str is null)
            {
                return null;
            }

            char[] arr = str.ToCharArray();

            arr = Array.FindAll<char>(arr, c => (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '-'));

            return new string(arr);
        }

        /// <summary>
        /// Splits a string into a dictionary as denoted by the provided K/V separator and KVP delimeter characters
        /// </summary>
        /// <param name="source">The source string to split</param>
        /// <param name="delimeter">The character that separates the key value pairs</param>
        /// <param name="separator">The character that separates the key and value within the pair</param>
        /// <returns>A dictionary representing the values</returns>
        public static Dictionary<string, string> ToDictionary(this string source, char delimeter = ';', char separator = '=')
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (!source.Contains(separator))
            {
                return new Dictionary<string, string>();
            }

            int eq;
            int sc;

            if (Vector.IsHardwareAccelerated)
            {
                eq = source.VectorizedCount(separator);

                sc = source.VectorizedCount(delimeter);
            }
            else
            {
                eq = source.Count(c => c == separator);

                sc = source.Count(c => c == delimeter);
            }

            if (sc != eq - 1 && sc != eq)
            {
                return new Dictionary<string, string>();
            }

            Dictionary<string, string> toReturn = new Dictionary<string, string>(eq);

            foreach (string skvp in source.Trim(delimeter).Split(delimeter))
            {
                string[] vs = skvp.Split(separator);

                toReturn.Add(vs[0], vs[1]);
            }

            return toReturn;
        }

        /// <summary>
        /// Parses a string to its Int value by stripping out invalid characters
        /// </summary>
        /// <param name="input">The input string to parse</param>
        /// <returns>An integer representing the string value, or 0 if empty</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>")]
        public static int ToInt(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return 0;
            }

            char[] toReturn = new char[input.Length];

            int index = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '-' || char.IsDigit(input[i]))
                {
                    toReturn[index++] = input[i];
                }
            }

            if (index == 0)
            {
                return 0;
            }
            else
            {
                return int.Parse(new string(toReturn, 0, index));
            }
        }

        /// <summary>
        /// Returns a portion of a string up to the last instance of a specified delimiter
        /// </summary>
        /// <param name="s">The source string</param>
        /// <param name="toText">The delimiter</param>
        /// <param name="inclusive">Whether or not to return the delimiter as part of result</param>
        /// <param name="comparison">The string comparison to use when searching</param>
        /// <returns>A portion of a string up to the last instance of a specified delimiter</returns>
        public static string ToLast(this string s, string toText, bool inclusive = false, StringComparison comparison = StringComparison.Ordinal)
        {
            if (s is null)
            {
                return null;
            }

            if (toText is null)
            {
                throw new ArgumentNullException(nameof(toText));
            }

            int i = s.LastIndexOf(toText, comparison);

            if (i >= 0)
            {
                if (inclusive)
                {
                    return s.Substring(0, i + toText.Length);
                }
                else
                {
                    return s.Substring(0, i);
                }
            }

            return s;
        }

        /// <summary>
        /// Returns a portion of a string up to the last instance of a specified delimiter
        /// </summary>
        /// <param name="s">The source string</param>
        /// <param name="toText">The delimiter</param>
        /// <param name="inclusive">Whether or not to return the delimiter as part of result</param>
        /// <returns>A portion of a string up to the last instance of a specified delimiter</returns>
        public static string ToLast(this string s, char toText, bool inclusive = false)
        {
            return s?.Substring(0, s.LastIndexOf(toText) + (inclusive ? 1 : 0));
        }
    }
}