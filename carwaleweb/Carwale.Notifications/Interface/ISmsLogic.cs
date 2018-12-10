using Carwale.Entity.Notifications;

namespace Carwale.Notifications.Interface
{
    public interface ISmsLogic
    {
        bool Send(SMS sms);
    }
}
