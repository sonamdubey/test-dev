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
                m_AreaName = value;
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
                m_CityName = value;
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
                m_PriceList = value;
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
                m_InsuranceAmount = value;
            }
        }

        private string m_ImagePath;
        public string ImagePath {
            get 
            {
                return m_ImagePath;
            }
            set
            {
                m_ImagePath = value;
            }
        }


        public NewBikePriceQuoteMailToDealerTemplate(string makeName, string modelName, string dealerName, string customerName, string customerEmail, string customerMobile,
            string areaName, string cityName, List<PQ_Price> priceList, int totalPrice, List<OfferEntity> offerList, DateTime date, string imagePath, uint insuranceAmount = 0)
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
            m_ImagePath = imagePath;
            m_InsuranceAmount = insuranceAmount;

            discountList = OfferHelper.ReturnDiscountPriceList(offerList, priceList);
        }

        public override string ComposeBody()
        {
            StringBuilder sb = null;
            
            try
            {
                sb = new StringBuilder();

                sb.AppendFormat(
                    "<div style=\"max-width:692px; margin:0 auto; border:1px solid #8a9093; font-family: Arial, Helvetica, sans-serif; font-size:14px; color:#4d5057; background:#f5f5f5; word-wrap:break-word;\">"
                        + "<div style=\"padding:10px 20px; font-size:10px; border-bottom:3px solid #c20000;\"><a href=\"#\" target=\"_blank\" style=\"text-decoration:none; color:#2e67b2;\">Click here</a> to view in your browser.</div>"
                        + "<div style=\"padding:10px 20px; margin-bottom:20px;\">"
                            + "<div style=\"float:left; max-width:110px;\">"
                                + "<a href=\"#\" target=\"_blank\"><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/bw-logo.png\" alt=\"BikeWale\" title=\"BikeWale\" width=\"100%\" border=\"0\"/></a>"
                            + "</div>"
                            + "<div style=\"float:right; color:#82888b; line-height:32px;\"> {0} "
                            + "</div>"
                            + "<div style=\"clear:both;\"></div>"
                        + "</div>"

                        + "<div style=\"margin-bottom:25px; padding:0 20px;\">"
                            + "<div style=\"font-weight:bold; margin-bottom:20px;\">Dear&nbsp;{1},</div>"
                            + "<div style=\"margin-bottom:15px;\">Purchase Enquiry for {2}.</div>"
                            + "<div style=\"margin-bottom:15px;\">You have a new prospective buyer for {2}.</div>"
                            + "<div>Please check the details below:</div>"
                        + "</div>"

                        + "<div style=\"margin:0 10px 20px; padding:0 10px 10px; background:#fff;\"> <!-- customer details starts here -->"
                            + "<div style=\"color:#2a2a2a; font-weight:bold; padding:18px 0; margin-bottom:20px; border-bottom:1px solid #e2e2e2;\">Customer details</div>"
                            + "<div style=\"width:100%;\">"
                                + "<div style=\"width:48%; float:left; margin:7px 6px 7px 0;\">"
                                    + "<span style=\"color:#82888b;\">Name:</span>"
                                    + "<span> {3}</span>"
                                + "</div>"
                                + "<div style=\"width:49%; float:left; margin:7px 0;\">"
                                    + "<span style=\"color:#82888b;\">Email id:</span>"
                                    + "<span> {4}</span>"
                                + "</div>"
                                + "<div style=\"width:48%; float:left; margin:7px 6px 7px 0;\">"
                                    + "<span style=\"color:#82888b;\">Contact no:</span>"
                                    + "<span> {5}</span>"
                                + "</div>"
                                + "<div style=\"width:49%; line-height:1.4; float:left; margin:7px 0;\">"
                                    + "<span style=\"color:#82888b;\">Location:</span>"
                                    + "<span> {6}</span>"
                                + "</div>"
                                + "<div style=\"clear:both;\"></div>"
                            + "</div>"
                        + "</div>"
                        + "<div style=\"margin:0 10px 20px; background:#fff;\">"
                            + "<div style=\"color:#2a2a2a; font-weight:bold; padding:18px 0; margin:0 10px 20px; border-bottom:1px solid #e2e2e2;\">{2}</div>"
                            + "<div style=\"margin:25px 0 0; text-align:center;\"> <!-- bike details starts here -->"
                                + "<div style=\"display:inline-block; vertical-align:top; margin:0 20px 10px 10px;\">"
                                    + "<div style=\"width:192px; height:107px; margin-bottom:20px;\">"
                                        + "<img src=\"{7}\" alt=\"{2}\" title=\"{2}\" width=\"100%\" border=\"0\"/>"
                                    + "</div>"
                                + "</div>"
                                + "<div style=\"display:inline-block; vertical-align:top; max-width:428px; text-align:left; padding:0 10px;\">"
                    , Date.ToString("MMM dd, yyyy") , DealerName.Trim(), MakeName + " " + ModelName, CustomerName, CustomerEmail, CustomerMobile, AreaName+", "+ CityName, ImagePath);
                //Pricelist codediv
                if (PriceList != null && PriceList.Count > 0)
                {
                    foreach (var list in PriceList)
                    {
                        sb.AppendFormat(
                            "<div style=\"padding-bottom:20px;\">"
                                + "<div style=\"width:70%; float:left; color:#82888b;\">{0}</div>"
                                + "<div style=\"width:30%; float:left; text-align:right; font-weight:bold;\"><span><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/rupee-large.png\" alt=\"Rupee\" title=\"Rupee\" border=\"0\" /></span>{1}</div>"
                                + "<div style=\"clear:both;\"></div>"
                            + "</div>"
                            , list.CategoryName , Format.FormatPrice(list.Price.ToString()));
                    }
                    if (discountList != null && discountList.Count > 0)
                    {
                        sb.AppendFormat(
                            "<div style=\"padding:20px 0; border-top:1px solid #8a9093;\">"
                                + "<div style=\"width:70%; float:left; color:#82888b;\">Total on road price</div>"
                                + "<div style=\"width:30%; float:left; text-align:right; font-weight:bold;\"><span><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/rupee-large.png\" alt=\"Rupee\" title=\"Rupee\" border=\"0\" /></span><span style=\"text-decoration:line-through;\">{0}</span></div>"
                                + "<div style=\"clear:both;\"></div>"
                            + "</div>"
                            , Format.FormatPrice(TotalPrice.ToString())
                            );
                        foreach (var list in discountList) {
                            sb.AppendFormat(
                                "<div style=\"padding-bottom:20px;\">"
                                    +"<div style=\"width:70%; float:left; color:#82888b;\">{0}</div>"
                                    +"<div style=\"width:30%; float:left; text-align:right; font-weight:bold;\"><span><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/rupee-large.png\" alt=\"Rupee\" title=\"Rupee\" border=\"0\" /></span>{1}</div>"
                                    +"<div style=\"clear:both;\"></div>"
                                +"</div>"
                            , list.CategoryName, Format.FormatPrice(list.Price.ToString()));
                        }
                    }
                    sb.AppendFormat(
                        "<div>"
                            + "<div style=\"width:70%; float:left; font-weight:bold;\">Total on road price</div>"
                            + "<div style=\"width:30%; float:left; text-align:right; font-weight:bold;\"><span><img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/rupee-large.png\" alt=\"Rupee\" title=\"Rupee\" border=\"0\" /></span>{0}</div>"
                            + "<div style=\"clear:both;\"></div>"
                        + "</div>"
                        + "<div style=\"visibility:hidden;\"><!-- dummy data for acquiring width; start -->"
                            + "<div style=\"width:70%; float:left; color:#82888b;\">Ex-showroom price Ex-showroom price Ex-showroom</div>"
                            + "<div style=\"width:30%; float:left; text-align:right; font-weight:bold;\">xxxxxxx</div>"
                            + "<div style=\"clear:both;\"></div>"
                        + "</div><!-- dummy data for acquiring width; end -->"
                        , Format.FormatPrice(Convert.ToString(TotalPrice - TotalDiscountedPrice()))
                        );
                }

                sb.AppendFormat(
                                "</div>"
                            + "</div> <!-- bike details ends here -->"
                        + "</div>"
                        );
                        //offer section start here.

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
                        +"</div> <!-- bw offers ends here -->"
                        );
                }
                sb.AppendFormat(
                         "<div style=\"margin:0 10px 20px; padding:0 10px; background:#fff;\"> <!-- what's next starts here -->"
                            + "<div style=\"color:#2a2a2a; font-weight:bold; padding:18px 0; margin-bottom:20px; border-bottom:1px solid #e2e2e2;\">What's next!</div>"
                            + "<div style=\"margin-top:10px; line-height:1.4; font-weight:bold; text-align:center;\">"
                                + "<div style=\"max-width:170px; margin:10px 20px 20px; display:inline-block; vertical-align:top;\">"
                                    + "<div style=\"margin-bottom:10px;\">"
                                        + "<img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/bw-booking-step1.png\" alt=\"Get in touch with the customer to initiate booking\" title=\"Get in touch with the customer to initiate booking\" border=\"0\" style=\"width:140px; height:144px;\" />"
                                    + "</div>"
                                    + "<div>Get in touch with the customer to initiate booking</div>"
                                + "</div>"
                                + "<div style=\"max-width:170px; margin:10px 20px 20px; display:inline-block; vertical-align:top;\">"
                                    + "<div style=\"margin-bottom:10px;\">"
                                       + " <img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/bw-booking-step2.png\" alt=\"Complete formalities of Documents and Finance\" title=\"Complete formalities of Documents and Finance\" border=\"0\" style=\"width:140px; height:144px;\" />"
                                    + "</div>"
                                    + "<div>Complete formalities of Documents and Finance</div>"
                                + "</div>"
                               + " <div style=\"max-width:170px; margin:10px 20px 20px; display:inline-block; vertical-align:top;\">"
                                    + "<div style=\"margin-bottom:10px;\">"
                                        + "<img src=\"http://img1.carwale.com/bikewaleimg/images/bikebooking/mailer/bw-booking-step3.png\" alt=\"Deliver bike to customer along with offers claimed\" title=\"Deliver bike to customer along with offers claimed\" border=\"0\" style=\"width:140px; height:144px;\" />"
                                    + "</div>"
                                    + "<div>Deliver bike to customer along with offers claimed</div>"
                                + "</div>"
                            + "</div>"
                        + "</div> <!-- what's next ends here -->"

                        + "<div style=\"margin-bottom:2px; padding:0 20px; line-height:1.4; border-bottom:2px solid #c20000;\">"
                            + "<div style=\"margin-bottom:10px;\">Please let us know when customer makes further payment / takes delivery, and we will transfer the pre-booking amount to your bank account.</div>"
                            + "<div style=\"margin-bottom:25px;\">Please feel free to call Rohit at 99203 13466 for any queries or help required in the process.</div>"
                            + "<div style=\"margin-bottom:25px;\">Regards,<br />Team BikeWale</div>"
                        + "</div>"

                    + "</div>"
                    );
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
