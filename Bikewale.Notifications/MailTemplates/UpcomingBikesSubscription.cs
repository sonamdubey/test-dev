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
            throw new NotImplementedException();
        }
   }
}
