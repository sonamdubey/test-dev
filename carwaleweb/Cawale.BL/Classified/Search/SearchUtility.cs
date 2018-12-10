using Carwale.Interfaces.Classified.Search;
using Carwale.Utility;
using System;
using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace Carwale.BL.Classified.Search
{
    public class SearchUtility : ISearchUtility
    {
        public static string GetSearchAbsolutePath(string makeName, string rootName, string cityName, int pageNo = 0, string queryString = null)
        {
            StringBuilder newUrl = new StringBuilder("/used/");
            if (!string.IsNullOrEmpty(makeName))
            {
                newUrl.Append($"{ Format.FormatURL(makeName) }-");
                if (!string.IsNullOrEmpty(rootName))
                {
                    newUrl.Append($"{ Format.FormatURL(rootName) }-");
                }
            }
            newUrl.Append("cars");
            if (!string.IsNullOrEmpty(cityName))
            {
                newUrl.Append($"-in-{ Format.FormatURL(cityName) }");
            }
            else if (string.IsNullOrEmpty(makeName))
            {
                newUrl.Append("-for-sale");
            }
            newUrl.Append("/");

            if (pageNo > 1)
            {
                newUrl.Append($"page-{ pageNo }/");
            }

            if (!string.IsNullOrEmpty(queryString))
            {
                newUrl.Append($"?{ queryString }");
            }
            return newUrl.ToString();
        }

        [Obsolete("Mthod is deprecated, use the static version (SearchUtility.GetSearchAbsolutePath)")]
        public string GetURL(string makeName, string rootName, string cityName, int pageNo = 0, string queryString = null)
        {
            return GetSearchAbsolutePath(makeName, rootName, cityName, pageNo, queryString);
        }

        public NameValueCollection RemoveParamsFromQs(string qs, string[] keys)
        {
            var nvc = HttpUtility.ParseQueryString(qs);
            foreach(string key in keys)
            {
                nvc.Remove(key);
            }
            return nvc;
        }
    }
}
