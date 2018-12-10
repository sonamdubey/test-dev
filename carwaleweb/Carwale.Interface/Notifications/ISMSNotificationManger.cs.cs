using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Notifications
{
    public interface ISMSNotificationManger
    {
        bool SendSMSToCustomer<T>(T t);
        bool SendSMSToDealer<T>(T t);
    }
}
