// C# Document
using System;
using System.Net;
using System.Text;

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
				string url = "https://www.carwale.com/ads/ICICILomb/form.aspx?"
						   + "nm=" + name
						   + "&em=" + eMail
						   + "&mb=" + mobile
						   + "&ct=" + city;
							
				html = GetData(url);
				*/
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "MailerAds.GetAdScript");
                objErr.SendMail();
            }

            return html;
        }

        //for the post method for aspx
        public string GetData(string url)
        {
            string data = string.Empty;
            if (!string.IsNullOrEmpty(url))
            {
                using (WebClient webClient = new WebClient())
                {
                    byte[] reqHTML = webClient.DownloadData(url);
                    UTF8Encoding objUTF8 = new UTF8Encoding();
                    data = objUTF8.GetString(reqHTML);
                }
            }

            return data;

        }

    }
}