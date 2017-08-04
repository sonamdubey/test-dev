﻿using Bikewale.BAL.ServiceCenter;
using Bikewale.ManufacturerCampaign.DAL;
using BikewaleOpr.BAL;
using BikewaleOpr.BAL.ContractCampaign;
using BikewaleOpr.BAL.Images;
using BikewaleOpr.BAL.Security;
using BikewaleOpr.BAL.ServiceCenter;
using BikewaleOpr.BAL.Used;
using BikewaleOpr.DALs.BikeColorImages;
using BikewaleOpr.DALs.Bikedata;
using BikewaleOpr.DALs.ContractCampaign;
using BikewaleOpr.DALs.Images;
using BikewaleOpr.DALs.ManufactureCampaign;
using BikewaleOpr.DALs.ServiceCenter;
using BikewaleOpr.DALs.UserReviews;
using BikewaleOpr.Interface.BikeColorImages;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.ContractCampaign;
using BikewaleOpr.Interface.Images;
using BikewaleOpr.Interface.ManufacturerCampaign;
using BikewaleOpr.Interface.Security;
using BikewaleOpr.Interface.ServiceCenter;
using BikewaleOpr.Interface.Used;
using BikewaleOpr.Interface.UserReviews;
using BikewaleOpr.Used;
using Microsoft.Practices.Unity;



namespace BikewaleOpr.Service.UnityConfiguration
{
    /// <summary>
    /// Created By : Sangram Nandkhile  05 July 2016
    /// modified By :- Subodh Jain 09 Jan 2017
    /// Summary : Added color bike repository
    /// Modified by :   Sumit Kate on 18 jan 2017
    /// Description :   Register IDealerCampaignRepository
    /// Modified by : Sajal Gupta on 03-03-2017
    /// Description : Register IBikeModels
    /// </summary>
    public static class UnityBootstrapper
    {
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static IUnityContainer Initialize()
        {
            IUnityContainer container = new UnityContainer();

            container.RegisterType<IManufacturerCampaignRepository, ManufacturerCampaign>();
            container.RegisterType<IContractCampaign, ContractCampaign>();
            container.RegisterType<IManufacturerReleaseMaskingNumber, ManufacturerReleaseMaskingNumber>();
            container.RegisterType<ISellerRepository, SellerRepository>();
            container.RegisterType<ISellBikes, SellBikes>();
            container.RegisterType<IColorImagesBikeRepository, ColorImagesBikeRepository>();
            container.RegisterType<IDealerCampaignRepository, DealerCampaignRepository>();
            container.RegisterType<IBikeMakes, BikeMakesRepository>();
            container.RegisterType<IBikeModelsRepository, BikeModelsRepository>();
            container.RegisterType<IImage, ImageBL>();
            container.RegisterType<IImageRepository, ImageRepository>();
            container.RegisterType<ISecurity, SecurityBL>();
            container.RegisterType<IUserReviewsRepository, UserReviewsRepository>();
            container.RegisterType<Bikewale.ManufacturerCampaign.Interface.IManufacturerCampaignRepository, Bikewale.ManufacturerCampaign.DAL.ManufacturerCampaignRepository>();
            container.RegisterType<IServiceCenter, ServiceCenter>();
            container.RegisterType<IServiceCenterRepository, ServiceCenterRepository>();


            return container;
        }
    }
}