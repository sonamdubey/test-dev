using RabbitMqPublishing.Common;
using System;
using System.Configuration;
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
        /// <summary>
        /// Created By : Lucky Rathroe
        /// Created On : 23 June 2016
        /// Description : To handle SetBWUtmz cookies
        /// </summary>
        public static void SetBWUtmz()
        {
            try
            {
                var request = HttpContext.Current.Request;
                string httpReffer = string.Empty,
                    utmcsr = string.Empty,
                    utmccn = string.Empty,
                    utmgclid = string.Empty,
                    utmcmd = string.Empty;

                if(request.UrlReferrer != null)
                {
                    httpReffer = request.UrlReferrer.OriginalString;
                }

                
                if (httpReffer.Contains(ConfigurationManager.AppSettings["bwHostUrl"]))
                {
                    SetCookie("BWUtmz", 180);
                }
                else
                {
                    string url = request.Url.ToString();
                    Regex serachEng = new Regex("google|bing|yahoo|ask|yandex|baidu|aol");
                    Match match = null;

                    if (request.Cookies.Get("utm_source") != null && request.Cookies.Get("utm_medium") != null && request.Cookies.Get("utm_campaign") != null)
                    {
                        utmcsr = request.Cookies.Get("utm_source").Value;
                        utmcmd = request.Cookies.Get("umt_medium").Value;
                        utmccn = request.Cookies.Get("utm_campaign").Value;
                    }
                    else if (url.Contains("gclid"))
                    {
                        utmcsr = "google";
                        utmgclid = "gclid";
                        utmcmd = "cpc";
                    }
                    else if ((match = serachEng.Match(url)) != null && match.Success)
                    {
                        if (match.Groups.Count >= 0)
                        {
                            utmcsr = match.Groups[0].Value;
                        }
                        utmcmd = "organic";
                        utmccn = "(organic)";
                    }
                    else if (!string.IsNullOrEmpty(httpReffer))
                    {
                        utmcsr = request.UrlReferrer.Host;
                        utmccn = "(referral)";
                        utmcmd = "referral";
                    }
                    else if (request.Cookies.Get("BWUtmz") != null)
                    {
                        Regex utm = new Regex("utmcsr=([()A-Za-z0-9.-]+)[|]*");
                        string utmz = request.Cookies.Get("BWUtmz").Value;

                        if ((match = utm.Match(utmz)) != null && match.Success)
                        {
                            if (match.Groups.Count > 0)
                            {
                                utmcsr = match.Groups[1].Value; 
                            }
                        }
                        utm = new Regex("utmccn=([()A-Za-z0-9.-]+)[|]*");
                        if ((match = utm.Match(utmz)) != null && match.Success)
                        {
                            if (match.Groups.Count > 0)
                            {
                                utmccn = match.Groups[1].Value; 
                            }
                        }
                        utm = new Regex("utmcmd=([()A-Za-z0-9.-]+)[|]*");
                        if ((match = utm.Match(utmz)) != null && match.Success)
                        {
                            if (match.Groups.Count > 0)
                            {
                                utmcmd = match.Groups[1].Value; 
                            }
                        }
                    }
                    else if (request.Cookies.Get("__utmz") != null)
                    {
                        Regex utm = new Regex("utmcsr=([()A-Za-z0-9.-]+)[|]*");
                        string utmz = request.Cookies.Get("__utmz").Value;

                        if ((match = utm.Match(utmz)) != null && match.Success)
                        {
                            if (match.Groups.Count > 0)
                            {
                                utmcsr = match.Groups[1].Value; 
                            }
                        }

                        utm = new Regex("utmccn=([()A-Za-z0-9.-]+)[|]*");
                        if ((match = utm.Match(utmz)) != null && match.Success)
                        {
                            if (match.Groups.Count > 0)
                            {
                                utmccn = match.Groups[1].Value; 
                            }
                        }

                        utm = new Regex("utmcmd=([()A-Za-z0-9.-]+)[|]*");
                        if ((match = utm.Match(utmz)) != null && match.Success)
                        {
                            if (match.Groups.Count > 0)
                            {
                                utmcmd = match.Groups[1].Value; 
                            }
                        }
                    }
                    else
                    {
                        utmcsr = "(direct)";
                        utmccn = "(direct)";
                        utmcmd = "(none)";
                    }
                    SetCookie("BWUtmz", 180, string.Format("utmcsr={0}|utmgclid=gclid|utmccn={1}|utmcmd={2}", utmcsr, utmccn, utmcmd));
                }
            }
            catch(Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Utility.BWCookies.SetBWUtmz");
            }
        }

        /// <summary>
        /// Created By : Lucky Rathore
        /// Created On : 23 June 2016
        /// Descritpion : Set New cookie and extend time of cookie if cookie exist.
        /// </summary>
        /// <param name="name">name of the Cookie.</param>
        /// <param name="lifeTime">Expiry time for cookies in Days</param>
        /// <param name="value">value to be stored in cookie</param>
        /// <returns></returns>
        public static bool SetCookie(string name, uint lifeTime, string value = null)
        {
            try
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(name);
                if (cookie == null)
                {
                    cookie = new HttpCookie(name);
                }
                if (value != null)
                {
                    cookie.Value = value;
                }
                cookie.Expires = DateTime.Now.AddDays(lifeTime);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Utility.BWCookies.SetCookie");
                return false;
            }
            return true;
        }
    }
}
