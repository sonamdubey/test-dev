using System.Text;

namespace Bikewale.Notifications.MailTemplates.UsedBikes
{
    /// <summary>
    /// Created by  :  Aditi Srivastava on 14 Oct 2016
    /// Description :  Used Bike Listing Individual Seller Email Template
    /// </summary>
    public class ListingEmailtoIndividualTemplate : ComposeEmailBase
    {
        private string sellerEmail,
            sellerName,
            profileNo,
            bikeName,
            bikePrice;
        /// <summary>
        /// Initialize the member variables values
        /// </summary>
        /// <param name="sellerEmail"></param>
        /// <param name="sellerName"></param>
        /// <param name="buyerName"></param>
        /// <param name="buyerEmail"></param>
        /// <param name="buyerContactNo"></param>
        /// <param name="profileNo"></param>
        /// <param name="bikeName"></param>
        /// <param name="bikePrice"></param>
        public ListingEmailtoIndividualTemplate(string sellerEmail, string sellerName, string profileNo, string bikeName, string bikePrice)
        {
            this.sellerEmail = sellerEmail;
            this.sellerName = sellerName;
            this.profileNo = profileNo.ToUpper();
            this.bikeName = bikeName;
            this.bikePrice = bikePrice;
        }
        /// <summary>
        /// Created by  :   Aditi Srivastava on 14 Oct 2016
        /// Description :   Prepares the Email Body for successful listing
        /// </summary>
        /// <returns></returns>
        public override string ComposeBody()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<p>Hi {0}</p>", sellerName);
            sb.AppendFormat("<p>Thank you for listing your {0} bike (profile id: {1}) on BikeWale.", bikeName, profileNo);
            sb.AppendFormat("We will verify the listing to check for any discrepancies.<br> Once the bike listing ");
            sb.AppendFormat("is verified and approved by the BikeWale team, it will be available for potential buyers to contact you.</p>");
            sb.AppendFormat("<p>You can view, edit, delete your listing and check replies to all your listings by logging into <a href='http://www.bikewale.com/mybikewale/'>My BikeWale</a> account.</p>");
            sb.AppendFormat("<p>Thank you for using BikeWale and good luck with your listing!</p>");
            sb.AppendFormat("<p>In case you have any queries, feel free to write at <a href='mailto:contact@bikewale.com'>contact@bikewale.com</a></p>");
            sb.AppendFormat("<p>Thanks<br>");
            sb.AppendFormat("Team BikeWale<br>");
            sb.AppendFormat("<a href='http://www.bikewale.com/'>www.bikewale.com</a><br></p>");
            return sb.ToString();
        }
    }
}


