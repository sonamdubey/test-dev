using Carwale.Entity.Customers;
using Carwale.Interfaces.Users;
using Carwale.Notifications;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Carwale.BL.Customers
{
    public class UserFeedbackBL : IUserFeedbackBL
    {
        private readonly IFeedbackRepository _feedbackRepo;
        private static readonly string feedbackEmail = ConfigurationManager.AppSettings.Get("UsedCarFeedbackEmail");

        public UserFeedbackBL(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepo = feedbackRepository;
        }

        public void ProcessFeedback(UserFeedback feedback)
        {
            if (!String.IsNullOrEmpty(feedback.Feedback))
            {
                feedback.Feedback = HttpUtility.HtmlEncode(feedback.Feedback);
            }
            _feedbackRepo.InsertFeedback(feedback);
            SendFeedback(feedback);
        }

        private void SendFeedback(UserFeedback feedback)
        {
            string subject = String.Empty;
            string feedbackContent = String.Empty;

            if (feedback != null)
            {
                if (feedback.Source.Contains("/recommend-cars/"))
                {
                    subject = "Visitor has given feedback for Recommend Car.";
                    feedbackContent = "Recommend me a car Feedback: <br>";
                }
                else if (feedback.Source.Contains("/carvaluation/"))
                {
                    subject = "Visitor has given feedback for Used Valuation.";
                    feedbackContent = "Car Valuation System Feedback: <br>";
                }
                else if (feedback.Source.Contains("/used/cars"))
                {
                    subject = "Visitor has given feedback for Used Car Search.";
                    feedbackContent = "Used Cars Search Page Feedback: <br>";
                }
                else
                {
                    subject = "Visitor has given feedback for Carwale.";
                    feedbackContent = "Carwale Feedback: <br>";
                }
                subject += feedback.UserIp;
                feedbackContent += "User rated : <b>" + feedback.FeedbackRating + "/10</b><br>" + feedback.Feedback + "<br>" + feedback.CarInfo;

                StringBuilder body = new StringBuilder();
                body.Append("<h4>Dear Sir,</h4>");
                body.Append("<p>Some visitor has given feedback for carwale.</p>");
                body.Append("<p>Sender Details :<br>");
                if (!String.IsNullOrEmpty(feedback.UserName))
                {
                    body.Append("Name : <b>" + feedback.UserName + "</b><br>");
                }
                if (!String.IsNullOrEmpty(feedback.Email))
                {
                    body.Append("Email : <b>" + feedback.Email + "</b><br>");
                }
                body.Append("IP : <b>" + feedback.UserIp + "</b></p>");
                body.Append("<p><b>Feedback :</b><br>" + feedbackContent + " </p>");
                body.Append("<p><a href='" + feedback.Source + "'>" + feedback.Source + "</a></p>");
                body.Append("<br><br>Regards,<br>");
                body.Append("Customer Care,<br><b>CarWale</b>");

                Email mail = new Email();
                mail.SendMail(feedbackEmail, subject, body.ToString());
            }
        }
    }
}
