
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
        private SellerDetailsListEntity objSellerDetailsListsEntity;
        private readonly Utility.UrlShortner url = new Utility.UrlShortner();
        private readonly ExpiringListingSellerDetailsRepository objSellerDetails = new ExpiringListingSellerDetailsRepository();
        private readonly SMSTypes newSms = new SMSTypes();

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

                objSellerDetailsListsEntity = objSellerDetails.getExpiringListings();

                Logs.WriteInfoLog("started function SendExpiringListingReminderSMSAndEmail()");
                SendExpiringListingReminderSMSAndEmail();
                Logs.WriteInfoLog("function SendExpiringListingReminderSMSAndEmail() ended");

            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception in NotifySellerAboutListingExpiry : " + ex.Message);
                SendMail.HandleException(ex, "NotifySellerAboutListingExpiry");
            }
        }

        /// <summary>
        /// Created By Sajal Gupta on 23-11-2016.
        /// Desc : Send sms and email to seller about expiry listing.
        /// </summary>
        private void SendExpiringListingReminderSMSAndEmail()
        {
            try
            {
                if (objSellerDetailsListsEntity != null)
                {
                    foreach (var seller in objSellerDetailsListsEntity.sellerDetailsOneDayRemaining)
                    {
                        SendSMS(seller, EnumSMSServiceType.BikeListingExpiryOneDaySMSToSeller);
                        SendEmail(seller, EnumSMSServiceType.BikeListingExpiryOneDaySMSToSeller);
                    }

                    foreach (var seller in objSellerDetailsListsEntity.sellerDetailsSevenDaysRemaining)
                    {
                        SendSMS(seller, EnumSMSServiceType.BikeListingExpirySevenDaySMSToSeller);
                        SendEmail(seller, EnumSMSServiceType.BikeListingExpirySevenDaySMSToSeller);
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception in SendExpiringListingReminderSMSAndEmail : " + ex.Message);
                SendMail.HandleException(ex, "SendExpiringListingReminderSMSAndEmail");
            }
        }

        /// <summary>
        /// Created By Sajal Gupta on 23-11-2016.
        /// Desc : Send sms.
        /// </summary>
        private void SendSMS(SellerDetailsEntity seller, EnumSMSServiceType dayRemaining)
        {
            try
            {
                string repostUrl = string.Format(_repostUrl, _hostUrl, seller.inquiryId);
                string removeUrl = string.Format(_removeUrl, _hostUrl, seller.inquiryId);
                string message;

                UrlShortnerResponse shortRepostUrl = url.GetShortUrl(repostUrl);
                UrlShortnerResponse shortRemoveUrl = url.GetShortUrl(removeUrl);

                if (shortRepostUrl != null)
                    repostUrl = shortRepostUrl.ShortUrl;

                if (shortRemoveUrl != null)
                    removeUrl = shortRemoveUrl.ShortUrl;

                if (dayRemaining == EnumSMSServiceType.BikeListingExpiryOneDaySMSToSeller)
                {
                    message = string.Format(_messageOneDay, seller.makeName, seller.modelName, removeUrl, repostUrl);
                    newSms.ExpiringListingReminderSMS(seller.sellerMobileNumber, "SendExpiringListingReminderSMS()", dayRemaining, message);
                    Logs.WriteInfoLog("One day remaining Message sent to inquiryId " + seller.inquiryId);
                }
                else
                {
                    message = string.Format(_messageSevenDay, seller.makeName, seller.modelName, removeUrl, repostUrl);
                    newSms.ExpiringListingReminderSMS(seller.sellerMobileNumber, "SendExpiringListingReminderSMS()", dayRemaining, message);
                    Logs.WriteInfoLog("Seven day remaining Message sent to inquiryId " + seller.inquiryId);
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception in SendSMS : " + ex.Message);
                SendMail.HandleException(ex, string.Format("SendSMS for para {0}, {1}", seller, dayRemaining));
            }

        }



        /// <summary>
        /// Created By Sajal Gupta on 23-11-2016.
        /// Desc : Send email.
        /// </summary>
        private void SendEmail(SellerDetailsEntity seller, EnumSMSServiceType dayRemaining)
        {
            try
            {
                string repostUrl = string.Format(_repostUrl, _hostUrl, seller.inquiryId);
                UrlShortnerResponse shortRepostUrl;
                shortRepostUrl = url.GetShortUrl(repostUrl);

                if (shortRepostUrl != null)
                    repostUrl = shortRepostUrl.ShortUrl;

                if (dayRemaining == EnumSMSServiceType.BikeListingExpiryOneDaySMSToSeller)
                {
                    ComposeEmailBase objEmail = new ExpiringListingReminderEmail(seller.sellerName, seller.makeName, seller.modelName, EnumSMSServiceType.BikeListingExpiryOneDaySMSToSeller, repostUrl);
                    objEmail.Send(seller.sellerEmail, _emailSubjectOneDay);
                    Logs.WriteInfoLog("One day remaining Email sent to inquiryId " + seller.inquiryId);
                }
                else
                {
                    ComposeEmailBase objEmail = new ExpiringListingReminderEmail(seller.sellerName, seller.makeName, seller.modelName, EnumSMSServiceType.BikeListingExpirySevenDaySMSToSeller, repostUrl);
                    objEmail.Send(seller.sellerEmail, _emailSubjectSevenDay);
                    Logs.WriteInfoLog("Seven day remaining Email sent to inquiryId " + seller.inquiryId);
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception in SendEmail : " + ex.Message);
                SendMail.HandleException(ex, string.Format("SendEmail for par {0}, {1}", seller, dayRemaining));
            }
        }

    }
}






