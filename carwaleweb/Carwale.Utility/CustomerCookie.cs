using System;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;

namespace Carwale.Utility
{
    public class CustomerCookie
    {
        public static string CookieDomain
        {
            get
            {
                try
                {
                    var host = HttpContext.Current.Request.Url.Host;
                    Match mc = new Regex("(.*).carwale.com").Match(host);
                    if (mc.Success && mc.Groups[1].Value == "www") host = "carwale.com";
                    return host;
                }
                catch (Exception)
                {

                }
                return System.Configuration.ConfigurationManager.AppSettings["Domain"] ?? "carwale.com";
            }
        }

        public static string StateId
        {
            get
            {
                var cookieObj = HttpContext.Current.Request.Cookies["_CustStateId"];
                string val;	//default false

                if (cookieObj != null && cookieObj.Value != string.Empty)
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
                objCookie = new HttpCookie("_CustStateId");
                objCookie.Value = value;
                //objCookie.Expires = DateTime.Now.AddHours(3);
                HttpContext.Current.Response.Cookies.Add(objCookie);
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

        public static int CustomerAreaId
        {
            get
            {
                int val;
                var cookieValue = HttpContextUtils.GetCookie("_CustAreaId");
                if (!int.TryParse(cookieValue, out val) && val <= 0)
                {
                    val = -1;
                }
                return val;
            }
            internal set
            {
                HttpContext.Current.Response.Cookies["_CustAreaId"].Value = value.ToString();
                HttpContext.Current.Response.Cookies["_CustAreaId"].Expires = DateTime.Now.AddMonths(6);
                HttpContext.Current.Response.Cookies["_CustAreaId"].Domain = CookieDomain;
            }
        }

        public static string CustomerAreaName
        {
            get
            {
                var cookieVal = HttpContextUtils.GetCookie("_CustAreaName");
                if(!string.IsNullOrEmpty(cookieVal))
                {
                    return HttpUtility.UrlDecode(cookieVal);
                }
                return string.Empty;

            }
            internal set
            {
                HttpContext.Current.Response.Cookies["_CustAreaName"].Value = value;
                HttpContext.Current.Response.Cookies["_CustAreaName"].Expires = DateTime.Now.AddMonths(6);
                HttpContext.Current.Response.Cookies["_CustAreaName"].Domain = CookieDomain;
            }
        }

        public static double CustomerLatitude
        {
            get
            {
                double val;
                var cookieValue = HttpContextUtils.GetCookie("_CustLatitude");
                if (!double.TryParse(cookieValue, out val) && !RegExValidations.IsValidLatitude(val))
                {
                    val = -100;  //invalid latitude
                }

                return val;
            }
            internal set
            {
                HttpContext.Current.Response.Cookies["_CustLatitude"].Value = value.ToString(CultureInfo.InvariantCulture);
                HttpContext.Current.Response.Cookies["_CustLatitude"].Expires = DateTime.Now.AddMonths(6);
                HttpContext.Current.Response.Cookies["_CustLatitude"].Domain = CookieDomain;
            }
        }

        public static double CustomerLongitude
        {
            get
            {
                double val;
                var cookieValue = HttpContextUtils.GetCookie("_CustLongitude");
                if (!double.TryParse(cookieValue, out val) && !RegExValidations.IsValidLongitude(val))
                {
                    val = -200; //invalid longitude
                }

                return val;
            }
            internal set
            {
                HttpContext.Current.Response.Cookies["_CustLongitude"].Value = value.ToString(CultureInfo.InvariantCulture);
                HttpContext.Current.Response.Cookies["_CustLongitude"].Expires = DateTime.Now.AddMonths(6);
                HttpContext.Current.Response.Cookies["_CustLongitude"].Domain = CookieDomain;
            }
        }

        public static string CustState
        {
            get
            {
                var cookieObj = HttpContext.Current.Request.Cookies["_CustState"];
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
                objCookie = new HttpCookie("_CustState");
                objCookie.Value = value;
                //objCookie.Expires = DateTime.Now.AddHours(3);
                objCookie.Domain = CookieDomain;
                HttpContext.Current.Response.Cookies.Add(objCookie);
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

        public static string LandLine
        {
            get
            {
                var cookieObj = HttpContext.Current.Request.Cookies["_CustLandLine"];
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
                objCookie = new HttpCookie("_CustLandLine");
                objCookie.Value = value;
                //objCookie.Expires = DateTime.Now.AddHours(3);
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

        //this function expires the cookie for the needing of the contact information
        public void ExpireCookies()
        {
            HttpContext.Current.Response.Cookies["_CustStateId"].Expires = DateTime.Now.AddYears(-1);
            HttpContext.Current.Response.Cookies["_CustCityId"].Expires = DateTime.Now.AddYears(-1);
            HttpContext.Current.Response.Cookies["_CustCity"].Expires = DateTime.Now.AddYears(-1);
            HttpContext.Current.Response.Cookies["_CustEmail"].Expires = DateTime.Now.AddYears(-1);
            HttpContext.Current.Response.Cookies["_CustomerName"].Expires = DateTime.Now.AddYears(-1);
            HttpContext.Current.Response.Cookies["_CustMobile"].Expires = DateTime.Now.AddYears(-1);
            HttpContext.Current.Response.Cookies["_CustLandLine"].Expires = DateTime.Now.AddYears(-1);
            HttpContext.Current.Response.Cookies["_CustCityMaster"].Expires = DateTime.Now.AddYears(-1);
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
                return Convert.ToInt32(HttpContext.Current.Request.Cookies["_abtest"].Value);
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

        public static HttpCookie StartSession(string userName, string userId, string email, bool isEmailVerified = false)
        {
            //create a ticket and add it to the cookie
            FormsAuthenticationTicket ticket;
            //now add the id and the role to the ticket, concat the id and role, separated by ',' 
            ticket = new FormsAuthenticationTicket(
                        1,
                        userName,
                        DateTime.Now,
                        DateTime.Now.AddDays(365),
                        false,
                        userId + ":" + email + ":" + isEmailVerified.ToString()
                    );

            //add the ticket into the cookie
            HttpCookie objCookie;
            objCookie = new HttpCookie(".ASPXAUTH");
            objCookie.Value = FormsAuthentication.Encrypt(ticket);
            return objCookie;
        }

        public static string GetUserId()
        {
            string userId = "-1";

            if (HttpContext.Current.User.Identity.IsAuthenticated == true)
            {
                FormsIdentity fi = (FormsIdentity)HttpContext.Current.User.Identity;
                FormsAuthenticationTicket ticket = fi.Ticket;

                string strRole = ticket.UserData.Split(':')[1].ToString().ToUpper();
                string strUserId = ticket.UserData.Split(':')[0].ToString();

                userId = ticket.UserData.Split(':')[0].ToString();
            }

            return userId;
        }
        /// <summary>
        /// Gets the Value in CompareVersions Cookie eg: 2370|2410
        /// </summary>
        /// <returns></returns>
        public static string CompareVersionsCookie
        {
            get
            {
                var compareVersions = string.Empty;

                if (!string.IsNullOrWhiteSpace(HttpContext.Current.Request.Cookies["CompareVersions"]?.Value))
                {
                    compareVersions = WebUtility.UrlDecode(HttpContext.Current.Request.Cookies["CompareVersions"].Value);
                }
                return compareVersions;
            }
        }

        public static void EndSession()
        {
            FormsAuthentication.SignOut();
        }
    }
}
