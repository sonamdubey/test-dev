using System.Text;

namespace Bikewale.Notifications.MailTemplates.UsedBikes
{
    /// <summary>
    /// Created By Sajal Gupta on 23-11-2016
    /// Description : To send email to seller for expiring time of listing.
    /// </summary>
    public class ExpiringListingReminderEmail : ComposeEmailBase
    {
        private string _sellerName, _makeName, _modelName, _repostUrl, _remainingTime;
        private EnumSMSServiceType _remainingDays;

        public ExpiringListingReminderEmail(string sellerName, string makeName, string modelName, EnumSMSServiceType remainingDays, string repostUrl)
        {
            _sellerName = sellerName;
            _makeName = makeName;
            _modelName = modelName;
            _remainingDays = remainingDays;
            _repostUrl = repostUrl;

            if (remainingDays == EnumSMSServiceType.BikeListingExpiryOneDaySMSToSeller)
                _remainingTime = "24 hours";
            else
                _remainingTime = "7 days";
        }

        /// <summary>
        /// Created By Sajal Gupta on 23-11-2016
        /// Desc : Prepares the Email Body for Expiring Listing Reminder.
        /// </summary>
        /// <returns></returns>
        public override string ComposeBody()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<p>Hi {0},</p><p>Your ad posting on BikeWale of {1} {2} will expire in next {3}. If you have already sold your bike, we request you to delete your Ad.</p><p>If not sold yet, please re-post your bike Ad with comprehensive bike details and better photos quality. Visit {4} to re-post your Ad.", _sellerName, _makeName, _modelName, _remainingTime, _repostUrl);

            if (EnumSMSServiceType.BikeListingExpiryOneDaySMSToSeller.Equals(_remainingDays))
                sb.AppendFormat("The Ad will not be visible to active buyers after 24 hours if not re-posted.</p>");
            else
                sb.AppendFormat("</p>");

            sb.AppendFormat("<p>Cheers!<br>Team BikeWale</p>");
            return sb.ToString();
        }
    }
}

