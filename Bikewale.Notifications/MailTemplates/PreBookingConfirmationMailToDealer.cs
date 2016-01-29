using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.BikeBooking;
using System.Web;
using Bikewale.Utility;

namespace Bikewale.Notifications.MailTemplates
{
    /// <summary>
    /// Created By : Lucky Rathore on 20 Jan 2016.
    /// Summary : To send pre-booking email to dealer.
    /// </summary>
    public class PreBookingConfirmationMailToDealer : ComposeEmailBase
    {
        private string MailHTML = null;

        public PreBookingConfirmationMailToDealer(string customerName, string customerMobile, string customerArea, string customerEmail, uint totalPrice, uint bookingAmount,
            uint balanceAmount, List<PQ_Price> priceList, string bookingReferenceNo, string bikeName, string bikeColor, string dealerName, List<OfferEntity> offerList, string imagePath, uint insuranceAmount = 0)
        {
            List<PQ_Price> discountList = OfferHelper.ReturnDiscountPriceList(offerList, priceList);
            StringBuilder sb = null;
            try
            {
                sb = new StringBuilder();
                sb.AppendFormat(
                    "<div style=\"max-width:692px; margin:0 auto; border:1px solid #4d5057; font-family: Arial, Helvetica, sans-serif; font-size:14px; color:#4d5057; background:#f5f5f5; word-wrap:break-word;\">"
                        + "<div style=\"padding:10px 20px; margin-bottom:20px;\">"
                            + "<div style=\"float:left; max-width:110px;\">"
                                + "<a href=\"#\" target=\"_blank\"><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/bw-logo.png\" alt=\"BikeWale\" title=\"BikeWale\" width=\"100%\" border=\"0\"/></a>"
                            + "</div>"
                            + "<div style=\"float:right; color:#82888b; line-height:32px;\">{0}"
                            + "</div>"
                            + "<div style=\"clear:both;\"></div>"
                        + "</div>"

                        + "<div style=\"padding:0 20px;\">"
                            + "<div style=\"font-weight:bold; margin-bottom:20px;\">Dear {1},</div>"
                            + "<div style=\"margin-bottom:15px;\">Pre-Booking for {2} {3}</div>"
                            + "<div style=\"line-height:1.4; margin-bottom:15px;\">Please call customer {6} ASAP and proceed with further selling process, Customer has paid <span><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/rupee-large.png\" alt=\"Rupee\" title=\"Rupee\" border=\"0\" /></span><span style=\"color:#4d5057; font-weight:bold;\">{4}</span> to pre-book {2} {3}, with Pre-booking Ref Number<span style=\"color:#4d5057; font-weight:bold;\">&nbsp;{5}</span>. Check below for more details:</div>"
                        + "</div>",
                        DateTime.Now.ToString("MMM dd, yyyy"), //0
                        dealerName, //1
                        bikeName, //2
                        string.Empty,//3
                        Format.FormatPrice(bookingAmount.ToString()), //4
                        bookingReferenceNo.Trim(), //5
                        customerName //6
                        );

                sb.AppendFormat(
                    "<div style=\"margin:0 10px 20px; background:#fff;\">"
                    + "<div style=\"color:#2a2a2a; font-weight:bold; padding:18px 0; margin:0 10px 20px; border-bottom:1px solid #e2e2e2;\">{0} [{2}]</div>"
                    + "<div style=\"margin:25px 0 0; text-align:center;\"> <!-- bike details starts here -->"
                        + "<div style=\"display:inline-block; vertical-align:top; margin:0px 30px 10px 10px;\">"
                            + "<div style=\"width:192px; height:107px; margin-bottom:20px;\">"
                                + "<img src=\"{1}\" alt=\"{0}\" title=\"{0}\" width=\"100%\" border=\"0\"/>"
                            + "</div>"
                        + "</div>"
                        + "<div style=\"display:inline-block; vertical-align:top; max-width:428px; text-align:left; padding:0 20px 0 10px;\">"
                        , bikeName, imagePath, bikeColor
                    );

                //PriceList Section 
                if (priceList != null && priceList.Count > 0)
                {
                    foreach (var list in priceList)
                    {
                        sb.AppendFormat(
                            "<div style=\"padding-bottom:20px;\">"
                                + "<div style=\"width:70%; float:left; color:#82888b;\">{0}</div>"
                                + "<div style=\"width:30%; float:left; text-align:right; font-weight:bold;\"><span><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/rupee-large.png\" alt=\"Rupee\" title=\"Rupee\" border=\"0\" /></span>{1}</div>"
                                + "<div style=\"clear:both;\"></div>"
                            + "</div>"
                            , list.CategoryName, Format.FormatPrice(list.Price.ToString()));
                    }
                    if (discountList != null && discountList.Count > 0)
                    {
                        sb.AppendFormat(
                            "<div style=\"padding:20px 0; border-top:1px solid #8a9093;\">"
                                + "<div style=\"width:70%; float:left; color:#82888b;\">Total on road price</div>"
                                + "<div style=\"width:30%; float:left; text-align:right; font-weight:bold;\"><span><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/rupee-large.png\" alt=\"Rupee\" title=\"Rupee\" border=\"0\" /></span><span style=\"text-decoration:line-through;\">{0}</span></div>"
                                + "<div style=\"clear:both;\"></div>"
                            + "</div>"
                            , Format.FormatPrice(totalPrice.ToString())
                            );
                        foreach (var list in discountList)
                        {
                            sb.AppendFormat(
                                "<div style=\"padding-bottom:20px;\">"
                                    + "<div style=\"width:70%; float:left; color:#82888b;\">{0}</div>"
                                    + "<div style=\"width:30%; float:left; text-align:right; font-weight:bold;\"><span><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/rupee-large.png\" alt=\"Rupee\" title=\"Rupee\" border=\"0\" /></span>{1}</div>"
                                    + "<div style=\"clear:both;\"></div>"
                                + "</div>"
                            , list.CategoryName, Format.FormatPrice(list.Price.ToString()));
                        }
                    }
                    sb.AppendFormat(
                        "<div>"
                            + "<div style=\"width:70%; float:left; font-weight:bold;\">Total on road price</div>"
                            + "<div style=\"width:30%; float:left; text-align:right; font-weight:bold;\"><span><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/rupee-large.png\" alt=\"Rupee\" title=\"Rupee\" border=\"0\" /></span>{0}</div>"
                            + "<div style=\"clear:both;\"></div>"
                        + "</div>"
                        , Format.FormatPrice(Convert.ToString(totalPrice - TotalDiscountedPrice(discountList)))
                        );
                }
                sb.AppendFormat(
                                "</div>"
                            + "</div> <!-- bike details ends here -->"
                        + "</div>"
                        );

                //Personal detail Section. 
                sb.AppendFormat("<div style=\"text-align:center; margin-bottom:20px;\">"
                    + "<div style=\"max-width:320px; min-height:203px; display:inline-block; vertical-align:top; background:#fff; margin:0 10px;\">"
                        + "<div style=\"padding:0 10px; text-align:left;\">"
                            + "<div style=\"color:#2a2a2a; font-weight:bold; padding:18px 0; margin-bottom:30px; border-bottom:1px solid #e2e2e2;\">Customer details</div>"
                            + "<div style=\"padding-bottom:15px;\">"
                                + "<span style=\"color:#82888b;\">Name:</span>"
                                + "<span> {0}</span>"
                            + "</div>"
                            + "<div style=\"padding-bottom:15px;\">"
                                + "<span style=\"color:#82888b;\">Contact no:</span>"
                                + "<span> {1}</span>"
                            + "</div>"
                            + "<div style=\"padding-bottom:15px;\">"
                                + "<span style=\"color:#82888b;\">Email id:</span>"
                                + "<span> {2}</span>"
                            + "</div>"
                            + "<div style=\"padding-bottom:15px; line-height:1.4;\">"
                                + "<span style=\"color:#82888b;\">Location:</span>"
                                + "<span> {3} </span>"
                            + "</div>"
                        + "</div>"
                    + "</div>"
                    + "<div style=\"max-width:320px; min-height:203px; display:inline-block; vertical-align:top; background:#fff; margin:0 10px;\">"
                        + "<div style=\"padding:0 10px; text-align:left;\">"
                            + "<div style=\"color:#2a2a2a; font-weight:bold; padding:18px 0; margin-bottom:20px; border-bottom:1px solid #e2e2e2;\">Payment details</div>"
                            + "<div style=\"display:table; text-align:center; padding-bottom:20px;\">"
                                + "<div style=\"width:120px; height:110px; display:table-cell; vertical-align:middle; padding-left:10px; padding-right:20px; border-right:1px solid #e2e2e2;\">"
                                    + "<div style=\"margin-bottom:10px;\">Advance Payment</div>"
                                    + "<div><span><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/rupee-large.png\" alt=\"Rupee\" border=\"0\"/></span><span style=\"font-size:20px; font-weight:bold; color:#1a1a1a;\">{4}</span></div>"
                                + "</div>"
                                + "<div style=\"width:120px; height:110px; display:table-cell; vertical-align:middle; padding-left:20px; padding-right:10px;\">"
                                    + "<div style=\"margin-bottom:10px;\">Balance Payment</div>"
                                    + "<div><span><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/rupee-large.png\" alt=\"Rupee\" border=\"0\"/></span><span style=\"font-size:20px; font-weight:bold; color:#1a1a1a;\">{5}</div>"
                                + "</div>"
                            + "</div>"
                        + "</div>"
                    + "</div>"
                + "</div>"
            , customerName //0
            , customerMobile //1
            , customerEmail //2
            , customerArea //3
            , Format.FormatPrice(bookingAmount.ToString()) //4
            , Format.FormatPrice((balanceAmount - TotalDiscountedPrice(discountList)).ToString()) //5
            );
                //Offer Text 
                if (offerList != null && offerList.Count > 0)
                {
                    sb.AppendFormat(
                        "<div style=\"margin:0 10px 20px; padding:0 10px; background:#fff;\"> <!-- bw offers starts here -->"
                            + "<div style=\"color:#2a2a2a; font-weight:bold; padding:18px 0; margin-bottom:20px; border-bottom:1px solid #e2e2e2;\">Applicable Offers for this purchase</div>"
                            + "<ul style=\"margin-left:15px; padding:0;\">"
                        );
                    foreach (var offer in offerList)
                    {
                        sb.AppendFormat("<li style=\"padding-bottom:20px;\">{0}</li>", offer.OfferText);
                    }
                    sb.AppendFormat(
                            "</ul>"
                        + "</div> <!-- bw offers ends here -->"
                        );
                }
                sb.AppendFormat(
                        "<div style=\"margin-bottom:2px; padding:0 20px; line-height:1.4; border-bottom:2px solid #c20000;\">"
                            + "<div style=\"margin-bottom:25px;\">Please feel free to call 8828305054 for any queries or help required in the process.</div>"
                            + "<div style=\"margin-bottom:25px;\">Regards,<br />Team BikeWale</div>"
                        + "</div>"
                        + "<div style=\"margin-top:20px; margin-bottom:10px; max-width:670px;\">"
                            + "<a href=\"https://play.google.com/store/apps/details?id=com.bikewale.app&utm_source=&utm_medium=email&utm_campaign=\" target=\"_blank\">"
                            + "<img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/bw-footer-banner.jpg\" style=\"border:0; width:100%\"></a>"
                        + "</div>"
                    + "</div>"
                    + "</body>"
                    );

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.Notification.PreBookingConfirmationMailToDealer.ComposeBody");
                objErr.SendMail();
            }
            MailHTML = sb.ToString();
        }

        public override string ComposeBody()
        {
            return MailHTML;
        }

        /// <summary>
        /// Creted By : Lucky Rathore
        /// Created on : 08 January 2016
        /// </summary>
        /// <returns>Total dicount on specific Version.</returns>
        protected UInt32 TotalDiscountedPrice(List<PQ_Price> discountList)
        {
            UInt32 totalPrice = 0;

            if (discountList != null && discountList.Count > 0)
            {
                foreach (var priceListObj in discountList)
                {
                    totalPrice += priceListObj.Price;
                }
            }

            return totalPrice;
        }
    }
}

