using Carwale.BL.Experiments;
using Carwale.Notifications;
using Carwale.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;

namespace Carwale.UI.Common
{
    public class CookiesCustomers
    {
        public static void SetDomain(HttpContext context)
        {
            var host = context.Request.Url.Host;
            Match mc = new Regex("(.*).carwale.com").Match(host);
            if (mc.Success && mc.Groups[1].Value == "www") host = "carwale.com";
            context.Items["CookieDomain"] = host;
        }

        public static string CookieDomain
        {
            get
            {
                try
                {
                    return (string)HttpContext.Current.Items["CookieDomain"];
                }
                catch (Exception ex)
                {
                    ExceptionHandler objErr = new ExceptionHandler(ex, "CookiesCustomers.CookieDomain.Get()");
                    objErr.LogException();
                }
                return System.Configuration.ConfigurationManager.AppSettings["Domain"] ?? "carwale.com";
            }
        }

        public static string CityId
        {
            get
            {
                var cookieObj = HttpContext.Current.Request.Cookies["_CustCityId"];
                string val;	//default false

                if (cookieObj != null &&
                   Utility.RegExValidations.IsNumeric(cookieObj.Value))
                {
                    val = cookieObj.Value;
                }
                else
                {
                    val = "-1";
                }

                return val;
            }
            set
            {
                HttpCookie objCookie;
                objCookie = new HttpCookie("_CustCityId");
                objCookie.Value = value;
                //objCookie.Expires = DateTime.Now.AddHours(3);
                objCookie.Domain = CookieDomain;
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }

        public static string City
        {
            get
            {
                var cookieObj = HttpContext.Current.Request.Cookies["_CustCity"];
                string val;	//default false

                if (cookieObj != null && cookieObj.Value != string.Empty)
                {
                    val = cookieObj.Value;
                }
                else
                {
                    val = string.Empty;
                }

                return val;
            }
            set
            {
                HttpCookie objCookie;
                objCookie = new HttpCookie("_CustCity");
                objCookie.Value = value;
                //objCookie.Expires = DateTime.Now.AddHours(3);
                objCookie.Domain = CookieDomain;
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }

        public static string MasterCity
        {
            get
            {
                var cookieObj = HttpContext.Current.Request.Cookies["_CustCityMaster"];
                string val;	//default false

                if (cookieObj != null && cookieObj.Value != string.Empty)
                {
                    val = cookieObj.Value;
                }
                else
                    val = string.Empty;

                return HttpUtility.UrlDecode(val);
            }
            internal set
            {
                HttpContext.Current.Response.Cookies["_CustCityMaster"].Value = value.ToString();
                HttpContext.Current.Response.Cookies["_CustCityMaster"].Expires = DateTime.Now.AddMonths(6);
                HttpContext.Current.Response.Cookies["_CustCityMaster"].Domain = CookieDomain;
            }
        }

        public static string MasterZone
        {
            get
            {
                var cookieObj = HttpContext.Current.Request.Cookies["_CustZoneMaster"];
                string val;	//default false

                if (cookieObj != null && cookieObj.Value != string.Empty)
                {
                    val = cookieObj.Value;
                }
                else
                    val = string.Empty;

                return HttpUtility.UrlDecode(val);
            }
            internal set
            {
                HttpContext.Current.Response.Cookies["_CustZoneMaster"].Value = value.ToString();
                HttpContext.Current.Response.Cookies["_CustZoneMaster"].Expires = DateTime.Now.AddMonths(6);
                HttpContext.Current.Response.Cookies["_CustZoneMaster"].Domain = CookieDomain;
            }
        }

        public static string MasterArea
        {
            get
            {
                var cookieObj = HttpContext.Current.Request.Cookies["_CustAreaName"];
                string val;	//default false

                if (cookieObj != null && cookieObj.Value != string.Empty)
                {
                    val = cookieObj.Value;
                }
                else
                    val = string.Empty;

                return HttpUtility.UrlDecode(val);
            }
            internal set
            {
                HttpContext.Current.Response.Cookies["_CustAreaName"].Value = value.ToString();
                HttpContext.Current.Response.Cookies["_CustAreaName"].Expires = DateTime.Now.AddMonths(6);
                HttpContext.Current.Response.Cookies["_CustAreaName"].Domain = CookieDomain;
            }
        }
        public static int MasterCityId
        {
            get
            {
                int val;
                var cookieObj = HttpContext.Current.Request.Cookies["_CustCityIdMaster"];
                if (cookieObj != null && Utility.RegExValidations.IsNumeric(cookieObj.Value))
                {
                    val = Convert.ToInt32(cookieObj.Value);
                }
                else
                    val = -1;

                return val;
            }
            internal set
            {
                HttpContext.Current.Response.Cookies["_CustCityIdMaster"].Value = value.ToString();
                HttpContext.Current.Response.Cookies["_CustCityIdMaster"].Expires = DateTime.Now.AddMonths(6);
                HttpContext.Current.Response.Cookies["_CustCityIdMaster"].Domain = CookieDomain;
            }
        }

        public static int MasterAreaId
        {
            get
            {
                int val;
                var cookieObj = HttpContext.Current.Request.Cookies["_CustAreaId"];
                if (cookieObj != null && Utility.RegExValidations.IsNumeric(cookieObj.Value))
                {
                    val = Convert.ToInt32(cookieObj.Value);
                }
                else
                    val = -1;

                return val;
            }
            internal set
            {
                HttpContext.Current.Response.Cookies["_CustAreaId"].Value = value.ToString();
                HttpContext.Current.Response.Cookies["_CustAreaId"].Expires = DateTime.Now.AddMonths(6);
                HttpContext.Current.Response.Cookies["_CustAreaId"].Domain = CookieDomain;
            }
        }

        public static string Email
        {
            get
            {
                var cookieObj = HttpContext.Current.Request.Cookies["_CustEmail"];
                string val;	//default false

                if (cookieObj != null && cookieObj.Value != string.Empty)
                {
                    val = cookieObj.Value;
                }
                else
                {
                    val = string.Empty;
                }

                return val;
            }
            set
            {
                HttpCookie objCookie;
                objCookie = new HttpCookie("_CustEmail");
                objCookie.Value = value;
                //objCookie.Expires = DateTime.Now.AddHours(3);
                objCookie.Domain = CookieDomain;
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }

        public static string CustomerName
        {
            get
            {
                var cookieObj = HttpContext.Current.Request.Cookies["_CustomerName"];
                string val;	//default false

                if (cookieObj != null &&
                    cookieObj.Value != string.Empty)
                {
                    val = cookieObj.Value;
                }
                else
                {
                    val = string.Empty;
                }

                return val;
            }
            set
            {
                HttpCookie objCookie;
                objCookie = new HttpCookie("_CustomerName");
                objCookie.Value = value;
                //objCookie.Expires = DateTime.Now.AddHours(3);
                objCookie.Domain = CookieDomain;
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }

        public static string Mobile
        {
            get
            {
                var cookieObj = HttpContext.Current.Request.Cookies["_CustMobile"];
                string val;	//default false

                if (cookieObj != null && cookieObj.Value != string.Empty)
                {
                    val = cookieObj.Value;
                }
                else
                {
                    val = string.Empty;
                }

                return val;
            }
            set
            {
                HttpCookie objCookie;
                objCookie = new HttpCookie("_CustMobile");
                objCookie.Value = value;
                //objCookie.Expires = DateTime.Now.AddHours(3);
                objCookie.Domain = CookieDomain;
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }

        public static int MasterZoneId
        {
            get
            {
                int val;
                var cookieObj = HttpContext.Current.Request.Cookies["_CustZoneIdMaster"];
                if (cookieObj != null && Utility.RegExValidations.IsNumeric(cookieObj.Value))
                {
                    val = Convert.ToInt32(cookieObj.Value);
                }
                else
                    val = -1;

                return val;
            }
            internal set
            {
                HttpContext.Current.Response.Cookies["_CustZoneIdMaster"].Value = value.ToString();
                HttpContext.Current.Response.Cookies["_CustZoneIdMaster"].Expires = DateTime.Now.AddMonths(6);
                HttpContext.Current.Response.Cookies["_CustZoneIdMaster"].Domain = CookieDomain;
            }
        }

        public static string UserModelHistory
        {
            get
            {
                var cookieObj = HttpContext.Current.Request.Cookies["_userModelHistory"];
                string val;	//default false

                if (cookieObj != null && cookieObj.Value != string.Empty)
                {
                    val = cookieObj.Value;
                }
                else
                {
                    val = string.Empty;
                }

                return val;
            }
            set
            {
                HttpCookie objCookie;
                objCookie = new HttpCookie("_userModelHistory");
                objCookie.Value = value;
                //objCookie.Expires = DateTime.Now.AddHours(3);
                objCookie.Domain = CookieDomain;
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }

        public static string AdvantageModelHistory
        {
            get
            {
                var cookieObj = HttpContext.Current.Request.Cookies["_advHistory"];
                string val;	//default false

                if (cookieObj != null && cookieObj.Value != string.Empty)
                {
                    val = cookieObj.Value;
                }
                else
                {
                    val = string.Empty;
                }

                return val;
            }
            set
            {
                HttpCookie objCookie;
                objCookie = new HttpCookie("_advHistory");
                objCookie.Value = value;
                //objCookie.Expires = DateTime.Now.AddHours(3);
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }

        public static int AbTest
        {
            get
            {
                // This cookie is set in Global.asax.cs on BeginRequest event
                try
                {
                    var cookieObj = HttpContext.Current.Request.Cookies["_abtest"];
                    int val = 0;

                    if (cookieObj != null && cookieObj.Value != string.Empty)
                    {
                        val = Convert.ToInt16(cookieObj.Value);
                    }
                    else
                    {
                        cookieObj = HttpContext.Current.Response.Cookies["_abtest"];
                        val = Convert.ToInt16(cookieObj.Value);
                    }
                    return val;
                }
                catch (Exception)
                {
                    return 0;
                }

            }
        }

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
                cookie.Domain = CookieDomain;
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            catch (Exception)
            { }
            return true;
        }
        
        //this method returns the default version for a model in a session
        public static int GetVersionStateForModel(int modelId)
        {
            IDictionary<int, int> result;
            var versionState = HttpContext.Current.Request.Cookies.Get("versionstate");
            if (versionState != null)
            {
                result = JsonConvert.DeserializeObject<IDictionary<int, int>>(versionState.Value);
                return result != null && result.ContainsKey(modelId) ? result[modelId] : -1;
            }
            return -1;
        }

        /// <summary>
        /// Created By : Lucky Rathroe
        /// Created On : 23 June 2016
        /// Description : To handle SetCWUtmz/BWUtmz cookies as mention in following story (refferd from Pivotal story).
        /// If the HTTP referrer is carwale.com then set the expiry of the cookie to 6 months from that time
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
        /// Modified By : Lucky Rathore
        /// Modified On : 11 July 2016
        /// Description : Change RegEx for search Engine bikewale domain name for http Refferer.
        /// Ported To carwale 22-11
        /// Modified By: Meet Shah
        /// Modified On: 6 Feb 2018
        /// Description:  Added utm_term and utm_content params
        /// </summary>
        public static void SetCWUtmz()
        {
            try
            {
                var request = HttpContext.Current.Request;
                string httpReffer = string.Empty,
                    utmcsr = string.Empty,
                    utmccn = string.Empty,
                    utmgclid = string.Empty,
                    utmcmd = string.Empty,
                    utmtrm = string.Empty,
                    utmcnt = string.Empty;

                if (request.UrlReferrer != null)
                {
                    httpReffer = request.UrlReferrer.Host; //e.g. www.google.com, www.carwale.com
                }


                if (httpReffer.Contains("carwale.com"))
                {
                    SetCookie("_cwutmz", 180);
                }
                else
                {
                    string url = request.Url.ToString();
                    Regex serachEng = new Regex("www.google.([a-z]+)|www.google.co.([a-z]+)|([a-z]+).search.yahoo.com|www.bing.com|www.aol.in|www.aol.com|www.aolsearch.com|www.ask.com|www.yandex.com|www.baidu.com");
                    Match match = null;
                    //step 1. Check if the URL contains utm_source, utm_medium in the URL. If yes then store utm_source in utmcsr, umt_medium in utmcmd and utm_campaign in utmccn
                    if (!(string.IsNullOrEmpty(request.QueryString["utm_source"]) || string.IsNullOrEmpty(request.QueryString["utm_medium"])))
                    {
                        utmcsr = request.QueryString["utm_source"];
                        utmcmd = request.QueryString["utm_medium"];
                        utmccn = request.QueryString["utm_campaign"];
                        utmtrm = request.QueryString["utm_term"];
                        utmcnt = request.QueryString["utm_content"];
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
                    else if (request.Cookies.Get("_cwutmz") != null) //step 5. If HTTP referrer is null then check if there is a BW source cookie, if yes then replicate that cookie
                    {
                        Regex utm = new Regex("utmcsr=([()A-Za-z0-9.-_!@#$%^*]+)[|]*");
                        string utmz = request.Cookies.Get("_cwutmz").Value;

                        if ((match = utm.Match(utmz)) != null && match.Success && match.Groups.Count > 0)
                        {
                            utmcsr = match.Groups[1].Value;
                        }
                        utm = new Regex("utmccn=([()A-Za-z0-9.-_!@#$%^*]+)[|]*");
                        if ((match = utm.Match(utmz)) != null && match.Success && match.Groups.Count > 0)
                        {
                            utmccn = match.Groups[1].Value;
                        }
                        utm = new Regex("utmcmd=([()A-Za-z0-9.-_!@#$%^*]+)[|]*");
                        if ((match = utm.Match(utmz)) != null && match.Success && match.Groups.Count > 0)
                        {
                                utmcmd = match.Groups[1].Value;
                        }
                        utm = new Regex("gclid=([()A-Za-z0-9.-_!@#$%^*]+)[|]*");
                        if ((match = utm.Match(utmz)) != null && match.Success && match.Groups.Count > 0)
                        {
                            utmgclid = match.Groups[1].Value;
                        }
                        utm = new Regex("utmtrm=([()A-Za-z0-9.-_!@#$%^*]+)[|]*");
                        if ((match = utm.Match(utmz)) != null && match.Success && match.Groups.Count > 0)
                        {
                            utmtrm = match.Groups[1].Value;
                        }
                        utm = new Regex("utmcnt=([()A-Za-z0-9.-_!@#$%^*]+)[|]*");
                        if ((match = utm.Match(utmz)) != null && match.Success && match.Groups.Count > 0)
                        {
                            utmcnt = match.Groups[1].Value;
                        }
                    }
                    else if (request.Cookies.Get("__utmz") != null) //step 6. If not then, check if there is a __utmz cookie, if yes then replicate utmcsr, utmccn, utmcmd, gclid
                    {
                        Regex utm = new Regex("utmcsr=([()A-Za-z0-9.-_!@#$%^*]+)[|]*");
                        string utmz = request.Cookies.Get("__utmz").Value;

                        if ((match = utm.Match(utmz)) != null && match.Success && match.Groups.Count > 0)
                        {
                            utmcsr = match.Groups[1].Value;
                        }

                        utm = new Regex("utmccn=([()A-Za-z0-9.-_!@#$%^*]+)[|]*");
                        if ((match = utm.Match(utmz)) != null && match.Success && match.Groups.Count > 0)
                        {
                            utmccn = match.Groups[1].Value;
                        }

                        utm = new Regex("utmcmd=([()A-Za-z0-9.-_!@#$%^*]+)[|]*");
                        if ((match = utm.Match(utmz)) != null && match.Success && match.Groups.Count > 0)
                        {
                            utmcmd = match.Groups[1].Value;
                        }

                        utm = new Regex("gclid=([()A-Za-z0-9.-_!@#$%^*]+)[|]*");
                        if ((match = utm.Match(utmz)) != null && match.Success && match.Groups.Count > 0)
                        {
                            utmgclid = match.Groups[1].Value;
                        }
                        utm = new Regex("utmtrm=([()A-Za-z0-9.-_!@#$%^*]+)[|]*");
                        if ((match = utm.Match(utmz)) != null && match.Success && match.Groups.Count > 0)
                        {
                            utmtrm = match.Groups[1].Value;
                        }
                        utm = new Regex("utmcnt=([()A-Za-z0-9.-_!@#$%^*]+)[|]*");
                        if ((match = utm.Match(utmz)) != null && match.Success && match.Groups.Count > 0)
                        {
                            utmcnt = match.Groups[1].Value;
                        }
                    }
                    else //step 7. If not then set utmcsr=(direct), utmccn=(direct), utmcmd=(none) 
                    {
                        utmcsr = "(direct)";
                        utmccn = "(direct)";
                        utmcmd = "(none)";
                    }
                    SetCookie("_cwutmz", 180, string.Format("utmcsr={0}|utmgclid={1}|utmccn={2}|utmcmd={3}|utmtrm={4}|utmcnt={5}", utmcsr, utmgclid, utmccn, utmcmd, utmtrm, utmcnt)); //step 8. Set the BW source cookie with utmcsr=<value>|utmgclid=gclid|utmccn=<value>|utmcmd=<value> with a 6 month expiry
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CWUTMZ(Ignore)");
                objErr.LogException();
            }
        }
        public static bool IsEligibleForORP
        {
            get
            {
                return ProductExperiments.IsEligibleForORP();

            }
        }

        public static bool IsReCaptchaVerified
        {
            get
            {
                int val = -1;
                var cookieObj = HttpContext.Current.Request.Cookies["grec"];
                if (cookieObj != null && Utility.RegExValidations.IsNumeric(cookieObj.Value))
                {
                    val = Convert.ToInt32(cookieObj.Value);
                }

                return val == 1;
            }
            internal set
            {
                HttpContext.Current.Response.Cookies["grec"].Value = value ? "1" : "0";
                HttpContext.Current.Response.Cookies["grec"].Expires = DateTime.Now.AddMonths(1);
                HttpContext.Current.Response.Cookies["grec"].Domain = CookieDomain;
            }
        }

    }//End Class
}//namespace