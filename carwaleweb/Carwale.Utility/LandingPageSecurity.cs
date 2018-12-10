using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Utility
{
    public class LandingPageSecurity
    {
        public static string EncryptUserId(long userId)
        {
            string userKey = "";

            userKey = ((32605878 * userId) + 27890291).ToString();

            return userKey;
        }

        public static string DecryptUserId(ulong userKey)
        {
            string userId = (((decimal)userKey - 27890291) / 32605878).ToString();

            if (userId.IndexOf('.') < 1)
                return userId.Substring(0, userId.Length);
            else
                return userId.Substring(0, userId.IndexOf('.') + 2);
        }

        public static string EncodeTo64(string toEncode)
        {

            byte[] toEncodeAsBytes
                  = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue
                  = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        public static string DecodeFrom64(string encodedData)
        {
            byte[] encodedDataAsBytes
                = System.Convert.FromBase64String(encodedData);
            string returnValue =
               System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
            return returnValue;
        }
    }
}
