using System.Text.RegularExpressions;

namespace Bikewale.Utility.StringExtention
{
    /// <summary>
    /// Created By : Sushil Kumar
    /// Created On : 14th October 2015
    /// Summary : To truncate string 
    /// </summary>
    public static class StringHelper
    {
        public static string Truncate(this string str, int length)
        {
            if (string.IsNullOrEmpty(str)) return str;
            return str.Length <= length ? str : str.Substring(0, length);
        }

        /// <summary>
        /// Created By : Lucky Rathore 
        /// Created on : 23 feb 2016
        /// Summary : To capitlize string
        /// </summary>
        /// <param name="str">string to be capitlize</param>
        /// <returns>capitlize string</returns>
        public static string Capitlization(string str)
        {
            var regCapitalize = Regex.Replace(str, @"\b(\w)", m => m.Value.ToUpper());
            str = Regex.Replace(regCapitalize, @"(\s(of|in|by|and)|\'[st])\b", m => m.Value.ToLower(), RegexOptions.IgnoreCase);
            return str;

        }
        // Added by Sangram Nandkhile on 24 May 2016
        // Desc: To be used instead of  " " in entire code eg. StringHelper.Space
        public static string Space = " ";
    }
}
