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
            this.profileNo = profileNo;
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
            sb.AppendFormat("<p>Dear {0},</p>", sellerName);
            sb.AppendFormat("<p>Congratulations!</p>");
            sb.AppendFormat("<p>Your bike - {0}(#{1}) priced at Rs. {2} has been listed for sale at BikeWale</p>", bikeName, profileNo, bikePrice);
           // sb.AppendFormat("<p>{0} has shown interest in your bike - {1}(#{2}) priced at Rs. {3} which is listed for sale at BikeWale</p>", buyerName, bikeName, profileNo, bikePrice);
           // sb.AppendFormat("<p>{0}'s contact details are mentioned below<br />", buyerName);
           // sb.AppendFormat("Name: {0}<br />Email: {1}<br />Contact Number: {2}<br />", buyerName, buyerEmail, buyerContactNo);
            sb.AppendFormat("<p>If you would like to make any changes in your listing, ");
            sb.AppendFormat("<a href='http://www.bikewale.com/MyBikewale/'>click here to update.</a></p>");
            sb.AppendFormat("<p>Please remove your bike if it has been sold already, so that ever growing number of ");
            sb.AppendFormat("prospective buyers do not cause inconvenience to you. ");
            sb.AppendFormat("<a href='http://www.bikewale.com/MyBikewale/'>Click here to remove your bike now.</a></p>");
            sb.AppendFormat("<p>We are committed to deliver value by bringing genuine buyers for your bike.</p>");
            sb.AppendFormat("<p>We gauge that with the sale of {0}, you would be interested buying a new bike. Want to ", bikeName);
            sb.AppendFormat("know about a new bike’s price? BikeWale’s <a href='http://www.bikewale.com/pricequote/'>Instant Price Quote</a> is a helpful tool which will ");
            sb.AppendFormat("enable you to evaluate your purchase options on the price front.</p>");
            sb.AppendFormat("<br><br>Warm Regards,<br>");
            sb.AppendFormat("Team BikeWale");
            return sb.ToString();
        }
    }
}


