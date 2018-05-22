using Bikewale.Cache.Core;
using Bikewale.Comparison.BAL;
using Bikewale.Comparison.Cache;
using Bikewale.Comparison.DAL;
using Bikewale.Comparison.Interface;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.ManufacturerCampaign.DAL;
using BikewaleOpr.BAL;
using BikewaleOpr.BAL.BikePricing;
using BikewaleOpr.BAL.ContractCampaign;
using BikewaleOpr.BAL.ServiceCenter;
using BikewaleOpr.BAL.Used;
using BikewaleOpr.BAL.Users;
using BikewaleOpr.CommuteDistance;
using BikewaleOpr.DAL;
using BikewaleOpr.DALs;
using BikewaleOpr.DALs.AdOperation;
using BikewaleOpr.DALs.AdSlot;
using BikewaleOpr.DALs.Banner;
using BikewaleOpr.DALs.Bikedata;
using BikewaleOpr.DALs.BikePricing;
using BikewaleOpr.DALs.ConfigurePageMetas;
using BikewaleOpr.DALs.ContractCampaign;
using BikewaleOpr.DALs.Location;
using BikewaleOpr.DALs.ServiceCenter;
using BikewaleOpr.DALs.UserReviews;
using BikewaleOpr.Interface;
using BikewaleOpr.Interface.AdSlot;
using BikewaleOpr.Interface.Banner;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.ConfigurePageMetas;
using BikewaleOpr.Interface.ContractCampaign;
using BikewaleOpr.Interface.Dealers;
using BikewaleOpr.Interface.Location;
using BikewaleOpr.Interface.ServiceCenter;
using BikewaleOpr.Interface.Used;
using BikewaleOpr.Interface.UserReviews;
using BikewaleOpr.Interface.Users;
using Microsoft.Practices.Unity;
using System.Web.Mvc;
using Unity.Mvc5;

namespace BikewaleOpr
{
    /// <summary>
    /// Created by : Ashish G. Kamble on 6 Jan 2017
    /// Modified by : Sajal Gupta on 09-03-2017
    /// Description : Added IBikeModels, IUsedBikes, IHomePage
    /// Modified by : Vivek Singh Tomar on 1st Aug 2017
    /// Description : Added IBikeMakes
    /// Modified by : Vivek Singh Tomar on 7th Aug 2017
    /// Summary : Added IDealers
    /// Modified by : Vivek Singh Tomar on 11th Aug 2017
    /// Summary : Added IBikeSeriesRepository
    /// Modified by : Rajan Chauhan on 13th Dec 2017
    /// Summary : Added IBikeBodyStylesRepository and IBikeBodyStyles
    /// Modified by : Kartik Rathod on 30 mar 18
    /// Desc    : added IUsers
    /// </summary>
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterType<IBikeMakesRepository, BikeMakesRepository>()
                .RegisterType<IBikeModelsRepository, BikeModelsRepository>()
                .RegisterType<IBikeVersions, BikeVersionsRepository>()
                .RegisterType<IBikeModels, BikeModels>()
                .RegisterType<IUsedBikes, UsedBikes>()
                .RegisterType<IHomePage, HomePage>()
                .RegisterType<IUserReviewsRepository, UserReviewsRepository>()
                .RegisterType<IDealerCampaignRepository, DealerCampaignRepository>()
                .RegisterType<ICommuteDistance, CommuteDistanceBL>()
                .RegisterType<ILocation, LocationRepository>()
                .RegisterType<Bikewale.ManufacturerCampaign.Interface.IManufacturerCampaignRepository, ManufacturerCampaignRepository>()
                .RegisterType<Bikewale.ManufacturerCampaign.Interface.IManufacturerCampaignCache, Bikewale.ManufacturerCampaign.Cache.ManufacturerCampaignCache>()
                .RegisterType<Bikewale.ManufacturerCampaign.Interface.IManufacturerCampaign, Bikewale.ManufacturerCampaign.BAL.ManufacturerCampaign>()
                .RegisterType<IContractCampaign, ContractCampaign>()

                .RegisterType<ILocation, LocationRepository>()
                .RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>()
                .RegisterType<IDealerPrice, DealerPrice>()
                .RegisterType<IDealers, DealersRepository>()
                .RegisterType<IDealerPriceRepository, DealerPriceRepository>()
                .RegisterType<IShowroomPricesRepository, BikeShowroomPrices>()
                .RegisterType<ICacheManager, MemcacheManager>()
                .RegisterType<ISponsoredComparisonCacheRepository, SponsoredComparisonCacheRepository>()
                .RegisterType<ISponsoredComparison, SponsoredComparison>()
                .RegisterType<IContractCampaign, ContractCampaign>()
                .RegisterType<IBikeMakes, BikeMakes>()
                .RegisterType<ISponsoredComparisonRepository, SponsoredComparisonRepository>()
                .RegisterType<IBannerRepository, BannerRepository>()
                .RegisterType<IContractCampaign, ContractCampaign>()
                .RegisterType<IServiceCenter, ServiceCenter>()
                .RegisterType<IManageBookingAmountPage, ManageBookingAmountPage>()
                .RegisterType<IPageMetasRepository, PageMetasRepository>()
                .RegisterType<IServiceCenterRepository, ServiceCenterRepository>()
                .RegisterType<IBikeSeriesRepository, BikeSeriesRepository>()
                .RegisterType<IBikeSeries, BikeSeries>()
                .RegisterType<IAdSlotRepository, AdSlot>()
                .RegisterType<IBikeBodyStylesRepository, BikeBodyStyleRepository>()
                .RegisterType<IBikeBodyStyles, BikeBodyStyles>()
                .RegisterType<IAdOperation, AdOperation>()
                .RegisterType<IUsers, Users>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

        }
    }
}