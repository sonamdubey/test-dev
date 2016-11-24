
using Bikewale.Entities.UrlShortner;
using Bikewale.Notifications;
using Bikewale.Notifications.MailTemplates.UsedBikes;
using Consumer;
using System;

namespace Bikewale.ExpiringListingReminder
{
    /// <summary>
    /// Created By Sajal Gupta on 23-11-2016.
    /// Desc : Send sms and email to seller about expiry listing.
    /// </summary>
    public class NotifySellerListingExpiry
    {
        private SellerDetailsListsEntity objSellerDetailsListsEntity;

        private readonly string _repostUrl = "{0}/used/inquiry/{1}/repost/";
        private readonly string _removeUrl = "{0}/used/inquiry/{1}/remove/";

        private readonly string _emailSubjectOneDay = "BikeWale listing expiry in 24 hours";
        private readonly string _emailSubjectSevenDay = "BikeWale listing expiry in 7 days";

        private readonly string _messageOneDay = "Your Ad on BikeWale will expire in next 24 hours. If you have already sold your {0} {1} bike, visit {2} to remove your ad. If not sold yet, visit {3} to re-post it. Team BikeWale";
        private readonly string _messageSevenDay = "Your Ad on BikeWale will expire in next 7 days. If you have already sold your {0} {1} bike, visit {2} to remove your ad. If not sold yet, visit {3} to re-post it. Team BikeWale";

        private readonly string _hostUrl = Bikewale.Utility.BWConfiguration.Instance.BwHostUrl;

        /// <summary>
        /// Created By : Sajal Gupta on 24-11-2016
        /// Desc : To call both sms and email sending function.
        /// </summary>
        public void NotifySellerAboutListingExpiry()
        {
            try
            {
                ExpiringListingSellerDetailsRepository objSellerDetails = new ExpiringListingSellerDetailsRepository();
                objSellerDetailsListsEntity = objSellerDetails.getExpiringListings();

                Logs.WriteInfoLog("started function SendExpiringListingReminderSMS()");
                SendExpiringListingReminderSMS();
                Logs.WriteInfoLog("function SendExpiringListingReminderSMS() ended");

                Logs.WriteInfoLog("started function SendExpiringListingReminderEmail()");
                SendExpiringListingReminderEmail();
                Logs.WriteInfoLog("function SendExpiringListingReminderEmail() ended");

            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception in NotifySellerAboutListingExpiry : " + ex.Message);
                SendMail.HandleException(ex, "NotifySellerAboutListingExpiry");
            }
        }

        /// <summary>
        /// Created By Sajal Gupta on 23-11-2016.
        /// Desc : Send sms to seller about expiry listing.
        /// </summary>
        public void SendExpiringListingReminderSMS()
        {
            try
            {
                Utility.UrlShortner url = new Utility.UrlShortner();

                foreach (var seller in objSellerDetailsListsEntity.sellerDetailsOneDayRemaining)
                {
                    string repostUrl = string.Format(_repostUrl, _hostUrl, seller.inquiryId);
                    string removeUrl = string.Format(_removeUrl, _hostUrl, seller.inquiryId);

                    UrlShortnerResponse shortRepostUrl = url.GetShortUrl(repostUrl);
                    UrlShortnerResponse shortRemoveUrl = url.GetShortUrl(removeUrl);

                    if (shortRepostUrl != null)
                        repostUrl = shortRepostUrl.ShortUrl;

                    if (shortRemoveUrl != null)
                        removeUrl = shortRemoveUrl.ShortUrl;

                    string message = string.Format(_messageOneDay, seller.makeName, seller.modelName, removeUrl, repostUrl);

                    Logs.WriteInfoLog("One day remaining Message sent to inquiryId " + seller.inquiryId);

                    SMSTypes newSms = new SMSTypes();
                    newSms.ExpiringListingReminderSMS(seller.sellerMobileNumber, "SendExpiringListingReminderSMS()", 1, message);
                }

                foreach (var seller in objSellerDetailsListsEntity.sellerDetailsSevenDaysRemaining)
                {

                    string repostUrl = string.Format(_repostUrl, _hostUrl, seller.inquiryId);
                    string removeUrl = string.Format(_removeUrl, _hostUrl, seller.inquiryId);

                    UrlShortnerResponse shortRepostUrl = url.GetShortUrl(repostUrl);
                    UrlShortnerResponse shortRemoveUrl = url.GetShortUrl(removeUrl);

                    if (shortRepostUrl != null)
                        repostUrl = shortRepostUrl.ShortUrl;

                    if (shortRemoveUrl != null)
                        removeUrl = shortRemoveUrl.ShortUrl;

                    string message = string.Format(_messageSevenDay, seller.makeName, seller.modelName, removeUrl, repostUrl);

                    Logs.WriteInfoLog("Seven day remaining Message sent to inquiryId " + seller.inquiryId);

                    SMSTypes newSms = new SMSTypes();
                    newSms.ExpiringListingReminderSMS(seller.sellerMobileNumber, "SendExpiringListingReminderSMS()", 7, message);
                }

            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception in SendExpiringListingReminderSMS : " + ex.Message);
                SendMail.HandleException(ex, "SendExpiringListingReminderSMS");
            }
        }


        /// <summary>
        /// Created By Sajal Gupta on 23-11-2016.
        /// Desc : Send email to seller about expiry listing.
        /// </summary>
        public void SendExpiringListingReminderEmail()
        {
            try
            {
                foreach (var seller in objSellerDetailsListsEntity.sellerDetailsOneDayRemaining)
                {
                    string repostUrl = string.Format(_repostUrl, _hostUrl, seller.inquiryId);

                    Logs.WriteInfoLog("One day remaining Email sent to inquiryId " + seller.inquiryId);

                    ComposeEmailBase objEmail = new ExpiringListingReminderEmail(seller.sellerName, seller.makeName, seller.modelName, 1, repostUrl);
                    objEmail.Send(seller.sellerEmail, _emailSubjectOneDay);
                }

                foreach (var seller in objSellerDetailsListsEntity.sellerDetailsSevenDaysRemaining)
                {
                    string repostUrl = string.Format(_repostUrl, _hostUrl, seller.inquiryId);

                    Logs.WriteInfoLog("Seven day remaining Email sent to inquiryId " + seller.inquiryId);

                    ComposeEmailBase objEmail = new ExpiringListingReminderEmail(seller.sellerName, seller.makeName, seller.modelName, 7, repostUrl);
                    objEmail.Send(seller.sellerEmail, _emailSubjectSevenDay);
                }

            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception in SendExpiringListingReminderEmail : " + ex.Message);
                SendMail.HandleException(ex, "SendExpiringListingReminderEmail");
            }
        }

    }
}






