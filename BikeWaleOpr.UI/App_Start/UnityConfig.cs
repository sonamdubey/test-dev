using Bikewale.Comparison.BAL;
using Bikewale.Comparison.Cache;
using Bikewale.Comparison.DAL;
using Bikewale.Comparison.Interface;
using Bikewale.ManufacturerCampaign.DAL;
using BikewaleOpr.BAL;
using BikewaleOpr.BAL.ContractCampaign;
using BikewaleOpr.BAL.Used;
using BikewaleOpr.CommuteDistance;
using BikewaleOpr.DALs.Bikedata;
using BikewaleOpr.DALs.ContractCampaign;
using BikewaleOpr.DALs.Location;
using BikewaleOpr.DALs.UserReviews;
using BikewaleOpr.Interface;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.ContractCampaign;
using BikewaleOpr.Interface.Location;
using BikewaleOpr.Interface.Used;
using BikewaleOpr.Interface.UserReviews;
using Microsoft.Practices.Unity;
using System.Web.Mvc;
using Unity.Mvc5;
using BikewaleOpr.Interface.UserReviews;
using BikewaleOpr.DALs.UserReviews;
using BikewaleOpr.Interface.ContractCampaign;
using BikewaleOpr.DALs.ContractCampaign;
using BikewaleOpr.CommuteDistance;
using BikewaleOpr.Interface.Location;
using BikewaleOpr.DALs.Location;
using BikewaleOpr.BAL.ContractCampaign;
using Bikewale.ManufacturerCampaign.DAL;
using BikewaleOpr.Interface.Banner;
using BikewaleOpr.DALs.Banner;
using BikewaleOpr.Interface.ServiceCenter;
using BikewaleOpr.BAL.ServiceCenter;
using BikewaleOpr.DALs.ServiceCenter;

namespace BikewaleOpr
{
    /// <summary>
    /// Created by : Ashish G. Kamble on 6 Jan 2017
    /// Modified by : Sajal Gupta on 09-03-2017
    /// Description : Added IBikeModels, IUsedBikes, IHomePage
    /// </summary>
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterType<IBikeMakes, BikeMakesRepository>()
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
                .RegisterType<ISponsoredComparisonCacheRepository, SponsoredComparisonCacheRepository>()
                .RegisterType<ISponsoredComparison, SponsoredComparison>()
                .RegisterType<ISponsoredComparisonRepository, SponsoredComparisonRepository>()
                .RegisterType<IBannerRepository, BannerRepository>()
                .RegisterType<IContractCampaign, ContractCampaign>()
                .RegisterType<IServiceCenter, ServiceCenter>()
                .RegisterType<IServiceCenterRepository, ServiceCenterRepository>();




            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

        }
    }
}