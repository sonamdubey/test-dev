using Bikewale.BAL.AdSlot;
using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.BAL.Authors;
using Bikewale.BAL.BikeBooking;
using Bikewale.BAL.BikeData;
using Bikewale.BAL.BikeData.NewLaunched;
using Bikewale.BAL.BikeData.UpComingBike;
using Bikewale.BAL.BikeSearch;
using Bikewale.BAL.Campaign;
using Bikewale.BAL.CMS;
using Bikewale.BAL.Customer;
using Bikewale.BAL.Dealer;
using Bikewale.BAL.EditCMS;
using Bikewale.BAL.Filters;
using Bikewale.BAL.Lead;
using Bikewale.BAL.Pager;
using Bikewale.BAL.PriceQuote;
using Bikewale.BAL.PWA.CMS;
using Bikewale.BAL.QuestionAndAnswers;
using Bikewale.BAL.ServiceCenter;
using Bikewale.BAL.UsedBikes;
using Bikewale.BAL.UserProfile;
using Bikewale.BAL.UserReviews;
using Bikewale.BAL.UserReviews.Search;
using Bikewale.Cache.AdSlot;
using Bikewale.Cache.Authors;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.BikeSearch;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Compare;
using Bikewale.Cache.Core;
using Bikewale.Cache.DealersLocator;
using Bikewale.Cache.Finance;
using Bikewale.Cache.Helper.PriceQuote;
using Bikewale.Cache.HomePage;
using Bikewale.Cache.Location;
using Bikewale.Cache.PriceQuote;
using Bikewale.Cache.PWA.CMS;
using Bikewale.Cache.QuestionAndAnswers;
using Bikewale.Cache.ServiceCenter;
using Bikewale.Cache.Used;
using Bikewale.Cache.UsedBikes;
using Bikewale.Cache.UserReviews;
using Bikewale.Cache.Videos;
using Bikewale.CacheHelper.BikeData;
using Bikewale.Comparison.BAL;
using Bikewale.Comparison.Cache;
using Bikewale.Comparison.DAL;
using Bikewale.Comparison.Interface;
using Bikewale.DAL;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Compare;
using Bikewale.DAL.Customer;
using Bikewale.DAL.Dealer;
using Bikewale.DAL.Finance.CapitalFirst;
using Bikewale.DAL.HomePage;
using Bikewale.DAL.Location;
using Bikewale.DAL.QuestionAndAnswers;
using Bikewale.DAL.ServiceCenter;
using Bikewale.DAL.Used;
using Bikewale.DAL.UsedBikes;
using Bikewale.DAL.UserReviews;
using Bikewale.DAL.Videos;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Customer;
using Bikewale.Entities.service;
using Bikewale.Interfaces;
using Bikewale.Interfaces.AdSlot;
using Bikewale.Interfaces.Authors;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Campaign;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Compare;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Interfaces.Filters;
using Bikewale.Interfaces.Finance;
using Bikewale.Interfaces.Finance.CapitalFirst;
using Bikewale.Interfaces.HomePage;
using Bikewale.Interfaces.Lead;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.NewBikeSearch;
using Bikewale.Interfaces.Pager;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Interfaces.PWA.CMS;
using Bikewale.Interfaces.QuestionAndAnswers;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Interfaces.Used;
using Bikewale.Interfaces.UsedBikes;
using Bikewale.Interfaces.UserProfile;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Interfaces.UserReviews.Search;
using Bikewale.Interfaces.Videos;
using Bikewale.ManufacturerCampaign.Interface;
using Microsoft.Practices.Unity;
using System;
using System.Web.Mvc;
using Unity.Mvc5;

namespace Bikewale
{
    /// <summary>
    /// Created by : Ashish G. Kamble on 6 Jan 2017
    /// Modified by :   Sumit Kate on 29 Jun 2017
    /// Description :   Register Manufacturer campaign interfaces
    /// Modified by : Vivek Singh Tomar on 27th Sep 2017
    /// Description : Added IBikeSeriesRepository
    /// Modified by : Ashutosh Sharma on 31 Oct 2017
    /// Description : Added IAdSlot, IAdSlotCacheRepository, IAdSlotRepository.
    /// Modified by :   Sumit Kate on 05 Jan 2018
    /// Description :   Register IBikeSearchCacheRepository
    /// Modified by :   Rajan Chauhan on 11 Apr 2018
    /// Description :   Registered IBikeVersionRepository
    /// Modified by : Sanskar Gupta on 03 July 2018
    /// Description : Registered `IUserProfileBAL`
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
            container.RegisterType<IBikeMakesCacheRepository, BikeMakesCacheRepository>();
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
            container.RegisterType<IBikeModelsCacheRepository<int>>();
            container.RegisterType<IBikeVersions<BikeVersionEntity, uint>, BikeVersions<BikeVersionEntity, uint>>();
            container.RegisterType<IBikeVersionCacheRepository<BikeVersionEntity, uint>, BikeVersionsCacheRepository<BikeVersionEntity, uint>>();
            container.RegisterType<IBikeVersionsRepository<BikeVersionEntity, uint>, BikeVersionsRepository<BikeVersionEntity, uint>>();
            container.RegisterType<IHomePageBannerRepository, HomePageBannerRepository>();
            container.RegisterType<IHomePageBannerCacheRepository, HomePageBannerCacheRepository>();
            container.RegisterType<ICityMaskingCacheRepository, CityMaskingCache>();
            container.RegisterType<IPriceQuote, BAL.PriceQuote.PriceQuote>();
            container.RegisterType<IPriceQuoteCache, PriceQuoteCache>();
            container.RegisterType<IUserReviewsCache, UserReviewsCacheRepository>();
            container.RegisterType<IUserReviewsRepository, UserReviewsRepository>();
            container.RegisterType<IUserReviewsSearch, UserReviewsSearch>();
            container.RegisterType<IUserReviews, UserReviews>();
            container.RegisterType<IUsedBikesCache, UsedBikesCache>();
            container.RegisterType<IUsedBikes, UsedBikes>();
            container.RegisterType<IUsedBikesRepository, UsedBikesRepository>();
            container.RegisterType<IBikeCompare, Bikewale.BAL.Compare.BikeComparison>();

            container.RegisterType<ICustomer<CustomerEntity, uint>, Customer<CustomerEntity, uint>>();
            container.RegisterType<ICustomerRepository<CustomerEntity, uint>, CustomerRepository<CustomerEntity, uint>>();
            container.RegisterType<IPWACMSContentRepository, PWACMSRenderedData>();
            container.RegisterType<IPWACMSCacheRepository, PWACMSCacheRepository>();
            container.RegisterType<ICMS, CMS>();
            container.RegisterType<ISurveyRepository, SurveyRepository>();
            container.RegisterType<ISurvey, BAL.Survey>();
            container.RegisterType<IManufacturerCampaign, Bikewale.ManufacturerCampaign.BAL.ManufacturerCampaign>();
            container.RegisterType<IManufacturerCampaignCache, Bikewale.ManufacturerCampaign.Cache.ManufacturerCampaignCache>();
            container.RegisterType<Bikewale.ManufacturerCampaign.Interface.IManufacturerCampaignRepository, Bikewale.ManufacturerCampaign.DAL.ManufacturerCampaignRepository>();
            container.RegisterType<ISponsoredComparisonCacheRepository, SponsoredComparisonCacheRepository>();
            container.RegisterType<ISponsoredComparison, SponsoredComparison>();
            container.RegisterType<ISponsoredComparisonRepository, SponsoredComparisonRepository>();
            container.RegisterType<IAuthors, Authors>();
            container.RegisterType<IAuthorsCacheRepository, AuthorsCacheRepository>();
            container.RegisterType<IBikeSeriesRepository, BikeSeriesRepository>();
            container.RegisterType<IBikeSeries, BikeSeries>();
            container.RegisterType<IBikeSeriesCacheRepository, BikeSeriesCacheRepository>();
            container.RegisterType<IAdSlot, AdSlot>();
            container.RegisterType<IAdSlotCacheRepository, AdSlotCacheRepository>();
            container.RegisterType<IAdSlotRepository, DAL.AdSlot.AdSlot>();
            container.RegisterType<ISearchResult, DAL.NewBikeSearch.SearchResult>();
            container.RegisterType<IProcessFilter, DAL.NewBikeSearch.ProcessFilter>();
            container.RegisterType<IBikeSearchResult, BikeSearchResult>();
            container.RegisterType<IBikeSearchCacheRepository, BikeSearchCacheRepository>();
            container.RegisterType<IPageFilters, PageFilters>();
            container.RegisterType<IPQByCityArea, PQByCityArea>();
            container.RegisterType<Bikewale.Interfaces.AutoBiz.IDealerPriceQuote, Bikewale.DAL.AutoBiz.DealerPriceQuoteRepository>();
            container.RegisterType<IBikeModelsCacheHelper, BikeModelsCacheHelper>();
            container.RegisterType<ILead, LeadProcess>();
            container.RegisterType<IApiGatewayCaller, ApiGatewayCaller>();
            container.RegisterType<Bikewale.Interfaces.AutoBiz.IDealers, Bikewale.DAL.AutoBiz.DealersRepository>();
            container.RegisterType<IBikeSearch, BikeSearch>();
            container.RegisterType<IDealer, Dealer>();
            container.RegisterType<IDealerPriceQuoteCache, DealerPriceQuoteCache>();
            container.RegisterType<IFinanceCacheRepository, FinanceCacheRepository>();
            container.RegisterType<IFinanceRepository, FinanceRepository>();
            container.RegisterType<IUserProfileBAL, UserProfileBAL>();
            container.RegisterType<IQuestions, Questions>();
            container.RegisterType<IQuestionsCacheRepository, QuestionsCacheRepository>();
            container.RegisterType<IQuestionsRepository, QuestionsRepository>();
            container.RegisterType<QuestionsAnswers.BAL.IQuestions, QuestionsAnswers.BAL.Questions>();
            container.RegisterType<QuestionsAnswers.DAL.IQuestionsRepository, QuestionsAnswers.DAL.QuestionsRepository>();
            container.RegisterType<QuestionsAnswers.Cache.ICacheManager, QuestionsAnswers.Cache.MemcacheManager>();
            container.RegisterType<QuestionsAnswers.Cache.IQuestionsCacheRepository, QuestionsAnswers.Cache.QuestionsCacheRepository>();
            container.RegisterType<ICustomerAuthentication<CustomerEntity, UInt32>, CustomerAuthentication<CustomerEntity, UInt32>>();
            container.RegisterType<IAnswerRepository, AnswerRepository>();
            container.RegisterType<IAnswers, Answers>();
            container.RegisterType<ICampaignBL, CampaignBL>();
            container.RegisterType<IPriceQuoteCacheHelper, PriceQuoteCacheHelper>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}