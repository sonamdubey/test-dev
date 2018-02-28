using System;
using System.Web;
using System.Web.Security;

namespace Bikewale.Utility
{
    /// <summary>
    /// Created By  :   Sadhana Upadyay on 29 Dec 2015
    /// Summary     :   
    /// </summary>
    public class CurrentUser
    {
        private static string bwCookieName = "BWC", bwVisitorCookieName = "_cwv";
        private static ushort bwCookieExpiryInYears = 2, bwVisitorSessionIdMaxLength = 10, bwCookieSessionIdMaxLength = 25;
        private static ushort maxVisitorSessionTimeInSeconds = 1800;
        /// <summary>
        /// Created By  :   Sadhana Upadyay on 29 Dec 2015
        /// Summary     :   
        /// </summary>
        public static void GenerateUniqueCookie()
        {
            HttpCookie bwc = HttpContext.Current.Request.Cookies["BWC"];
            HttpCookie _cwv = HttpContext.Current.Request.Cookies["_cwv"];
            if (bwc == null)
            {
                //create the cwc cookie with a unique random value
                string bwcCookieValue = RandomNoGenerator.GetUniqueKey(25);

                Cookie bwcCookie = new Cookie("BWC");
                bwcCookie.Value = bwcCookieValue;
                bwcCookie.Expires = DateTime.Now.AddYears(2);
                CookieManager.Add(bwcCookie);

                //also add the cookie for cwv, to identify the session or the visit and also the number of times this visitor has come.
                //Since this has been implemented later, hence add value for visit count only when cwc is being initialized
                //the format for _cwv cookie is : cwcValue.random_key_for_cwv.visit_start_timestamp.visit_last_timestamp.visit_count
                string bwvCookieValue = RandomNoGenerator.GetUniqueKey(10); //add 10 digit unique key for the cwv

                Cookie bwvCookie = new Cookie("_cwv");

                long currServerTimeStamp = GetCurrentUnixTimeStamp();
                bwvCookie.Value = bwcCookieValue + "." + bwvCookieValue + "." + currServerTimeStamp + "." + currServerTimeStamp + "." + currServerTimeStamp + ".1" + ".r";
                bwvCookie.Expires = DateTime.Now.AddYears(2);
                CookieManager.Add(bwvCookie);
            }
            else if (bwc != null && _cwv == null)
            {
                //also add the cookie for cwv, to identify the session or the visit and also the number of times this visitor has come.
                //Since this has been implemented later, hence add value for visit count only when cwc is being initialized
                //the format for _cwv cookie is : cwcValue.random_key_for_cwv.visit_start_timestamp.visit_last_timestamp.visit_count
                string bwvCookieValue = RandomNoGenerator.GetUniqueKey(10); //add 10 digit unique key for the cwv

                Cookie bwvCookie = new Cookie("_cwv");

                long currServerTimeStamp = GetCurrentUnixTimeStamp();
                bwvCookie.Value = bwc.Value + "." + bwvCookieValue + "." + currServerTimeStamp + "." + currServerTimeStamp + "." + currServerTimeStamp + ".1" + ".r";
                bwvCookie.Expires = DateTime.Now.AddYears(2);
                CookieManager.Add(bwvCookie);

            }
        }


        /// <summary>
        /// Created by  :   Sumit Kate on 27 Feb 2018
        /// Description :   GenerateUniqueCookie V2.
        /// </summary>
        public static void GenerateUniqueCookieV2()
        {
            HttpCookie bwc = HttpContext.Current.Request.Cookies[bwCookieName];
            try
            {
                if (bwc == null)
                {
                    //create the bwc cookie with a unique random value. This is session id.
                    string bwcCookieValue = RandomNoGenerator.GetUniqueKey(bwCookieSessionIdMaxLength);

                    Cookie bwcCookie = new Cookie(bwCookieName);
                    bwcCookie.Value = bwcCookieValue;
                    bwcCookie.Expires = DateTime.Now.AddYears(bwCookieExpiryInYears);
                    CookieManager.Add(bwcCookie);

                    //also add the cookie for cwv, to identify the session or the visit and also the number of times this visitor has come.
                    //Since this has been implemented later, hence add value for visit count only when cwc is being initialized
                    //the format for _cwv cookie is : cwcValue.random_key_for_cwv.visit_start_timestamp.previous_visit_timestamp.current_visit_timestamp.visit_count
                    string bwvCookieValue = RandomNoGenerator.GetUniqueKey(bwVisitorSessionIdMaxLength); //add 10 digit unique key for the cwv
                    Cookie bwvCookie = new Cookie(bwVisitorCookieName);
                    long currServerTimeStamp = GetCurrentUnixTimeStamp();
                    bwvCookie.Value = string.Format("{0}.{1}.{2}.{3}.{4}.1",
                                                    bwcCookieValue,
                                                    bwvCookieValue,
                                                    currServerTimeStamp,
                                                    currServerTimeStamp,
                                                    currServerTimeStamp);
                    bwvCookie.Expires = DateTime.Now.AddYears(bwCookieExpiryInYears);
                    CookieManager.Add(bwvCookie);
                }
                else
                {
                    //if the cwv cookie exists, then check whether the sesssion has expired or not. The default session timeout is 30 minutes. 
                    //If the difference between the last updated time and current time is more than 30 minutes, then re-initialize the cwv cookie
                    //Also, increase the visit count by 1

                    //format of _cwv = cwcValue.random_key_for_cwv.visit_start_timestamp.previous_visit_timestamp.current_visit_timestamp.visit_count
                    //Visit_Count would not be there in some cases. In those cases the number of variables would be 4
                    //timestamp would be in seconds and based on UNIX Time Stamp. Which means number of seconds since
                    //1st Jan 1970 00:00:00
                    string bwcValue = bwc.Value;
                    bwc.Expires = DateTime.Now.AddYears(-5);
                    HttpContext.Current.Response.SetCookie(bwc);
                    Cookie bwcCookie = new Cookie(bwCookieName);
                    bwcCookie.Value = bwcValue;
                    bwcCookie.Expires = DateTime.Now.AddYears(bwCookieExpiryInYears);
                    CookieManager.Add(bwcCookie);
                    HttpCookie bwv = HttpContext.Current.Request.Cookies[bwVisitorCookieName];
                    if (bwv == null || bwv.Value.Split('.').Length < 6)
                    {

                        //start a new visit
                        long currServerTimeStamp = GetCurrentUnixTimeStamp();
                        string _bwvCookieVal = string.Format("{0}.{1}.{2}.{3}.{4}.1",
                                                    bwcValue,
                                                    RandomNoGenerator.GetUniqueKey(bwVisitorSessionIdMaxLength),
                                                    currServerTimeStamp,
                                                    currServerTimeStamp,
                                                    currServerTimeStamp);

                        //add this cookie
                        Cookie bwvCookie = new Cookie(bwVisitorCookieName);
                        bwvCookie.Value = _bwvCookieVal;
                        bwvCookie.Expires = DateTime.Now.AddYears(bwCookieExpiryInYears);
                        CookieManager.Add(bwvCookie);
                    }
                    else
                    {
                        //get all the variables
                        string[] bwvValues = bwv.Value.Split('.');

                        string bwcVal = bwvValues[0];
                        string bwvVal = bwvValues[1];
                        string _svisitStartTS = bwvValues[2];
                        string _svisitLastTS = bwvValues[3];
                        string _svisitCurrCookieTS = bwvValues[4];
                        string _svisitCount = bwvValues[5];

                        //get current unix timestamp in seconds
                        long curTS = GetCurrentUnixTimeStamp();

                        //convert lst time stamp to long
                        long _lvisitStartTS;
                        long _lvisitLastTS;
                        long _lvisitCurrCookieTS;
                        long _lvisitCount;
                        bool isNumericStartTS = long.TryParse(_svisitStartTS, out _lvisitStartTS);	//false if the data is tampered
                        bool isNumericLastTS = long.TryParse(_svisitLastTS, out _lvisitLastTS);	//false if the data is tampered
                        bool isNumericCurrCookieTS = long.TryParse(_svisitCurrCookieTS, out _lvisitCurrCookieTS);	//false if the data is tampered
                        bool isNumericCount = long.TryParse(_svisitCount, out _lvisitCount);

                        if (isNumericStartTS && isNumericLastTS && isNumericCurrCookieTS && isNumericCount && _lvisitStartTS <= _lvisitLastTS && _lvisitLastTS <= _lvisitCurrCookieTS && _lvisitCurrCookieTS <= curTS)
                        {
                            //if the data is not tampered, then check whether the session is more than 30 mins. If yes, then create a new 
                            //visit id and reinitiate all the values in the cwv cookie. Also do the increment of the visit count by one, if it existed
                            ///
                            if ((curTS - _lvisitCurrCookieTS) >= maxVisitorSessionTimeInSeconds)//sessionDiff = ;// 30 * 60;
                            {
                                //start a new visit
                                string _bwvCookieVal = string.Format("{0}.{1}.{2}.{3}.{4}.{5}",
                                                bwcVal,
                                                RandomNoGenerator.GetUniqueKey(bwVisitorSessionIdMaxLength),
                                                curTS,
                                                curTS,
                                                curTS,
                                                _lvisitCount + 1
                                                );
                                //add this cookie
                                bwv.Expires = DateTime.Now.AddYears(-5);
                                HttpContext.Current.Response.SetCookie(bwv);
                                Cookie cwvCookie = new Cookie(bwVisitorCookieName);
                                cwvCookie.Value = _bwvCookieVal;
                                cwvCookie.Expires = DateTime.Now.AddYears(bwCookieExpiryInYears);
                                CookieManager.Add(cwvCookie);
                            }
                            else
                            {
                                string _cwvCookieVal = HttpContext.Current.Request.Cookies[bwVisitorCookieName].Value;
                                bwv.Expires = DateTime.Now.AddYears(-5);
                                HttpContext.Current.Response.SetCookie(bwv);
                                Cookie cwvCookie = new Cookie(bwVisitorCookieName);
                                cwvCookie.Value = string.Format("{0}.{1}.{2}.{3}.{4}.{5}",
                                                bwcVal,
                                                bwvVal,
                                                _svisitStartTS,
                                                _svisitCurrCookieTS,
                                                curTS,
                                                _lvisitCount
                                                );
                                cwvCookie.Expires = DateTime.Now.AddYears(bwCookieExpiryInYears);
                                CookieManager.Add(cwvCookie);
                            }
                        }
                        else
                        {
                            //start a new visit
                            long currServerTimeStamp = GetCurrentUnixTimeStamp();
                            string _cwvCookieVal = string.Format("{0}.{1}.{2}.{3}.{4}.1",
                                                        bwcValue,
                                                        RandomNoGenerator.GetUniqueKey(bwVisitorSessionIdMaxLength),
                                                        currServerTimeStamp,
                                                        currServerTimeStamp,
                                                        currServerTimeStamp);

                            //add this cookie
                            Cookie cwvCookie = new Cookie(bwVisitorCookieName);
                            cwvCookie.Value = _cwvCookieVal;
                            cwvCookie.Expires = DateTime.Now.AddYears(bwCookieExpiryInYears);
                            CookieManager.Add(cwvCookie);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static long GetCurrentUnixTimeStamp()
        {
            DateTime unixTimeStamp = new DateTime(1970, 1, 1);	//Jan 1 1970
            TimeSpan diff = DateTime.Now - unixTimeStamp;
            return (long)diff.TotalSeconds;
        }

        ///<summary>
        /// This PopulateWhere gets the current user email as logged in. 
        ///if no user is logged in then it returns ""
        ///</summary>
        public static string Email
        {
            get
            {
                string email = "";
                if (HttpContext.Current.User.Identity.IsAuthenticated == true)
                {
                    FormsIdentity fi = (FormsIdentity)HttpContext.Current.User.Identity;
                    FormsAuthenticationTicket ticket = fi.Ticket;
                    email = ticket.UserData.Split(':')[1].ToString();
                }

                return email;
            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 1st April 2014
        /// Summary : Function to get Client IP Address
        /// </summary>
        /// <returns></returns>
        public static string GetClientIP()
        {
            //string clientIp = HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] == null ? DBNull.Value.ToString() : HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];
            //return clientIp;
            string[] serVars = { "HTTP_CLIENT_IP", "HTTP_X_FORWARDED_FOR", "HTTP_X_FORWARDED", "HTTP_X_CLUSTER_CLIENT_IP", "HTTP_FORWARDED_FOR", "HTTP_FORWARDED", "REMOTE_ADDR" };
            string clientIp = string.Empty;
            foreach (string serverVariable in serVars)
            {
                clientIp = HttpContext.Current.Request.ServerVariables[serverVariable] == null ? DBNull.Value.ToString() : HttpContext.Current.Request.ServerVariables[serverVariable];
                if (!String.IsNullOrEmpty(clientIp))
                {
                    if (serverVariable == "HTTP_X_FORWARDED_FOR")
                    {
                        if (!string.IsNullOrEmpty(clientIp))
                        {
                            string[] ipRange = clientIp.Split(',');
                            if (ipRange != null)
                            {
                                clientIp = ipRange[ipRange.Length - 1];
                            }
                        }
                    }
                    break;
                }
            }
            //string clientIp = HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] == null ? DBNull.Value.ToString() : HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];

            return clientIp;
        }
        /// <summary>
        /// Created By : Sushil Kumar on 14th December 2017
        /// Description : Create a random number which is assign to user for ab testing
        /// </summary>
        public static void SetBikewaleABTestingUser()
        {
            Random r = new Random((int)DateTime.Now.Ticks);
            BWCookies.SetCookie("_bwtest", 365, Convert.ToString(r.Next(1, 101)));
        }
    }
}
