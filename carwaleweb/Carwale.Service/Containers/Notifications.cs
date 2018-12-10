using AEPLCore.Queue;
using Carwale.Notifications;
using Carwale.Notifications.Interface;
using Microsoft.Practices.Unity;

namespace Carwale.Service.Containers
{
    public static class Notifications
    {
        public static void RegisterTypes(UnityContainer container)
        {
            container
                .RegisterType<ISmsLogic, SmsLogic>(new ContainerControlledLifetimeManager())
                .RegisterType<IPublishManager, PublishManager>(new ContainerControlledLifetimeManager());
        }
    }
}
