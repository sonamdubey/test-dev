using RabbitMqPublishing.Common;
using System;
using System.Text.RegularExpressions;
using System.Web;
namespace Bikewale.Utility
{
    /// <summary>
    /// Created By : Lucky Rathroe
    /// Created On : 23 June 2016
    /// Description : To handle cookies and utility funtions for cookies.
    /// </summary>
    public static class BWCookies
    {
        public string BWUtmz  { get; }

        /// <summary>
        /// Created By : Lucky Rathroe
        /// Created On : 23 June 2016
        /// Description : To handle SetBWUtmz cookies
        /// </summary>
        public void SetBWUtmz()
        {
            var request = HttpContext.Current.Request;
            string httpReffer = request.UrlReferrer.AbsolutePath, 
                utmcsr = string.Empty, 
                utmccn = string.Empty, 
                utmgclid = string.Empty, 
                utmcmd = string.Empty; //is need cookie name
            string url = request.Url.ToString();
            Regex serachEng = new Regex("google|bing|yahoo|ask|yandex|baidu|aol");
            Match match = null;
            if (httpReffer == "http://www.bikewale.com/")
            { 
                //?? cookie name
            }
            
            if (request.Cookies.Get("utm_source") != null && request.Cookies.Get("utm_medium") != null)
            {
                utmcsr = request.Cookies.Get("utm_source").Value;
                utmccn = request.Cookies.Get("utm_source").Value;
            }
            else if (httpReffer.Contains("httpReffer"))
            {
                utmcsr = "google";
                utmgclid = "gclid";
                utmcmd = "cpc";
            }
            else if (( match = serachEng.Match(url)) != null  && match.Success)
            {
                utmcsr = match.Groups[0].Value; //tst it :P
                utmcmd = "organic";
                utmccn = "(organic)"; //TO ask to piyush

            }
            else if (!string.IsNullOrEmpty(httpReffer))
            {
                utmcsr = httpReffer;
                utmccn="(referral)";
                utmcmd="referral";
            }
            else
            {
                if (request.Cookies.Get("BWSource") != null)
                { 
                  //??? cookie name and wt nest
                }
                else if (request.Cookies.Get("__utmz") != null)
                {
                    utmcsr = request.Cookies.Get("__utmz").Values["utmcsr"];
                    utmccn = request.Cookies.Get("__utmz").Values["utmccn"];
                    utmcmd = request.Cookies.Get("__utmz").Values["utmcmd"];
                }
                else
                { 
                    utmcsr = "(direct)";
                    utmccn = "(direct)";
                    utmcmd = "(none)";
                }
            }
            SetCookie("BWSource", string.Format("utmcsr={0}|utmgclid=gclid|utmccn={1}|utmcmd={2}", utmcsr, utmccn, utmcmd), 180);
        }

        /// <summary>
        /// Created By : Lucky Rathore
        /// Created On : 23 June 2016
        /// Descritpion : Set New cookie and extend time of cookie if cookie exist.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="lifeTime">Expiry time for cookies in Days</param>
        /// <returns></returns>
        public bool SetCookie(string name, string value, uint lifeTime)
        {
            try
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(name);
                if (cookie == null)
                {
                    cookie.Value = value;
                    cookie.Expires = DateTime.Now.AddDays(lifeTime);
                    HttpContext.Current.Request.Cookies.Add(cookie);
                }
                else
                {
                    cookie.Value = value;
                    cookie.Expires = DateTime.Now.AddMinutes(lifeTime); //Test it.
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("Exception : Bikewale.Utility.BWHttpClient.BWHttpClient.PostSync<T, U>({0},{1})", apiHost, apiUrl));
                return false;
            } 
            return true;
        }
    }
}
