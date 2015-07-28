/*
	Note: Please read this comments
	
	This is generalized class to store customer information in cookies for further use stead of asking user everytime. 
	if user is validated once. They are authorised to perform any task throughout the site. 
	Its can be usefull in various iterative tasks like Price Quote, Used bike search
	Where we can prefilled user information to respactive controls without asking user everytime
*/

using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;

namespace Bikewale.Common
{	
	/// <summary>
	///     Created By : Ashish G. Kamble on 23/8/2012
	/// </summary>
	
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
				HttpContext.Current.Response.Cookies.Add(objCookie);
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
				HttpContext.Current.Response.Cookies.Add(objCookie);
			}
		}

        public static string CustomerId
        {
            get
            {
                string val = "";	//default false

                if (HttpContext.Current.Request.Cookies["_CustomerId"] != null &&
                    HttpContext.Current.Request.Cookies["_CustomerId"].Value.ToString() != "")
                {
                    val = HttpContext.Current.Request.Cookies["_CustomerId"].Value.ToString();
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
                objCookie = new HttpCookie("_CustomerId");
                objCookie.Value = value;
                objCookie.Expires = DateTime.Now.AddHours(3);
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

        public static string SellInquiryId
        {
            get
            {
                string val = "";	//default false

                if (HttpContext.Current.Request.Cookies["_SellInquiryId"] != null && HttpContext.Current.Request.Cookies["_SellInquiryId"].Value.ToString() != "")
                {
                    val = HttpContext.Current.Request.Cookies["_SellInquiryId"].Value.ToString();
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
                objCookie = new HttpCookie("_SellInquiryId");
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
            HttpContext.Current.Response.Cookies["_SellInquiryId"].Expires = DateTime.Now.AddYears(-1);
		}
		
	}//End Class
}//namespace
