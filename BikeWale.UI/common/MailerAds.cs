// C# Document
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Collections;
using System.Collections.Specialized;

namespace Bikewale.Common
{
	public class MailerAds
	{
		public string GetAdScript(string customerId)
		{
			string html = "";
			
			try
			{
				/*//get all the customer details
				CustomerDetails cd = new CustomerDetails(customerId);
				
				string name = cd.Name;
				string fName = cd.FirstName;
				string lName = cd.LastName;
				string eMail = cd.Email;
				string mobile = cd.Mobile;
				string city = cd.City;
				
				//download the data from the icici lombard page
				string url = "http://www.carwale.com/ads/ICICILomb/form.aspx?"
						   + "nm=" + name
						   + "&em=" + eMail
						   + "&mb=" + mobile
						   + "&ct=" + city;
							
				html = GetData(url);
				*/
			}
			catch(Exception err)
			{
				ErrorClass objErr = new ErrorClass(err,"MailerAds.GetAdScript");
				objErr.SendMail();
			}
			
			return html;			
		}
		
		//for the post method for aspx
		public string GetData(string url) 
		{
			WebClient webClient = new WebClient(); 
			string strUrl = url; 
			byte[] reqHTML;
			reqHTML = webClient.DownloadData(strUrl); 
			UTF8Encoding objUTF8 = new UTF8Encoding(); 

			return objUTF8.GetString(reqHTML);
		}

	}
}