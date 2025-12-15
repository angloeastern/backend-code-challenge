using System;
using System.Collections.Generic;
using System.Text;

namespace AEChallenge.Application.Extensions
{
    public static class StringExtensions
    {
        public static string TruncateMessage(this string str, int maxLength = 100)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            int lengthToTake = Math.Min(str.Length, maxLength);
            return str[..lengthToTake];
        }
    }
}
