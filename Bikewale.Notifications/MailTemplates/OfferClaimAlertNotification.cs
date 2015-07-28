using Bikewale.Entities.BikeBooking;
using System.Text;

namespace Bikewale.Notifications.MailTemplates
{
    /// <summary>
    /// Author      :   Sumit Kate
    /// Date        :   04/06/2015
    /// This is new email template. Email will be sent when a customer accepts an offer claim form.
    /// Currently the Helmet Offers data is hard-coded.
    /// </summary>
    public class OfferClaimAlertNotification : ComposeEmailBase
    {
        public string BikeName { get; set; }
        public string Helmet { get; set; }
        public RSAOfferClaimEntity OfferEntity { get; set; }
        public OfferClaimAlertNotification(RSAOfferClaimEntity offerEntity, string bikeName, string helmet)
        {
            this.OfferEntity = offerEntity;
            this.BikeName = bikeName;
            this.Helmet = helmet;
        }
        public override StringBuilder ComposeBody()
        {
            StringBuilder sb = new StringBuilder();
            string otherOffer = string.Empty;

            sb.Append("Dear,<br/>");
            sb.Append("An offer claim form has just been submitted against BikeWale booking! Details are following:<br/>");
            sb.Append("Offer(s) Claimed:<br/>");
            sb.AppendLine("<ol>");
            sb.AppendFormat("<li>{0}</li>", "Road Side Assistance<br/>");
            if (!string.IsNullOrWhiteSpace(Helmet))
                sb.AppendFormat("<li>{0}</li>", Helmet);
            sb.Append("</ol>");
            sb.Append("Customer and Booking Details:<br/>");
            sb.AppendFormat("Customer Name: {0}<br/>", OfferEntity.CustomerName);
            sb.AppendFormat("Customer Contact: {0}<br/>", OfferEntity.CustomerMobile);
            sb.AppendFormat("Customer Address:{0}<br/>", OfferEntity.CustomerAddress);
            sb.AppendFormat("Purchased Bike: {0}<br/>", BikeName);
            sb.AppendFormat("Registration Number: {0}<br/>", OfferEntity.BikeRegistrationNo);
            sb.AppendFormat("Registration Address: {0}<br/>", OfferEntity.DealerAddress);
            sb.AppendFormat("Delivery Date: {0}<br/>", OfferEntity.DeliveryDate.ToShortDateString());
            sb.AppendFormat("Any comments: {0}<br/>", OfferEntity.Comments);
            sb.Append("Dealership Details:<br/>");
            sb.AppendFormat("Dealer Name: {0}<br/>", OfferEntity.DealerName);
            sb.AppendFormat("Dealer Address: {0}<br/>", OfferEntity.DealerAddress);
            sb.Append("Best Regards,<br/>");
            sb.Append("Team BikeWale<br/>");
            return sb;
        }
    }
}
