using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Utility
{
    /// <summary>
    /// Written By : Ashish G. Kamble on 2 Sept 2016
    /// Class to manage cookies. Also sets website domain for cookies
    /// Modified by :   Sumit Kate on 04 Jan 2017
    /// Description :   Added local domains
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
        private const string[] _localDomains = new string[] { "localhost", "webserver" };
        /// <summary>
        /// Modified by :   Sumit Kate on 04 Jan 2017
        /// Description :   Support local/webserver Cookie Domain
        /// </summary>
        /// <param name="name"></param>
        public Cookie(string name)
        {
            this.Name = name;
            Path = "/";
            Expires = DateTime.MinValue;
            if (!_localDomains.Contains(Utility.BWConfiguration.Instance.WebsiteDomain))
                Domain = "." + Utility.BWConfiguration.Instance.WebsiteDomain;
            else
                Domain = Utility.BWConfiguration.Instance.WebsiteDomain;
            Secure = false;
            Values = new List<KeyValuePair<string, string>>();
        }
    }

    /// <summary>
    /// Written By : Ashish G. Kamble on 2 Sept 2016
    /// Class to add the cookies to the browser with given values
    /// Modified By :Subodh jain 2 jan 2017
    /// Dec:-Added if contion in Expires time
    /// </summary>
    public static class CookieManager
    {
        public static void Add(Cookie obj)
        {
            if (obj != null)
            {
                HttpCookie cookie = new HttpCookie(obj.Name);
                cookie.Value = obj.Value;
                if (DateTime.MinValue != obj.Expires)
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
