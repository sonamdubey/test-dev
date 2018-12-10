using Carwale.BL.Stock;
using Carwale.BL.SponsoredCar;
using Carwale.DAL.Classified.ElasticSearch;
using Carwale.Entity.Classified;
using Carwale.Interfaces.Classified.ElasticSearch;
using Carwale.Interfaces.Stock;
using Carwale.Interfaces.SponsoredCar;
using Microsoft.Practices.Unity;

namespace Carwale.Service.Containers
{
    public static class ClassifiedESTypes
    {
        public static void RegisterTypes(IUnityContainer unityContainer)
        {
            unityContainer
                .RegisterType<IAggregationQueryDescriptor, AggregationQueryDescriptor>(new ContainerControlledLifetimeManager())
                .RegisterType<IProcessAggregationElasticJson, ProcessAggregationElasticJson>(new ContainerControlledLifetimeManager())
                .RegisterType<IAggregationsRepository, AggregationsRepository>(new ContainerControlledLifetimeManager())
                .RegisterType<IESStockQueryRepository, ESStockQueryRepository>(new ContainerControlledLifetimeManager())
                .RegisterType<IStockManager, StockManager>(new ContainerControlledLifetimeManager())
                .RegisterType<IQueryContainerRepository<StockBaseEntity>, QueryContainerRepository<StockBaseEntity>>(new ContainerControlledLifetimeManager())
                .RegisterType<IQueryContainerRepository<City>, QueryContainerRepository<City>>(new ContainerControlledLifetimeManager())
                .RegisterType<IStockQueryProcessor, FeaturedStockQueryProcessor>("featured", new ContainerControlledLifetimeManager())
                .RegisterType<IStockQueryProcessor, NonFeaturedStockQueryProcessor>("nonFeatured", new ContainerControlledLifetimeManager())
                .RegisterType<ISortDescriptorRepository, SortDescriptorRepository>(new ContainerControlledLifetimeManager());
        }
    }
}
