using Carwale.BL.Otp;
using Carwale.Interfaces.Otp;
using Microsoft.Practices.Unity;

namespace Carwale.Service.Containers
{
    public static class Otp
    {
        public static void RegisterTypes(UnityContainer container)
        {
            container
                .RegisterType<IOtpLogic, OtpLogic>();
        }
    }
}
