using System;
using System.Collections.Generic;

namespace Carwale.Utility
{
    public static class StringUtility
    {
        public static string GetSubString(string description,int length)
        {
            if (description.IsNotNullOrEmpty() && length>0)
            {
                if (description.Length <= length)
                    return description;
                int position = description.LastIndexOf(" ", length - 1);
                if (position >= 0)
                    return description.Substring(0, position);
            }   
            return string.Empty;
        }

        public static string GetHtmlSubString(string htmlString, int length)
        {
            if (htmlString.IsNotNullOrEmpty() && length > 0)
            {
                string stringWithoutpTages = htmlString.Replace("<p>", string.Empty).Replace("</p>", string.Empty);
                return GetSubString(stringWithoutpTages, length);
            }
            return string.Empty;
        }
    }
}
