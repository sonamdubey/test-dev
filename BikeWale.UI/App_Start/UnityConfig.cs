using Bikewale.BAL.BikeBooking;
using Bikewale.BAL.BikeData;
using Bikewale.BAL.BikeData.NewLaunched;
using Bikewale.BAL.BikeData.UpComingBike;
using Bikewale.BAL.Customer;
using Bikewale.BAL.EditCMS;
using Bikewale.BAL.Pager;
using Bikewale.BAL.PriceQuote;
using Bikewale.BAL.ServiceCenter;
using Bikewale.BAL.UsedBikes;
using Bikewale.BAL.UserReviews;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Compare;
using Bikewale.Cache.Core;
using Bikewale.Cache.DealersLocator;
using Bikewale.Cache.HomePage;
using Bikewale.Cache.Location;
using Bikewale.Cache.PriceQuote;
using Bikewale.Cache.ServiceCenter;
using Bikewale.Cache.Used;
using Bikewale.Cache.UsedBikes;
using Bikewale.Cache.UserReviews;
using Bikewale.Cache.Videos;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Compare;
using Bikewale.DAL.Customer;
using Bikewale.DAL.Dealer;
using Bikewale.DAL.HomePage;
using Bikewale.DAL.Location;
using Bikewale.DAL.ServiceCenter;
using Bikewale.DAL.Used;
using Bikewale.DAL.UsedBikes;
using Bikewale.DAL.UserReviews;
using Bikewale.DAL.Videos;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Customer;
using Bikewale.Entities.service;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Compare;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Interfaces.HomePage;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Pager;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Interfaces.Used;
using Bikewale.Interfaces.UsedBikes;
using Bikewale.Interfaces.UserReviews;
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
            container.RegisterType<IDealerRepository, DealersRepository>();
            container.RegisterType<IServiceCenter, ServiceCenter<ServiceCenterLocatorList, int>>();
            container.RegisterType<IServiceCenterCacheRepository, ServiceCenterCacheRepository>();
            container.RegisterType<IServiceCenterRepository<ServiceCenterLocatorList, int>, ServiceCenterRepository<ServiceCenterLocatorList, int>>();
            container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>();
            container.RegisterType<IDealerPriceQuoteDetail, DealerPriceQuoteDetail>();
            container.RegisterType<IDealerPriceQuote, DealerPriceQuote>();
            container.RegisterType<IUsedBikeDetailsCacheRepository, UsedBikeDetailsCache>();
            container.RegisterType<IUsedBikeDetails, UsedBikeDetailsRepository>();
            container.RegisterType<IStateCacheRepository, StateCacheRepository>();
            container.RegisterType<IState, StateRepository>();
            container.RegisterType<IAreaCacheRepository, AreaCacheRepository>();
            container.RegisterType<IPriceQuote, BAL.PriceQuote.PriceQuote>();
            container.RegisterType<IBikeModelsCacheRepository<int>>();
            container.RegisterType<IBikeVersions<BikeVersionEntity, uint>, BikeVersions<BikeVersionEntity, uint>>();
            container.RegisterType<IBikeVersionCacheRepository<BikeVersionEntity, uint>, BikeVersionsCacheRepository<BikeVersionEntity, uint>>();
            container.RegisterType<IHomePageBannerRepository, HomePageBannerRepository>();
            container.RegisterType<IHomePageBannerCacheRepository, HomePageBannerCacheRepository>();
            container.RegisterType<ICityMaskingCacheRepository, CityMaskingCache>();
            container.RegisterType<IPriceQuote, BAL.PriceQuote.PriceQuote>();
            container.RegisterType<IPriceQuoteCache, PriceQuoteCache>();
            container.RegisterType<IUserReviewsCache, UserReviewsCacheRepository>();
            container.RegisterType<IUserReviewsRepository, UserReviewsRepository>();
            container.RegisterType<IUserReviews, UserReviews>();
            container.RegisterType<IUsedBikesCache, UsedBikesCache>();
            container.RegisterType<IUsedBikes, UsedBikes>();
            container.RegisterType<IUsedBikesRepository, UsedBikesRepository>();
            container.RegisterType<ICustomer<CustomerEntity, uint>, Customer<CustomerEntity, uint>>();
            container.RegisterType<ICustomerRepository<CustomerEntity, uint>, CustomerRepository<CustomerEntity, uint>>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}