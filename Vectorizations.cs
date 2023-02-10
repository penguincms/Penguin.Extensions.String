using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Penguin.Extensions.String
{
    public static class StringVectorizations
    {
        //https://medium.com/@SergioPedri/optimizing-string-count-all-the-way-from-linq-to-hardware-accelerated-vectorized-instructions-186816010ad9
        public static int VectorizedCount(this string source, char c)
        {
            /* Get a reference to the first string character.
            * Strings are supposed to be immutable in .NET, so
            * in order to do this we first get a ReadOnlySpan<char>
            * from our string, and then use the MemoryMarshal.GetReference
            * API, which returns a mutable reference to the first
            * element of the input span. As a result, we now have
            * a mutable char reference to the first character in the string. */
            ReadOnlySpan<char> span = source.AsSpan();
            ref char r0 = ref MemoryMarshal.GetReference(span);
            int length = span.Length;
            int i = 0, result;

            /* As before, only execute the SIMD-enabled branch if the Vector<T> APIs
             * are hardware accelerated. Note that we're using ushort instead of char
             * in the Vector<T> type, because the char type is not supported.
             * But that is fine: ushort and char have the same byte size and the same
             * numerical range, and they behave exactly the same in this context. */
            if (Vector.IsHardwareAccelerated)
            {
                int end = length - Vector<ushort>.Count;

                // SIMD register all set to 0, to store partial results
                Vector<ushort> partials = Vector<ushort>.Zero;

                // SIMD register with the target character c copied in every position
                Vector<ushort> vc = new(c);

                for (; i <= end; i += Vector<ushort>.Count)
                {
                    // Get the reference to the current characters chunk
                    ref char ri = ref Unsafe.Add(ref r0, i);

                    /* Read a Vector<ushort> value from that offset, by
                     * reinterpreting the char reference as a ref Vector<ushort>.
                     * As with the previous example, doing this allows us to read
                     * the series of consecutive character starting from the current
                     * offset, and to load them in a single SIMD register. */

                    // vi = { text[i], ..., text[i + Vector<char>.Count - 1] }
                    Vector<ushort> vi = Unsafe.As<char, Vector<ushort>>(ref ri);

                    /* The Vector.Equals method sets each T item in a Vector<T> to
                     * either all 1s if the two elements match (as if we had used
                     * the == operator), or to all 0s if a pair doesn't match. */
                    Vector<ushort> ve = Vector.Equals(vi, vc);

                    /* First we load Vector<ushort>.One, which is a Vector<ushort> with
                     * just 1 in each position. Then we do a bitwise and with the
                     * previous result. Since matching values were all 1s, and non
                     * matching values were all 0s, we will have 1 in the position
                     * of pairs of values that were the same, or 0 otherwise. */
                    Vector<ushort> va = Vector.BitwiseAnd(ve, Vector<ushort>.One);

                    // Accumulate the partial results in each position
                    partials += va;
                }

                /* The dot product of a vector with a vector with 1 in each
                 * position results in the horizontal sum of all the values
                 * in the first vector, because:
                 *
                 * { a, b, c } DOT { 1, 1, 1 } = a * 1 + b * 1 + c * 1.
                 *
                 * So result will hold all the matching characters up to this point. */
                result = Vector.Dot(partials, Vector<ushort>.One);
            }
            else
            {
                result = 0;
            }

            // Iterate over the remaining characters and count those that match
            for (; i < length; i++)
            {
                bool equals = Unsafe.Add(ref r0, i) == c;
                result += Unsafe.As<bool, byte>(ref equals);
            }

            return result;
        }
    }
}