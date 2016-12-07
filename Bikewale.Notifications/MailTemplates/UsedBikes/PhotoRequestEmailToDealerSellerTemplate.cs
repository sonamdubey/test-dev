
using System.Text;
namespace Bikewale.Notifications.MailTemplates.UsedBikes
{
    /// <summary>
    /// Created by  :   Sumit Kate on 01 Sep 2016
    /// Description :   Photo Request Email To Dealer Seller Template
    /// </summary>
    public class PhotoRequestEmailToDealerSellerTemplate : ComposeEmailBase
    {
        private string sellerName, buyerName, buyerContact, bikeName, profileId;
        public PhotoRequestEmailToDealerSellerTemplate(string sellerName, string buyerName, string buyerContact, string bikeName, string profileId)
        {
            this.sellerName = sellerName;
            this.buyerName = buyerName;
            this.buyerContact = buyerContact;
            this.bikeName = bikeName;
            this.profileId = profileId;
        }
        public override string ComposeBody()
        {
            StringBuilder sb = new StringBuilder();

            if (sellerName != "" && buyerName != "" && buyerContact != "" && bikeName != "")
            {
                sb.AppendFormat("Dear {0},", sellerName);
                sb.AppendFormat("<p>{0} ({1}) has requested you to upload photos of your {2}(Profile ID #{3}) on BikeWale. Please login to your dealer panel and upload the requested bike photos.</p>", buyerName, buyerContact, bikeName, profileId);
                sb.Append("<p>A study done by us shows that bikes with photos sell 50% faster than without photos. It also helps buyers make a faster decision.</p>");
                sb.Append("<p>If you have any difficulty, please feel free to contact us.</p>");
                sb.Append("<p>Warm Regards,</p>");
                sb.Append("<p>Team BikeWale</p>");
            }

            return sb.ToString();
        }
    }
}
