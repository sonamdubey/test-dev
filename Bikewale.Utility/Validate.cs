using System.Text.RegularExpressions;

namespace Bikewale.Utility
{
    public class Validate
    {
        /// <summary>
        /// Created By : Sadhana Upadhyay on 31 Aug 2015
        /// Summary : To validate Numeric Range
        /// </summary>
        /// <param name="strQSParam">122-567, 0-2</param>
        /// <returns>boolean (True / False)</returns>
        public static bool ValidateNumericRange(string strQSParam)
        {
            var reg = new Regex(@"^([0-9]+)-(([0-9]+)?)$");
            return reg.IsMatch(strQSParam);
        }

        /// <summary>
        /// Check whether the value passed is between the specified range
        /// </summary>
        /// <param name="val">Value to be checked</param>
        /// <param name="minVal">Minimum value in the range</param>
        /// <param name="maxVal">Maximum value in the range</param>
        /// <returns></returns>
        public static bool ValidRange(int val, int minVal, int maxVal)
        {
            bool retVal = false;

            if (val >= minVal && val <= maxVal)
                retVal = true;

            return retVal;
        }

        /// <summary>
        /// Created By : Deepak Israni on 12 June 2018
        /// Description: Function to validate email addresses.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool ValidateEmail(string email)
        {
            var reg = new Regex(@"^[A-z0-9._+-]+@[A-z0-9.-]+\.[A-z]{2,6}$");
            return reg.IsMatch(email);
        }
    }   //End of class
}   //End of namespace
