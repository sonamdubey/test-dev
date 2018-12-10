using Carwale.Entity.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Notifications
{
    public interface ISMSRepository
    {
        string SaveSMSSentData(SMS sms);
        void SaveDataForSMS(string tempCustomerId);
    }
}
