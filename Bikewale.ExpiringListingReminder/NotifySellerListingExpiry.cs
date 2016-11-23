
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

        /// <summary>
        /// Created By Sajal Gupta on 23-11-2016.
        /// Desc : Send sms to seller about expiry listing.
        /// </summary>
        public static void SendExpiringListingReminderSMS()
        {
            try
            {
                ExpiringListingSellerDetailsRepository objSellerDetails = new ExpiringListingSellerDetailsRepository();
                SellerDetailsListsEntity objSellerDetailsListsEntity = new SellerDetailsListsEntity();

                objSellerDetailsListsEntity = objSellerDetails.getExpiringListings();

                foreach (var seller in objSellerDetailsListsEntity.sellerDetailsOneDayRemaining)
                {
                    string repostUrl = string.Format("{0}/used/inquiry/{1}/repost/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, seller.inquiryId);
                    string removeUrl = string.Format("{0}/used/inquiry/{1}/remove/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, seller.inquiryId);

                    SMSTypes newSms = new SMSTypes();
                    newSms.ExpiringListingReminderSMS(seller.sellerName, "", 1, repostUrl, removeUrl, seller.makeName, seller.modelName);
                }

                foreach (var seller in objSellerDetailsListsEntity.sellerDetailsSevenDaysRemaining)
                {
                    string repostUrl = string.Format("{0}/used/inquiry/{1}/repost/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, seller.inquiryId);
                    string removeUrl = string.Format("{0}/used/inquiry/{1}/remove/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, seller.inquiryId);

                    SMSTypes newSms = new SMSTypes();
                    newSms.ExpiringListingReminderSMS(seller.sellerName, "", 7, repostUrl, removeUrl, seller.makeName, seller.modelName);
                }

            }
            catch (Exception ex)
            {
                SendMail.HandleException(ex, "SendExpiringListingReminderSMS");
            }
        }


        /// <summary>
        /// Created By Sajal Gupta on 23-11-2016.
        /// Desc : Send email to seller about expiry listing.
        /// </summary>
        public static void SendExpiringListingReminderEmail()
        {
            try
            {
                ExpiringListingSellerDetailsRepository objSellerDetails = new ExpiringListingSellerDetailsRepository();
                SellerDetailsListsEntity objSellerDetailsListsEntity = new SellerDetailsListsEntity();

                objSellerDetailsListsEntity = objSellerDetails.getExpiringListings();

                foreach (var seller in objSellerDetailsListsEntity.sellerDetailsOneDayRemaining)
                {
                    string repostUrl = string.Format("{0}/used/inquiry/{1}/repost/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, seller.inquiryId);

                    ComposeEmailBase objEmail = new ExpiringListingReminderEmail(seller.sellerName, seller.makeName, seller.modelName, 1, repostUrl);
                    objEmail.Send(seller.sellerEmail, "BikeWale listing expiry in 24 hours");
                }

                foreach (var seller in objSellerDetailsListsEntity.sellerDetailsSevenDaysRemaining)
                {
                    string repostUrl = string.Format("{0}/used/inquiry/{1}/repost/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, seller.inquiryId);
                    string removeUrl = string.Format("{0}/used/inquiry/{1}/remove/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, seller.inquiryId);

                    ComposeEmailBase objEmail = new ExpiringListingReminderEmail(seller.sellerName, seller.makeName, seller.modelName, 7, repostUrl);
                    objEmail.Send(seller.sellerEmail, "BikeWale listing expiry in 7 days");
                }

            }
            catch (Exception ex)
            {
                SendMail.HandleException(ex, "SendExpiringListingReminderEmail");
            }
        }

    }
}






