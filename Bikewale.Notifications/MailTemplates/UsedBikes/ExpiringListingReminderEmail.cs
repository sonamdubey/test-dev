using System;
using System.Text;

namespace Bikewale.Notifications.MailTemplates.UsedBikes
{
    /// <summary>
    /// Created By Sajal Gupta on 23-11-2016
    /// Description : To send email to seller for expiring time of listing.
    /// Modified by : Aditi Srivastava on 26 May 2017
    /// Summary     : Changed HTML of email
    /// </summary>
    public class ExpiringListingReminderEmail : ComposeEmailBase
    {
        private string _sellerName, _makeName, _modelName, _repostUrl, _remainingTime, _emailSubject;
        private EnumSMSServiceType _remainingDays;

        public ExpiringListingReminderEmail(string sellerName, string makeName, string modelName, EnumSMSServiceType remainingDays, string repostUrl, string emailSubject)
        {
            _sellerName = sellerName;
            _makeName = makeName;
            _modelName = modelName;
            _remainingDays = remainingDays;
            _repostUrl = repostUrl;
            _emailSubject = emailSubject;
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
            //banner
            sb.AppendFormat("<div style='max-width:692px; margin: 0 auto; border: 1px solid #f5f5f5; font-family: Arial, Helvetica, sans-serif; font-size:14px; color:#787878; background:#ffffff;'><div style='color:#fff; max-width:100%; min-height:175px; background:url(\"http://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/dealer-booking-banner.jpg\") no-repeat; padding:20px 20px 0; '><div style='max-width:100%; min-height:40px; background:#2a2a2a;'> <div style='float:left; max-width:82px; margin-top:5px; margin-left:20px;'> <a href='https://www.bikewale.com'><img src=\"http://imgd1.aeplcdn.com/0x0/bw/static/design15/mailer-images/bw-white-logo.png\" alt='BikeWale' title='BikeWale' width='100%' border='0' /></a> </div> <div style='float:right; margin-right:20px; font-size:14px; line-height:40px;'> {0} </div> <div style='clear:both'></div> </div> <div style='text-align:center'> <div style='width:100%; height:115px; font-size:28px; text-align:center; display:table;'> <div style='display:table-cell; vertical-align: middle;'>{1}</div> </div> </div> </div>", DateTime.Now.ToString("dd MMM yyyy"),_emailSubject);
            
            //main content
            sb.AppendFormat("<div> <div style='margin:15px 20px 20px;'> <div style='color:#4d5057; font-weight:bold; margin-bottom:20px;'>Hi {0},</div> <div style='color:#82888b; line-height:1.5;'> Your ad posting on BikeWale of <span style='color:#4d5057;'>{1} {2} </span> will expire in the next {3} on BikeWale. If not sold yet, please re-post your bike Ad with comprehensive bike details and better photos quality. Visit {4} to re-post your Ad. </div> </div>", _sellerName, _makeName, _modelName, _remainingTime, _repostUrl);
            if (EnumSMSServiceType.BikeListingExpiryOneDaySMSToSeller.Equals(_remainingDays))
                sb.AppendFormat("<div style='color:#82888b; margin:15px 20px 20px; line-height:1.5;'>The Ad will not be visible to active buyers after 24 hours if not re-posted.</div>");
            //common footer
            sb.AppendFormat("<div style='margin: 10px 20px 0 20px; line-height:1.5;'> <div style='font-size:14px; color:#82888b; margin:15px 0 30px 0;'>Thank you for using BikeWale. In case you have any queries, feel free to write at <a href='mailto:contact@bikewale.com' style='color: #0288d1; text-decoration: none;'>contact@bikewale.com</a></div> <div style='margin-bottom:25px; color:#82888b;'>Thanks,<br />Team BikeWale<br />www.bikewale.com</div> </div>");
            sb.AppendFormat("<div style='max-width:100%; background: url(\"http://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/query-bg-banner.jpg\") no-repeat center bottom / cover #2e2e2e; color:#fff;'> <div style='padding:7px 15px 7px 20px;display:inline-block;vertical-align:middle;'> <div style='float:left; width:46px; font-weight:bold;'><img src=\"http://imgd1.aeplcdn.com/0x0/bw/static/design15/mailer-images/bw-app-red-icon.png\" border='0'/></div> <div style='font-size:16px;height:46px;text-align:left;display:table;margin-left:60px;'> <div style='display:table-cell; vertical-align:middle;'>India’s #1 Bike Research Destination</div> </div> </div> <div style='margin:15px 20px 15px 0;display:inline-block;float:right'> <div> <a href='https://play.google.com/store/apps/details?id=com.bikewale.app'target='_blank' style='text-decoration:none; color:#fff; font-weight:bold; font-size:12px; width:70px; background-color:#ef3f30; padding:8px 10px; border-radius:2px; display:block;'>Get the App</a> </div> </div> <div style='clear:both;'></div> </div> <div style='margin:10px 0 4px 0; border-bottom:2px solid #c20000;'></div> </div> </div>");

            return sb.ToString();
        }
    }
}

