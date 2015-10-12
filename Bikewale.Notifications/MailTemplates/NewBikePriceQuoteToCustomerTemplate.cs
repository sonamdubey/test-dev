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
    /// Created By : Sadhana Upadhyay on 9 Nov 2014
    /// Summary : Template for new bike price quote customer
    /// </summary>
    public class NewBikePriceQuoteToCustomerTemplate : ComposeEmailBase
    {
        public string BikeName { get; set; }
        public string BikeImage { get; set; }
        public string DealerName { get; set; }
        public string DealerEmailId { get; set; }
        public string DealerMobileNo { get; set; }
        public string PinCode { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string Organization { get; set; }
        public string Website { get; set; }
        public string Address { get; set; }
        public string CustomerName { get; set; }
        public List<PQ_Price> PriceList { get; set; }
        public uint TotalPrice { get; set; }
        public List<OfferEntity> OfferList { get; set; }
        public DateTime Date { get; set; }
        public string ModelId { get; set; }
        public string DealerId { get; set; }
        public uint InsuranceAmount { get; set; }



        public NewBikePriceQuoteToCustomerTemplate(string bikeName, string bikeImage, string dealerName, string dealerEmailId, string dealerMobileNo,
            string organization, string website, string address, string customerName, DateTime date, List<PQ_Price> priceList, List<OfferEntity> offerList, string pinCode, string stateName, string cityName, uint totalPrice, uint insuranceAmount)
        {
            BikeName = bikeName;
            BikeImage = bikeImage;
            DealerName = dealerName;
            DealerEmailId = DealerEmailId;
            DealerMobileNo = dealerMobileNo;

            Organization = organization;
            Website = website;
            Address = address;
            CustomerName = customerName;
            PriceList = priceList;
            TotalPrice = totalPrice;
            OfferList = offerList;
            Date = date;
            PinCode = pinCode;
            StateName = stateName;
            CityName = cityName;
            InsuranceAmount = insuranceAmount;
        }

        public override string ComposeBody()
        {
            StringBuilder sb = null;
            try
            {
                sb = new StringBuilder();
                sb.Append("<div style=\"max-width:670px; margin:0 auto; border:1px solid #d8d8d8; font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; background:#eeeeee;padding:10px; word-wrap:break-word\">");
                //sb.Append("<div style=\" font-size:11px; float:left;\"><a target=\"_blank\" href=\"#\" style=\"text-decoration:none; color:#034fb6;\">Click here</a> to view in your browser</div>");
                sb.Append("<div style=\"clear:both;\"></div><div style=\"margin:5px 0 0; background:#fff;\"><div style=\"padding:7px; border-top:7px solid #333333;\">");
                sb.Append("<div style=\"float:left;\"><a target=\"_blank\" href=\"http://www.bikewale.com/\" style=\"text-decoration:none;\"><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/bw-logo.png\" border=\"0\" style=\"margin-right:20px;\"></a></div>");
                sb.Append("<div style=\"float:left; margin-top:5px; font-size:22px; color:#333333;\">Dealer Price Certificate</div><div style=\"float:right; margin:10px 0 0;\">" + Date.ToString("MMM dd, yyyy") + "</div>");
                sb.Append("<div style=\"clear:both;\"></div></div><div style=\"background:url(http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/shadow.png) repeat-x #eeeeee; height:10px;\"></div>");//<div style=\"padding-bottom:10px;\">");
                //sb.Append("<img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/banner.jpg\" width=\"100%\" border=\"0\"></div><div style=\"padding:10px 10px 0;\"><div style=\"font-size:14px; font-weight:bold; color:#333333; margin-bottom:10px;\">Dear " + CustomerName + ",</div>");
                sb.Append("<div style=\"padding:10px 10px 0;\"><div style=\"font-size:14px; font-weight:bold; color:#333333; margin-bottom:10px;\">Dear " + CustomerName + ",</div>");
                sb.Append("<div style=\"color:#666666;\"><span style=\"font-size:14px; font-weight:bold; color:#333;\">Congratulations</span> on selecting the fantastic <span style=\"font-size:14px; font-weight:bold; color:#333;\">" + BikeName + "</span> and securing the guaranteed pricing for your purchase</div>");
                sb.Append("<div style=\"background:url(http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/red-border.png) no-repeat; height:2px; margin:15px 0 0;\"></div>");
                //sb.Append("</div><div style=\"padding:10px 10px 0;\"><div style=\"font-size:16px; font-weight:bold; color:#333; margin-bottom:10px;\">What Happens Next?</div><div style=\"padding-top:10px; text-align:center;\">");
                //sb.Append("<div style=\"display:inline-block; max-width:136px; height:200px; border:2px solid #eeeeee; padding:20px 5px 10px; margin:0 5px 15px;text-align:center; vertical-align:top; position:relative;\">");
                //sb.Append("<div><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/dealer-confirmation.png\" border=\"0\"></div><div style=\"font-size:12px; font-weight:bold; color:#333;margin:15px 0;\">Get in touch with Dealership</div>");
                //sb.Append("<div style=\"color:#666666; text-align:left;\">" + DealerName + " will get back to you and schedule your visit to the showroom. Alternatively, you can also call them to set-up a visit  at a convenient time.</div></div>");
                //sb.Append("<div style=\"display:inline-block; max-width:136px; height:200px; border:2px solid #eeeeee; padding:20px 5px 10px; margin:0 5px 15px; text-align:center; vertical-align:top; position:relative;\">");
                //sb.Append("<div><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/claim-price.png\" border=\"0\"></div><div style=\"font-size:12px; font-weight:bold; color:#333;margin:15px 0 26px;\">Claim your Price</div>");
                //sb.Append("<div style=\"color:#666666; text-align:left;\">Please present this price certificate to dealership to claim the price for your purchase.</div>");
                //sb.Append("</div><div style=\"display:inline-block; max-width:136px; height:200px; border:2px solid #eeeeee; padding:20px 5px 10px; margin:0 5px 15px; text-align:center; vertical-align:top; position:relative;\">");
                //sb.Append("<div><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/documentation.png\" border=\"0\"></div><div style=\"font-size:12px; font-weight:bold; color:#333;margin:15px 0;\">Be ready with Documentation</div>");
                //sb.Append("<div style=\"color:#666666; text-align:left;\">Please be ready with all the required documents and payment to avoid multiple visits and faster vehicle delivery.</div>");
                //sb.Append("</div><div style=\"display:inline-block; max-width:136px; height:200px; border:2px solid #eeeeee; padding:20px 5px 10px; margin:0 5px 15px; text-align:center; vertical-align:top; position:relative;\">");
                //sb.Append("<div><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/buy-your-bike.png\" border=\"0\"></div><div style=\"font-size:12px; font-weight:bold; color:#333;margin:15px 0 26px;\">Buy your Bike!</div>");
                //sb.Append("<div style=\"color:#666666; text-align:left;\">Dealer will help you in RTO formalities. Ride out from the dealership on your newly purchased " + BikeName + ".</div>");
                //sb.Append("</div><div style=\"background:url(http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/red-border.png) no-repeat; height:2px; margin:15px 0 0;\"></div></div></div>");

                if (PriceList != null && PriceList.Count > 0)
                {
                    sb.Append("<div style=\"padding:10px 10px 0;\">");
                    sb.Append("<div style=\"color:#666666;\">Following are the Price and dealership details for your brand new <span style=\"font-size:14px; font-weight:bold; color:#333;\">" + BikeName + "</span> </div>");
                    sb.Append("<div style=\"padding:10px 0; text-align:center;\"><div style=\" display:inline-block; max-width:190px; border:1px solid #e3e3e3; background:#f5f5f5; padding:5px;\">");
                    sb.Append("<img src=\"" + BikeImage + "\" width=\"100%\" border=\"0\"></div><div style=\"padding:10px 10px 10px 5px; display:inline-block; vertical-align:top;\">");
                    sb.Append("<div style=\"font-size:14px; font-weight:bold; color:#333; text-align:left;\">On Road Price Breakup</div><table><tbody>");
                    if (InsuranceAmount > 0)
                    {
                        foreach (var list in PriceList)
                        {
                            if (list.CategoryName.ToUpper().Contains("INSURANCE"))
                            {
                                sb.Append("<tr><td align=\"left\" width=\"340px\" style=\"color:#333; padding-top:5px;\">" + list.CategoryName + " Free" + "</td><td align=\"right\" style=\"color:#333;text-decoration: line-through;\">" + Format.FormatPrice(list.Price.ToString()) + "</td></tr>");
                            }
                            else
                            {
                                sb.Append("<tr><td align=\"left\" width=\"340px\" style=\"color:#333; padding-top:5px;\">" + list.CategoryName + "</td><td align=\"right\" style=\"color:#333;\">" + Format.FormatPrice(list.Price.ToString()) + "</td></tr>");
                            }
                        }
                        sb.Append("<tr><td colspan=\"2\"><div style=\"border-top:1px solid #eaeaea; margin-top:5px;\"></div></td></tr>");
                        sb.Append("<tr><td align=\"left\" width=\"340px\" style=\"color:#333; padding-top:5px;\">Total On Road Price</td><td align=\"right\" style=\"color:#333;\"><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/rupee-large.png\" border=\"0\" style=\"margin-right:5px;text-decoration: line-through;\">" + Format.FormatPrice(TotalPrice.ToString()) + "</td></tr>");
                        sb.Append("<tr><td align=\"left\" width=\"340px\" style=\"color:#333; padding-top:5px;\">Minus Insurance</td><td align=\"right\" style=\"color:#333;\"><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/rupee-large.png\" border=\"0\" style=\"margin-right:5px;\">" + Format.FormatPrice(InsuranceAmount.ToString()) + "</td></tr>");
                        sb.Append("<tr><td align=\"left\" width=\"340px\" style=\"color:#333; padding-top:5px;\">BikeWale On Road (after insurance offer)</td><td align=\"right\" style=\"color:#333; font-weight:bold;\"><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/rupee-large.png\" border=\"0\" style=\"margin-right:5px;\">" + Format.FormatPrice((TotalPrice - InsuranceAmount).ToString()) + "</td></tr>");
                        sb.Append("</tbody></table></div></div>");
                    }
                    else
                    {
                        foreach (var list in PriceList)
                        {
                            sb.Append("<tr><td align=\"left\" width=\"340px\" style=\"color:#333; padding-top:5px;\">" + list.CategoryName + "</td><td align=\"right\" style=\"color:#333;\">" + Format.FormatPrice(list.Price.ToString()) + "</td></tr>");
                        }
                        sb.Append("<tr><td colspan=\"2\"><div style=\"border-top:1px solid #eaeaea; margin-top:5px;\"></div></td></tr>");
                        sb.Append("<tr><td align=\"left\" width=\"340px\" style=\"color:#333; padding-top:5px;\">Total On Road Price</td><td align=\"right\" style=\"color:#333; font-weight:bold;\"><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/rupee-large.png\" border=\"0\" style=\"margin-right:5px;\">" + Format.FormatPrice(TotalPrice.ToString()) + "</td></tr>");
                        sb.Append("</tbody></table></div></div>");
                    }
                }
                if (OfferList != null && OfferList.Count > 0)
                {
                    sb.Append("<div style=\"padding:10px 0 0;\"><div style=\"font-size:14px; font-weight:bold; color:#333333;\">Exciting Offers for BikeWale Customers</div>");

                    if (InsuranceAmount == 0)
                    {
                        foreach (var offer in OfferList)
                        {
                            sb.Append("<div style=\"margin-top:5px;\"><div style=\"float:left; margin-right:10px;\"><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/offers.png\" border=\"0\"></div><div style=\"padding-top:7px;\">" + offer.OfferText);
                            sb.Append("</div></div><div style=\"clear:both;\"></div>");
                        }
                    }
                    else
                    {
                        sb.Append("<div style=\"margin-top:5px;\"><div style=\"float:left; margin-right:10px;\"><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/offers.png\" border=\"0\"></div><div style=\"padding-top:7px;\">" + "Book Your Bike at BikeWale and Get Insurance Absolutely Free at the Dealership");
                        sb.Append("</div></div><div style=\"clear:both;\"></div>");
                    }
                }
                else
                {
                    sb.Append("</div><div><div>");
                }
                sb.Append("<div style=\"background:url(http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/red-border.png) no-repeat; height:2px; margin:15px 0 0;\"></div></div></div><div style=\"padding:10px 10px 0;\">");
                sb.Append("<div style=\"font-size:16px; color:#333; font-weight:bold;\">Authorised Dealer Details</div><div style=\"font-size:14px; font-weight:bold; color:#333; margin-top:10px;\"><span style=\"margin-right:10px;\"><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/dealer.png\"></span>" + Organization + "</div>");
                sb.Append("<div style=\"margin:5px 0 0 20px; max-width:430px;\">" + Address + ", " + CityName + ", " + StateName + "-" + PinCode + "</div>");
                //sb.Append("<div style=\"margin-left:20px;\"><a target=\"_blank\" href=\"#\" style=\"text-decoration:none; color:#034fb6;\">Locate on Map</a></div>");
                sb.Append("<div style=\"margin-top:20px;\"><span style=\"margin-right:10px;\"><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/call-icon.png\"></span>" + DealerMobileNo + "</div>");
                sb.Append("<div style=\"background:url(http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/red-border.png) no-repeat; height:2px; margin:15px 0 0;\"></div></div><div style=\"padding:10px 10px 15px;\">");
                sb.Append("<div style=\"padding:10px 5px 0;\"><div style=\"font-size:16px; font-weight:bold; color:#333; margin-bottom:10px;\">What Happens Next?</div><div style=\"padding-top:10px; text-align:center;\">");
                sb.Append("<div style=\"display:inline-block; max-width:136px; height:200px; border:2px solid #eeeeee; padding:20px 5px 10px; margin:0px 2px 15px;text-align:center; vertical-align:top; position:relative;\">");
                sb.Append("<div><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/dealer-confirmation.png\" border=\"0\"></div><div style=\"font-size:12px; font-weight:bold; color:#333;margin:15px 0;\">Get in touch with Dealership</div>");
                sb.Append("<div style=\"color:#666666; text-align:left;\">" + Organization + " will get back to you and schedule your visit to the showroom. Alternatively, you can also call them to set-up a visit  at a convenient time.</div></div>");
                sb.Append("<div style=\"display:inline-block; max-width:136px; height:200px; border:2px solid #eeeeee; padding:20px 5px 10px; margin:0px 2px 15px; text-align:center; vertical-align:top; position:relative;\">");
                sb.Append("<div><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/claim-price.png\" border=\"0\"></div><div style=\"font-size:12px; font-weight:bold; color:#333;margin:15px 0 26px;\">Claim your Price</div>");
                sb.Append("<div style=\"color:#666666; text-align:left;\">Please present this price certificate to dealership to claim the price for your purchase.</div>");
                sb.Append("</div><div style=\"display:inline-block; max-width:136px; height:200px; border:2px solid #eeeeee; padding:20px 5px 10px; margin:0px 2px 15px; text-align:center; vertical-align:top; position:relative;\">");
                sb.Append("<div><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/documentation.png\" border=\"0\"></div><div style=\"font-size:12px; font-weight:bold; color:#333;margin:15px 0;\">Be ready with Documentation</div>");
                sb.Append("<div style=\"color:#666666; text-align:left;\">Please be ready with all the required documents and payment to avoid multiple visits and faster vehicle delivery.</div>");
                sb.Append("</div><div style=\"display:inline-block; max-width:136px; height:200px; border:2px solid #eeeeee; padding:20px 5px 10px; margin:0px 2px 15px; text-align:center; vertical-align:top; position:relative;\">");
                sb.Append("<div><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/buy-your-bike.png\" border=\"0\"></div><div style=\"font-size:12px; font-weight:bold; color:#333;margin:15px 0 26px;\">Buy your Bike!</div>");
                sb.Append("<div style=\"color:#666666; text-align:left;\">Dealer will help you in RTO formalities. Ride out from the dealership on your newly purchased " + BikeName + ".</div>");
                sb.Append("</div><div style=\"background:url(http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/red-border.png) no-repeat; height:2px; margin:15px 0 0;\"></div></div></div>");
                sb.Append("<div style=\"margin-bottom:15px;\">We wish you a great buying experience!</div><div style=\"margin-bottom:15px;\">Please feel free to call us at 022-67398888 for any help required in the process.</div>");
                sb.Append("<div style=\"margin-bottom:2px;\">Best Regards</div><div>Team BikeWale</div></div></div><div style=\"background:url(http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/bottom-shadow.png) center center no-repeat; height:6px;\"></div>");
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("Notifications.ErrorTempate ComposeBody : " + ex.Message);
            }

            HttpContext.Current.Trace.Warn(sb.ToString());
            return sb.ToString();
        }
    }
}
