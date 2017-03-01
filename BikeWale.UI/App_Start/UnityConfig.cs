using Bikewale.BAL.BikeData;
using Bikewale.BAL.BikeData.NewLaunched;
using Bikewale.BAL.BikeData.UpComingBike;
using Bikewale.BAL.EditCMS;
using Bikewale.BAL.Pager;
using Bikewale.BAL.Videos;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.Cache.Videos;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Videos;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Interfaces.Pager;
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

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}