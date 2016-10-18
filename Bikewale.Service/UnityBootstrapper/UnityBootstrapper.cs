﻿using Bikewale.BAL.AppDeepLinking;
using Bikewale.BAL.AutoComplete;
using Bikewale.BAL.BikeBooking;
using Bikewale.BAL.BikeData;
using Bikewale.BAL.Compare;
using Bikewale.BAL.Customer;
using Bikewale.BAL.Dealer;
using Bikewale.BAL.EditCMS;
using Bikewale.BAL.Pager;
using Bikewale.BAL.PriceQuote;
using Bikewale.BAL.Used.Search;
using Bikewale.BAL.UsedBikes;
using Bikewale.Cache.App;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Compare;
using Bikewale.Cache.Core;
using Bikewale.Cache.Location;
using Bikewale.Cache.Used;
using Bikewale.Cache.UsedBikes;
using Bikewale.Cache.UserReviews;
using Bikewale.DAL.App;
using Bikewale.DAL.AppAlert;
using Bikewale.DAL.BikeBooking;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Customer;
using Bikewale.DAL.Dealer;
using Bikewale.DAL.Feedback;
using Bikewale.DAL.Location;
using Bikewale.DAL.NewBikeSearch;
using Bikewale.DAL.Used;
using Bikewale.DAL.UsedBikes;
using Bikewale.DAL.UserReviews;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Customer;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.App;
using Bikewale.Interfaces.AppAlert;
using Bikewale.Interfaces.AppDeepLinking;
using Bikewale.Interfaces.AutoComplete;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Compare;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Interfaces.Feedback;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.MobileVerification;
using Bikewale.Interfaces.NewBikeSearch;
using Bikewale.Interfaces.Pager;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Interfaces.Used;
using Bikewale.Interfaces.Used.Search;
using Bikewale.Interfaces.UsedBikes;
using Bikewale.Interfaces.UserReviews;
using Microsoft.Practices.Unity;
using System;

namespace Bikewale.Service.UnityConfiguration
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 27 Aug 2015
    /// </summary>
    public static class UnityBootstrapper
    {
        /// <summary>
        /// Modified By :   Lucky Rathore on on 06 Nov. 2015.
        /// Description :   Register BikeCompareCacheRepository.
        /// Modified By :   Sumit Kate on 08 Dec 2015
        /// Description :   Register AppVersionRepository.
        /// Modified By :   Sumit Kate on 05 Feb 2016
        /// Description :   Register IBookingListing, BookingListingRepository
        /// Modified By :   Lucky Rathore on 10 March 2016
        /// Description :   Register IDeepLinking, DeepLinking
        /// Modified By :   Lucky Rathore on 21 March 2016
        /// Description :   Register IDealerCacheRepository, DealerCacheRepository
        /// Modified By :   Sumit Kate on 20 July 2016
        /// Description :   Register Road Test/Feature/Article BAL classes for CMS Controller constructor resolution
        /// Modified By :   Sajal Gupta on 10-10-2016
        /// Description :   Register usedBikeDetailsRepository, usedBikeDetails
        /// </summary>
        /// <returns></returns>
        public static IUnityContainer Initialize()
        {
            IUnityContainer container = new UnityContainer();

            container.RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>();
            container.RegisterType<IAutoSuggest, AutoSuggest>();
            container.RegisterType<IArea, AreaRepository>();
            container.RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>();
            container.RegisterType<IPager, Pager>();
            container.RegisterType<IBikeVersions<BikeVersionEntity, int>, BikeVersions<BikeVersionEntity, int>>();
            container.RegisterType<IBikeCompare, BikeComparison>();
            container.RegisterType<IDealerPriceQuote, DealerPriceQuote>();
            container.RegisterType<ICity, CityRepository>();
            container.RegisterType<IMobileVerificationRepository, Bikewale.BAL.MobileVerification.MobileVerification>();
            container.RegisterType<IMobileVerification, Bikewale.BAL.MobileVerification.MobileVerification>();
            container.RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>();
            container.RegisterType<IBikeVersions<BikeVersionEntity, uint>, BikeVersions<BikeVersionEntity, uint>>();
            container.RegisterType<IPriceQuote, BAL.PriceQuote.PriceQuote>();
            container.RegisterType<ICustomerAuthentication<CustomerEntity, UInt32>, CustomerAuthentication<CustomerEntity, UInt32>>();
            container.RegisterType<ICustomer<CustomerEntity, UInt32>, Customer<CustomerEntity, UInt32>>();
            container.RegisterType<ICustomerRepository<CustomerEntity, UInt32>, CustomerRepository<CustomerEntity, UInt32>>();
            container.RegisterType<IDealer, Dealer>();
            container.RegisterType<IFeedback, FeedbackRepository>();
            container.RegisterType<IBikeSeries<BikeSeriesEntity, int>>();
            container.RegisterType<IState, StateRepository>();
            container.RegisterType<IUsedBikesRepository, UsedBikesRepository>();
            container.RegisterType<IUserReviews, UserReviewsRepository>();
            container.RegisterType<IUserReviewsCache, UserReviewsCacheRepository>();
            container.RegisterType<ISearchResult, SearchResult>();
            container.RegisterType<IProcessFilter, ProcessFilter>();
            container.RegisterType<ICacheManager, MemcacheManager>();
            container.RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>();
            container.RegisterType<IPopularUsedBikesCacheRepository, PopularUsedBikesCacheRepository>();
            container.RegisterType<IBikeCompareCacheRepository, BikeCompareCacheRepository>();
            container.RegisterType<IAppVersion, AppVersionRepository>();
            container.RegisterType<IAppVersionCache, AppVersionCacheRepository>();
            container.RegisterType<IAppAlert, AppAlertRepository>();
            container.RegisterType<ICityCacheRepository, CityCacheRepository>();
            container.RegisterType<IAreaCacheRepository, AreaCacheRepository>();
            //container.RegisterType<IBookingCancellation, Bikewale.BAL.BikeBooking.BookingCancellation>();
            container.RegisterType<IBookingListing, BookingListingRepository>();
            container.RegisterType<IOffer, OfferRepository>();
            container.RegisterType<IDeepLinking, DeepLinking>();
            container.RegisterType<Bikewale.Interfaces.PriceQuote.IDealerPriceQuoteDetail, Bikewale.BAL.PriceQuote.DealerPriceQuoteDetail>();
            container.RegisterType<Bikewale.Interfaces.Dealer.IDealerCacheRepository, Bikewale.Cache.DealersLocator.DealerCacheRepository>();
            container.RegisterType<ILeadNofitication, LeadNotificationBL>();
            container.RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>();
            container.RegisterType<IBikeVersionCacheRepository<BikeVersionEntity, uint>, BikeVersionsCacheRepository<BikeVersionEntity, uint>>();
            container.RegisterType<ICMSCacheContent, CMSCacheRepository>();
            container.RegisterType<IArticles, Articles>();
            container.RegisterType<ISearch, SearchBikes>();
            container.RegisterType<ISearchFilters, ProcessSearchFilters>();
            container.RegisterType<Bikewale.Interfaces.Used.Search.ISearchQuery, Bikewale.BAL.Used.Search.SearchQuery>();
            container.RegisterType<ISearchRepository, Bikewale.DAL.Used.Search.SearchRepository>();

            container.RegisterType<IUsedBikeDetails, UsedBikeDetailsRepository>();
            container.RegisterType<IUsedBikesRepository, UsedBikesRepository>();
            container.RegisterType<IUsedBikeDetailsCacheRepository, UsedBikeDetailsCache>();
            container.RegisterType<IUsedBikes, Bikewale.BAL.UsedBikes.UsedBikes>();

            container.RegisterType<IUsedBikeBuyer, Bikewale.BAL.Used.UsedBikeBuyer>();
            container.RegisterType<IUsedBikeBuyerRepository, UsedBikeBuyerRepository>();
            container.RegisterType<IUsedBikeSellerRepository, UsedBikeSellerRepository>();

            container.RegisterType<ISellBikes, SellBikes>();
            container.RegisterType<ISellBikesRepository<SellBikeAd, int>, SellBikesRepository<SellBikeAd, int>>();
            return container;
        }
    }
}