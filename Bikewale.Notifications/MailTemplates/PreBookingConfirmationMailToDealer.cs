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
        public List<PQ_Price> DiscountList { get; set; }

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
            DiscountList = OfferHelper.ReturnDiscountPriceList(offerList, priceList);
        }

        public override string ComposeBody()
        {
            StringBuilder sb = null;

            try
            {
                sb = new StringBuilder();
                sb.AppendFormat(
                    "<div style=\"max-width:692px; margin:0 auto; border:1px solid #4d5057; font-family: Arial, Helvetica, sans-serif; font-size:14px; color:#4d5057; background:#f5f5f5; word-wrap:break-word;\">"
	                    +"<div style=\"padding:10px 20px; font-size:10px; border-bottom:3px solid #c20000;\"><a href=\"#\" target=\"_blank\" style=\"text-decoration:none; color:#2e67b2;\">Click here</a> to view in your browser.</div>"
                        +"<div style=\"padding:10px 20px; margin-bottom:20px;\">"
    	                    +"<div style=\"float:left; max-width:110px;\">"
                                + "<a href=\"#\" target=\"_blank\"><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/bw-logo.png\" alt=\"BikeWale\" title=\"BikeWale\" width=\"100%\" border=\"0\"/></a>"
                            +"</div>"
                            +"<div style=\"float:right; color:#82888b; line-height:32px;\">{0}"
                            +"</div>"
                            +"<div style=\"clear:both;\"></div>"
                        +"</div>"
    
	                    +"<div style=\"padding:0 20px;\">"
                            +"<div style=\"font-weight:bold; margin-bottom:20px;\">Dear {1},</div>"
                            +"<div style=\"margin-bottom:15px;\">Pre-Booking for {2} {3}</div>"
                            + "<div style=\"line-height:1.4; margin-bottom:15px;\">Please call customer {6} ASAP and proceed with further selling process, Customer has paid <span><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/rupee-large.png\" alt=\"Rupee\" title=\"Rupee\" border=\"0\" /></span><span style=\"color:#4d5057; font-weight:bold;\">{4}</span> to pre-book {2} {3}, with Pre-booking Ref Number<span style=\"color:#4d5057; font-weight:bold;\">&nbsp;{5}</span>. Check below for more details:</div>"
                        +"</div>"
    
                        +"<div style=\"text-align:center; margin-bottom:20px;\">"
    	                    +"<div style=\"max-width:320px; min-height:203px; display:inline-block; vertical-align:top; background:#fff; margin:0 10px;\">"
        	                    +"<div style=\"padding:0 10px; text-align:left;\">"
            	                    +"<div style=\"color:#2a2a2a; font-weight:bold; padding:18px 0; margin-bottom:30px; border-bottom:1px solid #e2e2e2;\">Customer details</div>"
                                    +"<div style=\"padding-bottom:15px;\">"
                	                    +"<span style=\"color:#82888b;\">Name:</span>"
                                        +"<span> {6}</span>"
                                    +"</div>"
                                    +"<div style=\"padding-bottom:15px;\">"
                	                    +"<span style=\"color:#82888b;\">Contact no:</span>"
                                        +"<span> {7}</span>"
                                    +"</div>"
                                    +"<div style=\"padding-bottom:15px;\">"
                	                    +"<span style=\"color:#82888b;\">Email id:</span>"
                                        +"<span> {8}</span>"
                                    +"</div>"
                                    +"<div style=\"padding-bottom:15px; line-height:1.4;\">"
                	                    +"<span style=\"color:#82888b;\">Location:</span>"
                                        +"<span> {9} {10}</span>"
                                    +"</div>"
                                +"</div>"
                            +"</div>"
                            +"<div style=\"max-width:320px; min-height:203px; display:inline-block; vertical-align:top; background:#fff; margin:0 10px;\">"
        	                    +"<div style=\"padding:0 10px; text-align:left;\">"
            	                    +"<div style=\"color:#2a2a2a; font-weight:bold; padding:18px 0; margin-bottom:20px; border-bottom:1px solid #e2e2e2;\">Payment details</div>"
                                    +"<div style=\"display:table; text-align:center; padding-bottom:20px;\">"
                                        +"<div style=\"width:120px; height:110px; display:table-cell; vertical-align:middle; padding-left:10px; padding-right:20px; border-right:1px solid #e2e2e2;\">"
                                            +"<div style=\"margin-bottom:10px;\">Advance Payment</div>"
                                            + "<div><span><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/rupee-large.png\" alt=\"Rupee\" border=\"0\"/></span><span style=\"font-size:20px; font-weight:bold; color:#1a1a1a;\">{4}</span></div>"
                                        +"</div>"
                                        +"<div style=\"width:120px; height:110px; display:table-cell; vertical-align:middle; padding-left:20px; padding-right:10px;\">"
                                            +"<div style=\"margin-bottom:10px;\">Balance Payment</div>"
                                            + "<div><span><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/rupee-large.png\" alt=\"Rupee\" border=\"0\"/></span><span style=\"font-size:20px; font-weight:bold; color:#1a1a1a;\">{11}</div>"
                                        +"</div>"
                                    +"</div>"
                                +"</div>"
                            +"</div>"
                        +"</div>"
                    , DateTime.Now.ToString("MMM dd, yyyy"), DealerName, BikeName, string.Empty, Format.FormatPrice(BookingAmount.ToString()), BookingReferenceNo.Trim(), CustomerName, CustomerMobile, CustomerEmail, CustomerArea, string.Empty, Format.FormatPrice((BalanceAmount - TotalDiscountedPrice()).ToString()));
                //Offer Text 
                if (OfferList != null && OfferList.Count > 0)
                {
                    sb.AppendFormat(
                        "<div style=\"margin:0 10px 20px; padding:0 10px; background:#fff;\"> <!-- bw offers starts here -->"
                            + "<div style=\"color:#2a2a2a; font-weight:bold; padding:18px 0; margin-bottom:20px; border-bottom:1px solid #e2e2e2;\">Offers by BikeWale</div>"
                            + "<ul style=\"margin-left:15px; padding:0;\">"
                        );
                    foreach (var offer in OfferList)
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
                            + "<div style=\"margin-bottom:10px;\">Please let us know when customer makes further payment / takes delivery, and we will transfer the pre-booking amount to your bank account.</div>"
                            + "<div style=\"margin-bottom:25px;\">Please feel free to call Rohit at 99203 13466 for any queries or help required in the process.</div>"
                            + "<div style=\"margin-bottom:25px;\">Regards,<br />Team BikeWale</div>"
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
            return sb.ToString();
        }

        /// <summary>
        /// Creted By : Lucky Rathore
        /// Created on : 08 January 2016
        /// </summary>
        /// <returns>Total dicount on specific Version.</returns>
        protected UInt32 TotalDiscountedPrice()
        {
            UInt32 totalPrice = 0;

            if (DiscountList != null && DiscountList.Count > 0)
            {
                foreach (var priceListObj in DiscountList)
                {
                    totalPrice += priceListObj.Price;
                }
            }

            return totalPrice;
        }
    }
}

