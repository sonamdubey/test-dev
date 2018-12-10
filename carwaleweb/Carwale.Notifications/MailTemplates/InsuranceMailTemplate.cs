using Carwale.Entity.Insurance;
using Carwale.Entity.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Notifications.MailTemplates
{
    public class InsuranceMailTemplate
    {
        public EmailEntity GetInsuranceEmailTemplate(ClientMailEntity clientMailEntity)
        {
            StringBuilder message = new StringBuilder();
            message.Append("<p>Dear Team,<br><br>"
            + "Following are the lead details.<br><br>"
            + "Insurance Type: " + clientMailEntity.InsuranceType + "<br><br>"
            + "Car Details: " + clientMailEntity.Car.MakeName + " " + clientMailEntity.Car.ModelName + " " + clientMailEntity.Car.VersionName + "<br>"
            + "Location: " + clientMailEntity.Customer.City + ", " + clientMailEntity.Customer.State + "<br>"
            + "Car Registration date: " + clientMailEntity.RegistrationDate + "<br>"
            + "Customer Name: " + clientMailEntity.Customer.Name + "<br>"
            + "Email Id: " + clientMailEntity.Customer.Email + "<br>"
            + "Mobile number: " + clientMailEntity.Customer.Mobile + "<br></p>"
            + "<br>");
            message.Append("<br><br>Regards,<br>");
            message.Append("<b>Team CarWale</b>");

            var email = new EmailEntity()
            {
                Email = clientMailEntity.ClientEmails,
                Subject = "CarWale Lead ID - " + clientMailEntity.LeadId.ToString(),
                Body = message.ToString()
            };
            return email;
        }
    }
}
