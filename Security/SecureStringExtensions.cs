using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Penguin.Extensions.String.Security
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class SecureStringExtensions
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        #region Methods

        /// <summary>
        /// Converts a SecureString to a String
        /// </summary>
        /// <param name="instr">The SecureString to convert</param>
        /// <returns></returns>
        public static string ToInsecureString(this SecureString instr)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(instr);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }

        /// <summary>
        /// Tests two SecureStrings for equality
        /// </summary>
        /// <param name="a">The first string to compare</param>
        /// <param name="b">The second string to compare</param>
        /// <returns></returns>
        public static bool ValueEquals(this SecureString a, SecureString b)
        {
            // TODO: write your implementation of Equals() here
            IntPtr bstr1 = IntPtr.Zero;
            IntPtr bstr2 = IntPtr.Zero;
            try
            {
                bstr1 = Marshal.SecureStringToBSTR(a);
                bstr2 = Marshal.SecureStringToBSTR(b);
                int length1 = Marshal.ReadInt32(bstr1, -4);
                int length2 = Marshal.ReadInt32(bstr2, -4);
                if (length1 == length2)
                {
                    for (int x = 0; x < length1; ++x)
                    {
                        byte b1 = Marshal.ReadByte(bstr1, x);
                        byte b2 = Marshal.ReadByte(bstr2, x);
                        if (b1 != b2)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }

                return true;
            }
            finally
            {
                if (bstr2 != IntPtr.Zero)
                {
                    Marshal.ZeroFreeBSTR(bstr2);
                }

                if (bstr1 != IntPtr.Zero)
                {
                    Marshal.ZeroFreeBSTR(bstr1);
                }
            }
        }

        #endregion Methods
    }
}