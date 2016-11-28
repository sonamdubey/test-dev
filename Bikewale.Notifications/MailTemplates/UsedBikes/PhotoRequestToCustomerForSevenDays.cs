
using System.Text;
namespace Bikewale.Notifications.MailTemplates.UsedBikes
{
    public class PhotoRequestToCustomerForSevenDays : ComposeEmailBase
    {
        private string customerName, make, model, profileId;
        public PhotoRequestToCustomerForSevenDays(string customerName, string make, string model, string profileId)
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
                sb.AppendFormat(@"Dear {4},<p>Thanks for choosing BikeWale to sell your <Make Model> bike.You are missing on opportunities because you have not uploaded any photo yet.</p>
                      <p>Were you expecting more responses than what you have received? Our data suggests that Ads without photos get far lesser responses than Ads with high-quality photos.
                       BikeWale recommends you to add high-quality photos for your bike to get more responses.</p>
                       To add photos click on the <a href='{2}/used/sell/?id={3}'>link</a><p>Cheers!</p><p>Team BikeWale</p>", make, model, Utility.BWConfiguration.Instance.BwHostUrl, profileId, customerName);
            }

            return sb.ToString();
        }

    }
}
