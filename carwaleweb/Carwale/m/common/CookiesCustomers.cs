
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;

namespace MobileWeb.Common
{			
	public class CookiesCustomers
	{
		public static string StateId
		{
			get
			{
				string val = "";	//default false
			
				if(HttpContext.Current.Request.Cookies["_CustStateId"] != null && HttpContext.Current.Request.Cookies["_CustStateId"].Value.ToString() != "")
				{
					val = HttpContext.Current.Request.Cookies["_CustStateId"].Value.ToString();
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
				objCookie.Expires = DateTime.Now.AddHours(3);
				HttpContext.Current.Response.Cookies.Add(objCookie);
			}
		}
		
		public static string CityId
		{
			get
			{
				string val = "";	//default false
			
				if(HttpContext.Current.Request.Cookies["_CustCityId"] != null &&
					HttpContext.Current.Request.Cookies["_CustCityId"].Value.ToString() != "")
				{
					val = HttpContext.Current.Request.Cookies["_CustCityId"].Value.ToString();
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
				objCookie.Expires = DateTime.Now.AddHours(3);
                objCookie.Domain = Carwale.UI.Common.CookiesCustomers.CookieDomain;
				HttpContext.Current.Response.Cookies.Add(objCookie);
			}
		}
		
		public static string City
		{
			get
			{
				string val = "";	//default false
			
				if( HttpContext.Current.Request.Cookies["_CustCity"] != null && HttpContext.Current.Request.Cookies["_CustCity"].Value.ToString() != "" )
				{
					val = HttpContext.Current.Request.Cookies["_CustCity"].Value.ToString();
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
				objCookie = new HttpCookie("_CustCity");
				objCookie.Value = value;
				objCookie.Expires = DateTime.Now.AddHours(3);
                objCookie.Domain = Carwale.UI.Common.CookiesCustomers.CookieDomain;
				HttpContext.Current.Response.Cookies.Add(objCookie);
			}
		}

        public static string MasterCity        
        {
            get
            {
                string val = "";	//default false

                if (HttpContext.Current.Request.Cookies["_CustCityMaster"] != null && HttpContext.Current.Request.Cookies["_CustCityMaster"].Value.ToString() != "")
                {
                    val = HttpContext.Current.Request.Cookies["_CustCityMaster"].Value.ToString();
                }
                else
                {
                    val = "";
                }

                return HttpUtility.UrlDecode(val);
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
                HttpContext.Current.Response.Cookies["_CustZoneMaster"].Domain = Carwale.UI.Common.CookiesCustomers.CookieDomain;
            }
        }

        public static int MasterCityId
        {
            get
            {
                int val = -1;

                if (HttpContext.Current.Request.Cookies["_CustCityIdMaster"] != null && Carwale.Utility.RegExValidations.IsNumeric(HttpContext.Current.Request.Cookies["_CustCityIdMaster"].Value.ToString()))
                {
                    val = Convert.ToInt32(HttpContext.Current.Request.Cookies["_CustCityIdMaster"].Value.ToString());
                }

                return val;
            }
        }

        public static int MasterZoneId
        {
            get
            {
                int val = -1;

                if (HttpContext.Current.Request.Cookies["_CustZoneIdMaster"] != null && Carwale.Utility.RegExValidations.IsNumeric(HttpContext.Current.Request.Cookies["_CustZoneIdMaster"].Value.ToString()))
                {
                    val = Convert.ToInt32(HttpContext.Current.Request.Cookies["_CustZoneIdMaster"].Value.ToString());
                }

                return val;
            }
        }

		public static string CustState
		{
			get
			{
				string val = "";	//default false
			
				if( HttpContext.Current.Request.Cookies["_CustState"] != null && HttpContext.Current.Request.Cookies["_CustState"].Value.ToString() != "" )
				{
					val = HttpContext.Current.Request.Cookies["_CustState"].Value.ToString();
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
				objCookie = new HttpCookie("_CustState");
				objCookie.Value = value;
				objCookie.Expires = DateTime.Now.AddHours(3);
				HttpContext.Current.Response.Cookies.Add(objCookie);
			}
		}
		
		public static string Email
		{
			get
			{
				string val = "";	//default false
			
				if(HttpContext.Current.Request.Cookies["_CustEmail"] != null && HttpContext.Current.Request.Cookies["_CustEmail"].Value.ToString() != "")
				{
					val = HttpContext.Current.Request.Cookies["_CustEmail"].Value.ToString();
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
				objCookie = new HttpCookie("_CustEmail");
				objCookie.Value = value;
				objCookie.Expires = DateTime.Now.AddHours(3);
                objCookie.Domain = Carwale.UI.Common.CookiesCustomers.CookieDomain;
				HttpContext.Current.Response.Cookies.Add(objCookie);
			}
		}
		
		public static string CustomerName
		{
			get
			{
				string val = "";	//default false
			
				if(HttpContext.Current.Request.Cookies["_CustomerName"] != null &&
					HttpContext.Current.Request.Cookies["_CustomerName"].Value.ToString() != "")
				{
					val = HttpContext.Current.Request.Cookies["_CustomerName"].Value.ToString();
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
				objCookie = new HttpCookie("_CustomerName");
				objCookie.Value = value;
				objCookie.Expires = DateTime.Now.AddHours(3);
                objCookie.Domain = Carwale.UI.Common.CookiesCustomers.CookieDomain;
				HttpContext.Current.Response.Cookies.Add(objCookie);
			}
		}
		
		public static string Mobile
		{
			get
			{
				string val = "";	//default false
			
				if( HttpContext.Current.Request.Cookies["_CustMobile"] != null && HttpContext.Current.Request.Cookies["_CustMobile"].Value.ToString() != "" )
				{
					val = HttpContext.Current.Request.Cookies["_CustMobile"].Value.ToString();
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
				objCookie = new HttpCookie("_CustMobile");
				objCookie.Value = value;
				objCookie.Expires = DateTime.Now.AddHours(3);
                objCookie.Domain = Carwale.UI.Common.CookiesCustomers.CookieDomain;
				HttpContext.Current.Response.Cookies.Add(objCookie);
			}
		}

        public static string LandLine
        {
            get
            {
                string val = "";	//default false

                if (HttpContext.Current.Request.Cookies["_CustLandLine"] != null && HttpContext.Current.Request.Cookies["_CustLandLine"].Value.ToString() != "")
                {
                    val = HttpContext.Current.Request.Cookies["_CustLandLine"].Value.ToString();
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
                objCookie = new HttpCookie("_CustLandLine");
                objCookie.Value = value;
                objCookie.Expires = DateTime.Now.AddHours(3);
                HttpContext.Current.Response.Cookies.Add(objCookie);
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
		}

        public static string UserModelHistory
        {
            get
            {
                var cookieObj = HttpContext.Current.Request.Cookies["_userModelHistory"];
                string val = string.Empty;

                if (cookieObj != null && cookieObj.Value != string.Empty)
                {
                   val = cookieObj.Value;
                }

                return val;
            }
        }
        public static bool IsEligibleForORP
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(HttpContext.Current.Items["IsEligibleForORP"]);
                }
                catch (Exception)
                { }
                return false;
            }
        }
        }//End Class
}//namespace
