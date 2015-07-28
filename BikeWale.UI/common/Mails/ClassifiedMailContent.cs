/*
	This class will contain all the common function related to Sell Bike process
*/
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;
using Bikewale.Common;

namespace Bikewale.Mails
{
	public class ClassifiedMailContent
	{			
		//used for writing the debug messages
		private HttpContext objTrace = HttpContext.Current;
				
		public static string PhotoRequestToIndividualSeller(string sellerName, string buyerName, string buyerContact, string bikeName, string listingUrl)
		{
			StringBuilder sb = new StringBuilder();
			
			if( sellerName != "" && buyerName != "" && buyerContact != "" && bikeName != "" && listingUrl != "" )
			{
				sb.Append("Dear "+ sellerName +",");
				sb.Append("<p>"+ buyerName +" ("+ buyerContact +") has requested you to upload photos of your "+ bikeName +" on BikeWale. You can upload photos of your bike using the following link.</p>");
				sb.Append("<p>"+ listingUrl +"</p>");				
				sb.Append("<p>A study done by us shows that bikes with photos sell 24% faster than without photos. It also helps buyers make a faster decision.</p>");
				sb.Append("<p>If you have any difficulty, please feel free to contact us.</p>");
				sb.Append("<p>Warm Regards,</p>");
				sb.Append("<p>Team BikeWale</p>");
			}
			
			return sb.ToString();
		}
		
		public static string PhotoRequestToDealerSeller(string sellerName, string buyerName, string buyerContact, string bikeName, string profileId)
		{
			StringBuilder sb = new StringBuilder();
			
			if( sellerName != "" && buyerName != "" && buyerContact != "" && bikeName != "")
			{
				sb.Append("Dear "+ sellerName +",");
				sb.Append("<p>"+ buyerName +" ("+ buyerContact +") has requested you to upload photos of your "+ bikeName +"(Profile ID #"+ profileId +") on BikeWale. Please login to your dealer panel and upload the requested bike photos.</p>");							
				sb.Append("<p>A study done by us shows that bikes with photos sell 24% faster than without photos. It also helps buyers make a faster decision.</p>");
				sb.Append("<p>If you have any difficulty, please feel free to contact us.</p>");
				sb.Append("<p>Warm Regards,</p>");
				sb.Append("<p>Team BikeWale</p>");
			}
			
			return sb.ToString();
		}		
	}
}