using System;
using System.Text;

namespace Bikewale.Notifications.MailTemplates.UsedBikes
{
    /// <summary>
    /// Created by  :  Aditi Srivastava on 14 Oct 2016
    /// Description :  Used Bike Listing Individual Seller Email Template
    /// </summary>
    public class ListingEmailtoIndividualTemplate : ComposeEmailBase
    {
        private string sellerEmail,
            sellerName,
            profileNo,
            bikeName,
            bikePrice, modelImageUrl, kms, reviewLink;
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
        public ListingEmailtoIndividualTemplate(string sellerEmail, string sellerName, string profileNo, string bikeName,
            string bikePrice, string modelImageUrl, string kms, string reviewLink)
        {
            this.sellerEmail = sellerEmail;
            this.sellerName = sellerName;
            this.profileNo = profileNo.ToUpper();
            this.bikeName = bikeName;
            this.bikePrice = bikePrice;
            this.modelImageUrl = modelImageUrl;
            this.kms = kms;
            this.reviewLink = reviewLink;
        }
        /// <summary>
        /// Created by  :   Aditi Srivastava on 14 Oct 2016
        /// Description :   Prepares the Email Body for successful listing
        /// </summary>
        /// <returns></returns>
        public override string ComposeBody()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(string.Format("<div style='max-width:692px; margin:0 auto; border:1px solid #f5f5f5; font-family: Arial, Helvetica, sans-serif; font-size:14px; color:#787878; background:#ffffff;'><div style='color:#fff; max-width:100%; min-height:175px; background:url(\"http://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/dealer-booking-banner.jpg\") no-repeat; padding:20px 20px 0; '> <!-- banner starts here --> <div style='max-width:100%; min-height:40px; background:#2a2a2a;'> <div style='float:left; max-width:82px; margin-top:5px; margin-left:20px;'> <a href='' target='_blank'><img src='http://imgd1.aeplcdn.com/0x0/bw/static/design15/mailer-images/bw-white-logo.png' alt='BikeWale' title='BikeWale' width='100%' border='0' /></a> </div> <div style='float:right; margin-right:20px; font-size:14px; line-height:40px;'> {0} </div> <div style='clear:both'></div> </div> <div style='text-align:center'> <div style='width:100%; height:115px; font-size:28px; text-align:center; display:table;'> <div style='display:table-cell; vertical-align: middle;'>Your listing has been updated on BikeWale!</div> </div> </div> </div> <!-- banner ends here --> <div> <!-- main content starts here --><!-- Dynamic content here starts --><div style='margin:15px 20px 20px;color:#82888b; line-height:1.5;'>", DateTime.Now.ToString("dd MMMM yyyy")));
            sb.AppendFormat("<div style='color:#4d5057; font-weight:bold; margin-bottom:20px;'>Hi {0}</div>", sellerName);
            sb.AppendFormat("<p>Thank you for listing your <span style='color:#4d5057;'>{0}</span> bike (profile id: {1}) on BikeWale.", bikeName, profileNo);
            sb.AppendFormat("We will verify the listing to check for any discrepancies.<br> Once the bike listing ");
            sb.AppendFormat("is verified and approved by the BikeWale team, it will be available for potential buyers to contact you.</p>");
            sb.AppendFormat("<p>You can view, edit, delete your listing and check replies to all your listings by logging into <a href='https://www.bikewale.com/mybikewale/'>My BikeWale</a> account.</p>");
            sb.AppendFormat(string.Format("</div><!-- Dynamic content here ends --><!-- common used bike slug starts --><div style='margin:0 20px 15px; border-bottom:1px solid #f5f5f5; text-align: center;'><div style='max-width: 480px; display: inline-block; vertical-align: middle; text-align: left; margin-bottom: 10px;'><div style='width: 80px; float: left; margin-right: 10px;'><img src='{3}' alt='{0}' title='{0}' border='0' width='80' /></div><div style='width: auto; padding-top: 5px; padding-right: 20px;'><div style='color: #4d5057; font-weight: bold; margin-bottom: 5px;'>Ridden your bike for {1} kms. Help others in making the buying decision.</div></div><div style='clear: both;'></div></div><div style='display: inline-block; vertical-align: middle; text-align: left; margin-bottom: 20px;'><a href='{2}' target='_blank' style='text-decoration:none; color:#fff; font-weight:bold; font-size:12px; width:158px; display: inline-block; background-color:#41B4C4; border: 1px solid #41B4C4; padding:8px 0; border-radius:2px; text-align: center;'>Write a review</a></div></div> <div style='margin:10px 20px 0 20px; line-height:1.5;'> <div style='font-size:14px; color:#82888b; margin:15px 0 30px 0;'>Thank you for using BikeWale and good luck with your listing!<br />In case you have any queries, feel free to write at <a href='mailto:contact@bikewale.com' style='color: #0288d1; text-decoration: none;'>contact@bikewale.com</a></div> <div style='margin-bottom:25px; color:#82888b;'>Thanks,<br />Team BikeWale<br />www.bikewale.com</div> </div> <div style='max-width:100%; background:url(\"http://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/query-bg-banner.jpg\") no-repeat center bottom / cover #2e2e2e; color:#fff;'> <div style='padding:7px 15px 7px 20px;display:inline-block;vertical-align:middle;'> <div style='float:left; width:46px; font-weight:bold;'><img src='http://imgd1.aeplcdn.com/0x0/bw/static/design15/mailer-images/bw-app-red-icon.png' border='0'/></div> <div style='font-size:16px;height:46px;text-align:left;display:table;margin-left:60px;'> <div style='display:table-cell; vertical-align:middle;'>Indiaӳ #1 Bike Research Destination</div> </div> </div> <div style='margin:15px 20px 15px 0;display:inline-block;float:right'> <div> <a href='https://play.google.com/store/apps/details?id=com.bikewale.app&utm_source=UsedbikeMailer&utm_medium=email&utm_campaign=UsedbikeMail' target='_blank' style='text-decoration:none; color:#fff; font-weight:bold; font-size:12px; width:70px; background-color:#ef3f30; padding:8px 10px; border-radius:2px; display:block;'>Get the App</a> </div> </div> <div style='clear:both;'></div> </div> <div style='margin:10px 0 4px 0; border-bottom:2px solid #c20000;'></div> <!-- common used bike slug ends --> </div> <!-- main content ends here --></div>", bikeName, kms, reviewLink, modelImageUrl));
            return sb.ToString();
        }
    }
}


