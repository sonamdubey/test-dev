using Carwale.DAL.Stock;
using Carwale.Interfaces.Stock;
using Microsoft.Practices.Unity;
using Carwale.BL.Stock;

namespace Carwale.Service.Containers
{
    public class StockTypes
    {
        public static void RegisterTypes(UnityContainer container)
        {
            container
                .RegisterType<IStockRecommendationRepository, StockRecommendationRepository>()
                .RegisterType<IStockRecommendationsBL, StockRecommendationsBL>()
                .RegisterType<IStockPackagesRepository, StockPackagesRepository>()
                .RegisterType<IStockScoreRepository, StockScoreRepository>(new ContainerControlledLifetimeManager());
        }
    }
}
