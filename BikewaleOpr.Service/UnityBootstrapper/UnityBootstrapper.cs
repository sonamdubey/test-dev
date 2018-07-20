using Bikewale.Cache.Core;
using Bikewale.Comparison.BAL;
using Bikewale.Comparison.Cache;
using Bikewale.Comparison.DAL;
using Bikewale.Comparison.Interface;
using Bikewale.Interfaces.Cache.Core;
using BikewaleOpr.BAL;
using BikewaleOpr.BAL.Amp;
using BikewaleOpr.BAL.BikePricing;
using BikewaleOpr.BAL.ContractCampaign;
using BikewaleOpr.BAL.Images;
using BikewaleOpr.BAL.QuestionsAnswers;
using BikewaleOpr.BAL.Security;
using BikewaleOpr.BAL.ServiceCenter;
using BikewaleOpr.BAL.Used;
using BikewaleOpr.Cache.BikeData;
using BikewaleOpr.Cache.QuestionsAnswers;
using BikewaleOpr.DAL;
using BikewaleOpr.DALs;
using BikewaleOpr.DALs.AdOperation;
using BikewaleOpr.DALs.AdSlot;
using BikewaleOpr.DALs.Banner;
using BikewaleOpr.DALs.BikeColorImages;
using BikewaleOpr.DALs.Bikedata;
using BikewaleOpr.DALs.BikePricing;
using BikewaleOpr.DALs.ConfigurePageMetas;
using BikewaleOpr.DALs.ContractCampaign;
using BikewaleOpr.DALs.Images;
using BikewaleOpr.DALs.ManufactureCampaign;
using BikewaleOpr.DALs.QuestionsAnswers;
using BikewaleOpr.DALs.ServiceCenter;
using BikewaleOpr.DALs.UserReviews;
using BikewaleOpr.Interface;
using BikewaleOpr.Interface.AdSlot;
using BikewaleOpr.Interface.Amp;
using BikewaleOpr.Interface.Banner;
using BikewaleOpr.Interface.BikeColorImages;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.BikePricing;
using BikewaleOpr.Interface.ConfigurePageMetas;
using BikewaleOpr.Interface.ContractCampaign;
using BikewaleOpr.Interface.Dealers;
using BikewaleOpr.Interface.Images;
using BikewaleOpr.Interface.ManufacturerCampaign;
using BikewaleOpr.Interface.QnA;
using BikewaleOpr.Interface.QuestionsAnswers;
using BikewaleOpr.Interface.Security;
using BikewaleOpr.Interface.ServiceCenter;
using BikewaleOpr.Interface.Used;
using BikewaleOpr.Interface.UserReviews;
using BikewaleOpr.Used;
using Microsoft.Practices.Unity;
using QuestionsAnswers.BAL;
using QuestionsAnswers.DAL;

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
    /// Modified by : Vivek Singh Tomar on 1st Aug 2017
    /// Description : Added IBikeMakes
    /// Modified By : Ashutosh Sharma on 29-07-2017
    /// Description : Added IShowroomPricesRepository registration to BikeShowroomPrices
    /// Modified by : Ashutosh Sharma on 10 Nov 2017
    /// Description : Added registration for IBwPrice and BwPrice.
    /// Modified by : Rajan Chauhan on 13th Dec 2017
    /// Description : Added registration for IBikeBodyStyle and IBikeBodyStyleRepository
    /// Modified by : Sanskar Gupta on 13 June 2018
    /// Description : Added registration for `IQuestionsBAL`, `IQuestions`, `IQuestionsRepository`
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
            container.RegisterType<IBikeMakesRepository, BikeMakesRepository>();
            container.RegisterType<IBikeModelsRepository, BikeModelsRepository>();
            container.RegisterType<IImage, ImageBL>();
            container.RegisterType<IImageRepository, ImageRepository>();
            container.RegisterType<ISecurity, SecurityBL>();
            container.RegisterType<IBannerRepository, BannerRepository>();
            container.RegisterType<IUserReviewsRepository, UserReviewsRepository>();
            container.RegisterType<ISponsoredComparisonRepository, SponsoredComparisonRepository>();
            container.RegisterType<ICacheManager, MemcacheManager>();
            container.RegisterType<ISponsoredComparisonCacheRepository, SponsoredComparisonCacheRepository>();
            container.RegisterType<ISponsoredComparison, SponsoredComparison>();
            container.RegisterType<IBikeModels, BikeModels>();

            container.RegisterType<Bikewale.ManufacturerCampaign.Interface.IManufacturerCampaignRepository, Bikewale.ManufacturerCampaign.DAL.ManufacturerCampaignRepository>();
            container.RegisterType<Bikewale.ManufacturerCampaign.Interface.IManufacturerCampaign, Bikewale.ManufacturerCampaign.BAL.ManufacturerCampaign>();
            container.RegisterType<Bikewale.ManufacturerCampaign.Interface.IManufacturerCampaignCache, Bikewale.ManufacturerCampaign.Cache.ManufacturerCampaignCache>();
            container.RegisterType<IDealerPriceRepository, DealerPriceRepository>();
            container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
            container.RegisterType<IDealerPrice, DealerPrice>();
            container.RegisterType<IDealers, DealersRepository>();
            container.RegisterType<IVersionAvailability, VersionAvailability>();
            container.RegisterType<IShowroomPricesRepository, BikeShowroomPrices>();
            container.RegisterType<IBikeMakes, BikeMakes>();
            container.RegisterType<IServiceCenter, ServiceCenter>();
            container.RegisterType<IServiceCenterRepository, ServiceCenterRepository>();
            container.RegisterType<IBikeSeries, BikeSeries>();
            container.RegisterType<IBikeSeriesRepository, BikeSeriesRepository>();
            
            container.RegisterType<IPageMetasRepository, PageMetasRepository>();
            container.RegisterType<IPageMetas, PageMetas>();
            container.RegisterType<IAdSlotRepository, AdSlot>();
            container.RegisterType<IAdSlot, BAL.AdSlot.AdSlot>();
            container.RegisterType<IBwPrice, BwPrice>();
            container.RegisterType<IBikeBodyStylesRepository, BikeBodyStyleRepository>();
            container.RegisterType<IBikeBodyStyles, BikeBodyStyles>();
            container.RegisterType<IAdOperation, AdOperation>();
            container.RegisterType<IBWCache, BWCache>();
            container.RegisterType<IBikeVersionsCacheRepository, BikeVersionsCacheRepository>();
            container.RegisterType<IBikeVersions, BikeVersionsRepository>();
            container.RegisterType<QuestionsAnswers.BAL.IQuestions, QuestionsAnswers.BAL.Questions>();
            container.RegisterType<QuestionsAnswers.DAL.IQuestionsRepository, QuestionsAnswers.DAL.QuestionsRepository>();
            container.RegisterType<QuestionsAnswers.Cache.ICacheManager, QuestionsAnswers.Cache.MemcacheManager>();
            container.RegisterType<QuestionsAnswers.Cache.IQuestionsCacheRepository, QuestionsAnswers.Cache.QuestionsCacheRepository>();
            container.RegisterType<IQuestionsBAL, QuestionsBAL>();
            container.RegisterType<BikewaleOpr.Interface.QuestionsAnswers.IQuestionsRepository, BikewaleOpr.DALs.QuestionsAnswers.QuestionsRepository>();

            container.RegisterType<IQuestionsAndAnswersCache, QuestionsAndAnswersCache>();
            container.RegisterType<IAnswersBAL, AnswersBAL>();
            container.RegisterType<IAnswersRepository, AnswersRepository>();
            container.RegisterType<IAnswers, Answers>();
            container.RegisterType<IUsersRepository, UsersRepository>();
            container.RegisterType<IAmpCache, AmpCache>();
            return container;
        }
    }
}