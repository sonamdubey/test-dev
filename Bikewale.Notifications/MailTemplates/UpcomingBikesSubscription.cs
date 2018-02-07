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
            StringBuilder message = new StringBuilder();
            message.Append("Dear <UserName>,\n\n");
            message.Append("Thank you for subscribing to BikeWale.\n\n");
            message.AppendFormat("Glad to see that you have chosen BikeWale to research about your favorite bike. You will receive all the updates about {0} directly in your inbox.\n\n", bikeName);
            message.Append("And if you like BikeWale, don't forget to download our Android App from google play store-  https://goo.gl/L9XSAu \n\n");
            message.Append("Cheers!\n");
            message.Append("Team BikeWale\n\n");

            return message.ToString();

        }
   }
}
