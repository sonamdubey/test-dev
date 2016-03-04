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
    public class NewBikePriceQuoteMailToDealerTemplate_old : ComposeEmailBase
    {
      private string m_MakeName;
      public string MakeName
      {
        get 
        {
          return m_MakeName; 
        }
        set
        {
          m_MakeName = value;
        }
      }

      private string m_ModelName;
      public string ModelName
      {
        get
        {
          return m_ModelName;
        }
        set
        {
          m_ModelName = value;
        }
      }

      private string m_DealerName;
        public string DealerName
      {
        get
        {
          return m_DealerName;
        }
        set
        {
          m_DealerName = value;
        }
      }

        private string m_CustomerName;
        public string CustomerName
        {
          get
          {
            return m_CustomerName;
          }
          set
          {
            m_CustomerName = value;
          }
        }

        private string m_CustomerMobile;
        public string CustomerMobile
        {
          get
          {
            return m_CustomerMobile;
          }
          set
          {
            m_CustomerMobile = value;
          }
        }

        private string m_CustomerEmail;
        public string CustomerEmail
        {
          get
          {
            return m_CustomerEmail;
          }
          set
          {
            m_CustomerEmail = value;
          }
        }

        private string m_AreaName;
        public string AreaName
        {
          get
          {
            return m_AreaName;
          }
          set
          {
            m_AreaName= value;
          }
        }

        private string m_CityName;
        public string CityName
        {
          get
          {
            return m_CityName;
          }
          set
          {
            m_CityName=value;
          }
        }

        private List<PQ_Price> m_PriceList;
        public List<PQ_Price> PriceList
        {
          get
          {
            return m_PriceList;
          }
          set
          {
            m_PriceList=value;
          }
        }
        private List<PQ_Price> discountList;

        private int m_TotalPrice;
        public int TotalPrice
        {
          get
          {
            return m_TotalPrice;
          }
          set
          {
            m_TotalPrice = value;
          }
        }

        private List<OfferEntity> m_OfferList;
        public List<OfferEntity> OfferList
        {
          get
          {
            return m_OfferList;
          }
          set
          {
            m_OfferList = value;
          }
        }

        private DateTime m_Date;
        public DateTime Date
        {
          get
          {
            return m_Date;
          }
          set
          {
            m_Date = value;
          }
        }

        private uint m_InsuranceAmount;
        public uint InsuranceAmount
        {
          get
          {
            return m_InsuranceAmount;
          }
          set
          {
            m_InsuranceAmount=value;
          }
        }

        public NewBikePriceQuoteMailToDealerTemplate_old(string makeName, string modelName, string dealerName, string customerName, string customerEmail, string customerMobile,
            string areaName, string cityName, List<PQ_Price> priceList, int totalPrice, List<OfferEntity> offerList, DateTime date, uint insuranceAmount = 0)
        {
            m_MakeName = makeName;
            m_ModelName = modelName;
            m_DealerName = dealerName;
            m_CustomerName = customerName;
            m_CustomerEmail = customerEmail;
            m_CustomerMobile = customerMobile;
            m_AreaName = areaName;
            m_CityName = cityName;
            m_AreaName = areaName;
            m_PriceList = priceList;
            m_TotalPrice = totalPrice;
            m_OfferList = offerList;
            m_Date = date;
            m_InsuranceAmount = insuranceAmount;
            discountList = OfferHelper.ReturnDiscountPriceList(offerList, priceList);
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
                sb.Append("<div style=\"float:left;\"><a target=\"_blank\" href=\"#\" style=\"text-decoration:none;\"><img src=\"http://imgd1.aeplcdn.com/0x0/bw/static/design15/mailer-images/bw-logo.png\" border=\"0\" style=\"margin-right:20px;\"></a></div>");
                sb.Append("<div style=\"float:left; margin-top:5px; font-size:22px; color:#333333;\">Purchase Enquiry for " + MakeName + " " + ModelName + "</div><div style=\"float:right; margin:10px 0 0;\">" + Date.ToString("MMM dd, yyyy") + "</div>");
                sb.Append("<div style=\"clear:both;\"></div></div><div style=\"background:url(http://imgd2.aeplcdn.com/0x0/bw/static/design15/mailer-images/shadow.png) repeat-x #eeeeee; height:10px;\"></div><div style=\"padding:10px 10px 0;\">");
                sb.Append("<div style=\"font-size:14px; font-weight:bold; color:#333333;\">Dear " + DealerName + ",</div><div style=\" margin:20px 0 0; color:#666666;\">You have a new prospective buyer for " + MakeName + " " + ModelName + ", Please check the details below.</div>");
                sb.Append("<div style=\"background:url(http://imgd4.aeplcdn.com/0x0/bw/static/design15/mailer-images/red-border.png) no-repeat; height:2px; margin:15px 0 0;\"></div></div><div style=\"padding:10px 10px 0;\"><div style=\"font-size:16px; font-weight:bold; color:#333333; margin-bottom:5PX;\">Buyer Details</div>");
                sb.Append("<table style=\"overflow-X:scroll;\"><tbody><tr><td width=\"140px;\" style=\"font-weight:bold; color:#333333;\">Customer Name:</td><td style=\"color:#666666;\">" + CustomerName + "</td></tr>");
                sb.Append("<tr style=\"min-width:240px;\"><td width=\"140px;\" style=\"font-weight:bold; color:#333333;\">Locality:</td><td style=\"color:#666666;\">" + AreaName + ", " + CityName + "</td>");
                sb.Append("</tr><tr style=\"min-width:240px;\"><td width=\"140px;\" style=\"font-weight:bold; color:#333333;\">Mobile Number:</td><td style=\"color:#666666;\">" + CustomerMobile + "</td>");
                sb.Append("</tr><tr style=\"min-width:240px;\"><td width=\"140px;\" style=\"font-weight:bold; color:#333333;\">Email Id:</td><td style=\"color:#666666;\">" + CustomerEmail + "</td>");
                sb.Append("</tr></tbody></table><div style=\"background:url(http://imgd2.aeplcdn.com/0x0/bw/static/design15/mailer-images/red-border.png) no-repeat; height:2px; margin:15px 0 0;\"></div></div><div style=\"padding:10px 10px 0\">");

                if (PriceList != null && PriceList.Count > 0)
                {
                    sb.Append("<div style=\"font-size:16px; font-weight:bold; color:#333333; margin-bottom:5px;\">Price-Quote <span style=\"font-weight:normal; font-size:14px;\">submitted to customer on your behalf</span></div>");
                    sb.Append("<div style=\"font-size:14px; font-weight:bold; color:#333333;\">On Road Price Breakup</div><table><tbody>");
                    if (discountList != null && discountList.Count > 0)
                    {
                        foreach (var list in PriceList)
                        {
                            sb.Append("<tr><td style=\"padding-top:5px;\" width=\"450px;\">" + list.CategoryName + "</td><td align=\"right\" style=\"padding-top:5px;\" width=\"150px\">" + Format.FormatPrice(list.Price.ToString()) + "</td></tr>");
                        }
                        sb.Append("<tr><td colspan=\"2\"><div style=\"border-top:1px solid #eeeeee; margin-top:10px;\"></div></td></tr>");
                        sb.Append("<tr><td style=\"padding-top:5px;\" width=\"450px;\">Total On Road price</p></td><td align=\"right\" style=\"padding-top:5px; font-weight:bold;\" width=\"150px;\"><img src=\"http://imgd1.aeplcdn.com/0x0/bw/static/design15/mailer-images/rupee-large.png\" border=\"0\" style=\"margin-right:10px;text-decoration: line-through;\">" + Format.FormatPrice(TotalPrice.ToString()) + "</td></tr>");
                        foreach (var list in discountList)
                        {
                            sb.Append("<tr><td style=\"padding-top:5px;\" width=\"450px;\">Minus " + list.CategoryName + "</td><td align=\"right\" style=\"padding-top:5px;\" width=\"150px\">" + Format.FormatPrice(list.Price.ToString()) + "</td></tr>");
                        }
                        sb.Append("<tr><td colspan=\"2\"><div style=\"border-top:1px solid #eeeeee; margin-top:10px;\"></div></td></tr>");
                        sb.Append("<div style=\"background:url(http://imgd1.aeplcdn.com/0x0/bw/static/design15/mailer-images/red-border.png) no-repeat; height:2px; margin:15px 0 0;\"></div></div><div style=\"padding:10px 10px 0;\"><div style=\"font-size:16px; font-weight:bold; color:#333333; margin-bottom:5PX;\">Buyer Details</div>");
                        sb.Append("<tr><td style=\"padding-top:5px;\" width=\"450px;\">Total On Road price Quoted to Customer</p></td><td align=\"right\" style=\"padding-top:5px; font-weight:bold;\" width=\"150px;\"><img src=\"http://imgd3.aeplcdn.com/0x0/bw/static/design15/mailer-images/rupee-large.png\" border=\"0\" style=\"margin-right:10px;\">" + Format.FormatPrice(Convert.ToString(TotalPrice - TotalDiscountedPrice())) + "</td></tr>");
                        sb.Append("</tbody></table>");
                    }
                    else
                    {
                        foreach (var list in PriceList)
                        {
                            sb.Append("<tr><td style=\"padding-top:5px;\" width=\"450px;\">" + list.CategoryName + "</td><td align=\"right\" style=\"padding-top:5px;\" width=\"150px\">" + Format.FormatPrice(list.Price.ToString()) + "</td></tr>");
                        }

                        sb.Append("<tr><td colspan=\"2\"><div style=\"border-top:1px solid #eeeeee; margin-top:10px;\"></div></td></tr>");
                        sb.Append("<tr><td style=\"padding-top:5px;\" width=\"450px;\">Total On Road price</p></td><td align=\"right\" style=\"padding-top:5px; font-weight:bold;\" width=\"150px;\"><img src=\"http://imgd1.aeplcdn.com/0x0/bw/static/design15/mailer-images/rupee-large.png\" border=\"0\" style=\"margin-right:10px;\">" + Format.FormatPrice(TotalPrice.ToString()) + "</td></tr>");
                        sb.Append("</tbody></table>");
                    }
                }
                if (OfferList != null && OfferList.Count > 0 )
                {
                    sb.Append("<div style=\"background:url(http://imgd2.aeplcdn.com/0x0/bw/static/design15/mailer-images/red-border.png) no-repeat; height:2px; margin:15px 0 0;\"></div></div><div style=\"padding:10px 10px 0;\">");

                    sb.Append("<div style=\"font-size:14px; font-weight:bold; color:#333333;\">Applicable Offers for this purchase</div>");

                    foreach (var offer in OfferList)
                    {
                        sb.Append("<div style=\"margin-top:5px;\"><div style=\"float:left; margin-right:10px;\"><img src=\"http://imgd1.aeplcdn.com/0x0/bw/static/design15/mailer-images/offers.png\" border=\"0\"></div><div style=\"padding-top:7px;\">" + offer.OfferText);
                        sb.Append("</div></div><div style=\"clear:both;\"></div>");
                    }
                }

                sb.Append("<div style=\"background:url(http://imgd3.aeplcdn.com/0x0/bw/static/design15/mailer-images/red-border.png) no-repeat; height:2px; margin:15px 0 0;\"></div></div><div style=\"padding:10px 10px 0;\">");
                sb.Append("<div style=\"font-size:16px; font-weight:bold; color:#333; margin-bottom:10px;\"> Next Steps:</div><div style=\"padding-top:10px; text-align:center;\">");
                sb.Append("<div style=\"display:inline-block; max-width:160px; height:180px; border:2px solid #eeeeee; padding:20px 5px 10px; margin:0 5px 15px;text-align:center; vertical-align:top; position:relative;\">");
                sb.Append("<div><img src=\"http://imgd4.aeplcdn.com/0x0/bw/static/design15/mailer-images/dealer-confirmation.png\" border=\"0\"></div><div style=\"font-size:12px; font-weight:bold; color:#333; margin:15px 0 24px;\">Call the Customer</div>");
                sb.Append("<div style=\"color:#666666; text-align:left;\">To increase the prospects of sale, please immediately call the customer &amp; schedule a visit to showroom.</div>");
                sb.Append("</div><div style=\"display:inline-block; max-width:160px; height:180px; border:2px solid #eeeeee; padding:20px 5px 10px; margin:0 5px 15px; text-align:center; vertical-align:top; position:relative;\">");
                sb.Append("<div><img src=\"http://imgd1.aeplcdn.com/0x0/bw/static/design15/mailer-images/claim-price.png\" border=\"0\"></div><div style=\"font-size:12px; font-weight:bold; color:#333; margin:15px 0;\">Honour the Price Certificate</div>");
                sb.Append("<div style=\"color:#666666; text-align:left;\">Please honour the pricing &amp; extend the offers that we have committed to customer on your behalf.</div></div>");
                sb.Append("<div style=\"display:inline-block; max-width:160px; height:180px; border:2px solid #eeeeee; padding:20px 5px 10px; margin:0 5px 15px; text-align:center; vertical-align:top; position:relative;\">");
                sb.Append("<div><img src=\"http://imgd2.aeplcdn.com/0x0/bw/static/design15/mailer-images/documentation.png\" border=\"0\"></div><div style=\"font-size:12px; font-weight:bold; color:#333; margin:15px 0 19px;\">Close the Sale</div>");
                sb.Append("<div style=\"color:#666666; text-align:left;\">Please collect the payment, required documents, get all the formalities done  &amp; deliver the vehicle.</div></div>");
                sb.Append("<div style=\"background:url(http://imgd3.aeplcdn.com/0x0/bw/static/design15/mailer-images/red-border.png) no-repeat; height:2px; margin:15px 0 0;\"></div></div></div><div style=\"padding:10px;\">");
                sb.Append("<div style=\"color:#666666; padding-bottom:20px;\">Please feel free to call us at 8828305054 for any queries.</div>");
                sb.Append("<div style=\"color:#666666; padding-bottom:5px;\">Best Regards,</div><div style=\"color:#666666;\">Team BikeWale</div>");
                sb.Append("</div><div style=\"margin-top:20px; margin-bottom:10px; max-width:670px;\"><a href=\"https://play.google.com/store/apps/details?id=com.bikewale.app&utm_source=PQMailer&utm_medium=email&utm_campaign=DealerPQMail\" target=\"_blank\"><img src=\"http://imgd1.aeplcdn.com/0x0/bw/static/design15/mailer-images/bw-footer-banner.jpg\" style=\"border:0; width:100%\"></a>");
                sb.Append("</div></div></div><div style=\"background:url(http://imgd4.aeplcdn.com/0x0/bw/static/design15/mailer-images/bottom-shadow.png) center center no-repeat; height:6px;\"></div>");
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.Notification.NewBikePriceQuoteMailToDealerTemplate.ComposeBody");
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
