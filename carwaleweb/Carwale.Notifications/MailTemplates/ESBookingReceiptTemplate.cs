using Carwale.Entity.ES;
using Carwale.Entity.Notifications;
using Carwale.Utility;
using System.Text;

namespace Carwale.Notifications.MailTemplates
{
    public class ESBookingReceiptTemplate
    {
        public EmailEntity GetOnlineBookingSuccessTemplate(EsBookingSummary bookingSummary)
        {
            StringBuilder message = new StringBuilder();

            message.Append("<div style='text-align: left;'>");
            message.Append("<p style='font-size: 13px;'>Dear Customer,</p>");
            message.Append("<p style='font-size: 13px; margin-top: 20px;'>Congratulation on successfully booking the Volvo V90 Cross Country.</p>");

            message.Append("<p style='font-size: 13px; margin-top: 20px; font-weight: 600;'>Summary:</p>");
            message.Append(string.Format("<p style='font-size: 13px;'>Reference ID: <span'>{0}</span></p>", bookingSummary.TransactionId));
            message.Append(string.Format("<p style='font-size: 13px;'>Amount Received: <span>{0}(Rupees Fifty Thousand) towards the booking of {1} {2}</span></p>", Format.FormatFullPrice(bookingSummary.BookingAmount.ToString(), true), bookingSummary.CarMake, bookingSummary.CarModel));

            message.Append("<p style='font-size: 13px; margin-top: 20px; font-weight: 600;'>Customer Details:</p>");
            message.Append(string.Format("<p style='font-size: 13px;'>Name: <span>{0}</span></p>", bookingSummary.CustomerName));
            message.Append(string.Format("<p style='font-size: 13px;'>Mobile: <span>{0}</span></p>", bookingSummary.CustomerMobile));
            message.Append(string.Format("<p style='font-size: 13px;'>Email: <span>{0}</span></p>", bookingSummary.CustomerEmail));
            message.Append(string.Format("<p style='font-size: 13px;'>City: <span>{0}</span></p>", bookingSummary.CityName));

            message.Append("<p style='font-size: 13px; margin-top: 20px; font-weight: 600;'>Selected Volvo Dealer:</p>");
            message.Append(string.Format("<p style='font-size: 13px;'>{0}</p>", bookingSummary.DealerName));
            if (!string.IsNullOrEmpty(bookingSummary.DealerAddress))
                message.Append(string.Format("<p style='font-size: 13px;'>{0}</p>", bookingSummary.DealerAddress));
            if (!string.IsNullOrEmpty(bookingSummary.DealerMobile))
                message.Append(string.Format("<p style='font-size: 13px;'>Call - {0}</p>", bookingSummary.DealerMobile));

            message.Append("<p style='font-size: 13px; margin-top: 20px; font-weight: 600;'>Model Details:</p>");
            message.Append(string.Format("<p style='font-size: 13px;'>Model: <span>{0}</span></p>", bookingSummary.CarModel));
            message.Append(string.Format("<p style='font-size: 13px;'>Engine & Transmission: <span>{0}</span></p>", bookingSummary.Transmission));
            message.Append(string.Format("<p style='font-size: 13px;'>Exterior Color: <span>{0}</span></p>", bookingSummary.ExteriorColor));
            message.Append(string.Format("<p style='font-size: 13px;'>Interior Color: <span>{0}</span></p>", bookingSummary.InteriorColor));

            message.Append("<p style='font-size: 13px; margin-top: 20px;'>Our team will call you shortly to proceed ahead with the booking process.</p>");
            
            message.Append("<p style='font-size: 13px; margin-top: 5px;'>Thank you for choosing Volvo.</p>");

            message.Append("<br>Regards,<br>");
            message.Append("Team Volvo");
            return new EmailEntity()
            {
                Email = bookingSummary.CustomerEmail,
                Subject = "Booking Confirmation - Volvo V90 Cross Country",
                Body = message.ToString()
            };
        }

        public EmailEntity GetOnlineBookingFailureTemplate(EsBookingSummary bookingSummary)
        {
            StringBuilder message = new StringBuilder();

            message.Append("<div style='text-align: left;'>");
            message.Append("<p style='font-size: 13px;'>Dear Customer,</p>");
            message.Append("<p style='font-size: 13px; margin-top: 20px;'>We regret to inform you that your transaction to book the Volvo V90 Cross Country through Online payment was unsuccessful.</p></div>");
            message.Append("<p style='font-size: 13px; margin-top: 20px;'>You can retry the booking request using the below link:</p>");
            message.Append("<p style='font-size: 13px;'><a href='https://www.carwale.com/volvo-cars/v90-cross-country/booking/' target='_blank'>Retry Now></a></p>");
            message.Append("<p style='font-size: 13px;'>A representative from CarWale will call you shortly to assist in completing your booking.</p>");
            message.Append("<p style='font-size: 13px; margin-top: 5px;'>Thank you for choosing Volvo.</p></div>");

            message.Append("<br>Regards,<br>");
            message.Append("Team Volvo");
            return new EmailEntity()
            {
                Email = bookingSummary.CustomerEmail,
                Subject = "Transaction Unsuccessful for Booking of Volvo V90 Cross Country",
                Body = message.ToString()
            };
        }

        public EmailEntity GetChequeBookingSuccessTemplate(EsBookingSummary bookingSummary)
        {
            StringBuilder message = new StringBuilder();

            message.Append("<div style='text-align: left;'>");
            message.Append("<p style='font-size: 13px;'>Dear Customer,</p>");
            message.Append("<p style='font-size: 13px; margin-top: 20px;'>Congratulation on successfully placing a cheque collection request towards the booking of Volvo V90 Cross Country.</p>");

            message.Append("<p style='font-size: 13px; margin-top: 20px; font-weight: 600;'>Summary:</p>");
            message.Append(string.Format("<p style='font-size: 13px;'>Reference ID: <span>{0}</span></p>", bookingSummary.TransactionId));
            message.Append(string.Format("<p style='font-size: 13px;'>Amount to be Received: <span>{0}(Rupees Fifty Thousand) towards the booking of {1} {2}</span></p>", Format.FormatFullPrice(bookingSummary.BookingAmount.ToString(), true), bookingSummary.CarMake, bookingSummary.CarModel));

            message.Append("<p style='font-size: 13px; margin-top: 20px; font-weight: 600;'>Customer Details:</p>");
            message.Append(string.Format("<p style='font-size: 13px;'>Name: <span>{0}</span></p>", bookingSummary.CustomerName));
            message.Append(string.Format("<p style='font-size: 13px;'>Mobile: <span>{0}</span></p>", bookingSummary.CustomerMobile));
            message.Append(string.Format("<p style='font-size: 13px;'>Email: <span>{0}</span></p>", bookingSummary.CustomerEmail));
            message.Append(string.Format("<p style='font-size: 13px;'>City: <span>{0}</span></p>", bookingSummary.CityName));

            message.Append("<p style='font-size: 13px; margin-top: 20px; font-weight: 600;'>Selected Volvo Dealer:</p>");
            message.Append(string.Format("<p style='font-size: 13px;'>{0}</p>", bookingSummary.DealerName));
            if (!string.IsNullOrEmpty(bookingSummary.DealerAddress))
                message.Append(string.Format("<p style='font-size: 13px;'>{0}</p>", bookingSummary.DealerAddress));
            if (!string.IsNullOrEmpty(bookingSummary.DealerMobile))
                message.Append(string.Format("<p style='font-size: 13px;'>Call - {0}</p>", bookingSummary.DealerMobile));

            message.Append("<p style='font-size: 13px; margin-top: 20px; font-weight: 600;'>Model Details:</p>");
            message.Append(string.Format("<p style='font-size: 13px;'>Model: <span>{0}</span></p>", bookingSummary.CarModel));
            message.Append(string.Format("<p style='font-size: 13px;'>Engine & Transmission : <span>{0}</span></p>", bookingSummary.Transmission));
            message.Append(string.Format("<p style='font-size: 13px;'>Exterior Color: <span>{0}</span></p>", bookingSummary.ExteriorColor));
            message.Append(string.Format("<p style='font-size: 13px;'>Interior Color: <span>{0}</span></p>", bookingSummary.InteriorColor));

            message.Append("<p style='font-size: 13px; margin-top: 20px;'>Our team will call you shortly to proceed ahead with the cheque collection request.</p>");

            message.Append("<p style='font-size: 13px; margin-top: 5px;'>Thank you for choosing Volvo.</p>");

            message.Append("<br>Regards,<br>");
            message.Append("Team Volvo");

            return new EmailEntity()
            {
                Email = bookingSummary.CustomerEmail,
                Subject = "Booking Confirmation - Volvo V90 Cross Country",
                Body = message.ToString()
            };
        }
    }
}
