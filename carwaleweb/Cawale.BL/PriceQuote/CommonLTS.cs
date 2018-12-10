using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Carwale.BL.PriceQuote
{
    public class CommonLTS
    {
        public static string CookieLTS
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["CWLTS"] != null && HttpContext.Current.Request.Cookies["CWLTS"].Value.ToString() != "")
                {
                    return HttpContext.Current.Request.Cookies["CWLTS"].Value.ToString();
                }
                else return "-1";
            }
            set
            {
                HttpCookie objCookie;
                objCookie = new HttpCookie("CWLTS");
                objCookie.Value = value;
                objCookie.Expires = DateTime.Now.AddDays(60);
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }


        /* This function first check whether the cookie has been set or not. If the cookie is set then gets the value of the campaign code and 
        the corresponding campaignid from the cookie and return it.
		
        */
        public static string CampaignId
        {
            get
            {
                string cookieVal = CommonLTS.CookieLTS;
                if (cookieVal != "-1")
                {
                    return cookieVal.Split(':').Length > 2 ? cookieVal.Split(':')[2] : "-1";
                }
                else
                    return "-1";
            }
        }

        public static string CampaignCode
        {
            get
            {
                string cookieVal = CommonLTS.CookieLTS;
                if (cookieVal != "-1")
                {
                    return cookieVal.Split(':').Length > 0 ? cookieVal.Split(':')[0] : "";
                }
                else
                    return "";
            }
        }
    }
}
