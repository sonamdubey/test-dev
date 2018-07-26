using Bikewale.Entities;
using Bikewale.Entities.Customer;
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
        /// Modified By : Lucky Rathore
        /// Modified On : 11 July 2016
        /// Description : Change RegEx for search Engine bikewale domain name for http Refferer.
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

                if (request.UrlReferrer != null)
                {
                    httpReffer = request.UrlReferrer.Host; //e.g. www.google.com, www.carwale.com
                }


                if (httpReffer.Contains("bikewale.com"))
                {
                    SetCookie("_bwutmz", 180);
                }
                else
                {
                    string url = request.Url.ToString();
                    Regex serachEng = new Regex("www.google.([a-z]+)|www.google.co.([a-z]+)|([a-z]+).search.yahoo.com|www.bing.com|www.aol.in|www.aol.com|www.aolsearch.com|www.ask.com|www.yandex.com|www.baidu.com");
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
                            Regex serachEngNames = new Regex("google|yahoo|bing|ask|yandex|baidu|aol|duckduckgo");
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

                //set user cookie for ab testing
                if (request.Cookies.Get("_bwtest") == null)
                {
                    SetBikewaleABTestingUser();
                }
            }
            catch (Exception)
            {
                //
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 01 Sep 2016
        /// Description :   Sets buyer details from tempcurretnuser cookie for 1 day
        /// Modified by :   Sumit Kate on 02 Mar 2017
        /// Description :   Use _PQUser cookie
        /// </summary>
        /// <param name="buyerDetails"></param>
        /// <returns></returns>
        public static bool SetBuyerDetailsCookie(string buyerDetails)
        {
            bool isDone = false;
            try
            {
                Cookie cookie = new Cookie("_PQUser");
                cookie.Value = buyerDetails;
                cookie.Expires = DateTime.Now.AddYears(1);
                CookieManager.Add(cookie);
                isDone = true;
            }
            catch (Exception)
            {

            }

            return isDone;
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
                Cookie bwCookie = new Cookie(name);
                if (value != null)
                {
                    bwCookie.Value = value;
                }
                bwCookie.Expires = DateTime.Now.AddDays(lifeTime);
                CookieManager.Add(bwCookie);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 01 Sep 2016
        /// Description :   Gets the buyer details from TempCurrentUser cookie
        /// Modified by :   Sumit Kate on 02 Mar 2017
        /// Description :   Replaced old used bike cookie with PQUser cookie. Single user cookie will be used to form pre-fill
        /// </summary>
        /// <param name="BuyerName"></param>
        /// <param name="BuyerMobile"></param>
        /// <param name="BuyerEmail"></param>
        /// <param name="BuyerId"></param>
        public static void GetBuyerDetailsFromCookie(ref CustomerEntityBase customer)
        {
            try
            {
                var request = HttpContext.Current.Request;
                if (request.Cookies["_PQUser"] != null && request.Cookies["_PQUser"].Value.ToString() != "")
                {
                    string userData = request.Cookies["_PQUser"].Value.ToString();

                    if (userData.Length > 0 && userData.IndexOf('&') > 0)
                    {
                        string[] details = userData.Split('&');

                        int detailsLength = details.Length;

                        customer.CustomerName = details[0];
                        customer.CustomerMobile = detailsLength > 2 ? Utility.CommonValidators.ParseMobileNumber(details[2]) : "";
                        customer.CustomerEmail = detailsLength > 1 ?( details[1] == "" ? CurrentUser.Email : details[1]) : "";
                        if (detailsLength > 3)
                            customer.CustomerId = Convert.ToUInt64(BikewaleSecurity.DecryptUserId(Convert.ToInt64(details[3])));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 19th December 2017
        /// Description : To get abtest user cookie and check for valid test case
        /// Modified by : Snehal Dange on 30th April 2018
        /// Description: Modified the logic to handle 3 make pages in abtest
        /// </summary>
        public static MakeABTestCookie GetAbTestCookieFlag(string percentage)
        {
            MakeABTestCookie abTestResults = null;
            try
            {
                ushort cookieValue;
                abTestResults = new MakeABTestCookie();
                abTestResults.NewMakePageV1Status = false;
                string[] percentageString = percentage.Split(',');

                ushort newMakePagePercent = 0;
                ushort newMakePageV1Percent = 0;
                
                if(percentageString.Length == 2)
                {
                   newMakePagePercent = Convert.ToUInt16(percentageString[0]); //first make page experiment
                   newMakePageV1Percent = Convert.ToUInt16(percentageString[1]);
                }
                
                var cookie = HttpContext.Current.Request.Cookies["_bwtest"];

                if (cookie != null && !string.IsNullOrEmpty(Convert.ToString(cookie.Value)) && ushort.TryParse(cookie.Value, out cookieValue) && cookieValue > 0)
                {
                    if (cookieValue <= newMakePagePercent)
                    {
                        abTestResults.ViewName = "Index_Mobile_New.cshtml";
                        abTestResults.IsNewPage = true;
                    }
                    else if (cookieValue <= (newMakePageV1Percent + newMakePagePercent))
                    {
                        abTestResults.ViewName = "Index_Mobile_New_v1.cshtml";
                        abTestResults.IsNewPage = true;
                        abTestResults.NewMakePageV1Status = true;
                    }
                    else if (cookieValue <= (newMakePageV1Percent + newMakePagePercent))
                    {
                        abTestResults.ViewName = "Index_Mobile.cshtml";
                        abTestResults.OldMakePageV1Status = true;
                    }
                    else
                    {
                        abTestResults.ViewName = "Index_Mobile.cshtml";
                        abTestResults.IsNewPage = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return abTestResults;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 14th December 2017
        /// Description : Create a random number which is assign to user for ab testing
        /// </summary>
        public static void SetBikewaleABTestingUser()
        {
            Random r = new Random((int)DateTime.Now.Ticks);
            SetCookie("_bwtest", 365, Convert.ToString(r.Next(1, 101)));
        }
    }
}
