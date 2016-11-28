
using System.Text;
namespace Bikewale.Notifications.MailTemplates.UsedBikes
{
    /// <summary>
    /// Created By:-Subodh Jain 24 Nov 2016
    /// summary:- For photo upload Email in Three Days
    /// </summary>
    public class PhotoRequestToCustomerForThreeDays : ComposeEmailBase
    {
        private string _customerName, _make, _model, _profileId;
        public PhotoRequestToCustomerForThreeDays(string customerName, string make, string model, string profileId)
        {
            _customerName = customerName;
            _make = make;
            _model = model;
            _profileId = profileId;
        }
        /// <summary>
        /// Created By:-Subodh Jain 24 Nov 2016
        /// summary:- For photo upload Email in Three Days
        /// </summary>
        public override string ComposeBody()
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(_customerName) && !string.IsNullOrEmpty(_make) && !string.IsNullOrEmpty(_model) && !string.IsNullOrEmpty(_profileId))
            {
                sb.AppendFormat(@"Dear {4},<p>Thanks for choosing BikeWale to sell your {0} {1} bike. You are missing on opportunities because you have not uploaded any photo yet.
                    We have seen that buyers are showing more interest on Ads with high-quality photos.</p>
                    <p>Our data suggests that Ads with photos get 50% more responses than Ads without photos. BikeWale recommends you to add high-quality photos for your bike to get more responses. </p>
                    To add photos click on the <a href='{2}/used/sell/?id={3}'>link</a>
                    <p>Cheers!</p><p>Team BikeWale</p>", _make, _model, Utility.BWConfiguration.Instance.BwHostUrl, _profileId, _customerName);
            }

            return sb.ToString();
        }
    }
}
