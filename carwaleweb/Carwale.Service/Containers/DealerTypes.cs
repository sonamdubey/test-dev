using Carwale.BL.Dealers.Used;
using Carwale.DAL.Dealers.Used;
using Carwale.Interfaces.Dealers.Used;
using Microsoft.Practices.Unity;

namespace Carwale.Service.Containers
{
    public static class DealerTypes
    {
        public static void RegisterTypes(UnityContainer container)
        {
            container
                .RegisterType<IUsedDealerCitiesBL, UsedDealerCitiesBL>()
                .RegisterType<IUsedDealerCitiesRepository, UsedDealerCitiesRepository>()
                .RegisterType<IUsedDealerRatingsBL, UsedDealerRatingsBL>()
                .RegisterType<IUsedDealerRatingsRepository, UsedDealerRatingsRepository>()
                .RegisterType<IUsedDealerStocksBL, UsedDealerStocksBL>(new ContainerControlledLifetimeManager())
                .RegisterType<IUsedDealerStocksRepository, UsedDealerStocksRepository>(new ContainerControlledLifetimeManager());
        }
    }
}
