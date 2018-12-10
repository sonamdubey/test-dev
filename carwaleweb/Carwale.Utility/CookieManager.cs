using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Carwale.Utility
{
    public static class CookieManager
    {
        public static void Add(Cookie obj)
        {
            HttpCookie cookie = new HttpCookie(obj.Name);
            cookie.Value = obj.Value;
            cookie.Expires = obj.Expires;
            cookie.Domain = CookieDomain;
            cookie.Secure = obj.Secure;
            cookie.Path = obj.Path;

            foreach (KeyValuePair<string, string> value in obj.Values)
            {
                cookie[value.Key] = value.Value;
            }

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static string CookieDomain
        {
            get
            {
                try
                {
                    return (string)HttpContext.Current.Items["CookieDomain"];
                }
                catch (Exception)
                {
                    HttpContext.Current.Items["CookieDomain"] = CustomerCookie.CookieDomain;
                    return (string)HttpContext.Current.Items["CookieDomain"];
                }
            }
        }

        public static string GetCookie(string name)
        {
            var cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
                return HttpContext.Current.Request.Cookies[name].Value;
            else
                return string.Empty;
        }

        public static string GetEncryptedCookie(string name)
        {
            string cookie = HttpContext.Current.Request?.Cookies[name]?.Value;
            if (!string.IsNullOrEmpty(cookie))
            {
                return CarwaleSecurity.Decrypt(cookie, true);
            }
            return null;
        }

        public static void MakeSessionCookie(string name)
        {
            var cookie = HttpContext.Current.Request?.Cookies[name];
            if (cookie != null)
            {
                cookie.Expires = DateTime.MinValue;
                HttpContext.Current.Response.SetCookie(cookie);
            }
        }

        public static void Delete(string name)
        {
            HttpContext.Current.Response.Cookies[name].Expires = DateTime.Now.AddDays(-1);
        }

        public static void SetCookieByValue(string cookieName, string value, int expireTime)
        {
            HttpCookie Cookie = new HttpCookie(cookieName, value);
            Cookie.Expires = DateTime.Now.AddYears(expireTime);
            Cookie.Domain = CookieDomain;
            if (System.Web.HttpContext.Current.Request.Cookies.Get(cookieName) != null)
            {
                System.Web.HttpContext.Current.Response.Cookies.Set(Cookie);
            }
            else
            {
                System.Web.HttpContext.Current.Response.Cookies.Add(Cookie);
            }
        }
    }
}
