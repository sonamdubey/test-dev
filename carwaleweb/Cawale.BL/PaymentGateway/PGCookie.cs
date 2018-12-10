using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
//using System.Threading.Tasks;
using System.Web;

namespace Carwale.BL.PaymentGateway
{
    public class PGCookie
    {
        private static string CookieDomain
        {
            get
            {
                var host = HttpContext.Current.Request.Url.Host;
                Match mc = new Regex("(.*).carwale.com").Match(host);
                if (mc.Success && mc.Groups[1].Value == "www") host = "carwale.com";
                return host;
            }
        }

        public static string PGRecordId
        {
            get
            {
                string val = string.Empty;

                if (HttpContext.Current.Request.Cookies["PGRecordId"] != null &&
                    HttpContext.Current.Request.Cookies["PGRecordId"].Value.ToString() != "")
                {
                    val = HttpContext.Current.Request.Cookies["PGRecordId"].Value.ToString();
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
                objCookie = new HttpCookie("PGRecordId");
                objCookie.Value = value;
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }

        public static string PGRespCode
        {
            get
            {
                string val = string.Empty;

                if (HttpContext.Current.Request.Cookies["PGRespCode"] != null &&
                    HttpContext.Current.Request.Cookies["PGRespCode"].Value.ToString() != "")
                {
                    val = HttpContext.Current.Request.Cookies["PGRespCode"].Value.ToString();
                }
                else
                {
                    val = "-1";
                }

                return val;
            }
            set
            {
                HttpContext.Current.Request.Cookies.Remove("PGRespCode");
                HttpContext.Current.Response.Cookies["PGRespCode"].Value = value;
                //HttpCookie objCookie;
                //objCookie = new HttpCookie("PGRespCode");
                //objCookie.Value = value;
                //HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }

        public static string PGMessage
        {
            get
            {
                string val = string.Empty;

                if (HttpContext.Current.Request.Cookies["PGMessage"] != null &&
                    HttpContext.Current.Request.Cookies["PGMessage"].Value.ToString() != "")
                {
                    val = HttpContext.Current.Request.Cookies["PGMessage"].Value.ToString();
                }
                else
                {
                    val = "";
                }

                return val;
            }
            set
            {
                HttpCookie objCookie;
                objCookie = new HttpCookie("PGMessage");
                objCookie.Value = value;
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }

        public static string PromotionalOfferId
        {
            get
            {
                string val = string.Empty;

                if (HttpContext.Current.Request.Cookies["PromotionalOfferId"] != null &&
                    HttpContext.Current.Request.Cookies["PromotionalOfferId"].Value.ToString() != "")
                {
                    val = HttpContext.Current.Request.Cookies["PromotionalOfferId"].Value.ToString();
                }
                else
                    val = "-1";

                return val;
            }
            set
            {
                HttpCookie objCookie;
                objCookie = new HttpCookie("PromotionalOfferId");
                objCookie.Value = value;
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }

        public static string PGCarId
        {
            get
            {
                string val = string.Empty;

                if (HttpContext.Current.Request.Cookies["PGCarId"] != null &&
                    HttpContext.Current.Request.Cookies["PGCarId"].Value.ToString() != "")
                {
                    val = HttpContext.Current.Request.Cookies["PGCarId"].Value.ToString();
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
                objCookie = new HttpCookie("PGCarId");
                objCookie.Value = value;
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }

        public static string SendInqNotification
        {
            get
            {
                string val = string.Empty;

                if (HttpContext.Current.Request.Cookies["_SendInqNotification"] != null &&
                    HttpContext.Current.Request.Cookies["_SendInqNotification"].Value.ToString() != "")
                {
                    val = HttpContext.Current.Request.Cookies["_SendInqNotification"].Value.ToString();
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
                objCookie = new HttpCookie("_SendInqNotification");
                objCookie.Value = value;
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }

        public static string PGAmount
        {
            get
            {
                string val = string.Empty;

                if (HttpContext.Current.Request.Cookies["PGAmount"] != null &&
                    HttpContext.Current.Request.Cookies["PGAmount"].Value.ToString() != "")
                {
                    val = HttpContext.Current.Request.Cookies["PGAmount"].Value.ToString();
                }
                else
                {
                    val = "0";
                }

                return val;
            }
            set
            {
                HttpCookie objCookie;
                objCookie = new HttpCookie("PGAmount");
                objCookie.Value = value;
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }

        public static string PGTransId
        {
            get
            {
                string val = string.Empty;

                if (HttpContext.Current.Request.Cookies["PGTransId"] != null &&
                    HttpContext.Current.Request.Cookies["PGTransId"].Value.ToString() != "")
                {
                    val = HttpContext.Current.Request.Cookies["PGTransId"].Value.ToString();
                }
                else
                {
                    val = "-1";
                }

                return val;
            }
            set
            {
                HttpContext.Current.Request.Cookies.Remove("PGTransId");
                HttpContext.Current.Response.Cookies["PGTransId"].Value = value;
                //HttpCookie objCookie;
                //objCookie = new HttpCookie("PGTransId");
                //objCookie.Value = value;
                //HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }




        public static string PGPkgId
        {
            get
            {
                string val = string.Empty;

                if (HttpContext.Current.Request.Cookies["PGPkgId"] != null &&
                    HttpContext.Current.Request.Cookies["PGPkgId"].Value.ToString() != "")
                {
                    val = HttpContext.Current.Request.Cookies["PGPkgId"].Value.ToString();
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
                objCookie = new HttpCookie("PGPkgId");
                objCookie.Value = value;
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }

        /// <summary>
        /// Cookies used in Offer Booking Process
        /// </summary>
        /// 
       
        public static string CustomerName
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["_CustomerName"] != null && HttpContext.Current.Request.Cookies["_CustomerName"].Value.ToString() != "")
                {
                    return HttpContext.Current.Request.Cookies["_CustomerName"].Value.ToString();
                }
                else
                    return "-1";
            }
            set
            {
                HttpCookie objCookie = new HttpCookie("_CustomerName");
                objCookie.Value = value.ToString();
                objCookie.Expires = DateTime.Now.AddMonths(6);
                objCookie.Domain = CookieDomain;
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }
        
        public static string CustomerEmail
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["_CustEmail"] != null && HttpContext.Current.Request.Cookies["_CustEmail"].Value.ToString() != "")
                {
                    return HttpContext.Current.Request.Cookies["_CustEmail"].Value.ToString();
                }
                else
                    return "-1";
            }
            set
            {
                HttpCookie objCookie = new HttpCookie("_CustEmail");
                objCookie.Value = value.ToString();
                objCookie.Expires = DateTime.Now.AddMonths(6);
                objCookie.Domain = CookieDomain;
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }

        public static string CustomerMobile
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["_CustMobile"] != null && HttpContext.Current.Request.Cookies["_CustMobile"].Value.ToString() != "")
                {
                    return HttpContext.Current.Request.Cookies["_CustMobile"].Value.ToString();
                }
                else
                    return "-1";
            }
            set
            {
                HttpCookie objCookie = new HttpCookie("_CustMobile");
                objCookie.Value = value.ToString();
                objCookie.Expires = DateTime.Now.AddMonths(6);
                objCookie.Domain = CookieDomain;
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }

        public static string CustomerCity
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["_CustCity"] != null && HttpContext.Current.Request.Cookies["_CustCity"].Value.ToString() != "")
                {
                    return HttpContext.Current.Request.Cookies["_CustCity"].Value.ToString();
                }
                else
                    return "-1";
            }
            set
            {
                HttpCookie objCookie = new HttpCookie("_CustCity");
                objCookie.Value = value.ToString();
                objCookie.Domain = CookieDomain;
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }

        public static string ResponseId
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["_ResponseId"] != null && HttpContext.Current.Request.Cookies["_ResponseId"].Value.ToString() != "")
                {
                    return HttpContext.Current.Request.Cookies["_ResponseId"].Value.ToString();
                }
                else
                    return "-1";
            }
            set
            {
                HttpCookie objCookie = new HttpCookie("_ResponseId");
                objCookie.Value = value.ToString();
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }

        public static string DealerId
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["_DealerId"] != null && HttpContext.Current.Request.Cookies["_DealerId"].Value.ToString() != "")
                {
                    return HttpContext.Current.Request.Cookies["_DealerId"].Value.ToString();
                }
                else
                    return "-1";
            }
            set
            {
                HttpCookie objCookie = new HttpCookie("_DealerId");
                objCookie.Value = value.ToString();
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }
        
        public static string CouponId
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["_CouponId"] != null && HttpContext.Current.Request.Cookies["_CouponId"].Value.ToString() != "")
                {
                    return HttpContext.Current.Request.Cookies["_CouponId"].Value.ToString();
                }
                else
                    return "-1";
            }
            set
            {
                HttpCookie objCookie = new HttpCookie("_CouponId");
                objCookie.Value = value.ToString();
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }
        
        public static string VersionId
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["_PQVersionId"] != null && HttpContext.Current.Request.Cookies["_PQVersionId"].Value.ToString() != "")
                {
                    return HttpContext.Current.Request.Cookies["_PQVersionId"].Value.ToString();
                }
                else
                    return "-1";
            }
            set
            {
                HttpCookie objCookie = new HttpCookie("_PQVersionId");
                objCookie.Value = value.ToString();
                objCookie.Domain = CookieDomain;
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }

        public static string OfferId
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["_OfferId"] != null && HttpContext.Current.Request.Cookies["_OfferId"].Value.ToString() != "")
                {
                    return HttpContext.Current.Request.Cookies["_OfferId"].Value.ToString();
                }
                else
                    return "-1";
            }
            set
            {
                HttpCookie objCookie = new HttpCookie("_OfferId");
                objCookie.Value = value.ToString();
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }

        public static string PQCarName
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["_PQCarName"] != null && HttpContext.Current.Request.Cookies["_PQCarName"].Value.ToString() != "")
                {
                    return HttpContext.Current.Request.Cookies["_PQCarName"].Value.ToString();
                }
                else
                    return "-1";
            }
            set
            {
                HttpCookie objCookie = new HttpCookie("_PQCarName");
                objCookie.Value = value.ToString();
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }

        public static string PQCarImage
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["_PQCarImg"] != null && HttpContext.Current.Request.Cookies["_PQCarImg"].Value.ToString() != "")
                {
                    return HttpContext.Current.Request.Cookies["_PQCarImg"].Value.ToString();
                }
                else
                    return "-1";
            }
            set
            {
                HttpCookie objCookie = new HttpCookie("_PQCarImg");
                objCookie.Value = value.ToString();
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }

        public static string PQOfferDesc
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["_PQOfferDesc"] != null && HttpContext.Current.Request.Cookies["_PQOfferDesc"].Value.ToString() != "")
                {
                    return HttpContext.Current.Request.Cookies["_PQOfferDesc"].Value.ToString();
                }
                else
                    return "-1";
            }
            set
            {
                HttpCookie objCookie = new HttpCookie("_PQOfferDesc");
                objCookie.Value = value.ToString();
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }

        public static string PGResponseUrl
        {
            get
            {
                var pgResponseUrl = HttpContext.Current.Request.Cookies["_PGResponseUrl"];
                if (pgResponseUrl != null && !string.IsNullOrEmpty(pgResponseUrl.Value))
                {
                    return pgResponseUrl.Value;
                }
                else
                    return "-1";
            }
            set
            {
                HttpCookie objCookie = new HttpCookie("_PGResponseUrl");
                objCookie.Value = value;
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }
    }
}
