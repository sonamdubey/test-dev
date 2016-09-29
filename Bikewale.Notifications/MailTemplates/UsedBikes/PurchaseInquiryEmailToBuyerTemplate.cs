using System.Text;

namespace Bikewale.Notifications.MailTemplates.UsedBikes
{
    /// <summary>
    /// Created by  :   Sumit Kate on 23 Sep 2016
    /// Description :   Purchase Inquiry Buyer Email Template
    /// Modified by :   Sumit Kate on 29 Sep 2016
    /// Description :   Added Used bike listing page url
    /// </summary>
    public class PurchaseInquiryEmailToBuyerTemplate : ComposeEmailBase
    {
        private string sellerEmail, sellerName, sellerContactNo, sellerAddress, profileNo, buyerId, bikeName, kilometers, bikeYear, bikePrice, buyerName, listingUrl;
        /// <summary>
        /// Initialize the member variables values
        /// </summary>
        /// <param name="sellerEmail"></param>
        /// <param name="sellerName"></param>
        /// <param name="sellerContactNo"></param>
        /// <param name="sellerAddress"></param>
        /// <param name="profileNo"></param>
        /// <param name="buyerId"></param>
        /// <param name="bikeName"></param>
        /// <param name="kilometers"></param>
        /// <param name="bikeYear"></param>
        /// <param name="bikePrice"></param>
        /// <param name="buyerName"></param>
        public PurchaseInquiryEmailToBuyerTemplate(string sellerEmail, string sellerName,
                                                    string sellerContactNo, string sellerAddress,
                                                    string profileNo, string buyerId,
                                                    string bikeName, string kilometers,
                                                    string bikeYear, string bikePrice, string buyerName, string listingUrl)
        {
            this.sellerEmail = sellerEmail;
            this.sellerName = sellerName;
            this.sellerContactNo = sellerContactNo;
            this.sellerAddress = sellerAddress;
            this.profileNo = profileNo;
            this.buyerId = buyerId;
            this.bikeName = bikeName;
            this.kilometers = kilometers;
            this.bikeYear = bikeYear;
            this.bikePrice = bikePrice;
            this.buyerName = buyerName;
            this.listingUrl = listingUrl;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 23 Sep 2016
        /// Description :   Prepares the Email Body
        /// </summary>
        /// <returns></returns>
        public override string ComposeBody()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Dear {0},", buyerName);
            sb.AppendFormat("<p>You short listed a bike to buy. Congratulations!</p>");
            sb.AppendFormat("<p>The bike you have shown interest in is {0} (#{1}), done {2} KM listed for Rs. {3}/-. </p>", bikeName, profileNo, kilometers, bikePrice);
            sb.AppendFormat("<p><a href=\"{0}\">Please click here to view complete details of the bike.</a></p>", listingUrl);
            sb.AppendFormat("<p>You may contact the seller directly, details as below:<br>");
            sb.AppendFormat("Name: {0}<br />Phone(s): {1}<br />Address: {2}<br />", sellerName, sellerContactNo, sellerAddress);
            sb.AppendFormat("<p>Feel free to contact for any other assistance.</p>");
            sb.AppendFormat("<br>Warm Regards,<br><br>");
            sb.AppendFormat("Team bikewale");
            return sb.ToString();
        }
    }
}
