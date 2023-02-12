using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Penguin.Extensions.String
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
        public static IEnumerable<int> AllIndexesOf(this string str, string value, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (str is null)
            {
                throw new ArgumentNullException(nameof(str));
            }

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

            return s.Contains(search, comparisonType);
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
                    result = input[openingIndex..closingIndex];
                    break;
                }
            }

            return result;
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
        /// Removes all instances of the specified string, from the source (using Replace)
        /// </summary>
        /// <param name="input">The source string</param>
        /// <param name="toRemove">The substring to remove</param>
        /// <returns>The source string without any instances of the specified substring</returns>
        public static string Remove(this string input, string toRemove)
        {
            return input is null ? input : input.Replace(toRemove, string.Empty);
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

            arr = Array.FindAll(arr, c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '-');

            return new string(arr);
        }

       

        /// <summary>
        /// Converts a string to Base64 encoding
        /// </summary>
        /// <param name="input">the string to convert</param>
        /// <param name="encoding">Optional Encoding. UTF8 default</param>
        /// <returns></returns>
        public static string ToBase64(this string input, Encoding encoding = null)
        {
            return Convert.ToBase64String((encoding ?? Encoding.UTF8).GetBytes(input));
        }

        /// <summary>
        /// Converts a string from Base64 encoding
        /// </summary>
        /// <param name="input">the string to convert</param>
        /// <param name="encoding">Optional Encoding. UTF8 default</param>
        /// <returns></returns>
        public static string FromBase64(this string input, Encoding encoding = null)
        {
            return (encoding ?? Encoding.UTF8).GetString(Convert.FromBase64String(input));
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

            return index == 0 ? 0 : int.Parse(new string(toReturn, 0, index));
        }

      
    }
}