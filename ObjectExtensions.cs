﻿using System.Collections.Generic;

namespace Penguin.Extensions.Strings
{
    public static class ObjectExtensions
    {
        public static string Join<T>(this IEnumerable<T> source, string seperator = ", ")
        {
            return string.Join(seperator, source);
        }
    }
}