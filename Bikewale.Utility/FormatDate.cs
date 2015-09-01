using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Utility
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 1 Sept 2015
    /// Summary : Class have functions to format the date
    /// </summary>
    public static class FormatDate
    {
        /// <summary>
        /// Created By : Ashish G. Kamble on 1 Sept 2015
        /// Summary : function will return date in the DD/MM/YYYY format
        /// </summary>
        public static string GetDDMMYYYY(string _date)
        {
            if (String.IsNullOrEmpty(_date))
                return string.Empty;
            else
                return Convert.ToDateTime(_date).ToString("dd MMM yyyy");
        }

    }
}
