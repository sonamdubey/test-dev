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
                message.AppendLine("Hello,");
                message.AppendLine("");
                message.AppendLine("Thank you for subscribing to BikeWale.");
                message.AppendLine("");
                message.AppendFormat("Glad to see that you have chosen BikeWale to research about your favorite bike. You will receive all the updates about {0} directly in your inbox.", bikeName);
                message.AppendLine("");
                message.AppendLine("");
                message.AppendLine("And if you like BikeWale, don't forget to download our Android App from google play store-  https://goo.gl/L9XSAu ");
                message.AppendLine("");
                message.AppendLine("Cheers!");
                message.AppendLine("Team BikeWale");

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
