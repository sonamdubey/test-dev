using Carwale.Entity.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Notifications
{
    public interface IEmailNotifications
    {
        void SendMail(string email, string subject, string body);
        void SendMail(string email, string subject, string body, string replyTo);
        void SendMail(string email, string subject, string body, string replyTo, string[] cc, string[] bcc);
    }
}
