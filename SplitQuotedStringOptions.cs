namespace Penguin.Extensions.String
{
    public class QuotedStringOptions
    {
        public char QuoteCharacter { get; set; } = '"';
        public char ItemDelimeter { get; set; } = ',';
        public bool RemoveQuotes { get; set; } = true;
    }
}