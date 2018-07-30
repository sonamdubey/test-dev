using System;
using System.Web;

namespace Bikewale.BikeBooking.Common
{
    public static class CustomerPaymentCookie
    {
        public static void CreateCustomerPaymentCookie(string bookingId, bool isSMSSend, bool isMailSend)
        {
            HttpCookie objCookie = new HttpCookie("_CustomerPayment");
            objCookie.Values["BookingId"] = bookingId;
            objCookie.Values["IsSMSSend"] = isSMSSend.ToString();
            objCookie.Values["IsMailSend"] = isMailSend.ToString();

            HttpContext.Current.Response.Cookies.Add(objCookie);
        }

        public static bool IsExists { get { return HttpContext.Current.Request.Cookies["_CustomerPayment"] != null; } }

        private static string _BookingId = String.Empty;
        public static string BookingId
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["_CustomerPayment"] != null)
                    return HttpContext.Current.Request.Cookies["_CustomerPayment"]["BookingId"].ToString();
                else
                    return _BookingId;
            }
            set
            {
                HttpContext.Current.Request.Cookies["_CustomerPayment"]["BookingId"] = value;
            }
        }

        private static bool _isSMSSend = false;
        public static bool IsSMSSend
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["_CustomerPayment"] != null)
                    return Convert.ToBoolean(HttpContext.Current.Request.Cookies["_CustomerPayment"]["IsSMSSend"]);
                else
                    return _isSMSSend;
            }
            set
            {
                HttpContext.Current.Request.Cookies["_CustomerPayment"]["IsSMSSend"] = value.ToString();
            }
        }

        private static bool _isMailSend = false;
        public static bool IsMailSend
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["_CustomerPayment"] != null)
                    return Convert.ToBoolean(HttpContext.Current.Request.Cookies["_CustomerPayment"]["IsMailSend"]);
                else
                    return _isMailSend;
            }
            set
            {
                HttpContext.Current.Request.Cookies["_CustomerPayment"]["IsMailSend"] = value.ToString();
            }
        }
    }
}