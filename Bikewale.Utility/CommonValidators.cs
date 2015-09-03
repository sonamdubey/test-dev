using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bikewale.Utility
{
    public class CommonValidators
    {
        public static bool CheckIsDealerFromProfileNo(string profileNo)
        {
            bool retVal = true;

            switch (profileNo.Substring(0, 1).ToUpper())
            {
                case "D":
                    retVal = true;
                    break;

                case "S":
                    retVal = false;
                    break;

                default:
                    retVal = true;
                    break;
            }

            return retVal;
        }

        public static string GetProfileNo(string profileNo)
        {
            string retVal = "";

            switch (profileNo.Substring(0, 1).ToUpper())
            {
                case "D":
                    retVal = profileNo.Substring(1, profileNo.Length - 1);
                    break;

                case "S":
                    retVal = profileNo.Substring(1, profileNo.Length - 1);
                    break;

                default:
                    retVal = profileNo;
                    break;
            }

            return retVal;
        }

        public static string ParseMobileNumber(string input)
        {
            //get only the numeric data
            char[] chars = input.ToCharArray();
            string raw = "";

            for (int i = 0; i < chars.Length; i++)
            {
                if (Regex.IsMatch(chars[i].ToString(), @"^[0-9]$") == true)
                {
                    raw += chars[i].ToString();
                }
            }

            //if the number is less than 10
            if (raw.Length < 10)
                return "";

            //get the last 10 characters if it is greater than 10
            if (raw.Length > 10)
                raw = raw.Substring(raw.Length - 10, 10);


            return raw;
        }

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
    }
}
