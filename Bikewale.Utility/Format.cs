using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Utility
{
    public static class Format
    {
        public static string FormatPrice(string minPrice, string maxPrice)
        {
            if ((string.IsNullOrEmpty(minPrice) && string.IsNullOrEmpty(maxPrice)) || (minPrice == "0" && maxPrice == "0"))
            {
                return "N/A";
            }
            else if (minPrice == maxPrice)
            {
                return FormatNumeric(minPrice);
            }
            else
                return FormatNumeric(minPrice) + "-" + FormatNumeric(maxPrice);
        }

        public static string FormatPrice(string price)
        {
            if (price == "" || price == "0")
                return "N/A";
            else
                return FormatNumeric(price);
        }

        public static string FormatNumeric(string numberToFormat)
        {
            string formatted = "";
            int breakPoint = 3;

            for (int i = numberToFormat.Length - 1; i >= 0; i--)
            {
                formatted = numberToFormat[i].ToString() + formatted;
                if ((numberToFormat.Length - i) == breakPoint && numberToFormat.Length > breakPoint)
                {
                    //HttpContext.Current.Trace.Warn(formatted);
                    formatted = "," + formatted;
                    breakPoint += 2;
                }
            }

            return formatted;
        }
    }
}
