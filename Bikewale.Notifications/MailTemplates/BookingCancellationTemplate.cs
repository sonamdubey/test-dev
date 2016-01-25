using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Notifications.MailTemplates
{
    public class BookingCancellationTemplate : ComposeEmailBase
    {
        public string BWId { get; set; }
        public uint TransactionId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerMobile { get; set; }
        public string BookingDate { get; set; }
        public string DealerName { get; set; }
        public string BikeName { get; set; }
        public string CityName { get; set; }

        public BookingCancellationTemplate(string bwId, uint transactionId, string customerName, string customerEmail, string customerMobile, string bookingDate,
            string dealerName, string bikeName,string cityName)
        {
            BWId = bwId;
            TransactionId = transactionId;
            CustomerEmail = customerEmail;
            CustomerMobile = customerMobile;
            CustomerName = customerName;
            BookingDate = bookingDate;
            DealerName = dealerName;
            BikeName = bikeName;
            CityName = cityName;
        }

        public override string ComposeBody()
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.AppendFormat("<div>"
                    + "<table>"
                       + "<tbody>"
                            + "<tr>"
                                + "<td style=\"width:150px;\"><b>BW Id</b></td>"
                                + "<td>{0}</td>"
                            + "</tr>"
                            + "<tr>"
                                + "<td><b>Transaction Id</b></td>"
                                + "<td>{1}</td>"
                            + "</tr>"
                            + "<tr>"
                                + "<td><b>Booking Date</b></td>"
                                + "<td>{2}</td>"
                            + "</tr>"
                            + "<tr>"
                                + "<td><b>Customer Name</b></td>"
                                + "<td>{3}</td>"
                            + "</tr>"
                            + "<tr>"
                                + "<td><b>Customer Email</b></td>"
                                + "<td>{4}</td>"
                            + "</tr>"
                            + "<tr>"
                                + "<td><b>Customer Mobile</b></td>"
                                + "<td>{5}</td>"
                            + "</tr>"
                            + "<tr>"
                                + "<td><b>Bike Name</b></td>"
                                + "<td>{6}</td>"
                            + "</tr>"
                            + "<tr>"
                                + "<td><b>Dealer Name</b></td>"
                                + "<td>{7}</td>"
                            + "</tr>"
                             + "<tr>"
                                + "<td><b>Dealer City</b></td>"
                                + "<td>{8}</td>"
                            + "</tr>"
                        + "</tbody>"
                    + "</table>"
                + "</div>", BWId, TransactionId.ToString(), BookingDate, CustomerName, CustomerEmail, CustomerMobile, BikeName, DealerName,CityName);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception in BookingCancellationTemplate");
                objErr.SendMail();
            }
            return sb.ToString();
        }

    }
}
