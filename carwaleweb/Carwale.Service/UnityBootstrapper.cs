using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.Adapters;
using Carwale.BL;
using Carwale.BL.Accessories.Tyres;
using Carwale.BL.Advertizing.Apps;
using Carwale.BL.AppSiteAssociation;
using Carwale.BL.AutoComplete;
using Carwale.BL.Campaigns;
using Carwale.BL.CarData;
using Carwale.BL.CentralizedCacheRefresh;
using Carwale.BL.Classified.CarDetail;
using Carwale.BL.Classified.CarValuation;
using Carwale.BL.Classified.Chat;
using Carwale.BL.Classified.Leads;
using Carwale.BL.Classified.MyListings;
using Carwale.BL.Classified.PopularUsedCars;
using Carwale.BL.Classified.Search;
using Carwale.BL.Classified.Stock;
using Carwale.BL.Classified.UsedSellCar;
using Carwale.BL.CMS;
using Carwale.BL.CompareCars;
using Carwale.BL.CrossSell;
using Carwale.BL.Customers;
using Carwale.BL.CustomerVerification;
using Carwale.BL.Dealers;
using Carwale.BL.Deals;
using Carwale.BL.Elastic;
using Carwale.BL.Elastic.NewCarSearch;
using Carwale.BL.ES;
using Carwale.BL.Finance;
using Carwale.BL.GeoLocation;
using Carwale.BL.Home;
using Carwale.BL.ImageUpload;
using Carwale.BL.Insurance;
using Carwale.BL.Interface;
using Carwale.BL.LeadForm;
using Carwale.BL.Monetization;
using Carwale.BL.NewCars;
using Carwale.BL.Notifications;
using Carwale.BL.PaymentGateway;
using Carwale.BL.PriceQuote;
using Carwale.BL.SponsoredCar;
using Carwale.BL.Stock;
using Carwale.BL.Template;
using Carwale.BL.TestDrive;
using Carwale.BL.ThirdParty.Leads;
using Carwale.BL.Tracking;
using Carwale.BL.UserProfiling;
using Carwale.Cache.Accessories.Tyres;
using Carwale.Cache.Advertizing.App;
using Carwale.Cache.Campaigns;
using Carwale.Cache.CarData;
using Carwale.Cache.Classification;
using Carwale.Cache.Classified;
using Carwale.Cache.CompareCars;
using Carwale.Cache.CrossSell;
using Carwale.Cache.Dealers;
using Carwale.Cache.Deals;
using Carwale.Cache.ES;
using Carwale.Cache.Finance;
using Carwale.Cache.Geolocation;
using Carwale.Cache.IPToLocation;
using Carwale.Cache.LandingPage;
using Carwale.Cache.PriceQuote;
using Carwale.Cache.Security;
using Carwale.Cache.SponsoredData;
using Carwale.Cache.Stock;
using Carwale.Cache.Template;
using Carwale.Cache.UserProfiling;
using Carwale.DAL.Accessories.Tyres;
using Carwale.DAL.Advertizings.App;
using Carwale.DAL.ApiGateway;
using Carwale.DAL.ApiGateway.ApiGatewayHelper;
using Carwale.DAL.Blocking;
using Carwale.DAL.Campaigns;
using Carwale.DAL.CarData;
using Carwale.DAL.CarFinance;
using Carwale.DAL.Classification;
using Carwale.DAL.Classified;
using Carwale.DAL.Classified.CarDetails;
using Carwale.DAL.Classified.CarValuation;
using Carwale.DAL.Classified.Chat;
using Carwale.DAL.Classified.Leads;
using Carwale.DAL.Classified.MyListings;
using Carwale.DAL.Classified.SellCar;
using Carwale.DAL.Classified.UsedCarPhotos;
using Carwale.DAL.Classified.UsedLeads;
using Carwale.DAL.CompareCars;
using Carwale.DAL.Customers;
using Carwale.DAL.CustomerVerification;
using Carwale.DAL.Dealers;
using Carwale.DAL.Deals;
using Carwale.DAL.ES;
using Carwale.DAL.Finance;
using Carwale.DAL.Geolocation;
using Carwale.DAL.Insurance;
using Carwale.DAL.IPToLocation;
using Carwale.DAL.LandingPage;
using Carwale.DAL.Logs;
using Carwale.DAL.Notifications;
using Carwale.DAL.PaymentGateway;
using Carwale.DAL.PriceQuote;
using Carwale.DAL.ProfanityFilter;
using Carwale.DAL.SponsoredCar;
using Carwale.DAL.Stock;
using Carwale.DAL.Subscription;
using Carwale.DAL.Template;
using Carwale.DAL.UserProfiling;
using Carwale.DTOs.CarData;
using Carwale.DTOs.Deals;
using Carwale.DTOs.Finance;
using Carwale.Entity;
using Carwale.Entity.Dealers;
using Carwale.Entity.Deals;
using Carwale.Entity.Finance;
using Carwale.Entity.Geolocation;
using Carwale.Entity.PriceQuote;
using Carwale.Entity.ThirdParty.Leads;
using Carwale.Interfaces;
using Carwale.Interfaces.Accessories.Tyres;
using Carwale.Interfaces.Advertizings.App;
using Carwale.Interfaces.Author;
using Carwale.Interfaces.AutoComplete;
using Carwale.Interfaces.Blocking;
using Carwale.Interfaces.Campaigns;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CarData.RecentLaunchedCar;
using Carwale.Interfaces.CentralizedCacheRefresh;
using Carwale.Interfaces.Classification;
using Carwale.Interfaces.Classified;
using Carwale.Interfaces.Classified.CarDetail;
using Carwale.Interfaces.Classified.CarValuation;
using Carwale.Interfaces.Classified.Chat;
using Carwale.Interfaces.Classified.Leads;
using Carwale.Interfaces.Classified.MyListings;
using Carwale.Interfaces.Classified.PopularUsedCarsDetails;
using Carwale.Interfaces.Classified.Search;
using Carwale.Interfaces.Classified.SellCar;
using Carwale.Interfaces.Classified.UsedCarPhotos;
using Carwale.Interfaces.CMS;
using Carwale.Interfaces.CompareCars;
using Carwale.Interfaces.CrossSell;
using Carwale.Interfaces.Customer;
using Carwale.Interfaces.CustomerVerification;
using Carwale.Interfaces.Dealers;
using Carwale.Interfaces.Deals;
using Carwale.Interfaces.Deals.Cache;
using Carwale.Interfaces.Elastic;
using Carwale.Interfaces.ES;
using Carwale.Interfaces.Finance;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.IAppSiteAssociation;
using Carwale.Interfaces.ImageUpload;
using Carwale.Interfaces.Insurance;
using Carwale.Interfaces.IPToLocation;
using Carwale.Interfaces.LandingPage;
using Carwale.Interfaces.LeadForm;
using Carwale.Interfaces.Leads;
using Carwale.Interfaces.Logs;
using Carwale.Interfaces.Monetization;
using Carwale.Interfaces.NewCars;
using Carwale.Interfaces.Notifications;
using Carwale.Interfaces.PaymentGateway;
using Carwale.Interfaces.PriceQuote;
using Carwale.Interfaces.Prices;
using Carwale.Interfaces.ProfanityFilter;
using Carwale.Interfaces.SponsoredCar;
using Carwale.Interfaces.Stock;
using Carwale.Interfaces.Subscription;
using Carwale.Interfaces.Template;
using Carwale.Interfaces.UserProfiling;
using Carwale.Interfaces.Users;
using Carwale.Notifications;
using Carwale.Notifications.Classified;
using Carwale.Service.Adapters.NewCars;
using Carwale.Service.Adapters.PriceQuote;
using Carwale.Service.Adapters.Prices;
using Carwale.Service.APIServices;
using Carwale.Service.Containers;
using Microsoft.Practices.Unity;
using System;
using System.Configuration;

namespace Carwale.Service
{
    public static class UnityBootstrapper
    {
        public static UnityResolver Resolver = new UnityResolver(Initialise());

        public static T Resolve<T>()
        {
            return (T)Resolver.BeginScope().GetService(typeof(T));
        }

        public static T Resolve<T>(string name)
        {
            return (T)Resolver.GetContainer().Resolve(typeof(T), name);
        }

        public static IUnityContainer Initialise()
        {

            var container = new UnityContainer();

            container.RegisterType<ICacheManager, CacheManager>()

                // RegisterType for PQ 
                .RegisterType<IPQAdapter, PQAdapterAndroid>("pqAndroidAdapter")
                .RegisterType<IPQAdapter, PQAdapterAndroidV1>("pqAndroidAdapterv1")
                .RegisterType<IPQAdapter, PQAdapterAndroidV2>("pqAndroidAdapterv2")
                .RegisterType<IPQAdapter, PQAdapterDesktopV1>("pqDesktopAdapterv1")
                .RegisterType<IPQRepository, PQRepository>()
                .RegisterType<IPQCacheRepository, PQCacheRepository>()
                .RegisterType<IPriceQuoteBL, PriceQuoteBL>()
                .RegisterType<IPricesRepository<CarPriceQuote, VersionPriceQuote>, PricesRepository<CarPriceQuote, VersionPriceQuote>>()
                .RegisterType<IPricesCacheRepository<CarPriceQuote, VersionPriceQuote>, PricesCacheRepository<CarPriceQuote, VersionPriceQuote>>()
                .RegisterType<ICarDataRepository, CarDataRepository>()
                .RegisterType<ICarPriceQuoteAdapter, CarPriceQuoteAdapter>(new ContainerControlledLifetimeManager())
                .RegisterType<IPriceAdapter, PriceAdapter>()
                .RegisterType<INearbyCitiesSearch, NearbyCitiesSearch>()

                // Register Car Makes
                .RegisterType<ICarMakesRepository, CarMakesRepository>(new ContainerControlledLifetimeManager())
                .RegisterType<ICarMakesCacheRepository, CarMakesCacheRepository>(new ContainerControlledLifetimeManager())
                .RegisterType<ICarMakes, CarMakesBL>(new ContainerControlledLifetimeManager())

                // Make Page
                .RegisterType<IServiceAdapterV2, Carwale.BL.NewCars.UpcomingCarListAdapter>("UpcomingCarsList", new ContainerControlledLifetimeManager())
                .RegisterType<IServiceAdapterV2, MakePageAdapterDesktop>("MakePageDesktop", new ContainerControlledLifetimeManager())
                //Make page mobile
                .RegisterType<IServiceAdapterV2, MakePageAdapterMobile>("MakePageMobile", new ContainerControlledLifetimeManager())


                //Model Page
                .RegisterType<IServiceAdapterV2, ModelPageAdapterMobile>("ModelPageMobile", new ContainerControlledLifetimeManager())
                //version mobile page
                .RegisterType<IServiceAdapterV2, VersionPageAdapterMobile>("VersionPageMobile", new ContainerControlledLifetimeManager())
                //Model Page
                .RegisterType<IServiceAdapterV2, ModelPageAdapterDesktop>("ModelPageDesktop", new ContainerControlledLifetimeManager())
                //Android
                .RegisterType<IServiceAdapter, ModelPageAdapterAndroid>("ModelPageAndroid", new PerResolveLifetimeManager())
                //Android
                .RegisterType<IServiceAdapter, ModelPageAdapterAndroid_V1>("ModelPageAndroid_V1", new PerResolveLifetimeManager())
                .RegisterType<IServiceAdapterV2, ModelPageAdapterApp_V2>("ModelPageApp_V2", new ContainerControlledLifetimeManager())
                .RegisterType<IServiceAdapterV2, PriceInCityAdapter>("PriceInCity", new ContainerControlledLifetimeManager())
                //Mileage
                .RegisterType<IServiceAdapterV2, MileageAdapter>("Mileage", new ContainerControlledLifetimeManager())

                .RegisterType<IServiceAdapterV2, VersionPageAdapterDesktop>("VersionPageDesktop", new ContainerControlledLifetimeManager())
                .RegisterType<IServiceAdapterV2, VersionPageAdapterApp>("VersionPageAppAdapter", new ContainerControlledLifetimeManager())
                .RegisterType<Carwale.Interfaces.Home.IServiceAdapter, HomePageAdapter>("HomePageAdaptor", new PerResolveLifetimeManager())
                // Registry for Compare car Landing page
                .RegisterType<Carwale.Interfaces.NewCars.IServiceAdapter, CompareCarAdapter>("CompareCarAdapter", new PerResolveLifetimeManager())
                .RegisterType<IServiceAdapterV2, CompareCarDetailsAdapter>("CompareCarDetailsAdapterDesktop", new ContainerControlledLifetimeManager())
                .RegisterType<IServiceAdapterV2, Carwale.BL.NewCars.CompareCarDetailsAdaptorMobile>("CompareCarDetailsAdapterMobile", new ContainerControlledLifetimeManager())
                .RegisterType<Carwale.Interfaces.Home.IServiceAdapter, AppAdapterHome>("AppAdapterHome", new PerResolveLifetimeManager())
                .RegisterType<IServiceAdapterV2, VersionPageAdapterAndroidV1>("VersionPageAdapterAndroidV1", new ContainerControlledLifetimeManager())
                .RegisterType<Carwale.Interfaces.Home.IServiceAdapter, AppAdapterHomeV2>("AppAdapterHomeV2", new PerResolveLifetimeManager())
                .RegisterType<IServiceAdapterV2, AppAdapterHomeV3>("AppAdapterHomeV3", new ContainerControlledLifetimeManager())
                .RegisterType<IServiceAdapter, AppAdapterNewCars>("AppAdapterNewCars", new PerResolveLifetimeManager())
                .RegisterType<Carwale.Interfaces.Home.IServiceAdapter, AppAdapterNewCarsV2>("AppAdapterNewCarsV2", new PerResolveLifetimeManager())
                .RegisterType<IServiceAdapterV2, AppAdapterNewCarsV3>("AppAdapterNewCarsV3", new ContainerControlledLifetimeManager())
                .RegisterType<Carwale.Interfaces.NewCars.IServiceAdapter, GenericContentDetailAdapter>("GenericContentDetailAdaptor")
                //TopCarsByBodyType
                .RegisterType<IServiceAdapterV2, TopCarsByBodyTypeAdaptor>("TopCarsByBodyTypeAdaptor", new ContainerControlledLifetimeManager())

                // RegisterType for CarModels
                .RegisterType<ICarModels, CarModelsBL>(new ContainerControlledLifetimeManager())
                .RegisterType<ICarRecommendationLogic, CarRecommendationLogic>(new ContainerControlledLifetimeManager())
                .RegisterType<ICarModelRepository, CarModelsRepository>(new ContainerControlledLifetimeManager())
                .RegisterType<ICarModelCacheRepository, CarModelsCacheRepository>(new ContainerControlledLifetimeManager())
                .RegisterType<IModelRootsRepository, CarModelRootsRepository>(new ContainerControlledLifetimeManager())
                .RegisterType<ICarModelRootsCacheRepository, CarModelRootsCacheRepository>(new ContainerControlledLifetimeManager())
                .RegisterType<IPrices, CarPrices>(new ContainerControlledLifetimeManager())
                .RegisterType<INewCarSearchAppAdapter, NewCarSearchAppAdapter>()
                .RegisterType<NewCarElasticSearch>()

                // RegisterType for Car Versions
                .RegisterType<ICarVersionRepository, CarVersionsRepository>(new ContainerControlledLifetimeManager())
                .RegisterType<ICarVersionCacheRepository, CarVersionsCacheRepository>(new ContainerControlledLifetimeManager())
                .RegisterType<ICarVersions, CarVersionsBL>(new ContainerControlledLifetimeManager())
                //Recent Car Launches
                .RegisterType<IRecentLaunchedCarCacheRepository, RecentLaunchedCarCache>(new ContainerControlledLifetimeManager())
                .RegisterType<IRecentLaunchedCarRepository, RecentLaunchedCarRepository>(new ContainerControlledLifetimeManager())

                //RegisterType for Car Comparison
                .RegisterType<IServiceAdapterV2, CompareCarAdapterApp>("CompareCarApp", new ContainerControlledLifetimeManager())
                .RegisterType<ICompareCarsBL, CCData>(new ContainerControlledLifetimeManager())
                .RegisterType<ICompareCarsCacheRepository, CompareCarsCache>(new ContainerControlledLifetimeManager())
                .RegisterType<ICompareCarsRepository, CompareCarsRepository>(new ContainerControlledLifetimeManager())

                //RegisterType for Car Mileage
                .RegisterType<ICarMileage, Mileage>(new ContainerControlledLifetimeManager())

                // RegisterType for Sponsored dealer ad on PQ
                .RegisterType<IDealerSponsoredAdRespository, DealerSponsoredAdRespository>()
                .RegisterType<IDealerSponsoredAdCache, DealerSponsoredAdCache>()

                // RegisterType for user's Geo locations,  state , cities
                .RegisterType<IGeoCitiesRepository, GeoCitiesRepository>()
                .RegisterType<IGeoCitiesCacheRepository, GeoCitiesCacheRepository>(new ContainerControlledLifetimeManager())
                .RegisterType<IElasticLocation, ElasticLocation>(new ContainerControlledLifetimeManager())
                .RegisterType<IPQGeoLocationBL, PQGeoLocationBL>()
                .RegisterType<IRepository<Cities>, GeoCitiesCache>()

                // RegisterType for Insurance/Finance/ES
                .RegisterType<IInsurance, PolicyBoss>("PolicyBoss")
                .RegisterType<IInsurance, Coverfox>("Coverfox")
                .RegisterType<IInsurance, CW>("CW")
                .RegisterType<IInsurance, Ensureti>("Ensureti")
                .RegisterType<IInsuranceRepository, InsuranceRepository>("generic")
                .RegisterType<IInsurance, Chola>("chola")
                .RegisterType<IInsurance, RoyalSundaram>("RoyalSundaram")
                .RegisterType<ICommon, Common>()
                .RegisterType<IFinance<FinanceLead, ClientResponseDto>, Hdfc>("Hdfc")
                .RegisterType<IFinance<FinanceLead, ClientResponseDto>, Axis>("Axis")
                .RegisterType<IFinanceOperations, FinanceOperations>()
                .RegisterType<IFinanceAdapter, FinanceAdapter>("generic")
                .RegisterType<IFinanceCacheRepository, FinanceCacheRepository>()
                .RegisterType<ISurveyRepository, SurveyRepository>()
                .RegisterType<ISurveyCache, SurveyCache>()
                .RegisterType<ISurveyBL, ESSurveyBL>()
                .RegisterType<IUserProfilingBL, UserProfilingBL>()
                .RegisterType<IFinanceLinkDataRepository, FinanceLinkDataRepository>()
                .RegisterType<IFinanceLinkDataCache, FinanceLinkCacheRepository>()
                .RegisterType<IEsLeadFormRepository, EsLeadFormRepository>()
                .RegisterType<ISponsoredNavigationRepository, SponsoredNavigationRepo>()
                .RegisterType<ISponsoredNavigationCache, SponsoredNavigationCache>()
                .RegisterType<IPagesCache, PagesCache>()
                .RegisterType<IPagesRepository, PagesRepository>()

                //RegisterType for Subscription
                .RegisterType<ISubscriptionRepository, SubscriptionRepository>()

                //RegisterType for Accessories
                .RegisterType<ITyresRepository, TyresCacheRepository>()
                .RegisterType<IAccessoryRepo, AccessoryRepository>()
                .RegisterType<IAccessoryCache, AccessoryCacheRepository>()
                .RegisterType<ITyresBL, TyresBL>()
                .RegisterType<IServiceAdapterV2, TyresAdapterMobileSearch>("TyresSearchMobile")
                .RegisterType<IServiceAdapterV2, TyresAdapterDesktopSearch>("TyresSearchDesktop")

                // RegisterType for used car stock
                .RegisterType<IStockRepository, Carwale.DAL.Classified.Stock.StockRepository>()
                .RegisterType<IElasticSearchManager, ElasticSearchManager>()
                .RegisterType<IESOperations, ESStockOperations>()
                .RegisterType<ICommonOperationsRepository, CommonOperationsRepository>()
                .RegisterType<IClassifiedListing, ClassifiedListing>()
                .RegisterType<ISearchBL, SearchBL>()
                .RegisterType<IMetaKeywordsSearch, MetaKeywordsSearch>()
                .RegisterType<ISearchParamsProcesser, SearchParamsProcesser>()
                .RegisterType<IProcessFilters, ProcessFilters>()
                .RegisterType<ISearchUtility, SearchUtility>()
                .RegisterType<IListingsBL, ListingsBL>()

                // RegisterType for sponsored cars
                .RegisterType<ISponsoredCarBL, SponsoredCar>()
                .RegisterType<ISponsoredCar, SponsoredCarRepository>()
                .RegisterType<ISponsoredCarCache, SponsoredCarCacheRepository>()

                // RegisterType for new car dealers
                .RegisterType<INewCarDealersRepository, NewCarDealersRepository>()
                .RegisterType<INewCarDealersCache, NewCarDealersCache>()
                .RegisterType<INewCarDealers, NewCarDealers>()

                //Used Cars
                .RegisterType<IPopularUCDetails, PopularUCDetails>()
                .RegisterType<ICommonOperationsRepository, CommonOperationsRepository>()
                .RegisterType<ICarDetail, CarDetail>()
                .RegisterType<IListingDetails, CarDetailRepository>()
                .RegisterType<ICarDetailsCache, CarDetailsCache>()
                .RegisterType<ICommonOperationsCacheRepository, CommonOperationsCacheRepository>()
                .RegisterType<IDeepLinking, GetApiForCwApp>()
                .RegisterType<IChatSmsRepository, ChatSmsRepository>()
                .RegisterType<IChatBL, ChatBL>()
                .RegisterType<IChatNotifications, ChatNotifications>()
                .RegisterType<IUsedCarBuyerCacheRepository, UsedCarBuyerCacheRepository>()
                .RegisterType<IUsedCarBuyerRepository, UsedCarBuyerRepository>()

                //IpToLocation
                .RegisterType<IIPToLocation, Carwale.BL.IPToLocation.IPToLocation>()
                .RegisterType<IIPToLocationRepository, IPToLocationRepository>()
                .RegisterType<IIPToLocationCacheRepository, IPToLocationCacheRepository>()


                //Used Leads
                .RegisterType<IUsedLeadsRepository, UsedLeadsRepository>()
                .RegisterType<IGetDealerStatus, GetDealerStatus>()
                .RegisterType<IGetUsedCarDealerStatus, GetUsedCarDealerStatus>()
                .RegisterType<ILeadRepository, LeadRepository>()
                .RegisterType<ILeadCacheRepository, LeadCacheRepository>()
                .RegisterType<ILeadBL, LeadBL>()
                .RegisterType<ILeadNotifications, LeadNotifications>()
                .RegisterType<ISellerRepository, SellerRepository>()
                .RegisterType<ISellerCacheRepository, SellerCacheRepository>()
                // Sell Car
                .RegisterType<ISellCarRepository, SellCarRepository>()
                .RegisterType<ISellCarBL, SellCarBL>()
                .RegisterType<ISellCarCacheRepository, SellCarCacheRepository>()
                .RegisterType<ICarDetailsRepository, CarDetailsRepository>()
                .RegisterType<ICarDetailsBL, CarDetailsBL>()
                .RegisterType<IConsumerToBusinessBL, ConsumerToBusinessBL>()
                .RegisterType<IListingsBL, ListingsBL>()
                .RegisterType<IClassifiedMails, ClassifiedMails>()

                //My Listings
                .RegisterType<IMyListings, MyListings>()
                .RegisterType<IMyListingsRepository, MyListingsRepository>()
                .RegisterType<IMyListingsCacheRepository, MyListingsCacheRepository>()
                .RegisterType<IListingRepository, ListingRepository>()
                .RegisterType<IPremiumListingsLogic, PremiumListingsLogic>()

                //Use Car Photos
                .RegisterType<ICarPhotosRepository, CarPhotosRepository>()
                .RegisterType<IStockImageRepository, StockImageRepository>()

                //Car Valuation
                .RegisterType<ICarValuation, CarValuation>()
                .RegisterType<IValuationRepository, ValuationRepository>()
                .RegisterType<ICVBaseValueCalculator, CalculateBaseValue>()
                .RegisterType<ICVFinalValueCalculator, CalculateFinalValue>()
                .RegisterType<IValuationBL, ValuationBL>()
                .RegisterType<IValuationCacheRepository, ValuationCacheRepository>()

                //BlockedCommunication
                .RegisterType<IBlockedCommunicationsRepository, BlockedCommunicationsRepository>()

                //RegisterType for splash screen
                .RegisterType<IAppSplashRepository, AppSplashRepository>()
                .RegisterType<IAppSplashScreenBL, AppSplashScreenBL>()
                .RegisterType<IAppSplashCache, SplashScreenCache>()
                //RegisterType for model page
                //.RegisterType<ISponsoredDealerBranding, SponsoredDealerBranding>()
                .RegisterType<IStockCountCacheRepository, StockCountCacheRepository>()
                .RegisterType<IStockCountRepository, StockCountRepository>()

                //registerTypes for Campaign 
                .RegisterType<ICampaign, CampaignBL>()
                .RegisterType<ITemplate, Template>()
                .RegisterType<ICampaignPrediction, CampaignPredictionBL>()
                .RegisterType<ICampaignRepository, CampaignRepository>()
                .RegisterType<ICampaignCacheRepository, CampaignCacheRepository>()
                //Injection for new car leads
                .RegisterType<IDealerInquiry, PQInquiryToDealer>()
                .RegisterType<IRequestManager<ThirdPartyInquiryDetails>, ThirdPartyInquiryDealer<ThirdPartyInquiryDetails>>()
                .RegisterType<IAPIService<DealerInquiryDetails, APIResponseEntity>, TCApi_Inquiry<DealerInquiryDetails, APIResponseEntity>>()
                .RegisterType<ILead, Lead>()

                //Campaign recommendations
                .RegisterType<ICampaignRecommendationsBL, CampaignRecommendationsBL>()
                //injection for Dealer Generics
                .RegisterType<IDealerRepository, DealerRepository>()
                .RegisterType<IDealerCache, DealersCache>()
                .RegisterType<IDealers, Dealers>()
                //Cardeals
                .RegisterType<IDeals, DealsBL>()
                .RegisterType<IDealsCache, DealsCache>()
                .RegisterType<IDealsRepository, DealsRepository>()
                .RegisterType<IDealInquiriesCL, DealInquiriesCL>()
                //Payment Gateway
                .RegisterType<ITransaction, Transaction>()
                .RegisterType<ITransactionRepository, TransactionRepository>()
                .RegisterType<IPackageRepository, PackageRepository>()
                .RegisterType<ITransactionValidator, ValidateTransaction>()
                .RegisterType<IPaymentGateway, BillDesk>()
                .RegisterType<IRepository<DealsInquiryDetail>, DealsInquiryRepository>()
                .RegisterType<IDealInquiriesRepository, DealsInquiryRepository>()
                .RegisterType<IDealsUserInquiry<DropOffInquiryDetailDTO>, DealsInquiryBL>()
                .RegisterType<IDealsUserInquiry<DropOffInquiryDetailEntity>, DealsInquiryRepository>()

                //Notification register
                .RegisterType<IEmailNotifications, Email>()
                .RegisterType<ISMSNotifications, SMSNotification>()
                .RegisterType<ISMSRepository, SMSRepository>()

                //Templates register
                .RegisterType<ITemplatesCacheRepository, TemplatesCacheRepository>()
                .RegisterType<ITemplatesRepository, TemplatesRepository>()
                .RegisterType<ITemplateRender, TemplateRender>()

                .RegisterType<IAppSiteAssociationBL, AppSiteAssociationBL>()

                .RegisterType<IDealsNotification, DealsNotification>()

                //Customers
                .RegisterType<ICustomerRepository<Customer, CustomerOnRegister>, CustomerRepository<Customer, CustomerOnRegister>>()
                .RegisterType<ICustomerBL<Customer, CustomerOnRegister>, CustomerActions<Customer, CustomerOnRegister>>()

                //Cross sell register
                .RegisterType<ICrossSellCacheRepository, CrossSellCacheRepository>()
                .RegisterType<IPaidCrossSell, PaidCrossSell>()
                .RegisterType<IHouseCrossSell, HouseCrossSell>()
                .RegisterType<Carwale.BL.Advertizing.SponsoredCampaignApp>()
                //autocomplete
                .RegisterType<IAutoComplete_v1, AutoComplete_v1>()
                .RegisterType<SuggestionBL>()
                 //test drive
                 .RegisterType<ILandingPageRepository, LandingPageRepository>()
                .RegisterType<ILandingPageCacheRepo, LandingPageCacheRepo>()
                .RegisterType<ILandingPageBL, LandingPageAdapter>()

                //Customer Verification
                .RegisterType<ICustomerVerification, CustomerVerificationBL>()
                .RegisterType<ICustomerVerificationRepository, CustomerVerificationRepository>()
                .RegisterType<ICustomerVerificationLogic, CustomerVerificationLogic>()

                //User Feedback
                .RegisterType<IFeedbackRepository, FeedbackRepository>()
                .RegisterType<IUserFeedbackBL, UserFeedbackBL>()

                //Notification sub/unsub

                //Monetization
                .RegisterType<IMonetization, MonetizationBL>()

                .RegisterType<IMediaBL, MediaAdapter>()

                // CWCT City Mapping Repository
                .RegisterType<ICWCTCityMappingRepositiory, CWCTCityMappingRepository>()
                .RegisterType<IStockCertificationRepository, StockCertificationRepository>()
                // authors mapping
                .RegisterType<IAuthorRepository, Carwale.BL.Authors.AuthorsBL>()

                //Stock Certification
                .RegisterType<IStockCertificationRepository, StockCertificationRepository>()
                .RegisterType<IStockCertificationBL, StockCertificationBL>()

                //RegisterTypes for Used MultiCity Stock
                .RegisterType<IStockCitiesBL, StockCitiesBL>()
                .RegisterType<IStockCitiesRepository, StockCitiesRepository>()

                .RegisterType<IStockCertificationCacheRepository, StockCertificationCacheRepository>()
                .RegisterType<IStockBL, StockBL>()
                .RegisterType<IImageBL, ImageBL>()
                .RegisterType<ICustomerTracking, CustomerTracking>()
                .RegisterType<IStockConditionRepository, StockConditionRepository>()
                .RegisterType<ICustomerTracking, CustomerTracking>()
                .RegisterType<ICTBuyingIndexClient, CTBuyingIndexClient>(new InjectionConstructor(ConfigurationManager.AppSettings["carTradeBuyingIndexApi"] ?? string.Empty))
                .RegisterType<IC2BStockRepository, C2BStockRepository>()
                .RegisterType<IBookingRepository, BookingEngineRepository>()
                .RegisterType<IBookingCache, ESBookingCache>()
                .RegisterType<IStockConditionCacheRepository, StockConditionCacheRepository>()
                .RegisterType<IStockRegCertificatesRepository, StockRegCertificatesRepository>()

                // xml feed
                .RegisterType<IXmlFeed, Carwale.BL.XmlFeeds.CarMakesSitemapFeed>("MakeFeeds")
                .RegisterType<IXmlFeed, Carwale.BL.XmlFeeds.CarModelsSitemapFeed>("ModelFeeds")
                .RegisterType<ICarRoots, CarRootsBL>()
                .RegisterType<IPager, Pager>()
                .RegisterType<IUserProfilingCache, UserProfilingCache>()
                .RegisterType<IUserProfilingRepo, UserProfilingRepo>()
                .RegisterType<IChargesCacheRepository, ChargesCacheRepository>()
                .RegisterType<IChargesRepository, ChargesRepository>()
                .RegisterType<IChargeGroupsCacheRepository, ChargeGroupsCacheRepository>()
                .RegisterType<IChargeGroupsRepository, ChargeGroupsRepository>()
                .RegisterType<ICharges, Charges>()
                .RegisterType<IChargeGroupsRepository, ChargeGroupsRepository>()
                .RegisterType<IServiceAdapterV2, QuotationPageAdapterMobile>("MobileQuotationAdapter")
                .RegisterType<IServiceAdapterV2, QuotationPageAdapterDesktop>("DesktopQuotationAdapter")
                .RegisterType<IQuotationAdapterCommon, QuotationAdapterCommon>()
                .RegisterType<IBlockMobileRepository, BlockMobileRepository>()
                .RegisterType<IBlockIPRepository, BlockIPRepository>()
                .RegisterType<IBadWordsRepository, BadWordsRepository>()
                .RegisterType<ILoggingRepository, LoggingRepository>()
                .RegisterType<IDealerAdProvider, DealerAdProvider>()
                .RegisterType<IBodyTypeCache, BodyTypeCache>(new ContainerControlledLifetimeManager())
                .RegisterType<IClassificationRepository, ClassificationRepository>()
                .RegisterType<IServiceAdapterV2, NewCarSearchAppAdapterV2>("NewCarSearchAppAdapter")
                .RegisterType<IApiGatewayCaller, ApiGatewayCaller>()
                .RegisterType<INewCarElasticSearch, NewCarElasticSearch>()
                .RegisterType<ICarDataLogic, Carwale.BL.CarData.CarDataLogic>(new ContainerControlledLifetimeManager())
                .RegisterType<IOperationsTrackingRepository, OperationsTrackingRepository>()
                .RegisterType<IModelSimilarPriceDetailsRepo, ModelSimilarPriceDetailsRepository>()
                .RegisterType<IModelSimilarPriceDetailsBL, ModelSimilarPriceDetailsBL>()
                .RegisterType<BhriguTracker>()
                .RegisterType<Func<string, IServiceAdapterV2>>(new InjectionFactory(c => new Func<string, IServiceAdapterV2>(name => c.Resolve<IServiceAdapterV2>(name))))
                .RegisterType<ISecurityRepository<bool>, SecurityRepositoryCache<bool>>(new ContainerControlledLifetimeManager())
                .RegisterType<ICentralizedCacheRefreshBl, CentralizedCacheRefreshBl>(new ContainerControlledLifetimeManager())
                .RegisterType<ILeadFormAdapter, LeadFormAdapter>()
                .RegisterType<Carwale.Interfaces.UesrReview.IUserReviewLogic, Carwale.BL.UserReview.UserReviewLogic>(new ContainerControlledLifetimeManager())
                .RegisterType<Carwale.Interfaces.UesrReview.IUserReviewRepository, Carwale.DAL.UserReview.UserReviewRepository>(new ContainerControlledLifetimeManager())
                        .RegisterType<ITopCarsBl, TopCarsBl>(new ContainerControlledLifetimeManager())
                .RegisterType<IApiGatewayWidgetAdapter<Carwale.Entity.CarData.SimilarCarVmRequest, SimilarCarsDTO>, Carwale.Adapters.NewCars.SimilarCarsAdapter>("SimilarCarsAdapter");

            NewCarFinder.RegisterTypes(container);
            UserReviews.RegisterTypes(container);
            Carwale.Service.Containers.Offers.RegisterTypes(container);
            CMS.RegisterTypes(container);
            DealerTypes.RegisterTypes(container);
            StockTypes.RegisterTypes(container);
            Validation.RegisterTypes(container);
            ClassifiedTypes.RegisterTypes(container);
            ListingPaymentTypes.RegisterTypes(container);
            Otp.RegisterTypes(container);
            Prices.RegisterTypes(container);
            ClassifiedESTypes.RegisterTypes(container);
            ClassifiedSlots.RegisterTypes(container);
            GeoLocationTypes.RegisterTypes(container);
            AdapterTypes.RegisterTypes(container);
            Carwale.Service.Containers.Notifications.RegisterTypes(container);
            return container;
        }
    }
}
