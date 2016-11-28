
using System.Text;
namespace Bikewale.Notifications.MailTemplates.UsedBikes
{
    public class PhotoRequestToCustomerForThreeDays : ComposeEmailBase
    {
        private string customerName, make, model, profileId;
        public PhotoRequestToCustomerForThreeDays(string customerName, string make, string model, string profileId)
        {
            this.customerName = customerName;
            this.make = make;
            this.model = model;
            this.profileId = profileId;
        }
        public override string ComposeBody()
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(customerName) && !string.IsNullOrEmpty(make) && !string.IsNullOrEmpty(model) && !string.IsNullOrEmpty(profileId))
            {
                sb.AppendFormat(@"Dear {4},<p>Thanks for choosing BikeWale to sell your {0} {1} bike. You are missing on opportunities because you have not uploaded any photo yet.
                    We have seen that buyers are showing more interest on Ads with high-quality photos.</p>
                    <p>Our data suggests that Ads with photos get 50% more responses than Ads without photos. BikeWale recommends you to add high-quality photos for your bike to get more responses. </p>
                    To add photos click on the <a href='{2}/used/sell/?id={3}'>link</a>
                    <p>Cheers!</p><p>Team BikeWale</p>", make, model, Utility.BWConfiguration.Instance.BwHostUrl, profileId, customerName);
            }

            return sb.ToString();
        }
    }
}
