using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Utility
{
    /// <summary>
    /// Created by  :   Sumit Kate 08 Jan 2015
    /// Summary     :   Encoding Decoding Helper.
    /// </summary>
    public class EncodingDecodingHelper
    {
        /// <summary>
        ///     Written By : Ashish G. Kamble on 6 Nov 2012
        ///     Summary : Function will encode the string into base 64 string
        /// </summary>
        /// <param name="toEncode">string to encode</param>
        /// <returns>returns 64 character string</returns>
        public static string EncodeTo64(string toEncode)
        {
            try
            {
                byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
                string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);                
                return returnValue;
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }

        /// <summary>
        ///     Written By : Ashish G.kamble on 6 Nov 2012
        ///     Summary : Function will decode the encoded string into the original string
        /// </summary>
        /// <param name="encodedData">String which is decoded using EncodeTo64 method</param>
        /// <returns>Returns the decoded string</returns>
        public static string DecodeFrom64(string encodedData)
        {
            try
            {
                byte[] encodedDataAsBytes = System.Convert.FromBase64String(encodedData);
                string returnValue = System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
                return returnValue;
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }
    }
}
