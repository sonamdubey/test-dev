using Bikewale.BAL.BikeData;
using Bikewale.BAL.BikeData.NewLaunched;
using Bikewale.BAL.BikeData.UpComingBike;
using Bikewale.BAL.EditCMS;
using Bikewale.BAL.Pager;
using Bikewale.BAL.ServiceCenter;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Compare;
using Bikewale.Cache.Core;
using Bikewale.Cache.DealersLocator;
using Bikewale.Cache.Location;
using Bikewale.Cache.ServiceCenter;
using Bikewale.Cache.Videos;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Compare;
using Bikewale.DAL.Dealer;
using Bikewale.DAL.Location;
using Bikewale.DAL.ServiceCenter;
using Bikewale.DAL.Videos;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.service;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Compare;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Pager;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Interfaces.Videos;
using Microsoft.Practices.Unity;
using System.Web.Mvc;
using Unity.Mvc5;

namespace Bikewale
{
    /// <summary>
    /// Created by : Ashish G. Kamble on 6 Jan 2017
    /// </summary>
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterType<IArticles, Articles>();
            container.RegisterType<ICMSCacheContent, CMSCacheRepository>();
            container.RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>();
            container.RegisterType<ICacheManager, MemcacheManager>();
            container.RegisterType<IPager, Pager>();
            container.RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>();
            container.RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>();
            container.RegisterType<IBikeInfo, BikeInfo>();
            container.RegisterType<INewBikeLaunchesBL, NewBikeLaunchesBL>();
            container.RegisterType<IUpcoming, Upcoming>();
            container.RegisterType<IModelsCache, ModelsCache>();
            container.RegisterType<IModelsRepository, ModelsRepository>();
            container.RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>();
            container.RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>();
            container.RegisterType<IVideoRepository, ModelVideoRepository>();
            container.RegisterType<IVideosCacheRepository, VideosCacheRepository>();
            container.RegisterType<IVideos, Bikewale.BAL.Videos.Videos>();
            container.RegisterType<ICity, CityRepository>();
            container.RegisterType<ICityCacheRepository, CityCacheRepository>();
            container.RegisterType<IBikeCompareCacheRepository, BikeCompareCacheRepository>();
            container.RegisterType<IBikeCompare, BikeCompareRepository>();
            container.RegisterType<IDealerCacheRepository, DealerCacheRepository>();
            container.RegisterType<IDealer, DealersRepository>();
            container.RegisterType<IServiceCenter, ServiceCenter<ServiceCenterLocatorList, int>>();
            container.RegisterType<IServiceCenterCacheRepository, ServiceCenterCacheRepository>();
            container.RegisterType<IServiceCenterRepository<ServiceCenterLocatorList, int>, ServiceCenterRepository<ServiceCenterLocatorList, int>>();
            container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>();
            container.RegisterType<IBikeVersions<BikeVersionEntity, uint>, BikeVersions<BikeVersionEntity, uint>>();
            container.RegisterType<IBikeVersionCacheRepository<BikeVersionEntity, uint>, BikeVersionsCacheRepository<BikeVersionEntity, uint>>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}