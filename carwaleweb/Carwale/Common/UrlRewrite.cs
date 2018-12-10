// Mails Class
//
using System;
using System.Web;
using System.Configuration;
using System.Web.Mail;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;

namespace Carwale.UI.Common 
{
	public class UrlRewrite
	{
        public static string FormatSpecial(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return string.Empty;
            }
            string reg = @"[^/\-0-9a-zA-Z\s]*"; // everything except a-z, 0-9, / and -

            url = Regex.Replace(url, reg, "", RegexOptions.IgnoreCase);

            return url.Replace(" ", "").Replace("-", "").Replace("/", "").ToLower();
        }
        public static string GetLandingUrl(bool isFeatured, string make, string maskingName, string spotlightUrl, string CityName)
        {
            if (!(isFeatured && spotlightUrl != "") && CityName != string.Empty)
                return "/" + UrlRewrite.FormatSpecial(make) + "-cars/" + maskingName + "/price-in-" + CityName.Replace(" ", "").ToLower() + "/";
            else if (!(isFeatured && spotlightUrl != ""))
                return "/" + UrlRewrite.FormatSpecial(make) + "-cars/" + maskingName + "/";
            else
                return spotlightUrl;
        }

		public static void Return404() {
			HttpContext.Current.Response.StatusCode = 404;
			HttpContext.Current.Response.End();
		}
    }//class
}//namespace