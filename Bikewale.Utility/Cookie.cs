using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Bikewale.Utility
{
    /// <summary>
    /// Written By : Ashish G. Kamble on 2 Sept 2016
    /// Class to manage cookies. Also sets website domain for cookies
    /// </summary>
    public class Cookie
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Path { get; set; }
        public DateTime Expires { get; set; }
        public string Domain { get; set; }
        public bool Secure { get; set; }
        public List<KeyValuePair<string, string>> Values { get; set; }

        public Cookie(string name)
        {
            this.Name = name;
            Path = "/";
            Expires = DateTime.MinValue;
            if (Utility.BWConfiguration.Instance.WebsiteDomain != "localhost")
                Domain = "." + Utility.BWConfiguration.Instance.WebsiteDomain;
            Secure = false;
            Values = new List<KeyValuePair<string, string>>();
        }
    }

    /// <summary>
    /// Written By : Ashish G. Kamble on 2 Sept 2016
    /// Class to add the cookies to the browser with given values
    /// </summary>
    public static class CookieManager
    {
        public static void Add(Cookie obj)
        {
            if (obj != null)
            {
                HttpCookie cookie = new HttpCookie(obj.Name);
                cookie.Value = obj.Value;
                cookie.Expires = obj.Expires;
                cookie.Domain = obj.Domain;
                cookie.Secure = obj.Secure;                

                foreach (KeyValuePair<string, string> value in obj.Values)
                {
                    cookie[value.Key] = value.Value;
                }

                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }
    }
}
