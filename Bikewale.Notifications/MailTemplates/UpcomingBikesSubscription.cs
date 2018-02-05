using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Notifications.MailTemplates
{
    public class UpcomingBikesSubscription: ComposeEmailBase
    {
        // access modifiers ?
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
