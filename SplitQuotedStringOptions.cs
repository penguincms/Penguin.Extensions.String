using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.Extensions.String
{
    public class QuotedStringOptions
    {
        public char QuoteCharacter { get; set; } = '"';
        public char ItemDelimeter { get; set; } = ',';
        public bool RemoveQuotes { get; set; } = true;
    }
}
