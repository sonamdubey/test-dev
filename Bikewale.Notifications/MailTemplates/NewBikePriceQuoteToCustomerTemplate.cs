using Bikewale.Entities.BikeBooking;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bikewale.Notifications.MailTemplates
{
    /// <summary>
    /// Create By : Lucky Rathore on 27 Apr 2016
    /// Description : New Mail Template used for user mail. 
    /// </summary>
    public class NewBikePriceQuoteToCustomerTemplate : ComposeEmailBase
    {
        private StringBuilder mail = null;

        public NewBikePriceQuoteToCustomerTemplate(string bikeName, string versionName, string bikeImage, string dealerEmailId, string dealerMobileNo,
            string organization, string address, string customerName, List<PQ_Price> priceList, List<OfferEntity> offerList,
            string pinCode, string stateName, string cityName, uint totalPrice
            , double dealerLat, double dealerLong, string workingHours)
        {
            List<PQ_Price> discountList = OfferHelper.ReturnDiscountPriceList(offerList, priceList); //null Exception handle in the function.
            mail = new StringBuilder();

            mail.AppendFormat("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"> <html xmlns=\"http://www.w3.org/1999/xhtml\"> <head> <meta name=\"viewport\" content=\"width=device-width\" /> <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" /> <title>Emailer</title> </head> <body> <div style=\"max-width:692px; margin:0 auto; border:1px solid #f5f5f5; font-family: Arial, Helvetica, sans-serif; font-size:14px; color:#4d5057; background:#ffffff; word-wrap:break-word;\"> <div style=\"color:#fff; max-width:100%; min-height:195px; background:url('https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/lead-sub-banner.jpg') no-repeat; padding:0 20px; \"> <!-- banner starts here --><div style=\"padding-top:20px;\"></div><div style=\"clear:both;\"></div> <div style=\"max-width:100%; min-height:40px; background:#2a2a2a;\"> <div style=\"float:left; max-width:82px; margin-top:5px; margin-left:20px;\"> <a href=\"#\" target=\"_blank\"><img src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/bw-white-logo.png\" alt=\"BikeWale\" title=\"BikeWale\" width=\"100%\" border=\"0\" /></a> </div> <div style=\"float:right; margin-right:20px; font-size:14px; line-height:40px;\"> {0} </div> <div style=\"clear:both\"></div> </div> <div style=\"text-align:center\"> <div style=\"width:100%; height:115px; font-size:28px; text-align:center; display:table;\"> <div style=\"display:table-cell; vertical-align: middle;\">Thanks for your interest</div> </div> </div> </div> <!-- banner ends here --> <div> <!-- main content starts here --> <div style=\"font-weight:bold; margin-bottom:20px; padding:15px 20px 0;\">Dear {1},</div> <div style=\"color:#82888b; margin-bottom:18px; padding:0 20px; line-height:1.5;\">Congratulations on selecting the fantastic <span style=\"color:#4d5057;\">{2}</span>. Following are the details on its price, applicable offers and nearest dealer for your ready reference:</div> <div style=\"margin:0 20px 15px 20px; padding:15px 0 15px 0;border-top:1px solid #f5f5f5; border-bottom:1px solid #f5f5f5; text-align:center;\"> <!-- bike details starts here --> <div style=\"width:184px; min-height:150px; display:inline-block; vertical-align:top; margin:0 12px 10px 0; text-align:left;\"> <div style=\"font-weight:bold;\">{2}</div> <img src=\"{3}\" alt=\"{2}\" title=\"{2}\" border=\"0\" style=\"margin:20px 0 0 5px\"/> </div> <div style=\"display:inline-block; vertical-align:top; max-width:455px; text-align:left;\"> <div style=\"float:left; width:226px; margin-right:5px; padding-bottom:15px;\"><div style=\"color:#82888b; width:55px; float:left;\">Version: </div><div style=\"float:left; width:170px; font-weight:bold; \">{4}</div><div style=\"clear:both;\"></div></div> <div style=\"float:left; width:220px; padding-bottom:15px;\"><div style=\"clear:both;\"></div></div> <div style=\"clear:both;\"></div>"
                , DateTime.Now.ToString("dd MMM, yyyy"), customerName, bikeName, bikeImage, versionName);

            if (priceList != null && priceList.Count > 0)
            {
                mail.Append("<div style=\" border-top:1px solid #f5f5f5 \"></div>");
                foreach (var plist in priceList)
                {
                    mail.AppendFormat("<div style=\" padding-top:15px;\"> <div style=\"width:60%; float:left; color:#82888b;\">{0}</div> <div style=\"width:40%; float:left; text-align:right; font-weight:bold;\"><span><img src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/inr-rupee-icon.png\" alt=\"Rupee\" title=\"Rupee\" border=\"0\" /></span>{1}</div> <div style=\"clear:both;\"></div> </div>"
                        , plist.CategoryName, Format.FormatPrice(Convert.ToString(plist.Price)));
                }
                if (discountList != null && discountList.Count > 0)
                {
                    mail.Append("<div style=\" border-top:1px solid #f5f5f5 \"></div>");
                    foreach (var dlist in discountList)
                    {
                        mail.AppendFormat("<div style=\" padding-top:15px;\"> <div style=\"width:60%; float:left; color:#82888b;\">{0}</div> <div style=\"width:40%; float:left; text-align:right; font-weight:bold;\"><span><img src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/inr-rupee-icon.png\" alt=\"Rupee\" title=\"Rupee\" border=\"0\" /></span>{1}</div> <div style=\"clear:both;\"></div> </div>"
                            , dlist.CategoryName, Format.FormatPrice(Convert.ToString(dlist.Price)));
                    }
                }
                mail.AppendFormat("<div style=\"margin-top:15px; padding-top:15px; padding-bottom:15px;  border-top:1px solid #f5f5f5;\"> <div style=\"width:60%; float:left; color:#82888b;\">On-road price</div> <div style=\"width:40%; float:left; text-align:right; font-weight:bold;\"><span><img src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/inr-rupee-icon.png\" alt=\"Rupee\" title=\"Rupee\" border=\"0\" /></span>{0}</div> <div style=\"clear:both;\"></div> </div>"
                    , Format.FormatPrice(Convert.ToString(totalPrice)));
            }

            //Dealer deatails
            mail.AppendFormat("</div></div><!-- bike details ends here --><div style=\"margin:0 20px; \"> <div style=\"font-size:14px; color:#4d5057; font-weight:bold;\">{0}</div> <div style=\"font-size:14px; color:#82888b; margin:10px 0 10px 0;\">{1}, {2}, {3}, {4}</div> <div style=\"background:url(https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/call-icon.png) 0 2px no-repeat ;font-size:16px; color:#4d5057; margin:0 20px 10px 0; padding-left:15px; font-weight:bold; float:left;\">{5}</div> <div style=\"float:left; background:url(https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/mail-letter-icon.png) 0 4px no-repeat ;font-size:14px;  margin:2px 0 10px 0; padding-left:20px;\"><a href=\"mailto:{6}\" style=\"color:#82888b; text-decoration:none;\">{6}</a></div> <div style=\"clear:both;\"></div>"
                , organization, address, cityName, stateName, pinCode, dealerMobileNo, dealerEmailId);

            if (!string.IsNullOrEmpty(workingHours))
            {
                mail.AppendFormat("<div style=\"font-size:14px; color:#82888b;\">Working hours: {0}</div>", workingHours);//ToTest 
            }
            //Get Direction code.
            mail.AppendFormat("<div style=\"margin:10px 0 15px 0;\"><a href=\"https://maps.google.com/maps?q={0},{1}\" target=\"_blank\" style=\" background: url(https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/get-directions-icon.png) no-repeat 0 3px; padding-left:15px; color:#0288d1; font-size:14px; text-decoration: none;\">Get directions</a></div> </div>"
                , dealerLat, dealerLong); //url need to copy paste

            if (offerList != null && offerList.Any())
            {
                mail.Append("<div style=\"text-align:center; border-top:1px solid #f5f5f5;\"> <div style=\" padding-bottom:10px; padding-top:15px; margin:0 20px; text-align:left; font-size:14px; font-weight:bold; color:#4d5057;\">Exclusive offers from this dealer:</div> <div style=\" line-height:1.4; margin:0 20px; text-align:left;\">");
                foreach (var offer in offerList)
                {
                    mail.AppendFormat("<div style=\"max-width:210px; margin:10px 5px 10px; display:inline-block; vertical-align:top;\"> <div style=\"width:45px; float:left;\"><img src=\"https://imgd.aeplcdn.com/0x0/bw/static/icons/offers/{1}.png\" alt=\"Free bike insurance\" title=\"Free bike insurance\" border=\"0\" style=\"border:none;margin-right:5px;\" /></div> <div style=\"width:160px; float:left; text-align:left; font-size:14px; color:#82888b; margin:5px  0 0 0;\">{0}</div> <div style=\"clear:both;\"></div> </div>",
                        offer.OfferText, offer.OfferCategoryId);
                }
                mail.Append("</div> </div>");
            }
            mail.Append("<div style=\"border-top:1px solid #f5f5f5;margin:10px 20px 0 20px; line-height:1.5;\"> <div style=\"font-size:14px; color:#82888b; margin:15px 0 15px 0;\">The dealership will call you shortly to schedule your visit to dealership. You will be required to go to the dealership to complete the remaining procedure and take delivery of the vehicle. <div style=\"margin:15px 0 10px 0;\">We wish you a great buying experience! </div> <div style=\"margin-bottom:25px;\">Regards,<br />Team BikeWale</div> </div> </div> <div style=\"max-width:100%; background:url('https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/query-bg-banner.jpg') no-repeat center bottom / cover #2e2e2e; color:#fff;\"> <div style=\"padding:7px 15px 7px 20px;display:inline-block;vertical-align:middle;\"> <div style=\"float:left; width:46px; font-weight:bold;\"><img src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/bw-app-red-icon.png\" border=\"0\"/></div> <div style=\"font-size:16px;height:46px;text-align:left;display:table;margin-left:60px;\"> <div style=\"display:table-cell; vertical-align:middle;\">India’s #1 Bike Research Destination</div> </div> </div> <div style=\"margin:15px 20px 15px 0;display:inline-block;float:right\"> <div> <a href=\"https://play.google.com/store/apps/details?id=com.bikewale.app&utm_source=LeadMailer&utm_medium=email&utm_campaign=UserLeadMail\" target=\"_blank\" style=\"text-decoration:none; color:#fff; font-weight:bold; font-size:12px; width:70px; background-color:#ef3f30; padding:8px 10px; border-radius:2px; display:block; \">Get the App</a> </div> </div> <div style=\"clear:both;\"></div> </div> <div style=\"margin:10px 0 4px 0; border-bottom:2px solid #c20000;\"></div> </div> <!-- main content ends here --> </div> </body> </html>");
        }

        public override string ComposeBody()
        {
            return Convert.ToString(mail);
        }
    }
}
