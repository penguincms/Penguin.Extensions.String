using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Penguin.Extensions.String
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class StringExtensions
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        #region Methods

        /// <summary>
        /// Strips non-numeric characters from a string
        /// </summary>
        /// <param name="input">The original value</param>
        /// <returns>The value without non-numeric characters</returns>
        public static string ToNumeric(this string input)
        {
            return new string(input.Where(c => char.IsDigit(c)).ToArray());
        }

        /// <summary>
        /// Returns a list of indexes for the specified string
        /// </summary>
        /// <param name="str">The source string</param>
        /// <param name="value">The string to find</param>
        /// <returns>A list of indexes where the value is found</returns>
        public static List<int> AllIndexesOf(this string str, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("the string to find may not be empty", "value");
            }

            List<int> indexes = new List<int>();
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                {
                    return indexes;
                }

                indexes.Add(index);
            }
        }

        /// <summary>
        /// Checks if the parent string contains the substring, with optional case inequality
        /// </summary>
        /// <param name="s">The source string</param>
        /// <param name="search">The substring to search for</param>
        /// <param name="ignoreCase">Optional bool for ignoring case</param>
        /// <returns>A value indicating if the substring was found</returns>
        public static bool Contains(this string s, string search, bool ignoreCase)
        {
            if (s == null)
            {
                return false;
            }

            if (ignoreCase)
            {
                return s.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            else
            {
                return s.Contains(search);
            }
        }

        /// <summary>
        /// Finds a substring between two anchor characters. Allows for nested
        /// </summary>
        /// <param name="input">The string to search</param>
        /// <param name="openingclosing">The opening and closing character</param>
        /// <param name="inclusive">Bool indicating whether or not the returned string should include the enclosing characters</param>
        /// <returns>The substring between the nested characters</returns>
        public static string Enclose(this string input, string openingclosing, bool inclusive = true) => input.Enclose(openingclosing, openingclosing, inclusive);

        /// <summary>
        ///
        /// </summary>
        /// <param name="input">Finds a substring between two anchor characters. Allows for nested</param>
        /// <param name="opening">The opening character</param>
        /// <param name="closing">The closing character</param>
        /// <param name="inclusive"></param>
        /// <returns>The substring between the nested characters</returns>
        public static string Enclose(this string input, string opening, string closing, bool inclusive = true)
        {
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
        /// <param name="comparison">The string comparision to use when searching for a match</param>
        /// <returns>The substring found after the first instance of the delimiter</returns>
        public static string From(this string s, string fromText, bool inclusive = false, StringComparison comparison = StringComparison.Ordinal)
        {
            string thisString = s;

            if (s is null)
            {
                return null;
            }

            if (s.IndexOf(fromText, comparison) >= 0)
            {
                if (inclusive)
                {
                    thisString = s.Substring(s.IndexOf(fromText, comparison));
                }
                else
                {
                    thisString = s.Substring(s.IndexOf(fromText, comparison) + fromText.Length);
                }
            }

            return thisString;
        }

        /// <summary>
        /// Returns the portion of the source string after the last instance of the delimiter
        /// </summary>
        /// <param name="s">The source string</param>
        /// <param name="fromText">The delimiter</param>
        /// <returns>The substring found after the last instance of the delimiter</returns>
        public static string FromLast(this string s, string fromText)
        {
            string thisString = s;

            if (s is null)
            {
                return null;
            }

            if (s.Contains(fromText))
            {
                thisString = s.Substring(s.LastIndexOf(fromText) + fromText.Length);
            }

            return thisString;
        }

        /// <summary>
        /// Returns the portion of the source string after the last instance of the delimiter
        /// </summary>
        /// <param name="s">The source string</param>
        /// <param name="fromText">The delimiter</param>
        /// <returns>The substring found after the last instance of the delimiter</returns>
        public static string FromLast(this string s, char fromText)
        {
            string thisString = s;

            if (s is null)
            {
                return null;
            }

            if (s.Contains(fromText))
            {
                thisString = s.Substring(s.LastIndexOf(fromText) + 1);
            }

            return thisString;
        }

        /// <summary>
        /// Returns the leftmost portion of a string of a specified length
        /// </summary>
        /// <param name="str">The source string</param>
        /// <param name="count">The number of characters to return</param>
        /// <returns>A substring of the specified length from the source string</returns>
        public static string Left(this string str, int count) => str.Substring(0, count);

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
        /// Replaces a character at the specified index with antoher character
        /// </summary>
        /// <param name="input">The source string</param>
        /// <param name="index">The index at which to replace the character</param>
        /// <param name="newChar">The new character to insert</param>
        /// <returns>A new string with the character replaced</returns>
        public static string ReplaceAt(this string input, int index, char newChar)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
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
            string[] output = input.Split(new string[] { spliton }, options);

            if (preserveSplit)
            {
                output = output.Select(s => spliton + s).ToArray();
            }

            return output;
        }

        /// <summary>
        /// Splits a string on substrings by first replacing it with the specified char
        /// </summary>
        /// <param name="s">The source string to split</param>
        /// <param name="spliton">The string to split on</param>
        /// <param name="tempDelimiter">The temporary character to use as a delimiter</param>
        /// <returns>An array of strings</returns>
        public static string[] Split(this string s, string spliton, char tempDelimiter = '§')
        {
            if (s.Contains(tempDelimiter))
            {
                throw new ArgumentException("The source string can not contain the temporary delimiter");
            }

            return s.Replace(spliton, tempDelimiter.ToString()).Split(tempDelimiter);
        }

        /// <summary>
        /// Splits a CamelCase string on each uppercase letter by adding a space in front of each
        /// </summary>
        /// <param name="str">The string to split</param>
        /// <returns>A string where each uppercase letter is preceded by a space</returns>
        public static string SplitCamelCase(this string str) => Regex.Replace(
                Regex.Replace(
                    str,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    "$1 $2"
                ),
                @"(\p{Ll})(\P{Ll})",
                "$1 $2"
            );

        /// <summary>
        /// Splits a CSV row on the specified delimeter. Supports quoted
        /// </summary>
        /// <param name="row">The string to split</param>
        /// <param name="delimiter">The column delimiter</param>
        /// <returns>An IEnumerable used to obtain the split values</returns>
        public static IEnumerable<string> SplitCSVRow(this string row, char delimiter = ',')
        {
            StringBuilder currentString = new StringBuilder();
            bool inQuotes = false;
            bool quoteIsEscaped = false; //Store when a quote has been escaped.
            row = string.Format("{0}{1}", row, delimiter); //We add new cells at the delimiter, so append one for the parser.
            foreach (var character in row.Select((val, index) => new { val, index }))
            {
                if (character.val == delimiter) //We hit a delimiter character...
                {
                    if (!inQuotes) //Are we inside quotes? If not, we've hit the end of a cell value.
                    {
                        yield return currentString.ToString();
                        currentString.Clear();
                    }
                    else
                    {
                        currentString.Append(character.val);
                    }
                }
                else
                {
                    if (character.val != ' ')
                    {
                        if (character.val == '"') //If we've hit a quote character...
                        {
                            if (character.val == '\"' && inQuotes) //Does it appear to be a closing quote?
                            {
                                if (row[character.index + 1] == character.val) //If the character afterwards is also a quote, this is to escape that (not a closing quote).
                                {
                                    quoteIsEscaped = true; //Flag that we are escaped for the next character. Don't add the escaping quote.
                                }
                                else if (quoteIsEscaped)
                                {
                                    quoteIsEscaped = false; //This is an escaped quote. Add it and revert quoteIsEscaped to false.
                                    currentString.Append(character.val);
                                }
                                else
                                {
                                    inQuotes = false;
                                }
                            }
                            else
                            {
                                if (!inQuotes)
                                {
                                    inQuotes = true;
                                }
                                else
                                {
                                    currentString.Append(character.val); //...It's a quote inside a quote.
                                }
                            }
                        }
                        else
                        {
                            currentString.Append(character.val);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(currentString.ToString())) //Append only if not new cell
                        {
                            currentString.Append(character.val);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks if a specified string starts with another string
        /// </summary>
        /// <param name="s">The string to check</param>
        /// <param name="search">The substring to test for</param>
        /// <returns>A bool indicating whether or not the source starts with the substring</returns>
        public static bool StartsWith(this string s, string search) => s.Substring(0, search.Length) == search;

        /// <summary>
        /// Returns the portion of a string up until the first instance of a delimiter
        /// </summary>
        /// <param name="s">The source</param>
        /// <param name="toText">The delimeter</param>
        /// <param name="inclusive">A bool indicating whether or not the delimiter should be included in the return</param>
        /// <param name="comparison">String comparison options</param>
        /// <returns>The portion of a string up until the first instance of a delimiter</returns>
        public static string To(this string s, string toText, bool inclusive = false, StringComparison comparison = StringComparison.Ordinal)
        {
            if (s is null)
            { return null; }

            string thisString = s;

            if (s.IndexOf(toText, comparison) >= 0)
            {
                if (inclusive)
                {
                    thisString = s.Substring(0, s.IndexOf(toText, comparison) + toText.Length);
                }
                else
                {
                    thisString = s.Substring(0, s.IndexOf(toText, comparison));
                }
            }

            return thisString;
        }

        /// <summary>
        /// Strips non-alphanumeric characters from the string
        /// </summary>
        /// <param name="str">The source string</param>
        /// <returns>The source string in AlphaNumeric</returns>
        public static string ToAlphaNumeric(this string str)
        {
            if (str == null)
            {
                return null;
            }

            char[] arr = str.ToCharArray();

            arr = Array.FindAll<char>(arr, c => (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '-'));

            return new string(arr);
        }

        /// <summary>
        /// Splits a string into a dictionary as denoted by = and ; characters
        /// </summary>
        /// <param name="source">The source string to split</param>
        /// <returns>A dictionary representing the values</returns>
        public static Dictionary<string, string> ToDictionary(this string source)
        {
            if (!source.Contains("="))
            {
                return new Dictionary<string, string>();
            }

            int eq = source.Count(c => c == '=');
            int sc = source.Count(c => c == ';');
            if (sc != eq - 1 && sc != eq)
            {
                return new Dictionary<string, string>();
            }

            return source.Split(';').Where(v => !string.IsNullOrWhiteSpace(v)).ToDictionary(k => k.Split('=')[0], v => v.Split('=')[1]);
        }

        /// <summary>
        /// Parses a string to its Int value by stripping out invalid characters
        /// </summary>
        /// <param name="input">The input string to parse</param>
        /// <returns>An integer representing the string value, or 0 if empty</returns>
        public static int ToInt(this string input)
        {
            string toParse = Regex.Replace(input, @"[^\d]", string.Empty);

            if (string.IsNullOrWhiteSpace(toParse))
            {
                return 0;
            }
            else
            {
                return int.Parse(toParse);
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
            string thisString = s;

            if (s.IndexOf(toText, comparison) >= 0)
            {
                if (inclusive)
                {
                    thisString = s.Substring(0, s.LastIndexOf(toText, comparison) + toText.Length);
                }
                else
                {
                    thisString = s.Substring(0, s.LastIndexOf(toText, comparison));
                }
            }

            return thisString;
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
            string thisString = s;

            if (s.Contains(toText))
            {
                if (inclusive)
                {
                    thisString = s.Substring(0, s.LastIndexOf(toText) + 1);
                }
                else
                {
                    thisString = s.Substring(0, s.LastIndexOf(toText));
                }
            }

            return thisString;
        }

        #endregion Methods
    }
}