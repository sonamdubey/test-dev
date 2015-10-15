using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
