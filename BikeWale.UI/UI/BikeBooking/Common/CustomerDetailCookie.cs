using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.BikeBooking
{
    public class CustomerDetailCookie
    {
        public static void CreateCustomerDetailCookie(string customerName, string customerEmail, string customerMobile)
        {
            HttpCookie objCookie = new HttpCookie("_CustomerDetail");
            objCookie.Values["CustomerName"] = customerName;
            objCookie.Values["CustomerEmail"] = customerEmail;
            objCookie.Values["CustomerMobile"] = customerMobile;

            HttpContext.Current.Response.Cookies.Add(objCookie);

           // HttpContext.Current.Response.Cookies["_CustomerDetail"].Expires = System.DateTime.Now.AddMinutes(HttpContext.Current.Session.Timeout);
        }

        private static string _customerName = string.Empty;
        public static string CustomerName
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["_CustomerDetail"] != null)
                    return HttpContext.Current.Request.Cookies["_CustomerDetail"]["CustomerName"].ToString();
                else
                    return _customerName;
            }
        }

        private static string _customerEmail = string.Empty;
        public static string CustomerEmail
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["_CustomerDetail"] != null)
                    return HttpContext.Current.Request.Cookies["_CustomerDetail"]["CustomerEmail"];
                else
                    return _customerEmail;
            }
        }

        private static string _customerMobile = string.Empty;
        public static string CustomerMobile
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["_CustomerDetail"] != null)
                    return HttpContext.Current.Request.Cookies["_CustomerDetail"]["CustomerMobile"];
                else
                    return _customerMobile;
            }
        }
    }
}