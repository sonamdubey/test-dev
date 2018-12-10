using Carwale.BL.GeoLocation;
using Carwale.Interfaces.Geolocation;
using Microsoft.Practices.Unity;

namespace Carwale.Service.Containers
{
    public static class GeoLocationTypes
    {
        public static void RegisterTypes(UnityContainer unityContainer)
        {
            unityContainer
                .RegisterType<IGeoCityLogic, GeoCityLogic>(new ContainerControlledLifetimeManager());
        }
    }
}
