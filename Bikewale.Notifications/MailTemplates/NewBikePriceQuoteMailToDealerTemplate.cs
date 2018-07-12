using Bikewale.Entities.BikeBooking;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bikewale.Notifications.MailTemplates
{
    /// <summary>
    /// Created BY : Lucky Rathore on 12 May 2016
    /// Description : Template as per Desing Modified.
    /// </summary>
    public class NewBikePriceQuoteMailToDealerTemplate : ComposeEmailBase
    {
        private string MailHTML = null;

        /// <summary>
        /// Created BY : Lucky Rathore on 12 May 2016
        /// Description : To build string for mail template body.
        /// </summary>
        /// <param name="makeModel"></param>
        /// <param name="versionName"></param>
        /// <param name="dealerName"></param>
        /// <param name="customerName"></param>
        /// <param name="customerEmail"></param>
        /// <param name="customerMobile"></param>
        /// <param name="areaName"></param>
        /// <param name="cityName"></param>
        /// <param name="priceList"></param>
        /// <param name="totalPrice"></param>
        /// <param name="offerList"></param>
        /// <param name="imagePath"></param>
        public NewBikePriceQuoteMailToDealerTemplate(
            string makeModel, string versionName, string dealerName, string customerName,
            string customerEmail, string customerMobile, string areaName, string cityName,
            List<Entities.BikeBooking.PQ_Price> priceList, int totalPrice, List<Entities.BikeBooking.OfferEntity> offerList, string imagePath)
        {
            StringBuilder mail = new StringBuilder();
            areaName = string.IsNullOrEmpty(areaName) ? cityName : areaName + ", " + cityName;
            try
            {
                mail.AppendFormat("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"> <html xmlns=\"http://www.w3.org/1999/xhtml\"> <head> <meta name=\"viewport\" content=\"width=device-width\" /> <meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" /> <title>Emailer</title> </head> <body> <div style=\"max-width:692px; margin:0 auto; border:1px solid #f5f5f5; font-family: Arial, Helvetica, sans-serif; font-size:14px; color:#4d5057; background:#ffffff; word-wrap:break-word;\"> <div style=\"color:#fff; max-width:100%; min-height:195px; background:url('https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/pq-dealer-banner.jpg') no-repeat; padding:0 20px; \"> <!-- banner starts here --> <div style=\"padding-top:20px;\"></div> <div style=\"clear:both;\"></div> <div style=\"max-width:100%; min-height:40px; background:#2a2a2a;\"> <div style=\"float:left; max-width:82px; margin-top:5px; margin-left:20px;\"> <a href=\"#\" target=\"_blank\"><img src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/bw-white-logo.png\" alt=\"BikeWale\" title=\"BikeWale\" width=\"100%\" border=\"0\" /></a> </div> <div style=\"float:right; margin-right:20px; font-size:14px; line-height:40px;\"> {0} </div> <div style=\"clear:both\"></div> </div> <div style=\"text-align:center\"> <div style=\"width:100%; height:115px; font-size:28px; text-align:center; display:table;\"> <div style=\"display:table-cell; vertical-align: middle;\">You have a prospective buyer!</div> </div> </div> </div> <!-- banner ends here --> <div style=\"margin-bottom:15px; padding:15px 20px 0;\"> <div style=\"font-weight:bold; margin-bottom:20px;\">Dear {1},</div> <div style=\"line-height:1.5; color:#82888b;\">Our prospective buyer {4} has showed interest in <span style=\"color:#4d5057;\">{2} {3}</span> on BikeWale. Kindly get in touch with the customer and schedule a visit to your showroom.<br />Please check the details below:</div> </div> <!-- customer details starts here --> <div style=\"margin:15px 20px; border-bottom:1px solid #f5f5f5; border-top:1px solid #f5f5f5; padding-top:15px; word-break:break-all;\"> <div style=\"font-weight:bold; margin-bottom:15px;\">Our customer details:</div> <div style=\"display:inline-block; vertical-align:top; width:185px; margin:0 10px 15px 0;\"><span style=\"color:#82888b;width:45px;float:left;\">Name: </span><span style=\"font-weight:bold;width:140px; float:left;\">{4}</span></div><div style=\"display:inline-block; vertical-align:top; width:60%; margin:0 10px 15px 0;\"><span style=\"color:#82888b;\">Location: </span><span style=\"font-weight:bold;\">{7}</span></div><div style=\"clear:both;\"></div> <div style=\"display:inline-block; vertical-align:top; width:185px; margin:0 10px 15px 0;\"><span style=\"color:#82888b;\">Mobile no: </span><span style=\"font-weight:bold;\">{6}</span></div><div style=\"display:inline-block; vertical-align:top; width:60%; margin:0 10px 15px 0;\">{5}</div> <div style=\"clear:both;\"></div> </div> <!-- customer details ends here --> <div style=\"margin:0 20px 15px 20px; padding-bottom:15px; border-bottom:1px solid #f5f5f5;\"> <!-- bike details starts here --> <div style=\"width:184px; min-height:150px; display:inline-block; vertical-align:top; margin:0 12px 10px 0; text-align:left;\"> <div style=\"font-weight:bold;\">{2}</div> <img src=\"{8}\" alt=\"{2}\" title=\"{2}\" border=\"0\" style=\"margin:20px 0 0 0\"/> </div> <div style=\"display:inline-block; vertical-align:top; max-width:455px; text-align:left;\"> <div style=\"float:left; width:226px; margin-right:5px; padding-bottom:15px;\"><div style=\"color:#82888b; width:55px; float:left;\">Version: </div><div style=\"float:left; width:170px; font-weight:bold; \">{3}</div><div style=\"clear:both;\"></div></div> <div style=\"float:left; width:220px; padding-bottom:15px; \"></div> <div style=\"clear:both;\"></div><div style=\"border-top:1px solid #f5f5f5; padding-bottom:15px;\"></div>"
                    , DateTime.Now.ToString("dd MMM, yyyy") //0
                    , dealerName //1
                    , makeModel //2
                    , versionName //3
                    , customerName
                    ,(!String.IsNullOrEmpty(customerEmail)?String.Format("<span style=\"color:#82888b;\">Email Id: </span><span style=\"font-weight:bold;\">{0}</span>", customerEmail) : "")
                    , customerMobile
                    , areaName
                    , imagePath
                    );

                if (priceList != null && priceList.Count > 0)
                {
                    foreach (var list in priceList)
                    {
                        mail.AppendFormat("<div style=\"padding-bottom:15px;\"> <div style=\"width:60%; float:left; color:#82888b;\">{0}</div> <div style=\"width:40%; float:left; text-align:right; font-weight:bold;\"><span><img src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/inr-rupee-icon.png\" alt=\"Rupee\" title=\"Rupee\" border=\"0\" /></span>{1}</div> <div style=\"clear:both;\"></div> </div>"
                            , list.CategoryName
                            , Utility.Format.FormatPrice(Convert.ToString(list.Price))
                            );
                    }
                    if (offerList != null && offerList.Count > 0)
                    {
                        List<PQ_Price> discountList = OfferHelper.ReturnDiscountPriceList(offerList, priceList);
                        if (discountList != null && discountList.Count > 0)
                        {
                            foreach (var list in discountList)
                            {
                                mail.AppendFormat("<div style=\"padding-bottom:15px;\"> <div style=\"width:60%; float:left; color:#82888b;\">{0}</div> <div style=\"width:40%; float:left; text-align:right; font-weight:bold;\"><span><img src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/inr-rupee-icon.png\" alt=\"Rupee\" title=\"Rupee\" border=\"0\" /></span>{1}</div> <div style=\"clear:both;\"></div> </div>"
                                    , list.CategoryName
                                    , Utility.Format.FormatPrice(Convert.ToString(list.Price))
                                    );
                            }
                        }
                    }
                    mail.AppendFormat("<div style=\"border-top:1px solid #f5f5f5; padding-bottom:15px;\"></div><div style=\"padding-bottom:15px;\"> <div style=\"width:60%; float:left; color:#82888b;\">On Road Price</div> <div style=\"width:40%; float:left; text-align:right; font-weight:bold;\"><span><img src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/inr-rupee-icon.png\" alt=\"Rupee\" title=\"Rupee\" border=\"0\" /></span>{0}</div> <div style=\"clear:both;\"></div> </div></div></div><div style=\"text-align:center;\"> "
                                    , Format.FormatPrice(Convert.ToString(totalPrice))
                                    );
                    if (offerList != null && offerList.Count > 0)
                    {
                        mail.Append("<div style=\" padding-bottom:10px; margin:15px 20px 0 20px; text-align:left; font-size:14px; font-weight:bold; color:#4d5057;\">Offers availed by our customer:</div> <div style=\"text-align:left; padding:0 20px; line-height:1.4;\">");
                        foreach (var list in offerList)
                        {
                            mail.AppendFormat("<div style=\"max-width:190px; margin:10px 5px 20px; display:inline-block; vertical-align:top;\"> <div style=\"width:45px; display:inline-block; vertical-align:middle;\"><img src=\"https://imgd.aeplcdn.com/0x0/bw/static/icons/offers/{0}.png\" alt=\"Free bike insurance\" title=\"Free bike insurance\" border=\"0\" style=\"border:none;margin-right:5px;\" /></div> <div style=\"width:140px; display:inline-block; vertical-align:middle; text-align:left; font-size:14px; color:#82888b; margin:5px  0 0 0;\">{1}</div> <div style=\"clear:both;\"></div> </div>"
                                , list.OfferCategoryId
                                , list.OfferText);
                        }
                    }
                }

                mail.AppendFormat("<div style=\"margin-bottom:20px; background:#f9f9f9;\"> <!-- what's next starts here --> <div style=\"\"> <div style=\"color:#4d5057; font-size:18px; text-align:center; font-weight:bold; padding:14px 0; margin-bottom:10px; border-bottom:1px solid #e2e2e2;\">Next steps</div> <div style=\"line-height:1.4; font-weight:bold; text-align:center;\"> <div style=\"max-width:195px; margin:10px 8px 20px; display:inline-block; vertical-align:top;\"> <div style=\"margin-bottom:10px;\"> <img src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/bw-booking-step1.png\" alt=\"Get in touch with the customer to initiate booking\" title=\"Get in touch with the customer to initiate booking\" border=\"0\" style=\"width:140px; height:144px;\" /> </div> <div>Get in touch with the customer to initiate booking</div> </div> <div style=\"max-width:195px; margin:10px 8px 20px; display:inline-block; vertical-align:top;\"> <div style=\"margin-bottom:10px;\"> <img src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/bw-booking-step2.png\" alt=\"Complete formalities of Documents and Finance\" title=\"Complete formalities of Documents and Finance\" border=\"0\" style=\"width:140px; height:144px;\" /> </div> <div>Complete formalities of Documents and Finance</div> </div> <div style=\"max-width:195px; margin:10px 8px 20px; display:inline-block; vertical-align:top;\"> <div style=\"margin-bottom:10px;\"> <img src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/bw-booking-step3.png\" alt=\"Deliver bike to customer along with offers claimed\" title=\"Deliver bike to customer along with offers claimed\" border=\"0\" style=\"width:140px; height:144px;\" /> </div> <div>Deliver bike to customer along with offers claimed</div> </div> </div> </div> </div> <!-- what's next ends here --> <div style=\"margin-bottom:2px; padding:0 20px; line-height:1.5; color:#82888b; \"><div style=\"margin-bottom:25px; text-align:left\">Regards,<br />Team BikeWale</div> </div> <div style=\"max-width:100%; background:url('https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/query-bg-banner.jpg') no-repeat center bottom / cover #2e2e2e; color:#fff;\"> <div style=\"padding:7px 15px 7px 20px;display:inline-block;vertical-align:middle;\"> <div style=\"float:left; width:46px; font-weight:bold;\"><img src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/bw-app-red-icon.png\" border=\"0\"/></div> <div style=\"font-size:16px;height:46px;text-align:left;display:table;margin-left:60px;\"> <div style=\"display:table-cell; vertical-align:middle;\">India’s #1 Bike Research Destination</div> </div> </div> <div style=\"margin:15px 20px 15px 0;display:inline-block;float:right\"> <div> <a href=\"https://play.google.com/store/apps/details?id=com.bikewale.app&utm_source=LeadMailer&utm_medium=email&utm_campaign=DealerLeadMail\" target=\"_blank\" style=\"text-decoration:none; color:#fff; font-weight:bold; font-size:12px; width:70px; background-color:#ef3f30; padding:8px 10px; border-radius:2px; display:block; \">Get the App</a> </div> </div> <div style=\"clear:both;\"></div> </div> <div style=\"margin:10px 0 4px 0; border-bottom:2px solid #c20000;\"></div> </div> </body> </html>");
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Notification.NewBikePriceQuoteMailToDealerTemplate.ComposeBody");
                
            }
            MailHTML = mail.ToString();
        }

        /// <summary>
        /// Modified BY : Lucky Rathore on 12 May 2016
        /// Description : return mailHTML string of mail template HTML.
        /// </summary>
        /// <returns>string of HTML of Mail template</returns>
        public override string ComposeBody()
        {
            return MailHTML;
        }

    }
}
