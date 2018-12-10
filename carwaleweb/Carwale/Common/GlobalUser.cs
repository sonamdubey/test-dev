using Carwale.BL.Customers;
using Carwale.Entity;
using Carwale.Interfaces;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carwale.UI.Common
{
    public static class GlobalUser
    {
        public static void Authenticate()
        {
            bool resp = false;
            try
            {
                if (CurrentUser.Id == "-1")
                {
                    //use rememberme to authenticate the user by using rememberme cookie if present
                    HttpCookie rememberMe = HttpContext.Current.Request.Cookies.Get("RememberMe");
                    if (rememberMe != null)
                    {
                        ICustomerBL<Customer, CustomerOnRegister> customerRepo = new CustomerActions<Customer, CustomerOnRegister>();

                        //collect the boolean value to
                        //if boolean value is true redirect the user to current request url so that
                        //the login control is intialised properly
                        resp = customerRepo.UseActiveRememberMeSession();
                    }
                }
            }
            catch (Exception ex)
            {

                HttpCookie rememberMe1 = HttpContext.Current.Request.Cookies.Get("RememberMe");
                rememberMe1.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(rememberMe1);

                Logger.LogException(ex);
            }
            finally
            {
                if (resp){
                    Uri oldUri = new Uri(HttpContext.Current.Request.Url.ToString());
                    UriBuilder builder = new UriBuilder(oldUri);
                    builder.Port = -1;
                    builder.Scheme = new Uri(System.Configuration.ConfigurationManager.AppSettings["GoogleRedirectURL"]).Scheme;
                    HttpContext.Current.Response.Redirect(builder.Uri.ToString());
                }

            }
        }

        public static void TrackUserBehaviour()
        {
            HttpCookie cwc = HttpContext.Current.Request.Cookies["CWC"];
            try
            {
                if (cwc == null)
                {
                    //create the cwc cookie with a unique random value
                    Carwale.UI.Common.PriceQuoteCommon objPQCommon = new Carwale.UI.Common.PriceQuoteCommon();
                    string cwcCookieValue = objPQCommon.GetUniqueKey(25);

                    Cookie cwcCookie = new Cookie("CWC");
                    cwcCookie.Value = cwcCookieValue;
                    cwcCookie.Expires = DateTime.Now.AddYears(2);
                    CookieManager.Add(cwcCookie);

                    //also add the cookie for cwv, to identify the session or the visit and also the number of times this visitor has come.
                    //Since this has been implemented later, hence add value for visit count only when cwc is being initialized
                    //the format for _cwv cookie is : cwcValue.random_key_for_cwv.visit_start_timestamp.previous_visit_timestamp.current_visit_timestamp.visit_count
                    string cwvCookieValue = objPQCommon.GetUniqueKey(10); //add 10 digit unique key for the cwv
                    Cookie cwvCookie = new Cookie("_cwv");
                    long currServerTimeStamp = GetCurrentUnixTimeStamp();
                    cwvCookie.Value = string.Format("{0}.{1}.{2}.{3}.{4}.1",
                                                    cwcCookieValue,
                                                    cwvCookieValue,
                                                    currServerTimeStamp,
                                                    currServerTimeStamp,
                                                    currServerTimeStamp);
                    cwvCookie.Expires = DateTime.Now.AddYears(2);
                    CookieManager.Add(cwvCookie);
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
                    string cwcValue = cwc.Value;
                    cwc.Expires = DateTime.Now.AddYears(-5);
                    HttpContext.Current.Response.SetCookie(cwc);
                    Cookie cwcCookie = new Cookie("CWC");
                    cwcCookie.Value = cwcValue;
                    cwcCookie.Expires = DateTime.Now.AddYears(2);
                    CookieManager.Add(cwcCookie);
                    HttpCookie cwv = HttpContext.Current.Request.Cookies["_cwv"];
                    if (cwv == null || cwv.Value.Split('.').Length < 6)
                    {
                        //cwv is not there. Add the cookie value
                        Carwale.UI.Common.PriceQuoteCommon objPQCommon = new Carwale.UI.Common.PriceQuoteCommon();
                        //start a new visit
                        long currServerTimeStamp = GetCurrentUnixTimeStamp();
                        string _cwvCookieVal = string.Format("{0}.{1}.{2}.{3}.{4}.1",
                                                    cwcValue,
                                                    objPQCommon.GetUniqueKey(10),
                                                    currServerTimeStamp,
                                                    currServerTimeStamp,
                                                    currServerTimeStamp);

                        //add this cookie
                        Cookie cwvCookie = new Cookie("_cwv");
                        cwvCookie.Value = _cwvCookieVal;
                        cwvCookie.Expires = DateTime.Now.AddYears(2);
                        CookieManager.Add(cwvCookie);
                    }
                    else
                    {
                        //get all the variables
                        string[] cwvValues = cwv.Value.Split('.');

                        string cwcVal = cwvValues[0];
                        string cwvVal = cwvValues[1];
                        string _svisitStartTS = cwvValues[2];
                        string _svisitLastTS = cwvValues[3];
                        string _svisitCurrCookieTS = cwvValues[4];
                        string _svisitCount = cwvValues[5];

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
                            if ((curTS - _lvisitCurrCookieTS) >= 1800)//sessionDiff = ;// 30 * 60;
                            {
                                Carwale.UI.Common.PriceQuoteCommon objPQCommon = new Carwale.UI.Common.PriceQuoteCommon();
                                //start a new visit
                                string _cwvCookieVal = string.Format("{0}.{1}.{2}.{3}.{4}.{5}",
                                                cwcVal,
                                                objPQCommon.GetUniqueKey(10),
                                                curTS,
                                                curTS,
                                                curTS,
                                                _lvisitCount + 1
                                                );
                                //add this cookie
                                cwv.Expires = DateTime.Now.AddYears(-5);
                                HttpContext.Current.Response.SetCookie(cwv);
                                Cookie cwvCookie = new Cookie("_cwv");
                                cwvCookie.Value = _cwvCookieVal;
                                cwvCookie.Expires = DateTime.Now.AddYears(2);
                                CookieManager.Add(cwvCookie);
                            }
                            else
                            {
                                string _cwvCookieVal = HttpContext.Current.Request.Cookies["_cwv"].Value;
                                cwv.Expires = DateTime.Now.AddYears(-5);
                                HttpContext.Current.Response.SetCookie(cwv);
                                Cookie cwvCookie = new Cookie("_cwv");
                                cwvCookie.Value = string.Format("{0}.{1}.{2}.{3}.{4}.{5}",
                                                cwcVal,
                                                cwvVal,
                                                _svisitStartTS,
                                                _svisitCurrCookieTS,
                                                curTS,
                                                _lvisitCount
                                                );
                                cwvCookie.Expires = DateTime.Now.AddYears(2);
                                CookieManager.Add(cwvCookie);
                            }
                        }
                        else
                        {
                            Carwale.UI.Common.PriceQuoteCommon objPQCommon = new Carwale.UI.Common.PriceQuoteCommon();
                            //start a new visit
                            long currServerTimeStamp = GetCurrentUnixTimeStamp();
                            string _cwvCookieVal = string.Format("{0}.{1}.{2}.{3}.{4}.1",
                                                        cwcValue,
                                                        objPQCommon.GetUniqueKey(10),
                                                        currServerTimeStamp,
                                                        currServerTimeStamp,
                                                        currServerTimeStamp);

                            //add this cookie
                            Cookie cwvCookie = new Cookie("_cwv");
                            cwvCookie.Value = _cwvCookieVal;
                            cwvCookie.Expires = DateTime.Now.AddYears(2);
                            CookieManager.Add(cwvCookie);
                        }
                    }

                }
            }//try/ 
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public static long GetCurrentUnixTimeStamp()
        {
			return DateTime.Now.ToUnixTime();
        }
    }
}