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


        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        public static string GetDaysAgo(string displayDate)
        {
            string retVal = "";
            TimeSpan tsDiff = DateTime.Now.Subtract(Convert.ToDateTime(displayDate));

            if (tsDiff.Days > 0)
            {
                retVal = tsDiff.Days.ToString();

                retVal+= retVal=="1"? "day ago": "days ago";

            }
            else if (tsDiff.Hours > 0)
            {
                retVal = tsDiff.Hours.ToString();

                retVal+= retVal=="1"?" hour ago":" hours ago";
            }
            else if (tsDiff.Minutes > 0)
            {
                retVal = tsDiff.Minutes.ToString();
                retVal+=retVal=="1"?" minute ago":" minutes ago";
            }
            else if (tsDiff.Seconds > 0)
            {
                retVal = tsDiff.Seconds.ToString();

                retVal+=retVal=="1"?" second ago":" seconds ago";

            }

            if (tsDiff.Days > 360)
            {
                retVal = Convert.ToString(tsDiff.Days / 360);

                retVal+=retVal=="1"?" year ago":" years ago";

            }
            else if (tsDiff.Days > 30)
            {
                retVal = Convert.ToString(tsDiff.Days / 30);

                retVal+=retVal=="1"?" month ago":" months ago";
            }

            return retVal;
        }
    }
}
