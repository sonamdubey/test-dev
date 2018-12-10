// Security Class
//

using System;
using System.Web;
using System.Configuration;
using System.Web.Mail;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Security.Cryptography;

namespace Carwale.UI.Common
{
    public class ScreenInput
    {
        // Generate verification code.
        public static bool IsValidRedirectUrl(string redirectUrl)
        {
            redirectUrl = redirectUrl.ToLower();
            if (redirectUrl.StartsWith("http") == true && !(redirectUrl.StartsWith("https://www.carwale.com") == true || redirectUrl.StartsWith("https://carwale.com") == true))
            {
                HttpContext.Current.Trace.Warn("Redirect Url is of separate domain : " + redirectUrl);
                return false;
            }
            else
                return true;

        }


    }
}