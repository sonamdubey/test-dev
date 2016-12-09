using System.Text;

namespace Bikewale.Notifications.MailTemplates.UsedBikes
{
    /// <summary>
    /// Created by  :  Aditi Srivastava on 14 Oct 2016
    /// Description :  Used Bike Approved Listing Individual Seller Email Template
    /// </summary>
    public class ListingApprovalEmailToSeller : ComposeEmailBase
    {
        private string sellerName,
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
        public ListingApprovalEmailToSeller(string sellerName, string profileNo, string bikeName)
        {
            this.sellerName = sellerName;
            this.profileNo = profileNo.ToUpper();
            this.bikeName = bikeName;
           }
        /// <summary>
        /// Created by  :   Aditi Srivastava on 18 Oct 2016
        /// Description :   Prepares the Email Body when listed bike is approved
        /// </summary>
        /// <returns></returns>
        public override string ComposeBody()
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<p>Hi {0},</p>", sellerName);
            sb.AppendFormat("<p>Congratulations! Your {0} bike listing (profile id: {1}) has been approved on BikeWale. ", bikeName, profileNo);
            sb.AppendFormat("Buyers will reach out to you on your email / phone.<br>");
            sb.AppendFormat("You can view, edit, delete your listing and check replies to all your listings by logging into <a href='https://www.bikewale.com/mybikewale/'>My BikeWale</a> account.</p>");
            sb.AppendFormat("<p>In case you have any queries, feel free to write at <a href='mailto:contact@bikewale.com'>contact@bikewale.com</a></p>");
            sb.AppendFormat("<p>Thanks<br>");
            sb.AppendFormat("Team BikeWale<br>");
            sb.AppendFormat("<a href='https://www.bikewale.com/'>www.bikewale.com</a><br></p>");
            return sb.ToString();
        }
    }
}
