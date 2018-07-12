using Bikewale.Entities.BikeBooking;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bikewale.Notifications.MailTemplates
{
    /// <summary>
    /// Created By : Lucky Rathore on 20 Jan 2016.
    /// Summary : To send pre-booking email to dealer.
    /// Modified By : Vivek Gupta on 11-5-2016
    /// Desc : Mail format (html has been revamped/changed)
    /// </summary>
    public class PreBookingConfirmationMailToDealer : ComposeEmailBase
    {
        private string MailHTML = null;

        public PreBookingConfirmationMailToDealer(string customerName, string customerMobile, string customerArea, string customerEmail, uint totalPrice, uint bookingAmount,
            uint balanceAmount, List<PQ_Price> priceList, string bookingReferenceNo, string bikeName, string bikeColor, string dealerName, List<OfferEntity> offerList, string imagePath, string versionName, uint insuranceAmount = 0)
        {
            List<PQ_Price> discountList = OfferHelper.ReturnDiscountPriceList(offerList, priceList);
            StringBuilder sb = null;
            try
            {
                sb = new StringBuilder();

                sb.AppendFormat("<div style=\"max-width:692px; margin:0 auto; border:1px solid #f5f5f5; font-family: Arial, Helvetica, sans-serif; font-size:14px; color:#4d5057; background:#ffffff; word-wrap:break-word;\">"
                + "<div style=\"color:#fff; max-width:100%; min-height:195px; background:url('https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/dealer-booking-banner.jpg') no-repeat; padding:0 20px; \">"
                + "<div style=\" padding-top:20px; \"></div>"
                + "<div style=\"clear:both;\"></div>"
                + "<div style=\"max-width:100%; min-height:40px; background:#2a2a2a;\">"
                + "        <div style=\"float:left; max-width:82px; margin-top:5px; margin-left:20px;\">"
                + "            <a href=\"#\" target=\"_blank\"><img src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/bw-white-logo.png\" alt=\"BikeWale\" title=\"BikeWale\" width=\"100%\" border=\"0\" /></a>"
                + "            </div>"
                + "            <div style=\"float:right; margin-right:20px; font-size:14px; line-height:40px;\">"
                + "            {0}"
                + "            </div>"
                + "            <div style=\"clear:both\"></div>"
                + "        </div>"
                + "        <div style=\"text-align:center\">"
                + "            <div style=\"width:100%; height:115px; font-size:28px; text-align:center; display:table;\">"
                + "                <div style=\"display:table-cell; vertical-align: middle;\">Congratulations!</div>"
                + "            </div>"
                + "        </div>"
                + "    </div><div>", DateTime.Now.ToString("dd MMMM yyyy"));


                sb.AppendFormat("<div style=\"margin:0 10px;\">"
                + "            <div style=\"display:inline-block; vertical-align:top; margin:15px 10px; max-width:430px; border-right:1px solid #f5f5f5;\">"
                + "                <div style=\"font-weight:bold; margin-bottom:20px;\">Dear {0},</div>"
                + "                <div style=\"margin-bottom:10px; color:#82888b; line-height:1.5;\">"
                + "                    Our customer {1} has booked <span style=\"color:#4d5057;\">{2}</span>"
                + "                    on BikeWale. The booking amount of <span style=\"font-weight:bold; color:#4d5057;\"><img border=\"0\" title=\"Rupee\" alt=\"Rupee\" src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/inr-rupee-icon.png\">{3}</span> has been received."
                + "                    BikeWale Booking Reference Number is <span style=\"font-weight:bold; color:#4d5057;\">{4}</span>."
                + "                    <br />Find the booking details below:"
                + "                </div>"
                + "            </div>"
                + "            <div style=\"display:inline-block; vertical-align:top; margin:15px 10px 0; color:#82888b; width:180px;\">"
                + "                <div style=\"padding-bottom:15px; border-bottom:1px solid #f5f5f5;\">"
                + "                    <div style=\"margin-bottom:10px;\">Advance payment</div>"
                + "                    <div style=\"font-weight:bold; font-size:16px; color:#4d5057;\"><img border=\"0\" title=\"Rupee\" alt=\"Rupee\" src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/inr-rupee-med-icon.jpg\"> {3}</div>"
                + "                </div>"
                + "                <div style=\"padding-top:15px; padding-bottom:15px;\">"
                + "                    <div style=\"margin-bottom:10px;\">Balance payable amount</div>"
                + "                    <div style=\"font-weight:bold; font-size:16px; color:#4d5057;\"><img border=\"0\" title=\"Rupee\" alt=\"Rupee\" src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/inr-rupee-med-icon.jpg\"> {5}</div>"
                + "                </div>"
                + "        </div>"
                + "        </div>",

                            dealerName, //0
                            customerName, //1
                            bikeName,//2
                            Format.FormatPrice(bookingAmount.ToString()), //3
                            bookingReferenceNo.Trim(), //4
                            Format.FormatPrice((balanceAmount - TotalDiscountedPrice(discountList)).ToString()) //5
                );


                //customer details starts here
                sb.AppendFormat(
                  " <div style=\"margin:0 20px 20px 15px; border-bottom:1px solid #f5f5f5; border-top:1px solid #f5f5f5; padding-top:15px; word-break:break-all;\">"
                + "            <div style=\"font-weight:bold; margin-bottom:15px;\">Our customer details:</div>"
                + "            <div style=\"display:inline-block; vertical-align:top; width:185px; margin:0 10px 15px 0;\"><span style=\"color:#82888b;width:45px; float:left;\">Name:      </span><span style=\"font-weight:bold;width:140px; float:left;\">{0}</span></div>"
                + "            <div style=\"display:inline-block; vertical-align:top; width:60%; margin:0 10px 15px 0;\"><span style=\"color:#82888b;\">Location:  </span><span style=\"font-weight:bold;\">{3}</span></div><div style=\"clear:both;\"></div>"
                + "            <div style=\"display:inline-block; vertical-align:top; width:185px; margin:0 10px 15px 0;\"><span style=\"color:#82888b;\">Mobile no: </span><span style=\"font-weight:bold;\">{2}</span></div>"
                + "            <div style=\"display:inline-block; vertical-align:top; width:60%; margin:0 10px 15px 0;\"><span style=\"color:#82888b;\">Email Id:  </span><span style=\"font-weight:bold;\">{1}</span></div>"
                + "            <div style=\"clear:both;\"></div>"
                + " </div>"
                    , customerName //0
                    , customerEmail//1
                    , customerMobile //2
                    , customerArea //3
                );


                //bike details starts here
                sb.AppendFormat(
                  "<div style=\"margin:0 20px 15px 20px;padding-bottom:15px;\">"
                + "    <div style=\"width:184px; min-height:150px; display:inline-block; vertical-align:top; margin:0 12px 10px 0; text-align:left;\">"
                + "            <div style=\"font-weight:bold;\">{0}</div>"
                + "            <img src=\"{3}\" alt=\"{0}\" title=\"{0}\" border=\"0\" style=\"margin:20px 0 0 5px;\"/>"
                + "    </div>"
                + "    <div style=\"display:inline-block; vertical-align:top; max-width:455px; text-align:left;\">"
                + "                <div style=\"float:left; width:226px; margin-right:5px; padding-bottom:15px;\"><div style=\"color:#82888b; width:55px; float:left;\">Version: </div><div style=\"float:left; width:170px; font-weight:bold; \">{1}</div><div style=\"clear:both;\"></div></div>"
                + "                <div style=\"float:left; width:220px; padding-bottom:15px; \"><div style=\"color:#82888b; width:50px; float:left;\">Colour: </div><div style=\"float:left; width:163px; font-weight:bold;\">{2}</div><div style=\"clear:both;\"></div></div>"
                + "    <div style=\"clear:both;\"></div>"
                , bikeName //0
                , versionName //1
                , bikeColor //2
                , imagePath //3
                );

                //price list

                sb.AppendFormat(
                           "<div style=\" padding-top:20px; padding-bottom:15px; border-top:1px solid #f5f5f5;\">"
                         + "     <div style=\"width:60%; float:left; color:#82888b;\">On-road price</div>"
                         + "     <div style=\"width:40%; float:left; text-align:right; font-weight:bold;\"><span><img src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/inr-rupee-icon.png\" alt=\"Rupee\" title=\"Rupee\" border=\"0\" /></span>{0}</div>"
                         + "     <div style=\"clear:both;\"></div>"
                         + "</div>"
                      , Format.FormatPrice(Convert.ToString(totalPrice - TotalDiscountedPrice(discountList)))
                      );

                sb.AppendFormat(
                      "   <div style=\"padding-bottom:15px;\">"
                    + "      <div style=\"width:60%; float:left; color:#82888b;\">Booking amount</div>"
                    + "      <div style=\"width:40%; float:left; text-align:right; font-weight:bold;\"><span><img src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/inr-rupee-icon.png\" alt=\"Rupee\" title=\"Rupee\" border=\"0\" /></span>{0}</div>"
                    + "      <div style=\"clear:both;\"></div>"
                    + "   </div>"
                    , Format.FormatPrice(bookingAmount.ToString())
                    );

                sb.AppendFormat("<div style=\"padding-bottom:15px;\">"
                + "                <div style=\"width:60%; float:left; color:#82888b;\">Balance payable amount</div>"
                + "                    <div style=\"width:40%; float:left; text-align:right; font-weight:bold;\"><span><img src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/inr-rupee-icon.png\" alt=\"Rupee\" title=\"Rupee\" border=\"0\" /></span>{0}</div>"
                + "                    <div style=\"clear:both;\"></div>"
                + "                </div> </div></div>"
                , Format.FormatPrice((balanceAmount - TotalDiscountedPrice(discountList)).ToString())
                );



                //bike details ends here


                //offers start here

                if (offerList != null && offerList.Count > 0)
                {
                    sb.AppendFormat(
                        "<div style=\"text-align:center; border-top:1px solid #f5f5f5;\">"
                    + "        <div style=\" padding-bottom:10px; margin:15px 20px 0 20px; text-align:left; font-size:14px; font-weight:bold; color:#4d5057;\">Offers availed by our customer:</div>"
                    + "            <div style=\"padding:0 20px; text-align:left; line-height:1.4;\">");
                    foreach (var offer in offerList)
                    {
                        sb.AppendFormat(
                          "<div style=\"max-width:190px; margin:10px 5px 10px; display:inline-block; vertical-align:top;\">"
                                + "<div style=\"width:45px; display:inline-block; vertical-align:middle;\"><img src=\"https://imgd.aeplcdn.com/0x0/bw/static/icons/offers/{0}.png\" alt=\"{1}\" title=\"{1}\" border=\"0\" style=\"border:none; margin-right:5px;\" /></div>"
                                + "<div style=\"width:140px; display:inline-block; vertical-align:middle; text-align:left; font-size:14px; color:#82888b; margin:5px 0 0 0;\">{1}</div>"
                                + "<div style=\"clear:both;\"></div>"
                        + "</div>"
                        , offer.OfferCategoryId
                        , offer.OfferText
                        );
                    }
                    sb.AppendFormat("</div></div>");
                }

                //offers ends here

                sb.AppendFormat("    <div style=\"border-top:1px solid #f5f5f5;margin:10px 20px 0 20px; line-height:1.5;\">"
                + "        <div style=\"font-size:14px; color:#82888b; margin:15px 0 10px 0;\">Please let us know when customer makes further payment / takes delivery, and we will transfer booking amount to your bank account.</div>"
                + "            <div style=\"font-size:14px; color:#82888b; margin-bottom:10px;\">Please feel free to call Asif at 99307 46975 for any queries or help required in the process.</div>"
                + "            <div style=\"margin-bottom:25px; color:#82888b;\">Regards,<br />Team BikeWale</div>"
                + ""
                + "        </div>"
                + ""
                + "        <div style=\"max-width:100%; background:url('https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/query-bg-banner.jpg') no-repeat center bottom / cover #2e2e2e; color:#fff;\">"
                + "        <div style=\"padding:7px 15px 7px 20px;display:inline-block;vertical-align:middle;\">"
                + "            <div style=\"float:left; width:46px; font-weight:bold;\"><img src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/bw-app-red-icon.png\" border=\"0\"/></div>"
                + "                <div style=\"font-size:16px;height:46px;text-align:left;display:table;margin-left:60px;\">"
                + "                <div style=\"display:table-cell; vertical-align:middle;\">India’s #1 Bike Research Destination</div>"
                + "                </div>"
                + "            </div>"
                + "            <div style=\"margin:15px 20px 15px 0;display:inline-block;float:right\">"
                + "            <div>"
                + "            <a href=\" https://play.google.com/store/apps/details?id=com.bikewale.app&utm_source=BookingMailer&utm_medium=email&utm_campaign=DealerBookingMail \" target=\"_blank\" style=\"text-decoration:none; color:#fff; font-weight:bold; font-size:12px; width:70px; background-color:#ef3f30; padding:8px 10px; border-radius:2px; display:block;\">Get the App</a>"
                + "                </div>"
                + "            </div>"
                + "             <div style=\"clear:both;\"></div>"
                + "        </div>"
                + ""
                + "        <div style=\"margin:10px 0 4px 0; border-bottom:2px solid #c20000;\"></div>"
                + ""
                + "    </div>"
                + "</div>");
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Notification.PreBookingConfirmationMailToDealer.ComposeBody");
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

