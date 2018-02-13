using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Notifications.MailTemplates
{

    /// <summary>
    /// Created by: Dhruv Joshi on 6 Feb 2018
    /// Description: Mail Template for notification subscription
    /// </summary>
    public class UpcomingBikesSubscription: ComposeEmailBase
    {

        private string bikeName;

        public UpcomingBikesSubscription(string bikeName)
        {
            this.bikeName = bikeName;
        }

        public override string ComposeBody()
        {
            try
            {
                StringBuilder message = new StringBuilder();
                message.AppendFormat("Hello,<br><br>");
                
                message.AppendFormat("Thank you for subscribing to BikeWale.<br><br>");
                
                message.AppendFormat("Glad to see that you have chosen BikeWale to research about your favorite bike. You will receive all the updates about {0} directly in your inbox.<br><br>", bikeName);

                message.AppendFormat("And if you like BikeWale, don't forget to download our Android App from google play store-  <a href = 'https://goo.gl/L9XSAu'>https://goo.gl/L9XSAu </a><br><br>");
                
                message.AppendFormat("Cheers!<br>");
                message.AppendFormat("Team BikeWale");

                return message.ToString();
            }
            catch(Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Notifications.MailTemplates.UpcomingBikesSubscription");
                return null;
            }
        }
    }
}
