using Carwale.BL.Interface.Stock.Search;
using Carwale.BL.Stock.Search;
using Carwale.DAL.Classified;
using Carwale.DAL.Classified.ElasticSearch;
using Carwale.DAL.Interface.Classified.ElasticSearch;
using Carwale.Entity.Stock.Search;
using Carwale.Interfaces.Classified;
using Microsoft.Practices.Unity;

namespace Carwale.Service.Containers
{
    public static class ClassifiedTypes
    {
        public static void RegisterTypes(UnityContainer container)
        {
            container
                .RegisterType<ICertificationProgramsRepository, CertificationProgramsRepository>()
                .RegisterType<INearbyCityLogic, NearbyCityLogic>(new ContainerControlledLifetimeManager())
                .RegisterType<INearbyCityRepository, NearbyCityRepository>(new ContainerControlledLifetimeManager())
                .RegisterType<IStockSearchLogic<SearchResultMobile>, MobileStockSearchLogic>()
                .RegisterType<IStockSearchLogic<SearchResultDesktop>, DesktopStockSearchLogic>();
        }
    }
}
