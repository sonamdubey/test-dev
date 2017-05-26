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
            //sb.AppendFormat("<p>Hi {0},</p><p>Your ad posting on BikeWale of {1} {2} will expire in next {3}. If you have already sold your bike, we request you to delete your Ad.</p><p>If not sold yet, please re-post your bike Ad with comprehensive bike details and better photos quality. Visit {4} to re-post your Ad.", _sellerName, _makeName, _modelName, _remainingTime, _repostUrl);

            //if (EnumSMSServiceType.BikeListingExpiryOneDaySMSToSeller.Equals(_remainingDays))
            //    sb.AppendFormat("The Ad will not be visible to active buyers after 24 hours if not re-posted.</p>");
            //else
            //    sb.AppendFormat("</p>");

            //sb.AppendFormat("<p>Cheers!<br>Team BikeWale</p>");



            sb.AppendFormat("<div style='max-width:692px; margin:0 auto; border:1px solid #f5f5f5; font-family: Arial, Helvetica, sans-serif; font-size:14px; color:#787878; background:#ffffff;'><div style='color:#fff; max-width:100%; min-height:175px; background:url(\"http://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/dealer-booking-banner.jpg') no-repeat; padding:20px 20px 0; \"> <div style='max-width:100%; min-height:40px; background:#2a2a2a;'> <div style='float:left; max-width:82px; margin-top:5px; margin-left:20px;'> <a href='' target='_blank'><img src=\"images/bw-logo.png\" alt='BikeWale' title='BikeWale' width='100%' border='0' /></a> </div> <div style='float:right; margin-right:20px; font-size:14px; line-height:40px;'> {0} </div> <div style='clear:both'></div> </div> <div style='text-align:center'> <div style='width:100%; height:115px; font-size:28px; text-align:center; display:table;'> <div style='display:table-cell; vertical-align: middle;'>Your {1} {2} listing has not been approved on BikeWale</div> </div> </div> </div>", DateTime.Now.ToString("dd MMM yyyy"), _makeName,_modelName);
            //banner ends

            sb.AppendFormat("<div> <div style='margin:15px 20px 20px;'> <div style='color:#4d5057; font-weight:bold; margin-bottom:20px;'>Hi {0},</div> <div style='color:#82888b; line-height:1.5;'> Your ad posting on BikeWale of <span style='color:#4d5057;'>{1} {2} </span> bike (profile id: {2}) will expire in next {3} on BikeWale. If not sold yet, please re-post your bike Ad with comprehensive bike details and better photos quality. Visit {4} to re-post your Ad. </div> </div>", _sellerName, _makeName, _modelName, _remainingTime, _repostUrl);
            if (EnumSMSServiceType.BikeListingExpiryOneDaySMSToSeller.Equals(_remainingDays))
                sb.AppendFormat("<div style='color:#4d5057; font-weight:bold; margin-bottom:20px;'>The Ad will not be visible to active buyers after 24 hours if not re-posted.</div>");
            //common footer
            sb.AppendFormat("<div style='margin:10px 20px 0 20px; line-height:1.5;'> <div style='font-size:14px; color:#82888b; margin:15px 0 30px 0;'>Thank you for using BikeWale and good luck with your listing!<br />In case you have any queries, feel free to write at <a href='mailto:contact@bikewale.com' style='color: #0288d1; text-decoration: none;'>contact@bikewale.com</a></div> <div style='margin-bottom:25px; color:#82888b;'>Thanks,<br />Team BikeWale<br />www.bikewale.com</div> </div>");
            sb.AppendFormat("<div style='max-width:100%; background:url(\"http://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/query-bg-banner.jpg\") no-repeat center bottom / cover #2e2e2e; color:#fff;'> <div style='padding:7px 15px 7px 20px;display:inline-block;vertical-align:middle;'> <div style='float:left; width:46px; font-weight:bold;'><img src='images/bw-icon.png' border='0'/></div> <div style='font-size:16px;height:46px;text-align:left;display:table;margin-left:60px;'> <div style='display:table-cell; vertical-align:middle;'>India’s #1 Bike Research Destination</div> </div> </div> <div style='margin:15px 20px 15px 0;display:inline-block;float:right'> <div> <a href='#' target='_blank' style='text-decoration:none; color:#fff; font-weight:bold; font-size:12px; width:70px; background-color:#ef3f30; padding:8px 10px; border-radius:2px; display:block;'>Get the App</a> </div> </div> <div style='clear:both;'></div> </div> <div style='margin:10px 0 4px 0; border-bottom:2px solid #c20000;'></div> </div></div>");


            return sb.ToString();
        }
    }
}

