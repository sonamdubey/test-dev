using Bikewale.Entities.BikeBooking;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Bikewale.Notifications.MailTemplates
{
    /// <summary>
    /// Created By : Lucky Rathore on 11 May 2016.
    /// Summary : Template Revamped.
    /// </summary>    
    public class PreBookingConfirmationToCustomer : ComposeEmailBase
    {
        private string MailHTML = string.Empty;

        /// <summary>
        /// Created By : Lucky Rathore on 11 May 2016.
        /// Summary : Constructor to update MailHTML with html of mail.
        /// </summary>  
        public PreBookingConfirmationToCustomer(string customerName,
            List<PQ_Price> priceList, List<OfferEntity> offerList,
            string bookingReferenceNo, uint totalAmount,
            uint preBookingAmount, string makeModelName, string versionName, string color, string img,
            string dealerName, string dealerAddress, string dealerMobile, string dealerEmailId, string dealerWorkingTime, double dealerLatitude, double dealerLongitude)
        {
            StringBuilder mail = new StringBuilder();
            try
            {
                mail.AppendFormat("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"> <html xmlns=\"http://www.w3.org/1999/xhtml\"> <head> <meta name=\"viewport\" content=\"width=device-width\" /> <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" /> <title>Emailer</title> </head> <body> <div style=\"max-width:692px; margin:0 auto; border:1px solid #f5f5f5; font-family: Arial, Helvetica, sans-serif; font-size:14px; color:#4d5057; background:#ffffff; word-wrap:break-word;\"> <div style=\"color:#fff; max-width:100%; min-height:195px; background:url('https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/user-booking-banner.jpg') no-repeat; padding:0 20px; \"> <!-- banner starts here --><div style=\"padding-top:20px;\"></div><div style=\"clear:both;\"></div> <div style=\"max-width:100%; min-height:40px; background:#2a2a2a;\"> <div style=\"float:left; max-width:82px; margin-top:5px; margin-left:20px;\"> <a href=\"www.bikewale.com\" target=\"_blank\"><img src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/bw-white-logo.png\" alt=\"BikeWale\" title=\"BikeWale\" width=\"100%\" border=\"0\" /></a> </div> <div style=\"float:right; margin-right:20px; font-size:14px; line-height:40px;\"> {0} </div> <div style=\"clear:both\"></div> </div> <div style=\"text-align:center\"> <div style=\"width:100%; height:115px; font-size:28px; text-align:center; display:table;\"> <div style=\"display:table-cell; vertical-align: middle;\">Congratulations!</div> </div> </div> </div> <!-- banner ends here --> <div> <!-- main content starts here --> <!-- paid and balance amount details starts here --> <div style=\"margin:0 10px;\"> <div style=\"display:inline-block; vertical-align:top; margin:15px 10px 0; max-width:430px;border-right:1px solid #f5f5f5;\"> <div style=\"font-weight:bold; margin-bottom:20px;\">Dear {1},</div> <div style=\"margin-bottom:10px; color:#82888b; line-height:1.5;\"> Congratulations on booking the <span style=\"color:#4d5057;\">{2}</span> on BikeWale. The booking amount of <span style=\"font-weight:bold; color:#4d5057;\"><img border=\"0\" title=\"Rupee\" alt=\"Rupee\" src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/inr-rupee-icon.png\">{3}</span> has been received. Your BikeWale Booking Reference Number is <span style=\"font-weight:bold; color:#4d5057;\">{4}</span>. <br />Following are the details: </div> </div> <div style=\"display:inline-block; vertical-align:top; margin:15px 10px 0; color:#82888b; width:180px;\"> <div style=\"padding-bottom:15px; border-bottom:1px solid #f5f5f5;\"> <div style=\"margin-bottom:10px;\">Advance payment</div> <div style=\"font-weight:bold; font-size:16px; color:#4d5057;\"><img border=\"0\" title=\"Rupee\" alt=\"Rupee\" src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/inr-rupee-med-icon.jpg\"> {3}</div> </div> <div style=\"padding-top:15px; padding-bottom:15px;\"> <div style=\"margin-bottom:10px;\">Balance payable amount</div> <div style=\"font-weight:bold; font-size:16px; color:#4d5057;\"><img border=\"0\" title=\"Rupee\" alt=\"Rupee\" src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/inr-rupee-med-icon.jpg\"> {5}</div> </div> </div> </div> <div style=\"margin:0 20px 15px 20px; padding-top:15px; padding-bottom:15px; border-top:1px solid #f5f5f5; border-bottom:1px solid #f5f5f5;\"> <!-- bike details starts here --> <div style=\"width:184px; min-height:150px; display:inline-block; vertical-align:top; margin:0 12px 10px 0; text-align:left;\"> <div style=\"font-weight:bold;\">{6}</div> <img src=\"{9}\" alt=\"{2}\" title=\"{2}\" border=\"0\" style=\"margin:20px 0 0 0\"/> </div> <div style=\"display:inline-block; vertical-align:top; max-width:455px; text-align:left;\"> <div style=\"float:left; width:226px; margin-right:5px; padding-bottom:15px;\"><div style=\"color:#82888b; width:55px; float:left;\">Version: </div><div style=\"float:left; width:170px; font-weight:bold; \">{7}</div><div style=\"clear:both;\"></div></div> <div style=\"float:left; width:220px; padding-bottom:15px; \"><div style=\"color:#82888b; width:50px; float:left;\">Colour: </div><div style=\"float:left; width:163px; font-weight:bold;\">{8}</div><div style=\"clear:both;\"></div></div> ",
                    DateTime.Now.ToString("dd MMM, yyyy"), //0
                    customerName, //1
                    makeModelName,//2
                    Utility.Format.FormatPrice(Convert.ToString(preBookingAmount)),//3
                    bookingReferenceNo, //4
                    Utility.Format.FormatPrice(Convert.ToString(totalAmount - preBookingAmount)),//5
                    makeModelName,//6
                    versionName, //7
                    color, //8
                    img//9
                    );
                if (priceList != null && priceList.Count > 0)
                {
                    mail.Append("<div style=\"clear:both; border-top:1px solid #f5f5f5; margin-bottom:15px; \"></div>");
                    foreach (var list in priceList)
                    {
                        mail.AppendFormat("<div style=\"padding-bottom:15px;\"> <div style=\"width:60%; float:left; color:#82888b;\">{0}</div> <div style=\"width:40%; float:left; text-align:right; font-weight:bold;\"><span><img src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/inr-rupee-icon.png\" alt=\"Rupee\" title=\"Rupee\" border=\"0\" /></span> {1}</div> <div style=\"clear:both;\"></div> </div>"
                            , list.CategoryName, Utility.Format.FormatPrice(Convert.ToString(list.Price)));
                    }
                }
                mail.AppendFormat("<div style=\"margin-bottom:15px;border-bottom:1px solid #f5f5f5;\"></div><div style=\"padding-bottom:15px;\"> <div style=\"width:60%; float:left; color:#82888b;\">On-road price</div> <div style=\"width:40%; float:left; text-align:right; font-weight:bold;\"><span><img src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/inr-rupee-icon.png\" alt=\"Rupee\" title=\"Rupee\" border=\"0\" /></span>{0}</div> <div style=\"clear:both;\"></div> </div> <div style=\"padding-bottom:15px;\"> <div style=\"width:60%; float:left; color:#82888b;\">Booking amount</div> <div style=\"width:40%; float:left; text-align:right; font-weight:bold;\"><span><img src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/inr-rupee-icon.png\" alt=\"Rupee\" title=\"Rupee\" border=\"0\" /></span>{1}</div> <div style=\"clear:both;\"></div> </div> <div> <div style=\"width:60%; float:left; color:#82888b;\">Balance payable amount</div> <div style=\"width:40%; float:left; text-align:right; font-weight:bold;\"><span><img src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/inr-rupee-icon.png\" alt=\"Rupee\" title=\"Rupee\" border=\"0\" /></span>{2}</div> <div style=\"clear:both;\"></div> </div> </div> </div>"
                    , Format.FormatPrice(Convert.ToString(totalAmount))
                    , Format.FormatPrice(Convert.ToString(preBookingAmount))
                    , Format.FormatPrice(Convert.ToString(totalAmount - preBookingAmount))
                    );
                //balance amount test end and dealer Deatail start.
                mail.AppendFormat("<div style=\"margin:0 20px; \"> <div style=\"font-size:14px; color:#4d5057; font-weight:bold;\">{0}:</div> <div style=\"font-size:14px; color:#82888b; margin:10px 0 10px 0;\">{1}</div> <div style=\"background:url(https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/call-icon.png) 0 2px no-repeat ;font-size:16px; color:#4d5057; margin:0 20px 10px 0; padding-left:15px; font-weight:bold; float:left;\">{2}</div> <div style=\"float:left; background:url(https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/mail-letter-icon.png) 0 4px no-repeat ;font-size:14px;  margin:2px 0 10px 0; padding-left:20px;\"><a href=\"mailto:bikewale@motors.com\" style=\"color:#82888b; text-decoration:none;\">{3}</a></div> <div style=\"clear:both;\"></div> <div style=\"font-size:14px; color:#82888b;\">Working hours: {4}</div> <div style=\"margin:10px 0 15px 0;\"><a href=\"https://maps.google.com/maps?q={5},{6}\" target=\"_blank\" style=\" background: url( https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/get-directions-icon.png) no-repeat 0 3px; padding-left:15px; color:#0288d1; font-size:14px; text-decoration: none;\">Get directions</a></div> </div>"
                    , dealerName, dealerAddress, dealerMobile, dealerEmailId, dealerWorkingTime, dealerLatitude, dealerLongitude);

                if (offerList != null && offerList.Count > 0)
                {
                    mail.AppendFormat("<div style=\"border-bottom:1px solid #f5f5f5; margin-left:20px; margin-right:20px;\"></div><div style=\"text-align:center;\"> <div style=\" padding-bottom:10px; margin:15px 20px 0 20px; text-align:left; font-size:14px; font-weight:bold; color:#4d5057;\">Exclusive offers from this dealer:</div><div style=\"padding:0 20px; line-height:1.4; text-align:left;\">");
                    foreach (var list in offerList)
                    {
                        mail.AppendFormat("<div style=\"max-width:190px; margin:10px 5px 10px; display:inline-block; vertical-align:top;\"> <div style=\"width:45px; display:inline-block; vertical-align:middle;\"><img src=\"https://imgd.aeplcdn.com/0x0/bw/static/icons/offers/{0}.png\" alt=\"Free bike insurance\" title=\"Free bike insurance\" border=\"0\" style=\"border:none;margin-right:5px;\" /></div> <div style=\"width:140px; display:inline-block; vertical-align:middle; text-align:left; font-size:14px; color:#82888b; margin:5px  0 0 0;\">{1}</div> <div style=\"clear:both;\"></div> </div>",
                            list.OfferCategoryId, list.OfferText);
                    }
                    mail.Append("</div></div>");
                }
                mail.Append("<div style=\"margin-top:10px;margin-bottom:20px; background:#f9f9f9;\"> <div style=\"margin-right:10px; margin-left:10px; padding:0 10px;\"> <div style=\"color:#4d5057; font-size:18px; text-align:center; font-weight:bold; padding:14px 0; margin-bottom:10px; border-bottom:1px solid #e2e2e2;\">Next steps</div> <div style=\"line-height:1.4; font-weight:bold; text-align:center;\"> <div style=\"max-width:195px; margin:10px 8px 20px; display:inline-block; vertical-align:top;\"> <div style=\"margin-bottom:10px;\"> <img src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/bw-booking-step-1-success.png\" alt=\"Book your bike by paying booking amount\" title=\"Book your bike by paying booking amount\" border=\"0\" style=\"width:140px; height:144px;\" /> </div> <div>Book your bike by paying booking amount</div> </div> <div style=\"max-width:195px; margin:10px 8px 20px; display:inline-block; vertical-align:top;\"> <div style=\"margin-bottom:10px;\"> <img src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/bw-booking-step2.png\" alt=\"Provide documents & payment at dealership\" title=\"Provide documents & payment at dealership\" border=\"0\" style=\"width:140px; height:144px;\" /> </div> <div>Provide documents & payment at dealership</div> </div> <div style=\"max-width:195px; margin:10px 8px 20px; display:inline-block; vertical-align:top;\"> <div style=\"margin-bottom:10px;\"> <img src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/bw-booking-step3.png\" alt=\"Collect your bike from the dealer\" title=\"Collect your bike from the dealer\" border=\"0\" style=\"width:140px; height:144px;\" /> </div> <div>Collect your bike from the dealer</div> </div> </div> </div> </div> <div style=\"margin:0 20px 0 20px; line-height:1.5; border-top:1px solid #e2e2e2;\"> <div style=\"font-size:14px; color:#82888b; margin-bottom:10px; margin-top:10px;\">We wish you a great buying experience!</div> <div style=\"margin-bottom:20px; color:#82888b;\">Regards,<br />Team BikeWale</div> </div> <div style=\"max-width:100%; background:url('https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/query-bg-banner.jpg') no-repeat center bottom / cover #2e2e2e; color:#fff;\"> <div style=\"padding:7px 15px 7px 20px;display:inline-block;vertical-align:middle;\"> <div style=\"float:left; width:46px; font-weight:bold;\"><img src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/bw-app-red-icon.png\" border=\"0\"/></div> <div style=\"font-size:16px;height:46px;text-align:left;display:table;margin-left:60px;\"> <div style=\"display:table-cell; vertical-align:middle;\">India’s #1 Bike Research Destination</div> </div> </div> <div style=\"margin:15px 20px 15px 0;display:inline-block;float:right\"> <div> <a href=\"#\" target=\"_blank\" style=\"text-decoration:none; color:#fff; font-weight:bold; font-size:12px; width:70px; background-color:#ef3f30; padding:8px 10px; border-radius:2px; display:block; \">Get the App</a> </div> </div> <div style=\"clear:both;\"></div> </div> <div style=\"margin:10px 0 4px 0; border-bottom:2px solid #c20000;\"></div> </div> </div> </body> </html>");
                MailHTML = Convert.ToString(mail);
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("Notifications.ErrorTempate ComposeBody : " + ex.Message);
            }
        }

        /// <summary>
        /// Created By : Lucky Rathore on 11 May 2016.
        /// Summary : Retrun the MailHTML
        /// </summary>
        /// <returns></returns>
        public override string ComposeBody()
        {
            return MailHTML;
        }
    }
}
