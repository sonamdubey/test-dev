using System;
using System.Security.Cryptography;
using System.Text;

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

        /// <summary>
        /// Author  : Kartik Rathod on 19 jun 2018
        /// Desc    : Generate unique key (GUID) purpose to replace pqId 
        /// </summary>
        /// <returns>GUID</returns>
        public static string GenerateUniqueId()
        {
            Guid guId =  Guid.NewGuid();
            return guId.ToString();
        }
    }
}
