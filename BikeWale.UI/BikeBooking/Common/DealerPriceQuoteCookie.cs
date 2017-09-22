using System;
using System.Web;

namespace Bikewale.BikeBooking
{
    public class DealerPriceQuoteCookie
    {

        public static void CreateDealerPriceQuoteCookie(string pqId, bool isSMSSend, bool isMailSend)
        {
            HttpCookie objCookie = new HttpCookie("_DealerPriceQuote");
            objCookie.Values["PQId"] = pqId;
            objCookie.Values["IsSMSSend"] = isSMSSend.ToString();
            objCookie.Values["IsMailSend"] = isMailSend.ToString();

            HttpContext.Current.Response.Cookies.Add(objCookie);
        }

        private static string _pqId = "0";
        public static string PQId
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["_DealerPriceQuote"] != null)
                    return HttpContext.Current.Request.Cookies["_DealerPriceQuote"]["PQId"].ToString();
                else
                    return _pqId;
            }
            set
            {
                HttpContext.Current.Request.Cookies["_DealerPriceQuote"]["PQId"] = value;
            }
        }

        private static bool _isSMSSend = false;
        public static bool IsSMSSend
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["_DealerPriceQuote"] != null)
                    return Convert.ToBoolean(HttpContext.Current.Request.Cookies["_DealerPriceQuote"]["IsSMSSend"]);
                else
                    return _isSMSSend;
            }
            set
            {
                HttpContext.Current.Request.Cookies["_DealerPriceQuote"]["IsSMSSend"] = value.ToString();
            }
        }

        private static bool _isMailSend = false;
        public static bool IsMailSend
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["_DealerPriceQuote"] != null)
                    return Convert.ToBoolean(HttpContext.Current.Request.Cookies["_DealerPriceQuote"]["IsMailSend"]);
                else
                    return _isMailSend;
            }
            set
            {
                HttpContext.Current.Request.Cookies["_DealerPriceQuote"]["IsMailSend"] = value.ToString();
            }
        }
    }
}