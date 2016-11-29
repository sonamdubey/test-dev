
using System.Text;
namespace Bikewale.Notifications.MailTemplates.UsedBikes
{
    /// <summary>
    /// Created By:-Subodh Jain 24 Nov 2016
    /// summary:- For photo upload Email in Seven Days
    /// </summary>
    public class PhotoRequestToCustomerForSevenDays : ComposeEmailBase
    {
        private string _customerName, _make, _model, _profileId;
        public PhotoRequestToCustomerForSevenDays(string customerName, string make, string model, string profileId)
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
                sb.AppendFormat(@"Dear {4},<p>Thanks for choosing BikeWale to sell your <Make Model> bike.You are missing on opportunities because you have not uploaded any photo yet.</p>
                      <p>Were you expecting more responses than what you have received? Our data suggests that Ads without photos get far lesser responses than Ads with high-quality photos.
                       BikeWale recommends you to add high-quality photos for your bike to get more responses.</p>
                       To add photos click on the <a href='{2}/used/sell/?id={3}#uploadphoto'>link</a><p>Cheers!</p><p>Team BikeWale</p>", _make, _model, Utility.BWConfiguration.Instance.BwHostUrl, _profileId, _customerName);
            }

            return sb.ToString();
        }

    }
}
