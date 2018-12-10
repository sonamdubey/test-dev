/*
	This class will contain all the common function related to Sell Car process
*/
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;
using Carwale.UI.Common;

namespace Carwale.UI.Common
{
    public class ClassifiedMailContent
    {
        //used for writing the debug messages
        private HttpContext objTrace = HttpContext.Current;

        public static string PhotoRequestToIndividualSeller(string sellerName, string buyerName, string buyerContact, string carName, string listingUrl)
        {
            StringBuilder sb = new StringBuilder();

            if (sellerName != "" && buyerName != "" && buyerContact != "" && carName != "" && listingUrl != "")
            {
                sb.Append("Dear " + sellerName + ",");
                sb.Append("<p>" + buyerName + " (" + buyerContact + ") has requested you to upload photos of your " + carName + " on CarWale. You can upload photos of your car using the following link.</p>");
                sb.Append("<p>" + listingUrl + "</p>");
                sb.Append("<p>A study done by us shows that cars with photos sell 24% faster than without photos. It also helps buyers make a faster decision.</p>");
                sb.Append("<p>If you have any difficulty, please feel free to contact us.</p>");
                sb.Append("<p>Warm Regards,</p>");
                sb.Append("<p>Team CarWale</p>");
            }

                //Added by Aditi Dhaybar on 4/2/15 for requesting photos to the seller on m-site
            else if (sellerName != "" && buyerName == "" && buyerContact == "" && carName != "" && listingUrl != "")
            {
                sb.Append("Dear " + sellerName + ",");
                sb.Append("<p>" + "Someone has requested you to upload photos of your " + carName + " on CarWale. You can upload photos of your car using the following link.</p>");
                sb.Append("<p>" + listingUrl + "</p>");
                sb.Append("<p>A study done by us shows that cars with photos sell 24% faster than without photos. It also helps buyers make a faster decision.</p>");
                sb.Append("<p>If you have any difficulty, please feel free to contact us.</p>");
                sb.Append("<p>Warm Regards,</p>");
                sb.Append("<p>Team CarWale</p>");
            }

            return sb.ToString();
        }

        public static string PhotoRequestToDealerSeller(string sellerName, string buyerName, string buyerContact, string carName, string profileId)
        {
            StringBuilder sb = new StringBuilder();

            if (sellerName != "" && buyerName != "" && buyerContact != "" && carName != "")
            {
                sb.Append("Dear " + sellerName + ",");
                sb.Append("<p>" + buyerName + " (" + buyerContact + ") has requested you to upload photos of your " + carName + "(Profile ID #" + profileId + ") on CarWale. Please login to your dealer panel and upload the requested car photos.</p>");
                sb.Append("<p>A study done by us shows that cars with photos sell 24% faster than without photos. It also helps buyers make a faster decision.</p>");
                sb.Append("<p>If you have any difficulty, please feel free to contact us.</p>");
                sb.Append("<p>Warm Regards,</p>");
                sb.Append("<p>Team CarWale</p>");
            }
            //Added by Aditi Dhaybar on 4/2/15 for requesting photos to the seller on m-site
            else if (sellerName != "" && buyerName == "" && buyerContact == "" && carName != "")
            {
                sb.Append("Dear " + sellerName + ",");
                sb.Append("<p>" + "Someone has requested you to upload photos of your " + carName + "(Profile ID #" + profileId + ") on CarWale. Please login to your dealer panel and upload the requested car photos.</p>");
                sb.Append("<p>A study done by us shows that cars with photos sell 24% faster than without photos. It also helps buyers make a faster decision.</p>");
                sb.Append("<p>If you have any difficulty, please feel free to contact us.</p>");
                sb.Append("<p>Warm Regards,</p>");
                sb.Append("<p>Team CarWale</p>");
            }

            return sb.ToString();
        }
    }
}