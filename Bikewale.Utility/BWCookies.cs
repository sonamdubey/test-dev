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
        /// Description : To handle SetBWUtmz cookies as mention in following story (refferd from Pivotal story).
        /// If the HTTP referrer is bikewale.com then set the expiry of the cookie to 6 months from that time
        /// Else execute the logic
        /// 1. Check if the URL contains utm_source, utm_medium in the URL. If yes then store utm_source in utmcsr, umt_medium in utmcmd and utm_campaign in utmccn
        /// 2. If no utm parameters are present in the URL then check if the URL contains gclid then set utmcsr=google, utmgclid=gclid, utmcmd=cpc
        /// 3. If none of the above is true, look at HTTP referrer. If the HTTP referrer contains the domains - google, yahoo, bing, ask, yandex, baidu, aol then set utmcsr=<search engine names>, utmcmd=organic, utmccn=(organic)
        /// 4. If the HTTP referrer is none of the above then set utmcsr=<domain name>, utmccn=(referral), utmcmd=referral
        /// 5. If HTTP referrer is null then check if there is a BW source cookie, if yes then replicate that cookie
        /// 5. If not then, check if there is a __utmz cookie, if yes then replicate utmcsr, utmccn, utmcmd, gclid
        /// 7. If not then set utmcsr=(direct), utmccn=(direct), utmcmd=(none) 
        /// 8. Set the BW source cookie with utmcsr=<value>|utmgclid=gclid|utmccn=<value>|utmcmd=<value> with a 6 month expiry
        /// 9. Store this cookie in the __utmz cookie in the database
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
                    httpReffer = request.UrlReferrer.OriginalString; //e.g. www.google.com, www.carwale.com
                }


                if (httpReffer.Contains(ConfigurationManager.AppSettings["bwHostUrl"]))
                {
                    SetCookie("_bwutmz", 180);
                }
                else
                {
                    string url = request.Url.ToString();
                    Regex serachEng = new Regex("www.google.com|www.google.co.([a-z]+)|([a-z]+).search.yahoo.com|www.bing.com|www.aol.in|www.aol.com|www.aolsearch.com|www.ask.com|www.yandex.com|www.baidu.com");
                    Match match = null;
                    //step 1. Check if the URL contains utm_source, utm_medium in the URL. If yes then store utm_source in utmcsr, umt_medium in utmcmd and utm_campaign in utmccn
                    if (
                        !(string.IsNullOrEmpty(request.QueryString["utm_source"]) 
                        || string.IsNullOrEmpty(request.QueryString["utm_medium"]) 
                        || string.IsNullOrEmpty(request.QueryString["utm_campaign"]))
                        )
                    {
                        utmcsr = request.QueryString["utm_source"];
                        utmcmd = request.QueryString["utm_medium"];
                        utmccn = request.QueryString["utm_campaign"];
                    }
                    else if (!string.IsNullOrEmpty(request.QueryString["gclid"])) //step 2. If no utm parameters are present in the URL then check if the URL contains gclid then set utmcsr=google, utmgclid=gclid, utmcmd=cpc
                    {
                        utmcsr = "google";
                        utmgclid = request.QueryString["gclid"];
                        utmcmd = "cpc";
                    }
                    else if ((match = serachEng.Match(httpReffer)) != null && match.Success) //step 3. If none of the above is true, look at HTTP referrer. If the HTTP referrer contains the domains - google, yahoo, bing, ask, yandex, baidu, aol then set utmcsr=<search engine names>, utmcmd=organic, utmccn=(organic)
                    {
                        if (match.Groups.Count >= 0)
                        {
                            utmcsr = match.Groups[0].Value;
                            Regex serachEngNames = new Regex("google|yahoo|bing|ask|yandex|baidu|aol");
                            if ((match = serachEngNames.Match(utmcsr)) != null && match.Success)
                            {
                                utmcsr = match.Groups[0].Value;
                            }
                        }
                        utmcmd = "organic";
                        utmccn = "(organic)";
                    }
                    else if (!string.IsNullOrEmpty(httpReffer)) //step 4. If the HTTP referrer is none of the above then set utmcsr=<domain name>, utmccn=(referral), utmcmd=referral
                    {
                        utmcsr = request.UrlReferrer.Host;
                        utmccn = "(referral)";
                        utmcmd = "referral";
                    }
                    else if (request.Cookies.Get("_bwutmz") != null) //step 5. If HTTP referrer is null then check if there is a BW source cookie, if yes then replicate that cookie
                    {
                        Regex utm = new Regex("utmcsr=([()A-Za-z0-9.-_!@#$%^*]+)[|]*");
                        string utmz = request.Cookies.Get("_bwutmz").Value;

                        if ((match = utm.Match(utmz)) != null && match.Success)
                        {
                            if (match.Groups.Count > 0)
                            {
                                utmcsr = match.Groups[1].Value; 
                            }
                        }
                        utm = new Regex("utmccn=([()A-Za-z0-9.-_!@#$%^*]+)[|]*");
                        if ((match = utm.Match(utmz)) != null && match.Success)
                        {
                            if (match.Groups.Count > 0)
                            {
                                utmccn = match.Groups[1].Value; 
                            }
                        }
                        utm = new Regex("utmcmd=([()A-Za-z0-9.-_!@#$%^*]+)[|]*");
                        if ((match = utm.Match(utmz)) != null && match.Success)
                        {
                            if (match.Groups.Count > 0)
                            {
                                utmcmd = match.Groups[1].Value; 
                            }
                        }
                        utm = new Regex("gclid=([()A-Za-z0-9.-_!@#$%^*]+)[|]*");
                        if ((match = utm.Match(utmz)) != null && match.Success)
                        {
                            if (match.Groups.Count > 0)
                            {
                                utmgclid = match.Groups[1].Value;
                            }
                        }
                    }
                    else if (request.Cookies.Get("__utmz") != null) //step 6. If not then, check if there is a __utmz cookie, if yes then replicate utmcsr, utmccn, utmcmd, gclid
                    {
                        Regex utm = new Regex("utmcsr=([()A-Za-z0-9.-_!@#$%^*]+)[|]*");
                        string utmz = request.Cookies.Get("__utmz").Value;

                        if ((match = utm.Match(utmz)) != null && match.Success)
                        {
                            if (match.Groups.Count > 0)
                            {
                                utmcsr = match.Groups[1].Value; 
                            }
                        }

                        utm = new Regex("utmccn=([()A-Za-z0-9.-_!@#$%^*]+)[|]*");
                        if ((match = utm.Match(utmz)) != null && match.Success)
                        {
                            if (match.Groups.Count > 0)
                            {
                                utmccn = match.Groups[1].Value; 
                            }
                        }

                        utm = new Regex("utmcmd=([()A-Za-z0-9.-_!@#$%^*]+)[|]*");
                        if ((match = utm.Match(utmz)) != null && match.Success)
                        {
                            if (match.Groups.Count > 0)
                            {
                                utmcmd = match.Groups[1].Value; 
                            }
                        }

                        utm = new Regex("gclid=([()A-Za-z0-9.-_!@#$%^*]+)[|]*");
                        if ((match = utm.Match(utmz)) != null && match.Success)
                        {
                            if (match.Groups.Count > 0)
                            {
                                utmgclid = match.Groups[1].Value;
                            }
                        }
                    }
                    else //step 7. If not then set utmcsr=(direct), utmccn=(direct), utmcmd=(none) 
                    {
                        utmcsr = "(direct)";
                        utmccn = "(direct)";
                        utmcmd = "(none)";
                    }
                    SetCookie("_bwutmz", 180, string.Format("utmcsr={0}|utmgclid={1}|utmccn={2}|utmcmd={3}", utmcsr, utmgclid, utmccn, utmcmd)); //step 8. Set the BW source cookie with utmcsr=<value>|utmgclid=gclid|utmccn=<value>|utmcmd=<value> with a 6 month expiry
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
