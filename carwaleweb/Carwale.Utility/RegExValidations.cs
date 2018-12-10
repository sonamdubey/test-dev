using System;
using System.Net;
using System.Text.RegularExpressions;

namespace Carwale.Utility
{
    public static class RegExValidations
    {
        public const string NameRegex = @"^(?i)([a-zA-Z])[a-zA-Z'\-\._ ]+$";
        public const string MobileRegex = @"^[6789]\d{9}$";
        public const string EmailRegex = @"^[a-z0-9._-]+@([a-z0-9-]+\.)+[a-z]{2,6}$";

        /// <summary>
        /// Whether number is numeric or not
        /// </summary>
        /// <param name="inputvalue"></param>
        /// <returns></returns>
        public static bool IsNumeric(string inputvalue)
        {
            if (String.IsNullOrEmpty(inputvalue))
                return false;

            return Regex.IsMatch(inputvalue, @"^(\+|-)?\d+(\.\d+)?$");
        }

        /// <summary>
        /// Ex: 6,2,4,5
        /// </summary>
        /// <param name="strQsParam"></param>
        /// <returns></returns>
        public static bool ValidateCommaSeperatedNumbers(string strQsParam)
        {
            if (string.IsNullOrWhiteSpace(strQsParam)) return false;
            var reg = new Regex(@"^([0-9]+,)*[0-9]+$");
            return reg.IsMatch(strQsParam);
        }

        /// <summary>
        /// Ex: 1000-5000 or 2-8 or 50-100
        /// </summary>
        /// <param name="strQSParam"></param>
        /// <returns></returns>
        public static bool ValidateNumericRange(string strQSParam,string delimiter = "-")
        {
            if (string.IsNullOrWhiteSpace(strQSParam)) return false;
            var reg = new Regex(@"^(\+|-)?([0-9]+)" + delimiter + "((\\+|-)?([0-9]+)?)$");
            return reg.IsMatch(strQSParam);
        }

        /// <summary>
        /// Whether number is greater than 0
        /// </summary>
        /// <param name="strQSParam"></param>
        /// <returns></returns>
        public static bool IsPositiveNumber(string strQSParam)
        {
            if (string.IsNullOrWhiteSpace(strQSParam)) return false;
            var reg = new Regex(@"^[1-9]\d*$");
            return reg.IsMatch(strQSParam);
        }

        /// <summary>
        /// Returns true if number is in the range (startIndex - endIndex)
        /// </summary>
        /// <param name="inputString">eg: 2 or 1,2</param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public static bool IsNumberInRange(string inputString, int startIndex, int endIndex)
        {
            bool retVal = false;
            if (!String.IsNullOrEmpty(inputString) && ValidateCommaSeperatedNumbers(inputString))
            {
                string[] _arr = inputString.Split(',');

                for (int i = 0; i < _arr.Length; i++)
                {
                    if (Convert.ToInt32(_arr[i]) >= startIndex && Convert.ToInt32(_arr[i]) <= endIndex)
                        retVal = true;
                }
                return retVal;
            }
            else
                return retVal;
        }

        /// <summary>
        /// Created  By : Sadhana Upadhyay on 4 May 2015
        /// Summary : to check decimal Range. Example - 1-2 or 1.456463-45.5687
        /// </summary>
        /// <param name="strQSParam"></param>
        /// <returns></returns>
        public static bool ValidateDecimalRange(string strQSParam)
        {
            var reg = new Regex(@"^([0-9]+(\.[0-9]{1,7})?)-(([0-9]+(\.[0-9]{1,7})?)?)$");
            return reg.IsMatch(strQSParam);
        }

        public static bool IsValidEmail(string email)
        {
            var emailRegEx = new Regex(EmailRegex);
            return emailRegEx.IsMatch(email ?? string.Empty);
        }

        public static bool IsValidDate(string date)
        {
            var dateRegEx = new Regex(@"^[0-9]{4}[-/\s][0-9]{2}[-/\s][0-9]{2}$");
            return dateRegEx.IsMatch(date);
        }

        /// <summary>
        /// check for mobile validity
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool IsValidMobile(string mobile)
        {
            var mobileRegEx = new Regex(MobileRegex);
            return mobileRegEx.IsMatch(mobile ?? string.Empty);
        }
        
        public static bool IsValidName(string name)
        {
            Regex nameRegex = new Regex(NameRegex);
            return nameRegex.IsMatch(name ?? string.Empty);
        }

        /// <summary>
        /// Whether string from third party
        /// </summary>
        /// <param name="inputvalue"></param>
        /// <returns></returns>
        public static bool IsThirdPartyEmail(string inputvalue)
        {
            return Regex.IsMatch(inputvalue, @".*@cartrade.com$", RegexOptions.IgnoreCase);
        }

        public static bool IsValidIpAddress(string ipAddress)   // method returns true if the Int64 is parsed successfully, even if it represents an address that's not a valid IP address. For example, if ipString is "1", this method returns true even though "1" (or 0.0.0.1) is not a valid IP address
        {                                                       // more info:- https://msdn.microsoft.com/en-us/library/system.net.ipaddress.tryparse(v=vs.110).aspx#Anchor_1
            IPAddress ip;
            return (IPAddress.TryParse(ipAddress, out ip));
        }

        /// <summary>
        /// return whether given latitude and longitude are valid or not
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public static bool IsValidLatLong(double latitude, double longitude)
        {
            return IsValidLatitude(latitude) && IsValidLongitude(longitude);
        }

        /// <summary>
        /// returns whether given latitude is valid or not
        /// </summary>
        /// <param name="latitude"></param>
        /// <returns></returns>
        public static bool IsValidLatitude(double latitude)
        {
            return !(latitude < -90.0 || latitude > 90.0);
        }

        /// <summary>
        /// returns 
        /// </summary>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public static bool IsValidLongitude(double longitude)
        {
            return !(longitude < -180.0 || longitude > 180);
        }
    }
}
