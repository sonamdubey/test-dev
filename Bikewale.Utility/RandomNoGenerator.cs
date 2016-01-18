using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Utility
{
    public class RandomNoGenerator
    {
        /// <summary>
        /// Create a cookie value of length maxSize chars that would be used to track a user.
        /// </summary>
        /// <param name="maxSize">Sets the maximum number of characters that can be contained in the memory</param>
        /// <returns></returns>
        public static string GetUniqueKey(int maxSize)
        {
            char[] chars = new char[64];
            chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            data = new byte[maxSize];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length - 1)]);
            }
            return result.ToString();
        }
    }
}
