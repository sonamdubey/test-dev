using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bikewale.BAL.AutoComplete;
using Bikewale.BAL.BikeBooking;
using Bikewale.BAL.BikeData;
using Bikewale.BAL.Customer;
using Bikewale.BAL.Pager;
using Bikewale.DAL.BikeBooking;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Compare;
using Bikewale.DAL.Location;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Customer;
using Bikewale.Interfaces.AutoComplete;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Compare;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.Feedback;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.MobileVerification;
using Bikewale.Interfaces.Pager;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Interfaces.UsedBikes;
using Bikewale.Interfaces.UserReviews;
using Microsoft.Practices.Unity;
using Bikewale.DAL.UsedBikes;
using Bikewale.DAL.Feedback;
using Bikewale.DAL.UserReviews;
using Bikewale.Interfaces.NewBikeSearch;
using Bikewale.DAL.NewBikeSearch;
using Bikewale.BAL.Dealer;
using Bikewale.Interfaces.Dealer;
using Bikewale.DAL.Customer;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Cache.Core;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Compare;
using Bikewale.Cache.UsedBikes;
using Bikewale.Interfaces.App;
using Bikewale.DAL.App;
using Bikewale.Interfaces.AppAlert;
using Bikewale.DAL.AppAlert;
using Bikewale.BAL.Compare;
using Bikewale.Cache.Location;

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
            container.RegisterType<IFeedback,FeedbackRepository>();
            container.RegisterType<IBikeSeries<BikeSeriesEntity, int>>();
            container.RegisterType<IState,StateRepository>();
            container.RegisterType<IUsedBikes, UsedBikesRepository>();
            container.RegisterType<IUserReviews, UserReviewsRepository>();
            container.RegisterType<ISearchResult, SearchResult>();
            container.RegisterType<IProcessFilter, ProcessFilter>();
            container.RegisterType<ICacheManager, MemcacheManager>();
            container.RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>();
            container.RegisterType<IPopularUsedBikesCacheRepository,PopularUsedBikesCacheRepository>();
            container.RegisterType<IBikeCompareCacheRepository, BikeCompareCacheRepository>();
            container.RegisterType<IAppVersion, AppVersionRepository>();
            container.RegisterType<IAppAlert, AppAlertRepository>();
            container.RegisterType<ICityCacheRepository, CityCacheRepository>();
            container.RegisterType<IAreaCacheRepository, AreaCacheRepository>();           
            container.RegisterType<IBookingCancellation, Bikewale.BAL.BikeBooking.BookingCancellation>();
            return container;
        }
    }
}