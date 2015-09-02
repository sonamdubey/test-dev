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
    /// Created By : Sadhana Upadhyay on 30 Dec 2014
    /// Summary : To send pre-booking email to dealer
    /// </summary>
    public class PreBookingConfirmationMailToDealer : ComposeEmailBase
    {
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerArea { get; set; }
        public string CustomerEmail { get; set; }
        public uint TotalPrice { get; set; }
        public uint BookingAmount { get; set; }
        public uint BalanceAmount { get; set; }
        public List<PQ_Price> PriceList { get; set; }
        public string BookingReferenceNo { get; set; }
        public string BikeName { get; set; }
        public string BikeColor { get; set; }
        public string DealerName { get; set; }
        public List<OfferEntity> OfferList { get; set; }
        public uint InsuranceAmount { get; set; }
        public PreBookingConfirmationMailToDealer(string customerName, string customerMobile, string customerArea, string customerEmail, uint totalPrice, uint bookingAmount,
            uint balanceAmount, List<PQ_Price> priceList, string bookingReferenceNo, string bikeName, string bikeColor, string dealerName, List<OfferEntity> offerList, uint insuranceAmount = 0)
        {
            CustomerName = customerName;
            CustomerMobile = customerMobile;
            CustomerEmail = customerEmail;
            CustomerArea = customerArea;
            TotalPrice = totalPrice;
            BookingAmount = bookingAmount;
            BalanceAmount = balanceAmount;
            PriceList = priceList;
            BookingReferenceNo = bookingReferenceNo;
            BikeName = bikeName;
            BikeColor = bikeColor;
            DealerName = dealerName;
            OfferList = offerList;
            InsuranceAmount = insuranceAmount;
        }

        public override StringBuilder ComposeBody()
        {
            StringBuilder sb = null;

            try
            {
                sb = new StringBuilder();

                sb.Append("<div style=\"max-width:680px; margin:0 auto; border:1px solid #d8d8d8; font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#666666; background:#eeeeee;padding:10px 10px 10px 10px; word-wrap:break-word\">");
                sb.Append("<div style=\"margin:5px 0 0;\"><div style=\" background:#fff; padding:7px; border-top:7px solid #333333;\"><div style=\"float:left; margin-right:10px;\">");
                sb.Append("<a target=\"_blank\" href=\"http://www.bikewale.com/\"><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/bw-logo.png\" border=\"0\" alt=\"BikeWale\" title=\"BikeWale\"></a>");
                sb.Append("</div><div style=\" font-size:18px; font-weight:bold; float:left; margin:5px 0 0;\">Pre-Booking for " + BikeName + " [" + BikeColor + "]</div><div style=\"float:right; color:#666; margin:5px 0 0;\">" + DateTime.Now.ToString("MMM dd, yyyy") + "</div>");
                sb.Append("<div style=\"clear:both;\"></div></div><div style=\" background:#fff; padding:10px; margin:10px 0 0;\"><div style=\"padding:10px 0;\">");
                sb.Append("<div style=\" margin:0; font-size:14px; font-weight:bold; color:#333;\">Dear " + DealerName + ",</div>");
                sb.Append("<div style=\"margin:10px 0 0;\">Please call customer <span style=\"font-weight:bold;\">" + CustomerName + "</span> ASAP and proceed with further selling process, Customer has paid Rs. ");
                sb.Append("<span style=\"font-weight:bold;\">" + Format.FormatPrice(BookingAmount.ToString()) + "</span> to pre-book <span style=\"font-weight:bold;\">" + BikeName + "</span>, with Pre-booking Ref Number " + BookingReferenceNo + ". Check below for more details:.</div>");
                sb.Append("</div><div style=\"background:url(http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/red-border.png) no-repeat center center; height:2px; width:100%;\"></div><div style=\" padding:10px 0;\">");
                sb.Append("<div style=\"font-size:14px; font-weight:bold; color:#333; margin-bottom:10px;\">Customer Details:</div><table cellpadding=\"0\" cellspacing=\"0\">");
                sb.Append("<tbody><tr><td width=\"140\" style=\"font-weight:bold; color:#333; padding-bottom:5px;\">Customer Name:</td><td style=\"padding-bottom:5px;\">" + CustomerName + "</td></tr>");
                sb.Append("<tr><td style=\"font-weight:bold; color:#333; padding-bottom:5px;\">Customer Number</td><td style=\"padding-bottom:5px;\">" + CustomerMobile + "</td></tr>");
                sb.Append("<tr><td style=\"font-weight:bold; color:#333; padding-bottom:5px;\">Customer Email:</td><td style=\"padding-bottom:5px;\">" + CustomerEmail + "</td></tr>");
                sb.Append("<tr><td style=\"font-weight:bold; color:#333; padding-bottom:5px;\">Customer Location:</td><td style=\"padding-bottom:5px;\">" + CustomerArea + "</td></tr></tbody></table>");
                sb.Append("<div style=\"font-size:14px; font-weight:bold; color:#333; margin:10px 0 0;\">Selected Bike: <span style=\"font-size:12px;\">" + BikeName + " [" + BikeColor + "]</div>");
                sb.Append("</div><div style=\" text-align:center;\">");

                if (PriceList != null && PriceList.Count > 0)
                {
                    sb.Append("<div style=\"display:inline-block; border:1px solid #e2e2e2; background:#eaf8ff; margin:0 10px 10px 10px; padding:10px; text-align:left; vertical-align:top;\">");
                    sb.Append("<div style=\"font-weight:bold; text-align:center; margin:0 0 10px\">On-Road Price Breakup</div><table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\"><tbody>");

                    foreach (var items in PriceList)
                    {
                        sb.Append("<tr><td style=\"padding:5px 0 0;\" width=\"200\">" + items.CategoryName + "</td>");
                        sb.Append("<td style=\"padding:5px 0 0;\" width=\"100\" align=\"right\" class=\"numeri-cell\"><span>" + Format.FormatPrice(items.Price.ToString()) + "</span></td></tr>");
                    }
                    sb.Append("<tr><td colspan=\"2\" style=\" border-bottom:1px solid #e2e2e2; padding:5px 0 0;\"></td></tr><tr>");
                    sb.Append("<td style=\"padding:5px 0 0;\" class=\"price2\">Total On Road Price</td><td style=\"padding:5px 0 0; font-weight:bold;\" width=\"100\" align=\"right\" class=\"price2 numeri-cell\">Rs. " + Format.FormatPrice(TotalPrice.ToString()) + "</td></tr>");
                    sb.Append("</tbody></table></div>");
                }
                sb.Append("<div style=\"display:inline-block; vertical-align:top; margin:0 10px; color:#333;\"><div style=\"text-align:center; width:170px; margin:0 0 15px; padding:10px 0; border:1px solid #e2e2e2; background:#c0ffa7;\">");
                sb.Append("<div style=\" font-size:14px; text-align:center;\">Paid Amount</div><div style=\" font-size:14px; font-weight:bold;\">Rs. " + Format.FormatPrice(BookingAmount.ToString()) + "</div></div>");
                sb.Append("<div style=\"text-align:center; width:170px; margin:0 0 10px; padding:10px 0; border:1px solid #e2e2e2; background:#ff9c69;\">");
                sb.Append("<div style=\" font-size:14px; text-align:center;\">Balance Amount</div><div style=\" font-size:14px; font-weight:bold;\">Rs. " + Format.FormatPrice(BalanceAmount.ToString()) + "</div></div>");
                sb.Append("</div></div><div style=\"background:url(http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/red-border.png) no-repeat center center; height:2px; width:100%\"></div>");

                if (OfferList != null && OfferList.Count > 0)
                {
                    sb.Append("<div style=\"background: none repeat scroll 0 0 #fef5e6;border: 2px dotted #f5b048; margin:10px 0; padding: 10px;\"><div style=\"font-size:14px; font-weight:bold;\">Exclusive BikeWale Offer</div>");

                    if (InsuranceAmount == 0)
                    {
                        foreach (var items in OfferList)
                        {
                            sb.Append("<p style=\"padding:0 10px 0 0; margin:10px 0 5px;\"><span style=\" background:url(http://img1.carwale.com/bikewaleimg/images/bikebooking/images/bw-grey-bullet.png) no-repeat center center; padding:0 20px 0 0;\"></span>" + items.OfferText + ".</p>");
                        }
                    }
                    else
                    {
                        sb.AppendFormat("<p style=\"padding:0 10px 0 0; margin:10px 0 5px;\"><span style=\" background:url(http://img1.carwale.com/bikewaleimg/images/bikebooking/images/bw-grey-bullet.png) no-repeat center center; padding:0 20px 0 0;\"></span>Free Insurance for 1 year worth Rs. {0} at the dealership.</p>", InsuranceAmount);
                    }
                    sb.Append("</div><div style=\"background:url(http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/red-border.png) no-repeat center center; height:2px; width:100%\"></div>");
                }
                sb.Append("<div style=\"padding:10px 0;\"><p style=\" margin:7px 0;\">Please let us know when customer makes further payment / takes delivery, and we will transfer the pre-booking amount to your bank account.</p>");
                sb.Append("<p style=\" margin:7px 0;\">Please feel free to call Rohit at 99203 13466 for any queries or help required in the process.</p></div>");
                sb.Append("<div style=\"padding:10px 0 0;\"><p style=\" margin:5px 0;\">Best Regards,</p><p style=\" margin:5px 0;font-weight:bold; margin:0;\">Team BikeWale</p></div>");
                sb.Append("</div></div><div style=\"background:url(http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/bottom-shadow.png) center center #eeeeee no-repeat; height:9px; width:100%\"></div></div>");

                HttpContext.Current.Trace.Warn("DealerEmail", sb.ToString());
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("Notifications.ErrorTempate ComposeBody : " + ex.Message);
            }
            return sb;
        }
    }
}
