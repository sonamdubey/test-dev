using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Notifications
{
    public interface IEmailNotificationManger
    {
        bool SendEmailToCustomer<T>(T t);
        bool SendEmailToDealer<T>(T t);
    }
}
