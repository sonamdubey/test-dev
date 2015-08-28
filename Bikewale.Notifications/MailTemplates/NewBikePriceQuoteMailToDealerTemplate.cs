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
    /// Created By : Sadhana Upadhyay on 8 Nov 2014
    /// Summary : Template for new bike price quote dealer
    /// </summary>
    public class NewBikePriceQuoteMailToDealerTemplate : ComposeEmailBase
    {
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string DealerName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerEmail { get; set; }
        public string AreaName { get; set; }
        public string CityName { get; set; }
        public List<PQ_Price> PriceList { get; set; }
        public int TotalPrice { get; set; }
        public List<OfferEntity> OfferList { get; set; }
        public DateTime Date { get; set; }


        public NewBikePriceQuoteMailToDealerTemplate(string makeName,string modelName,string dealerName,string customerName,string customerEmail, string customerMobile,
            string areaName,string cityName,List<PQ_Price> priceList,int totalPrice,List<OfferEntity> offerList,DateTime date)
        {
            MakeName = makeName;
            ModelName = modelName;
            DealerName = dealerName;
            CustomerName = customerName;
            CustomerEmail = customerEmail;
            CustomerMobile = customerMobile;
            AreaName = areaName;
            CityName = cityName;
            AreaName = areaName;
            PriceList = priceList;
            TotalPrice = totalPrice;
            OfferList = offerList;
            Date = date;
        }

        public override StringBuilder ComposeBody()
        {
            StringBuilder sb = null;
            
            try
            {
                sb = new StringBuilder();

                sb.Append("<div style=\"max-width:670px; margin:0 auto; border:1px solid #d8d8d8; font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; background:#eeeeee;padding:10px; word-wrap:break-word\">");
                //sb.Append("<div style=\" font-size:11px; float:left;\"><a target=\"_blank\" href=\"#\" style=\"text-decoration:none; color:#034fb6;\">Click here</a> to view in your browser</div>");
	            sb.Append("<div style=\"clear:both;\"></div><div style=\"margin:5px 0 0; background:#fff;\"><div style=\"padding:7px; border-top:7px solid #333333;\">");
                sb.Append("<div style=\"float:left;\"><a target=\"_blank\" href=\"#\" style=\"text-decoration:none;\"><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/bw-logo.png\" border=\"0\" style=\"margin-right:20px;\"></a></div>");
	            sb.Append("<div style=\"float:left; margin-top:5px; font-size:22px; color:#333333;\">Purchase Enquiry for "+MakeName+" "+ModelName+"</div><div style=\"float:right; margin:10px 0 0;\">"+Date.ToString("MMM dd, yyyy")+"</div>");
                sb.Append("<div style=\"clear:both;\"></div></div><div style=\"background:url(http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/shadow.png) repeat-x #eeeeee; height:10px;\"></div><div style=\"padding:10px 10px 0;\">");
                sb.Append("<div style=\"font-size:14px; font-weight:bold; color:#333333;\">Dear " + DealerName + ",</div><div style=\" margin:20px 0 0; color:#666666;\">You have a new prospective buyer for " + MakeName + " " + ModelName + ", Please check the details below.</div>");
                sb.Append("<div style=\"background:url(http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/red-border.png) no-repeat; height:2px; margin:15px 0 0;\"></div></div><div style=\"padding:10px 10px 0;\"><div style=\"font-size:16px; font-weight:bold; color:#333333; margin-bottom:5PX;\">Buyer Details</div>");
            	sb.Append("<table style=\"overflow-X:scroll;\"><tbody><tr><td width=\"140px;\" style=\"font-weight:bold; color:#333333;\">Customer Name:</td><td style=\"color:#666666;\">"+CustomerName+"</td></tr>");
                sb.Append("<tr style=\"min-width:240px;\"><td width=\"140px;\" style=\"font-weight:bold; color:#333333;\">Locality:</td><td style=\"color:#666666;\">" + AreaName + ", " + CityName + "</td>");
                sb.Append("</tr><tr style=\"min-width:240px;\"><td width=\"140px;\" style=\"font-weight:bold; color:#333333;\">Mobile Number:</td><td style=\"color:#666666;\">"+CustomerMobile+"</td>");
                sb.Append("</tr><tr style=\"min-width:240px;\"><td width=\"140px;\" style=\"font-weight:bold; color:#333333;\">Email Id:</td><td style=\"color:#666666;\">"+CustomerEmail+"</td>");
                sb.Append("</tr></tbody></table><div style=\"background:url(http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/red-border.png) no-repeat; height:2px; margin:15px 0 0;\"></div></div><div style=\"padding:10px 10px 0\">");
                if (PriceList != null && PriceList.Count > 0)
                {
                    sb.Append("<div style=\"font-size:16px; font-weight:bold; color:#333333; margin-bottom:5px;\">Price-Quote <span style=\"font-weight:normal; font-size:14px;\">submitted to customer on your behalf</span></div>");
                    sb.Append("<div style=\"font-size:14px; font-weight:bold; color:#333333;\">On Road Proce Breakup</div><table><tbody>");
                    foreach (var list in PriceList)
                    {
                        sb.Append("<tr><td style=\"padding-top:5px;\" width=\"450px;\">" + list.CategoryName + "</td><td align=\"right\" style=\"padding-top:5px;\" width=\"150px\">" + Format.FormatPrice(list.Price.ToString()) + "</td></tr>");
                    }

                    sb.Append("<tr><td colspan=\"2\"><div style=\"border-top:1px solid #eeeeee; margin-top:10px;\"></div></td></tr>");
                    sb.Append("<tr><td style=\"padding-top:5px;\" width=\"450px;\">Total On Road price Quoted to Customer</p></td><td align=\"right\" style=\"padding-top:5px; font-weight:bold;\" width=\"150px;\"><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/rupee-large.png\" border=\"0\" style=\"margin-right:10px;\">" + Format.FormatPrice(TotalPrice.ToString()) + "</td></tr>");
                    sb.Append("</tbody></table>");
                }
                if (OfferList != null && OfferList.Count > 0)
                {
                    sb.Append("<div style=\"background:url(http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/red-border.png) no-repeat; height:2px; margin:15px 0 0;\"></div></div><div style=\"padding:10px 10px 0;\">");

                    sb.Append("<div style=\"font-size:14px; font-weight:bold; color:#333333;\">Applicable Offers for this purchase</div>");

                    foreach (var offer in OfferList)
                    {
                        sb.Append("<div style=\"margin-top:5px;\"><div style=\"float:left; margin-right:10px;\"><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/offers.png\" border=\"0\"></div><div style=\"padding-top:7px;\">" + offer.OfferText);
                        sb.Append("</div></div><div style=\"clear:both;\"></div>");
                    }
                }

                sb.Append("<div style=\"background:url(http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/red-border.png) no-repeat; height:2px; margin:15px 0 0;\"></div></div><div style=\"padding:10px 10px 0;\">");
                sb.Append("<div style=\"font-size:16px; font-weight:bold; color:#333; margin-bottom:10px;\"> Next Steps:</div><div style=\"padding-top:10px; text-align:center;\">");
                sb.Append("<div style=\"display:inline-block; max-width:160px; height:180px; border:2px solid #eeeeee; padding:20px 5px 10px; margin:0 5px 15px;text-align:center; vertical-align:top; position:relative;\">");
                sb.Append("<div><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/dealer-confirmation.png\" border=\"0\"></div><div style=\"font-size:12px; font-weight:bold; color:#333; margin:15px 0 24px;\">Call the Customer</div>");
                sb.Append("<div style=\"color:#666666; text-align:left;\">To increase the prospects of sale, please immediately call the customer &amp; schedule a visit to showroom.</div>");
                sb.Append("</div><div style=\"display:inline-block; max-width:160px; height:180px; border:2px solid #eeeeee; padding:20px 5px 10px; margin:0 5px 15px; text-align:center; vertical-align:top; position:relative;\">");
                sb.Append("<div><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/claim-price.png\" border=\"0\"></div><div style=\"font-size:12px; font-weight:bold; color:#333; margin:15px 0;\">Honour the Price Certificate</div>");
                sb.Append("<div style=\"color:#666666; text-align:left;\">Please honour the pricing &amp; extend the offers that we have committed to customer on your behalf.</div></div>");
                sb.Append("<div style=\"display:inline-block; max-width:160px; height:180px; border:2px solid #eeeeee; padding:20px 5px 10px; margin:0 5px 15px; text-align:center; vertical-align:top; position:relative;\">");
                sb.Append("<div><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/documentation.png\" border=\"0\"></div><div style=\"font-size:12px; font-weight:bold; color:#333; margin:15px 0 19px;\">Close the Sale</div>");
                sb.Append("<div style=\"color:#666666; text-align:left;\">Please collect the payment, required documents, get all the formalities done  &amp; deliver the vehicle.</div></div>");
                sb.Append("<div style=\"background:url(http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/red-border.png) no-repeat; height:2px; margin:15px 0 0;\"></div></div></div><div style=\"padding:10px;\">");
                sb.Append("<div style=\"color:#666666; padding-bottom:20px;\">Please feel free to call us at 9920313466 (Rohit Chauhan, Manager, BikeWale Sales) for any queries.</div>");
                sb.Append("<div style=\"color:#666666; padding-bottom:5px;\">Best Regards,</div><div style=\"color:#666666;\">Team BikeWale</div>");
                sb.Append("</div></div><div style=\"background:url(http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/bottom-shadow.png) center center no-repeat; height:6px;\"></div>");
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("Notifications.ErrorTempate ComposeBody : " + ex.Message);
            }
            return sb;
        }
    }
}
