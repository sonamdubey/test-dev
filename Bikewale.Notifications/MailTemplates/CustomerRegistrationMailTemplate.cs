using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Bikewale.Notifications.MailTemplates
{
    /// <summary>
    /// Created By : Aditi Srivastava on 2 Nov 2016
    /// Summary    : Compose email for new customer registration
    /// </summary>
    public class CustomerRegistrationMailTemplate : ComposeEmailBase
    {
        private string customerId,
            customerName,
            password;

        public CustomerRegistrationMailTemplate(string customerId, string customerName, string password)
        {
            this.customerId = customerId;
            this.customerName = customerName;
            this.password = password;
        }
           public override string ComposeBody()
            {
                StringBuilder message = new StringBuilder();
                message.AppendFormat("<img align=\"right\" src=\"http://imgd3.aeplcdn.com/0x0/bw/static/design15/old-images/d/bw-logo.png\" />");

                message.AppendFormat("<h4>Dear {0},</h4>", customerName);

                message.AppendFormat("<p>Greetings from BikeWale!</p>");

                message.AppendFormat("<p>Thank you for visiting BikeWale.com ");
                message.AppendFormat(" We are committed to improve your bike trading experience through our services");
                message.AppendFormat(" and are pleased to welcome you in the BikeWale family. </p>");
                message.AppendFormat("<p>With BikeWale, you can now ride");
                message.AppendFormat(" your biking passion at full-throttle with reviews, news, comparison, upcoming bikes and more. </p>");
                message.AppendFormat("<p>For future reference your user id and password are as listed below:</p>");

                message.AppendFormat("User ID : {0}<br>", customerId);
                message.AppendFormat("Password : {0}<br>", password);

                message.AppendFormat("<p>You can change your password by clicking here <a href='http://www.bikewale.com/MyBikewale/changepassword/'>Change Password</a></p>");

                message.AppendFormat("<br>Warm Regards,<br><br>");
                message.AppendFormat("Customer Care,<br><b>BikeWale</b>");

                return message.ToString();
            }
            
        

    }
}
