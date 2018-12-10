/*
	This class will use to bind controls like filling makes, states
	Written by: Satish Sharma On Jan 21, 2008 12:28 PM
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

namespace Carwale.UI.Common

{			
	/*********************************************************************/
		// Class manages all the cookies related to the research section
	/*********************************************************************/
	public class CookiesUserReviews
	{
		public static string URHelpful
		{
			get
			{
				string val = "";
			
				if(HttpContext.Current.Request.Cookies["URHelpful"] != null &&
					HttpContext.Current.Request.Cookies["URHelpful"].Value.ToString() != "")
				{
					val = HttpContext.Current.Request.Cookies["URHelpful"].Value.ToString();
				}	
				else
					val = "";	
				
				return val;
			}
			set
			{
				HttpCookie objCookie;
				objCookie = new HttpCookie("URHelpful");
				objCookie.Value = value;
				HttpContext.Current.Response.Cookies.Add(objCookie);
			}
		}
	}//class
}//namespace
