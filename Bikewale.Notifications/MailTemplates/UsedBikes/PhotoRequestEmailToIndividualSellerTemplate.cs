using System.Text;

namespace Bikewale.Notifications.MailTemplates.UsedBikes
{
    /// <summary>
    /// Created by  :   Sumit Kate on 01 Sep 2016
    /// Description :   Photo Request Email To Individual Seller Template
    /// </summary>
    public class PhotoRequestEmailToIndividualSellerTemplate : ComposeEmailBase
    {
        private string sellerName, buyerName, buyerContact, bikeName, listingUrl;
        public PhotoRequestEmailToIndividualSellerTemplate(string sellerName, string buyerName, string buyerContact, string bikeName, string listingUrl)
        {
            this.sellerName = sellerName;
            this.buyerName = buyerName;
            this.buyerContact = buyerContact;
            this.bikeName = bikeName;
            this.listingUrl = listingUrl;
        }
        public override string ComposeBody()
        {
            StringBuilder sb = new StringBuilder();

            if (sellerName != "" && buyerName != "" && buyerContact != "" && bikeName != "" && listingUrl != "")
            {
                sb.AppendFormat("Dear {0},", sellerName);
                sb.AppendFormat("<p>{0} ({1}) has requested you to upload photos of your {2} on BikeWale. You can upload photos of your bike using the following link.</p>", buyerName, buyerContact, bikeName);
                sb.AppendFormat("<p><a target='_blank' href='{0}'>{0}</a></p>", listingUrl);
                sb.Append("<p>A study done by us shows that bikes with photos sell 24% faster than without photos. It also helps buyers make a faster decision.</p>");
                sb.Append("<p>If you have any difficulty, please feel free to contact us.</p>");
                sb.Append("<p>Warm Regards,</p>");
                sb.Append("<p>Team BikeWale</p>");
            }

            return sb.ToString();
        }
    }
}
