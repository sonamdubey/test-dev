using System;
using System.Web;
using System.Configuration;
using System.Text;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Data;
using System.Security.Cryptography;
using RabbitMqPublishing;
using System.Collections.Specialized;
using Carwale.Notifications;
using Carwale.DAL.CoreDAL;
using Carwale.Notifications.Logs;
using Carwale.DAL.Customers;

namespace Carwale.UI.Common
{
    public class PriceQuoteCommon
    {
        //used for writing the debug messages
        private HttpContext objTrace = HttpContext.Current;
        public string TestimonialComment = string.Empty, TestimonialCustName = string.Empty;       

        /// <summary>
        /// Create a cookie value of length maxSize chars that would be used to track a user.
        /// </summary>
        /// <param name="maxSize">Sets the maximum number of characters that can be contained in the memory</param>
        /// <returns></returns>
        public string GetUniqueKey(int maxSize)
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
    }//class
}