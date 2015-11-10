using Bikewale.Entities.BikeBooking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Bikewale.Utility;

namespace Bikewale.Notifications.MailTemplates
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 19 Dec 2014
    /// Summary : To send Email to customer on payment success
    /// </summary>
    public class PreBookingConfirmationToCustomer : ComposeEmailBase
    {
        public string CustomerName { get; set; }
        public List<OfferEntity> OfferList { get; set; }
        public string BookingReferenceNo { get; set; }
        public uint PreBookingAmount { get; set; }
        public DateTime Date { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string DealerName { get; set; }
        public string DealerAddress { get; set; }
        public string DealerMobile { get; set; }
        public uint InsuranceAmount { get; set; }
        public string BikeName { get; set; }
        public PreBookingConfirmationToCustomer(string customerName, List<OfferEntity> offerList, string bookingReferenceNo, uint preBookingAmount, string makeName, string modelName, string dealerName, string dealerAddress, string dealerMobile, DateTime date, uint insuranceAmount = 0)
        {
            CustomerName = customerName;
            OfferList = offerList;
            BookingReferenceNo = bookingReferenceNo;
            PreBookingAmount = preBookingAmount;
            MakeName = makeName;
            ModelName = modelName;
            DealerName = dealerName;
            DealerMobile = dealerMobile;
            DealerAddress = dealerAddress;
            Date = date;
            InsuranceAmount = insuranceAmount;
        }

        public PreBookingConfirmationToCustomer(string customerName, List<OfferEntity> offerList, string bookingReferenceNo, uint preBookingAmount, string bikeName ,string makeName, string modelName, string dealerName, string dealerAddress, string dealerMobile, DateTime date, uint insuranceAmount = 0)
        {
            CustomerName = customerName;
            OfferList = offerList;
            BookingReferenceNo = bookingReferenceNo;
            PreBookingAmount = preBookingAmount;
            MakeName = makeName;
            ModelName = modelName;
            DealerName = dealerName;
            DealerMobile = dealerMobile;
            DealerAddress = dealerAddress;
            Date = date;
            InsuranceAmount = insuranceAmount;
            BikeName = bikeName;
        }

        public override string ComposeBody()
        {
            StringBuilder sb = null;

            try
            {
                sb = new StringBuilder();

                sb.Append("<div style=\"max-width:680px; margin:0 auto; border:1px solid #d8d8d8; font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#666666; background:#eeeeee;padding:10px 10px 10px 10px; word-wrap:break-word\">");
                sb.Append("<div style=\"margin:5px 0 0;\"><div style=\" background:#fff; padding:7px; border-top:7px solid #333333;\"><div style=\"float:left; margin-right:10px;\">");
                sb.Append("<a target=\"_blank\" href=\"http://www.bikewale.com/\"><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/bw-logo.png\" border=\"0\" alt=\"\" title=\"\"></a></div><div style=\" font-size:18px; font-weight:bold; float:left; margin:5px 0 0;\">Pre-Booking Confirmation</div>");
                sb.Append("<div style=\"float:right; color:#666; margin:5px 0 0;\">" + Date.ToString("MMM dd, yyyy") + "</div><div style=\"clear:both;\"></div></div><div style=\" background:#fff; padding:10px; margin:10px 0 0;\">");
                sb.Append("<div style=\"padding:10px 0;\"><p style=\" margin:0; font-size:14px; font-weight:bold; color:#333;\">Dear " + CustomerName + ",</p>");
                sb.Append("<p style=\"margin:10px 0 0;\">Thank you for making a payment of Rs. " + Format.FormatPrice(PreBookingAmount.ToString()) + " to pre-book the " + BikeName + ".</p>");
                sb.Append("<p style=\"margin:7px 0;\">Your BikeWale Pre-Booking Reference Number is <span style=\" font-size:14px; font-weight:bold;\">" + BookingReferenceNo + "</span>.</p>");
                sb.Append("<p style=\"margin:7px 0;\">You have just secured the following offers that come with purchase:</p></div>");

                if (InsuranceAmount > 0)
                {
                    sb.Append("<div style=\"background: none repeat scroll 0 0 #fef5e6;border: 2px dotted #f5b048; margin: 0 0 10px; padding: 10px;\"><div style=\"font-size:14px; font-weight:bold;\">Exclusive BikeWale Offer</div>");
                    sb.AppendFormat("<p style=\" margin:10px 0 5px;\"><span>Free Insurance for 1 year worth Rs. {0} at the dealership</p>", InsuranceAmount);
                    sb.Append("</div>");
                }
                else
                {
                    sb.Append("<div style=\"background: none repeat scroll 0 0 #fef5e6;border: 2px dotted #f5b048; margin: 0 0 10px; padding: 10px;\"><div style=\"font-size:14px; font-weight:bold;\">Exclusive BikeWale Offer</div>");
                    foreach (var item in OfferList)
                    {
                        sb.Append("<p style=\" margin:10px 0 5px;\"><span>" + item.OfferText + "</p>");
                    }
                    sb.Append("</div>");
                }
                sb.Append("<div style=\"padding:10px 0;\"><p style=\" margin:7px 0;\">If you are eligible for free Road Side Assistance (RSA) or free helmet offer, <a target=\"_blank\" href=\"http://www.bikewale.com/pricequote/rsaofferclaim.aspx\" style=\"text-decoration:none; color:#034fb6;\">click here</a> to claim your offer after bike delivery. </p></div>");
                sb.Append("<div style=\"background:url(http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/red-border.png) no-repeat center center; height:2px; width:100%;\"></div><div style=\" padding:10px 0;\">");
                sb.Append("<div style=\"font-size:14px; font-weight:bold; color:#333; margin-bottom:10px;\">Contact Details of the Assigned " + MakeName + " Dealership:</div><table cellpadding=\"0\" cellspacing=\"0\" style=\"color:#666;\">");
                sb.Append("<tbody><tr><td width=\"140\" style=\"font-weight:bold; color:#333; padding-bottom:5px; font-size:14px;\">Dealership Name:</td><td style=\"padding-bottom:5px; font-size:12px;\">" + DealerName + "</td>");
                sb.Append("</tr><tr><td style=\"font-weight:bold; color:#333; padding-bottom:5px; font-size:14px;\">Address:</td><td style=\"padding-bottom:5px;font-size:12px;\">" + DealerAddress + "</td></tr><tr>");
                sb.Append("<td style=\"font-weight:bold; color:#333; padding-bottom:5px; font-size:14px;\">Contact Number:</td><td style=\"padding-bottom:5px; font-size:12px;\">" + DealerMobile + "</td></tr></tbody></table>");
                sb.Append("</div><div style=\"background:url(http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/red-border.png) no-repeat center center; height:2px; width:100%\"></div><div style=\"padding:10px 0;\">");
                sb.Append("<p style=\" margin:7px 0;\">You would get a call from the dealership for next steps. Dealership will schedule your visit to the showroom to proceed with further buying process. The offer benefits will be shipped to you once you take delivery of the vehicle.</p>");
                sb.Append("<p style=\" margin:7px 0;\">Should you choose to cancel or change your pre-booking, please write us at <a style=\"text-decoration:none; color:#034fb6;\" href=\"mailto:contact@bikewale.com\">contact@bikewale.com</a>. Please mention the Pre-booking Reference Number and your mobile number in all correspondence. You can go through our <a target=\"_blank\" href=\"http://www.bikewale.com/pricequote/CancellationPolicy.aspx\" style=\"text-decoration:none; color:#034fb6;\">Cancellation Policy</a> and <a target=\"_blank\" href=\"http://www.bikewale.com/pricequote/faq.aspx\" style=\"text-decoration:none; color:#034fb6;\">FAQs</a> here.</p>");
                sb.Append("<p style=\" margin:7px 0;\">Wishing you a great buying experience!</p><p style=\" margin:7px 0;\">Please feel free to call us at 1800 120 8300 for any help required in the process.</p>");
                sb.Append("</div><div style=\"padding:10px 0 0;\"><p style=\" margin:5px 0;\">Best Regards,</p><p style=\" margin:5px 0;font-weight:bold; margin:0;\">Team BikeWale</p>");
                sb.Append("</div></div></div><div style=\"background:url(http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/bottom-shadow.png) center center #eeeeee no-repeat; height:9px; width:100%\"></div>");
                sb.Append("<div style=\"padding:5px 0px;width:100%\"><div style=\" width:110px; margin:0 auto\"><div style=\" float:left; padding-right:5px;\"><a href=\"https://twitter.com/Bikewale\" target=\"_blank\"><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/BW-RSA-t-icon.jpg\" alt=\"Twitter\" title=\"Twitter\" border=\"0\" /></a></div>");
                sb.Append("<div style=\"float:left; padding-right:5px;\"><a href=\"https://www.facebook.com/Bikewale.Official\" target=\"_blank\"><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/BW-RSA-f-icon.jpg\" alt=\"Facebook\" title=\"Facebook\" border=\"0\" /></a></div><div style=\"clear:both;\"></div>");
                sb.Append("</div><div style=\"clear:both;\"></div></div><div style=\"font-size:11px;\">This newsletter is to attempt to keep you updated with the latest launches. If you are not interested anymore, Please <a style=\"text-decoration:none; color:#034fb6;\" target=\"_blank\" href=\"http://www.bikewale.com/newsletter/unsubscribe.aspx\">unsubscribe here</a>.</div></div>");
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("Notifications.ErrorTempate ComposeBody : " + ex.Message);
            }
            return sb.ToString();
        }
    }
}
