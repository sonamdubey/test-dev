using System;
using System.Text;

namespace Bikewale.Notifications.MailTemplates.UsedBikes
{
    /// <summary>
    /// Created by  :  Aditi Srivastava on 20 Oct 2016
    /// Description :  Used Bike Rejected Listing Individual Seller Email Template
    /// Modified by : Aditi Srivastava on 26 May 2017
    /// Summary     : Changed HTML of email
    /// </summary>
    public class ListingRejectionEmailToSeller : ComposeEmailBase
    {
        private string sellerName,
            profileNo,
            bikeName;
        /// <summary>
        /// Initialize the member variables values
        /// </summary>
        /// <param name="sellerEmail"></param>
        /// <param name="sellerName"></param>
        /// <param name="buyerName"></param>
        /// <param name="buyerEmail"></param>
        /// <param name="buyerContactNo"></param>
        /// <param name="profileNo"></param>
        /// <param name="bikeName"></param>
        /// <param name="bikePrice"></param>
        public ListingRejectionEmailToSeller(string sellerName, string profileNo, string bikeName)
        {
            this.sellerName = sellerName;
            this.profileNo = profileNo.ToUpper();
            this.bikeName = bikeName;
        }
        /// <summary>
        /// Created by  :   Aditi Srivastava on 20 Oct 2016
        /// Description :   Prepares the Email Body for when bike listing is discarded
        /// </summary>
        /// <returns></returns>
        public override string ComposeBody()
        {
            StringBuilder sb = new StringBuilder();
            //banner
            sb.AppendFormat("<div style='max-width:692px; margin: 0 auto; border: 1px solid #f5f5f5; font-family: Arial, Helvetica, sans-serif; font-size:14px; color:#787878; background:#ffffff;'><div style='color:#fff; max-width:100%; min-height:175px; background:url(\"http://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/dealer-booking-banner.jpg\") no-repeat; padding:20px 20px 0; '><div style='max-width:100%; min-height:40px; background:#2a2a2a;'> <div style='float:left; max-width:82px; margin-top:5px; margin-left:20px;'> <a href='https://www.bikewale.com'><img src=\"http://imgd1.aeplcdn.com/0x0/bw/static/design15/mailer-images/bw-white-logo.png\" alt='BikeWale' title='BikeWale' width='100%' border='0' /></a> </div> <div style='float:right; margin-right:20px; font-size:14px; line-height:40px;'> {0} </div> <div style='clear:both'></div> </div> <div style='text-align:center'> <div style='width:100%; height:115px; font-size:28px; text-align:center; display:table;'> <div style='display:table-cell; vertical-align: middle;'>Your listing has not been approved on BikeWale</div> </div> </div> </div>", DateTime.Now.ToString("dd MMM yyyy"));
            //main content
            sb.AppendFormat(" <div><div style='margin: 15px 20px 20px;'> <div style='color:#4d5057; font-weight:bold; margin-bottom:20px;'>Hi {0},</div> <div style='color:#82888b; line-height:1.5;'> Your <span style='color:#4d5057;'>{1}</span> bike listing on BikeWale (profile id : {2}) has not been approved by the BikeWale team. Following could be the reasons why the listings get rejected on BikeWale:<ol><li> Inaccurate price</li><li> Inappropriate photos</li><li> Invalid manufacturing year</li></ol> </div> </div>", sellerName, bikeName, profileNo);
            sb.AppendFormat("<div style='margin: 15px 20px 20px; color:#82888b; line-height:1.5;'>We would request you to reupload the listing with correct details.</div>");
            //footer
            sb.AppendFormat("<div style='margin: 10px 20px 0 20px; line-height:1.5;'> <div style='font-size:14px; color:#82888b; margin:15px 0 30px 0;'>Thank you for using BikeWale. In case you have any queries, feel free to write at <a href='mailto:contact@bikewale.com' style='color: #0288d1; text-decoration: none;'>contact@bikewale.com</a></div> <div style='margin-bottom:25px; color:#82888b;'>Thanks,<br />Team BikeWale<br />www.bikewale.com</div> </div>");
            sb.AppendFormat("<div style='max-width:100%; background: url(\"http://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/query-bg-banner.jpg\") no-repeat center bottom / cover #2e2e2e; color:#fff;'> <div style='padding:7px 15px 7px 20px;display:inline-block;vertical-align:middle;'> <div style='float:left; width:46px; font-weight:bold;'><img src=\"http://imgd1.aeplcdn.com/0x0/bw/static/design15/mailer-images/bw-app-red-icon.png\" border='0'/></div> <div style='font-size:16px;height:46px;text-align:left;display:table;margin-left:60px;'> <div style='display:table-cell; vertical-align:middle;'>India’s #1 Bike Research Destination</div> </div> </div> <div style='margin:15px 20px 15px 0;display:inline-block;float:right'> <div> <a href='https://play.google.com/store/apps/details?id=com.bikewale.app&utm_source=UsedbikeMailer&utm_medium=email&utm_campaign=UsedbikeMail' target='_blank' style='text-decoration:none; color:#fff; font-weight:bold; font-size:12px; width:70px; background-color:#ef3f30; padding:8px 10px; border-radius:2px; display:block;'>Get the App</a> </div> </div> <div style='clear:both;'></div> </div> <div style='margin:10px 0 4px 0; border-bottom:2px solid #c20000;'></div> </div> </div>");
            return sb.ToString();
        }
    }
}
