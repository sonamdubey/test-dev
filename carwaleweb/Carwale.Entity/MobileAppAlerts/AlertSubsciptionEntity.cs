using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.MobileAppAlerts
{
    public class AlertSubsciptionEntity : CustomersBasicInfo
    {        
        public string IEMI_Number { get; set; }

        /// <summary>
        /// Token id that user gets after subscribing cloud messaging sevice
        /// </summary> 
        public string UserTokenId { get; set; }
        
        /// <summary>
        /// List of alerts use subscribed/unsubscribed for
        /// </summary>
        List<SubscriptionAction> SubscribedFor { get; set; }
        
        /// <summary>
        /// Name of Subscription Platform i.e. Android, iOS, WindowsMobile etc
        /// </summary>        
        public int SubscriptionPlatform { get; set; }     
    }

    public class SubscriptionAction
    {
        public string TypeName { get; set; }
        public bool Action { get; set; }
    }

}
