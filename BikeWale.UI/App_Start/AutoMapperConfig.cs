using AutoMapper;
using Bikewale.Entities.QuestionAndAnswers;

namespace Bikewale
{
    public static class AutoMapperConfig
    {
        public static void ConfigureAutoMapper()
        {
            Mapper.Initialize(cfg =>
            {
                #region EntityTODTOMapping
                cfg.CreateMap<Bikewale.Entities.QuestionAndAnswers.Answer, Bikewale.DTO.QuestionAndAnswers.AnswerDTO>();
                cfg.CreateMap<Bikewale.Entities.QuestionAndAnswers.Answer, QuestionsAnswers.Entities.AnswerBase>();
                cfg.CreateMap<QuestionsAnswers.Entities.AnswerBase, Bikewale.Entities.QuestionAndAnswers.Answer>();
                cfg.CreateMap<Bikewale.DTO.QuestionAndAnswers.AnswerDTO, Bikewale.Entities.QuestionAndAnswers.Answer>();
                cfg.CreateMap<Bikewale.DTO.App.AppAlert.AppIMEIDetailsInput, Bikewale.Entities.MobileAppAlert.AppFCMInput>();
                cfg.CreateMap<Bikewale.Entities.Location.AreaEntityBase, Bikewale.DTO.Area.AreaBase>();
                cfg.CreateMap<Bikewale.Entities.Location.AreaEntityBase, Bikewale.DTO.BikeBooking.Area.BBAreaBase>();
                cfg.CreateMap<Bikewale.Entities.Location.AreaEntityBase, DTO.PriceQuote.Area.v2.PQAreaBase>();
                cfg.CreateMap<Bikewale.Entities.Location.AreaEntityBase, Bikewale.DTO.PriceQuote.Area.PQAreaBase>();
                cfg.CreateMap<Bikewale.Entities.CMS.Articles.ArticleBase, Bikewale.DTO.CMS.Articles.CMSArticleBase>();
                cfg.CreateMap<Bikewale.Entities.CMS.Articles.ArticleDetails, Bikewale.DTO.CMS.Articles.CMSArticleDetails>();
                cfg.CreateMap<Bikewale.Entities.CMS.Articles.ArticlePageDetails, Bikewale.DTO.CMS.Articles.CMSArticlePageDetails>();
                cfg.CreateMap<Bikewale.Entities.CMS.Articles.ArticleSummary, Bikewale.DTO.CMS.Articles.CMSArticleSummary>();
                cfg.CreateMap<Bikewale.Entities.CMS.Articles.ArticleSummary, Bikewale.DTO.CMS.Articles.CMSArticleSummaryMin>();
                cfg.CreateMap<Bikewale.DTO.BikeBooking.Make.BBMakeBase, Bikewale.Entities.BikeData.BikeMakeEntityBase>();
                cfg.CreateMap<Bikewale.DTO.BikeBooking.Model.BBModelBase, Bikewale.Entities.BikeData.BikeModelEntityBase>();
                cfg.CreateMap<Bikewale.DTO.BikeBooking.Version.BBVersionBase, Bikewale.Entities.BikeData.BikeVersionEntityBase>();
                cfg.CreateMap<Bikewale.Entities.Compare.BikeColor, Bikewale.DTO.Compare.BikeColorDTO>();
                cfg.CreateMap<Bikewale.Entities.Compare.BikeCompareEntity, Bikewale.DTO.Compare.BikeCompareDTO>();
                cfg.CreateMap<Bikewale.Entities.PriceQuote.BikeDealerPriceDetail, Bikewale.DTO.PriceQuote.BikeBooking.BikeDealerPriceDetailDTO>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeDescriptionEntity, Bikewale.DTO.Make.BikeDescription>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeDescriptionEntity, Bikewale.DTO.BikeData.BikeDiscription>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeDescriptionEntity, Bikewale.DTO.Model.ModelDescription>();
                cfg.CreateMap<Bikewale.Entities.Compare.BikeEntityBase, Bikewale.DTO.Compare.BikeDTOBase>();
                cfg.CreateMap<Bikewale.Entities.Compare.BikeFeature, Bikewale.DTO.Compare.BikeFeatureDTO>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeMakeEntityBase, Bikewale.DTO.BikeBooking.Make.BBMakeBase>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeMakeEntityBase, Bikewale.DTO.PriceQuote.DetailedDealerQuotation.DDQMakeBase>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeMakeEntityBase, Bikewale.DTO.PriceQuote.DealerPriceQuote.DPQMakeBase>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeMakeEntityBase, Bikewale.DTO.Make.MakeBase>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeMakeEntityBase, Bikewale.DTO.PriceQuote.Make.PQMakeBase>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeMakeModelBase, Bikewale.DTO.BikeData.MakeModelBase>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeMakeModelEntity, Bikewale.DTO.BikeData.BikeMakeModel>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeMakePageEntity, Bikewale.DTO.Make.MakePage>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeModelContent, Bikewale.DTO.Model.BikeModelContentDTO>();
                cfg.CreateMap<Bikewale.ElasticSearch.Entities.BikeModelDocument, Bikewale.Entities.NewBikeSearch.BikeModelDocumentEntity>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeModelEntity, Bikewale.DTO.Model.v2.ModelDetails>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeModelEntity, Bikewale.DTO.Model.ModelDetail>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeModelEntity, Bikewale.DTO.Model.ModelDetails>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeModelEntityBase, Bikewale.DTO.BikeBooking.Model.BBModelBase>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeModelEntityBase, Bikewale.DTO.PriceQuote.DetailedDealerQuotation.DDQModelBase>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeModelEntityBase, Bikewale.DTO.PriceQuote.DealerPriceQuote.DPQModelBase>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeModelEntityBase, Bikewale.DTO.Model.ModelBase>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeModelEntityBase, Bikewale.DTO.PriceQuote.Model.PQModelBase>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeModelPageEntity, Bikewale.DTO.Model.BikeSpecs>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeModelPageEntity, Bikewale.DTO.Model.ModelPage>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeModelPageEntity, Bikewale.DTO.Model.v2.ModelPage>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeModelVersionsDetails, Bikewale.DTO.BikeData.ModelSpecificationsDTO>();
                cfg.CreateMap<Bikewale.Entities.Used.BikePhoto, Bikewale.DTO.Used.Search.BikePhoto>();
                cfg.CreateMap<Bikewale.Entities.PriceQuote.BikeQuotationEntity, Bikewale.DTO.PriceQuote.BikeQuotation.PQBikePriceQuoteOutput>();
                cfg.CreateMap<Bikewale.Entities.UserReviews.BikeRatingsInfo, Bikewale.DTO.UserReviews.BikeRatingData>();
                cfg.CreateMap<Bikewale.Entities.UserReviews.BikeRatingsReviewsInfo, Bikewale.DTO.UserReviews.BikeModelUserReviews>();
                cfg.CreateMap<Bikewale.Entities.UserReviews.BikeReviewsInfo, Bikewale.DTO.UserReviews.BikeReviewsData>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeSeriesEntityBase, Bikewale.DTO.Series.SeriesBase>();
                cfg.CreateMap<Bikewale.Entities.Compare.BikeSpecification, Bikewale.DTO.Compare.BikeSpecificationDTO>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeSpecificationEntity, Bikewale.DTO.Version.VersionSpecifications>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeVersionColorsWithAvailability, Bikewale.DTO.BikeData.BikeVersionColorsWithAvailabilityDTO>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeVersionEntity, DTO.Version.v2.VersionDetails>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeVersionEntity, Bikewale.DTO.Version.VersionDetails>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeVersionEntityBase, Bikewale.DTO.BikeBooking.Version.BBVersionBase>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeVersionEntityBase, Bikewale.DTO.PriceQuote.DetailedDealerQuotation.DDQVersionBase>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeVersionEntityBase, Bikewale.DTO.PriceQuote.DealerPriceQuote.DPQVersionBase>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeVersionEntityBase, Bikewale.DTO.Version.VersionBase>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeVersionMinSpecs, Bikewale.DTO.Model.v3.VersionDetail>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeVersionMinSpecs, Bikewale.DTO.Version.VersionMinSpecs>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeVersionSegmentDetails, Bikewale.DTO.BikeData.VersionSegmentDTO>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeVersionsListEntity, Bikewale.DTO.Version.ModelVersionList>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeVersionsListEntity, Bikewale.DTO.PriceQuote.Version.PQVersionBase>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeVersionsListEntity, Bikewale.DTO.Version.VersionBase>();
                cfg.CreateMap<Bikewale.Entities.Videos.BikeVideoEntity, Bikewale.DTO.Videos.VideoBase>();
                cfg.CreateMap<Bikewale.DTO.PriceQuote.PQCustomerDetailInput, Bikewale.Entities.PriceQuote.PQCustomerDetailInput>();
                cfg.CreateMap<Bikewale.DTO.PriceQuote.v3.PQCustomerDetailInput, Bikewale.Entities.PriceQuote.v2.PQCustomerDetailInput>();
                cfg.CreateMap<Bikewale.DTO.QuestionAndAnswers.Tag, Bikewale.Entities.QuestionAndAnswers.Tag>();
                cfg.CreateMap<Bikewale.DTO.Used.Search.BikePhoto, Bikewale.Entities.Used.BikePhoto>();
                cfg.CreateMap<Bikewale.DTO.UsedBikes.SellAdStatus, Bikewale.Entities.Used.SellAdStatus>();
                cfg.CreateMap<Bikewale.ElasticSearch.Entities.ImageEntity, Bikewale.Entities.NewBikeSearch.ImageEntity>();
                cfg.CreateMap<Bikewale.ElasticSearch.Entities.MakeEntity, Bikewale.Entities.NewBikeSearch.MakeEntity>();
                cfg.CreateMap<Bikewale.ElasticSearch.Entities.ModelEntity, Bikewale.Entities.NewBikeSearch.ModelEntity>();
                cfg.CreateMap<Bikewale.ElasticSearch.Entities.PriceEntity, Bikewale.Entities.NewBikeSearch.PriceEntity>();
                cfg.CreateMap<Bikewale.ElasticSearch.Entities.VersionEntity, Bikewale.Entities.NewBikeSearch.VersionEntity>();
                cfg.CreateMap<Bikewale.Entities.BikeBooking.AreaEntityBase, Bikewale.DTO.PriceQuote.DetailedDealerQuotation.DDQAreaBase>();
                cfg.CreateMap<Bikewale.Entities.BikeBooking.AreaEntityBase, Bikewale.DTO.PriceQuote.DealerPriceQuote.DPQAreaBase>();
                cfg.CreateMap<Bikewale.Entities.BikeBooking.EMI, Bikewale.DTO.PriceQuote.DetailedDealerQuotation.DDQEMI>();
                cfg.CreateMap<Bikewale.Entities.BikeBooking.PQ_Price, Bikewale.DTO.PriceQuote.DealerPriceQuote.DPQ_Price>();
                cfg.CreateMap<Bikewale.Entities.BikeBooking.PQ_Price, Bikewale.DTO.PriceQuote.DetailedDealerQuotation.DDQPQ_Price>();
                cfg.CreateMap<Bikewale.Entities.BikeBooking.v2.PQOutputEntity, Bikewale.DTO.PriceQuote.v3.PQOutput>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeColorsbyVersion, Bikewale.DTO.BikeData.BikeColorsbyVersionsDTO>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeMakeEntityBase, Bikewale.DTO.BikeBooking.Make.BBMakeBase>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeModelEntity, Bikewale.DTO.Model.ModelDetails>();
                cfg.CreateMap<Bikewale.Entities.BikeData.BikeModelEntityBase, Bikewale.DTO.BikeBooking.Model.BBModelBase>();
                cfg.CreateMap<Bikewale.Entities.BikeData.Features, Bikewale.DTO.Model.Features>();
                cfg.CreateMap<Bikewale.Entities.BikeData.Overview, Bikewale.DTO.Model.Overview>();
                cfg.CreateMap<Bikewale.Entities.BikeData.Specifications, Bikewale.DTO.Model.Specifications>();
                cfg.CreateMap<Bikewale.Entities.BikeData.Specs, Bikewale.DTO.Model.Specs>();
                cfg.CreateMap<Bikewale.Entities.BikeData.SpecsCategory, Bikewale.DTO.Model.SpecsCategory>();
                cfg.CreateMap<Bikewale.Entities.BikeData.v2.BikeModelContent, Bikewale.DTO.Model.v2.BikeModelContentDTO>();
                cfg.CreateMap<Bikewale.Entities.CMS.Articles.CMSContent, Bikewale.DTO.CMS.Articles.CMSContent>();
                cfg.CreateMap<Bikewale.Entities.CMS.Photos.CMSImage, Bikewale.DTO.CMS.Photos.CMSImageList>();
                cfg.CreateMap<Bikewale.Entities.CMS.Photos.ModelImage, Bikewale.DTO.CMS.Photos.CMSModelImageBase>();
                cfg.CreateMap<Bikewale.Entities.Compare.BikeModelColor, Bikewale.DTO.Compare.BikeModelColorDTO>();
                cfg.CreateMap<Bikewale.Entities.Location.AreaEntityBase, Bikewale.DTO.Area.AreaBase>();
                cfg.CreateMap<Bikewale.Entities.Location.AreaEntityBase, Bikewale.DTO.PriceQuote.Area.PQAreaBase>();
                cfg.CreateMap<Bikewale.Entities.NewBikeSearch.SearchBudgetLink, Bikewale.DTO.NewBikeSearch.SearchBudgetLink>();
                cfg.CreateMap<Bikewale.Entities.PriceQuote.NewBikeDealerBase, Bikewale.DTO.DealerLocator.DealerBase>();
                cfg.CreateMap<Bikewale.Entities.QuestionAndAnswers.Tag, QuestionsAnswers.Entities.Tag>();
                cfg.CreateMap<Bikewale.Entities.Used.BikePhoto, Bikewale.DTO.Used.Search.BikePhoto>();
                cfg.CreateMap<Bikewale.Entities.Used.Search.SearchResult, Bikewale.DTO.Used.Search.SearchResult>();
                cfg.CreateMap<Bikewale.Entities.Used.SellAdStatus, Bikewale.DTO.UsedBikes.SellAdStatus>();
                cfg.CreateMap<Bikewale.Entities.Used.UsedBikeBase, Bikewale.DTO.Used.Search.UsedBikeBase>();
                cfg.CreateMap<Bikewale.Entities.UserReviews.UserReviewRatingData, Bikewale.DTO.UserReviews.RateBikeDetails>();
                cfg.CreateMap<Bikewale.Entities.BikeBooking.BookingAmountEntityBase, Bikewale.DTO.PriceQuote.DetailedDealerQuotation.DDQBookingAmountBase>();
                cfg.CreateMap<Bikewale.Entities.PriceQuote.BookingPageDetailsEntity, Bikewale.DTO.PriceQuote.BikeBooking.BookingPageDetailsDTO>();
                cfg.CreateMap<Bikewale.Entities.QuestionAndAnswers.BWClientInfo, QuestionsAnswers.Entities.ClientInfo>();
                cfg.CreateMap<Bikewale.DTO.Finance.CapitalFirstVoucherDTO, Bikewale.Entities.Finance.CapitalFirst.CapitalFirstVoucherEntityBase>();
                cfg.CreateMap<Bikewale.DTO.Finance.CapitalFirstVoucherStatusDTO, Bikewale.Entities.Finance.CapitalFirst.CapitalFirstVoucherStatus>();
                cfg.CreateMap<Bikewale.Entities.Location.CityEntityBase, Bikewale.DTO.BikeBooking.City.BBCityBase>();
                cfg.CreateMap<Bikewale.Entities.Location.CityEntityBase, Bikewale.DTO.PriceQuote.City.v2.PQCityBase>();
                cfg.CreateMap<Bikewale.Entities.Location.CityEntityBase, Bikewale.DTO.City.CityBase>();
                cfg.CreateMap<Bikewale.Entities.Location.CityEntityBase, Bikewale.DTO.Location.CityEntityBaseDTO>();
                cfg.CreateMap<Bikewale.Entities.Location.CityEntityBase, Bikewale.DTO.City.CityFinance>();
                cfg.CreateMap<Bikewale.Entities.Location.CityEntityBase, Bikewale.DTO.PriceQuote.DetailedDealerQuotation.DDQCityBase>();
                cfg.CreateMap<Bikewale.Entities.Location.CityEntityBase, Bikewale.DTO.PriceQuote.DealerPriceQuote.DPQCityBase>();
                cfg.CreateMap<Bikewale.Entities.Location.CityEntityBase, Bikewale.DTO.PriceQuote.City.PQCityBase>();
                cfg.CreateMap<Bikewale.Entities.BikeData.ColorCodeBase, Bikewale.DTO.Model.ColorCode>();
                cfg.CreateMap<Bikewale.Entities.CMS.Photos.ColorImageBaseEntity, Bikewale.DTO.Model.ColorImageBaseDTO>();
                cfg.CreateMap<Bikewale.Entities.Compare.CompareBikeColor, Bikewale.DTO.Compare.CompareBikeColorDTO>();
                cfg.CreateMap<Bikewale.Entities.Compare.CompareBikeColorCategory, Bikewale.DTO.Compare.CompareBikeColorCategoryDTO>();
                cfg.CreateMap<Bikewale.Entities.Compare.CompareBikeData, Bikewale.DTO.Compare.CompareBikeDataDTO>();

                cfg.CreateMap<Bikewale.DTO.Customer.CustomerBase, Bikewale.Entities.Customer.CustomerEntityBase>();
                cfg.CreateMap<Bikewale.Entities.Customer.CustomerEntity, Bikewale.DTO.Customer.AuthenticatedCustomer>();
                cfg.CreateMap<Bikewale.Entities.Customer.CustomerEntity, Bikewale.DTO.Customer.RegisteredCustomer>();
                cfg.CreateMap<Bikewale.Entities.Customer.CustomerEntityBase, Bikewale.DTO.Customer.CustomerBase>();
                cfg.CreateMap<Bikewale.Entities.Customer.CustomerEntityBase, DTO.Customer.CustomerBase>();
                cfg.CreateMap<Bikewale.Entities.Customer.CustomerEntityBase, Bikewale.DTO.PriceQuote.CustomerDetails.PQCustomerBase>();
                cfg.CreateMap<Bikewale.Entities.Customer.CustomerEntityBase, QuestionsAnswers.Entities.Customer>();
                cfg.CreateMap<Bikewale.Entities.DealerLocator.DealerBikeModelsEntity, Bikewale.DTO.DealerLocator.DealerBikeModels>();
                cfg.CreateMap<Bikewale.Entities.DealerLocator.DealerBikesEntity, Bikewale.DTO.DealerLocator.DealerBikes>();
                cfg.CreateMap<Bikewale.Entities.DealerLocator.DealerDetailEntity, Bikewale.DTO.DealerLocator.DealerDetail>();
                cfg.CreateMap<Bikewale.Entities.PriceQuote.DealerDetails, Bikewale.DTO.PriceQuote.BikeBooking.DealerDetailsDTO>();
                cfg.CreateMap<Bikewale.Entities.PriceQuote.DealerOfferEntity, Bikewale.DTO.PriceQuote.BikeBooking.DealerOfferDTO>();
                cfg.CreateMap<Bikewale.Entities.PriceQuote.DealerPackageTypes, Bikewale.DTO.PriceQuote.DealerPackageType>();
                cfg.CreateMap<Bikewale.Entities.PriceQuote.DealerVersionPriceItemEntity, Bikewale.DTO.PriceQuote.BikeBooking.DealerVersionPriceItemDTO>();
                cfg.CreateMap<Bikewale.Entities.DealerLocator.DealerVersionPrices, Bikewale.DTO.Dealer.DealerVersionPricesDTO>();
                cfg.CreateMap<DTO.AWS.Token, Entities.AWS.Token>();
                cfg.CreateMap<DTO.BikeBooking.BookingListingFilterDTO, Entities.BikeBooking.BookingListingFilterEntity>();
                cfg.CreateMap<DTO.BikeData.NewLaunched.InputFilterDTO, Entities.BikeData.NewLaunched.InputFilter>();
                cfg.CreateMap<DTO.Customer.CustomerBase, Bikewale.Entities.Customer.CustomerEntityBase>();
                cfg.CreateMap<DTO.Images.ImageDTO, Entities.Images.Image>();
                cfg.CreateMap<DTO.Images.ImageTokenDTO, Entities.Images.ImageToken>();
                cfg.CreateMap<DTO.PriceQuote.v2.PQCustomerDetailInput, Entities.PriceQuote.PQCustomerDetailInput>();
                cfg.CreateMap<DTO.Upcoming.UpcomingNotificationDTO, Entities.UpcomingNotification.UpcomingBikeEntity>();
                cfg.CreateMap<DTO.UsedBikes.PhotoRequestDTO, Entities.Used.PhotoRequest>();
                cfg.CreateMap<Entities.App.AppVersion, DTO.App.AppVersion>();
                cfg.CreateMap<Entities.AppDeepLinking.DeepLinkingEntity, DTO.AppDeepLinking.DeepLinking>();
                cfg.CreateMap<Entities.AWS.Token, DTO.AWS.Token>();
                cfg.CreateMap<Entities.BikeBooking.BikeBookingListingEntity, DTO.BikeBooking.BikeBookingListingDTO>();
                cfg.CreateMap<Entities.BikeBooking.EMI, DTO.PriceQuote.v2.EMI>();
                cfg.CreateMap<Entities.BikeBooking.PagingUrl, DTO.BikeBooking.PagingUrl>();
                cfg.CreateMap<Entities.BikeData.BikeModelEntity, Bikewale.DTO.Model.ModelDetails>();
                cfg.CreateMap<Entities.BikeData.NewLaunched.InputFilter, DTO.BikeData.NewLaunched.InputFilterDTO>();
                cfg.CreateMap<Entities.BikeData.NewLaunched.NewLaunchedBikeEntityBase, DTO.BikeData.NewLaunched.NewLaunchedBikeDTOBase>();
                cfg.CreateMap<Entities.BikeData.NewLaunched.NewLaunchedBikeResult, DTO.BikeData.NewLaunched.NewLaunchedBikeResultDTO>();
                cfg.CreateMap<Entities.BikeData.Specs, DTO.Model.Specs>();
                cfg.CreateMap<Entities.BikeData.SpecsCategory, DTO.Model.v2.SpecsCategory>();
                cfg.CreateMap<Entities.Compare.TopBikeCompareBase, DTO.Compare.TopBikeCompareDTO>();
                cfg.CreateMap<Entities.Images.ImageToken, DTO.Images.ImageTokenDTO>();
                cfg.CreateMap<Entities.Location.CityEntityBase, Bikewale.DTO.City.CityBase>();
                cfg.CreateMap<Entities.PriceQuote.DealerDetails, DTO.PriceQuote.BikeBooking.DealerDetailsDTO>();
                cfg.CreateMap<Entities.PriceQuote.PQCustomerDetailOutputEntity, DTO.PriceQuote.PQCustomerDetailOutput>();
                cfg.CreateMap<Entities.PriceQuote.PQCustomerDetailOutputEntity, DTO.PriceQuote.v2.PQCustomerDetailOutput>();
                cfg.CreateMap<Entities.PriceQuote.v2.NewBikeDealerBase, DTO.PriceQuote.v3.DPQDealerBase>();
                cfg.CreateMap<Entities.PriceQuote.v2.PQByCityAreaEntity, DTO.PriceQuote.BikePQOutput>();
                cfg.CreateMap<Entities.PriceQuote.v2.PQCustomerDetailOutputEntity, DTO.PriceQuote.v3.PQCustomerDetailOutput>();
                cfg.CreateMap<Entities.PriceQuote.v4.PQByCityAreaEntity, Bikewale.DTO.PriceQuote.v2.BikePQOutput>();
                cfg.CreateMap<Entities.QuestionAndAnswers.Answer, QuestionsAnswers.Entities.Answer>();
                cfg.CreateMap<Entities.QuestionAndAnswers.Tag, Bikewale.DTO.QuestionAndAnswers.Tag>();
                cfg.CreateMap<Entities.Used.BikeInterestDetails, DTO.UsedBikes.BikeInterestDetailsDTO>();
                cfg.CreateMap<Entities.Used.PurchaseInquiryResultEntity, DTO.UsedBikes.PurchaseInquiryResultDTO>();
                cfg.CreateMap<Entities.Used.PurchaseInquiryStatusEntity, DTO.UsedBikes.PurchaseInquiryStatusDTO>();
                cfg.CreateMap<Entities.UserReviews.Search.SearchResult, Bikewale.DTO.UserReviews.Search.SearchResult>();
                cfg.CreateMap<Bikewale.Entities.BikeBooking.FacilityEntity, Bikewale.DTO.PriceQuote.DetailedDealerQuotation.DDQFacilityBase>();
                cfg.CreateMap<Bikewale.Entities.BikeData.FeaturedBikeEntity, Bikewale.DTO.BikeData.FeaturedBike>();
                cfg.CreateMap<Bikewale.Entities.CMS.Photos.ImageBaseEntity, Bikewale.DTO.Model.ImageBaseDTO>();
                cfg.CreateMap<Bikewale.Entities.Used.ImageUploadResultStatus, Bikewale.DTO.Used.ImageUploadResultStatusDTO>();
                cfg.CreateMap<Bikewale.Entities.Used.ImageUploadStatus, Bikewale.DTO.Used.ImageUploadStatusDTO>();
                cfg.CreateMap<Bikewale.DTO.BikeData.Upcoming.InputFilterDTO, Bikewale.Entities.BikeData.UpcomingBikesListInputEntity>();
                cfg.CreateMap<Bikewale.DTO.UserReviews.InputRatingSave, Bikewale.Entities.UserReviews.InputRatingSaveEntity>();
                cfg.CreateMap<Bikewale.DTO.UserReviews.InputRatingSave, Bikewale.DTO.UserReviews.RatingReviewInput>();
                cfg.CreateMap<Bikewale.Entities.Used.InquiryDetails, Bikewale.DTO.Used.InquiryDetailsDTO>();
                cfg.CreateMap<Bikewale.Entities.Finance.BajajAuto.LeadResponse, Bikewale.DTO.Finance.BajajAuto.BajajAutoLeadResponseDto>();
                cfg.CreateMap<Bikewale.Entities.Finance.CapitalFirst.LeadResponseMessage, Bikewale.DTO.Finance.CapitalFirstLeadResponseDTO>();
                cfg.CreateMap<Bikewale.Entities.BikeData.MakeModelListEntity, Bikewale.DTO.BikeData.MakeModelList>();
                cfg.CreateMap<Bikewale.Entities.ManufacturerDealer, Bikewale.DTO.ManufactureDealerDTO>();
                cfg.CreateMap<Bikewale.Entities.BikeData.MinSpecsEntity, Bikewale.DTO.BikeData.v2.MinSpecs>();
                cfg.CreateMap<Bikewale.Entities.BikeData.MinSpecsEntity, Bikewale.DTO.BikeData.MinSpecs>();
                cfg.CreateMap<Bikewale.Entities.BikeData.ModelColorImage, Bikewale.DTO.Model.ModelColorPhoto>();
                cfg.CreateMap<Bikewale.Entities.CMS.Photos.ModelImage, Bikewale.DTO.CMS.Photos.CMSModelImageBase>();
                cfg.CreateMap<Bikewale.Entities.CMS.Photos.ModelImages, Bikewale.DTO.CMS.Photos.CMSModelImages>();
                cfg.CreateMap<Bikewale.Entities.CMS.Photos.ModelImageWrapper, Bikewale.DTO.CMS.Photos.ModelImageList>();
                cfg.CreateMap<Bikewale.Entities.BikeData.MostPopularBikesBase, Bikewale.DTO.Widgets.MostPopularBikes>();
                cfg.CreateMap<Bikewale.Entities.BikeData.MostPopularBikesBase, Bikewale.DTO.Widgets.v2.MostPopularBikes>();
                cfg.CreateMap<Bikewale.Entities.PriceQuote.NewBikeDealerBase, DTO.PriceQuote.v2.DPQDealerBase>();
                cfg.CreateMap<Bikewale.Entities.Dealer.NewBikeDealerEntityBase, Bikewale.DTO.Dealer.NewBikeDealerBase>();
                cfg.CreateMap<Bikewale.Entities.BikeBooking.NewBikeDealers, Bikewale.DTO.PriceQuote.DetailedDealerQuotation.DDQNewBikeDealer>();
                cfg.CreateMap<Bikewale.Entities.BikeBooking.NewBikeDealers, Bikewale.DTO.PriceQuote.DealerPriceQuote.DPQNewBikeDealer>();
                cfg.CreateMap<Bikewale.Entities.BikeData.NewBikeModelColor, Bikewale.DTO.Model.ModelColor>();
                cfg.CreateMap<Bikewale.Entities.BikeData.NewBikeModelColor, Bikewale.DTO.Model.NewModelColor>();
                cfg.CreateMap<Bikewale.Entities.BikeData.NewLaunchedBikeEntity, Bikewale.DTO.BikeData.LaunchedBike>();
                cfg.CreateMap<Bikewale.Entities.BikeBooking.OfferEntity, Bikewale.DTO.BikeBooking.BikeBookingOfferDTO>();
                cfg.CreateMap<Bikewale.Entities.BikeBooking.OfferEntity, Bikewale.DTO.PriceQuote.DetailedDealerQuotation.DDQOfferBase>();
                cfg.CreateMap<Bikewale.Entities.BikeBooking.OfferEntity, Bikewale.DTO.PriceQuote.DealerPriceQuote.DPQOfferBase>();
                cfg.CreateMap<Bikewale.Entities.PriceQuote.OfferHtmlEntity, Bikewale.DTO.PriceQuote.OfferHtmlDTO>();
                cfg.CreateMap<Bikewale.Entities.PriceQuote.OtherVersionInfoEntity, DTO.Version.VersionBase>();
                cfg.CreateMap<Bikewale.Entities.PriceQuote.OtherVersionInfoEntity, Bikewale.DTO.PriceQuote.BikeQuotation.OtherVersionInfoDTO>();
                cfg.CreateMap<Bikewale.Entities.CMS.Articles.Page, Bikewale.DTO.CMS.Articles.CMSPage>();
                cfg.CreateMap<Bikewale.Entities.BikeBooking.PagingUrl, Bikewale.DTO.BikeBooking.PagingUrl>();
                cfg.CreateMap<Bikewale.Entities.NewBikeSearch.PagingUrl, Bikewale.DTO.NewBikeSearch.Pager>();
                cfg.CreateMap<Bikewale.Entities.NewBikeSearch.PagingUrl, Bikewale.DTO.NewBikeSearch.Pager>();
                cfg.CreateMap<Bikewale.Entities.UsedBikes.PopularUsedBikesEntity, Bikewale.DTO.UsedBikes.PopularUsedBikesBase>();
                cfg.CreateMap<Bikewale.Entities.BikeBooking.PQ_BikeVarient, Bikewale.DTO.PriceQuote.DealerPriceQuote.DPQ_BikeVarient>();
                cfg.CreateMap<Bikewale.Entities.BikeBooking.PQ_DealerDetailEntity, Bikewale.DTO.PriceQuote.DetailedDealerQuotation.DDQDealerDetailBase>();
                cfg.CreateMap<Bikewale.Entities.BikeBooking.PQ_Price, Bikewale.DTO.PriceQuote.DealerPriceQuote.DPQ_Price>();
                cfg.CreateMap<Bikewale.Entities.BikeBooking.PQ_QuotationEntity, Bikewale.DTO.PriceQuote.DetailedDealerQuotation.DDQQuotationBase>();
                cfg.CreateMap<Bikewale.Entities.BikeBooking.PQ_QuotationEntity, Bikewale.DTO.PriceQuote.DealerPriceQuote.DPQuotationOutput>();
                cfg.CreateMap<Bikewale.Entities.PriceQuote.PQByCityAreaEntity, Bikewale.DTO.PriceQuote.Version.v3.PQByCityAreaDTO>();
                cfg.CreateMap<Bikewale.Entities.PriceQuote.v3.PQByCityAreaEntity, Bikewale.DTO.PriceQuote.Version.v3.PQByCityAreaDTO>();
                cfg.CreateMap<Bikewale.Entities.PriceQuote.PQByCityAreaEntity, Bikewale.DTO.PriceQuote.Version.PQByCityAreaDTO>();
                cfg.CreateMap<Bikewale.Entities.PriceQuote.PQByCityAreaEntity, Bikewale.DTO.PriceQuote.Version.v2.PQByCityAreaDTOV2>();
                cfg.CreateMap<Bikewale.Entities.PriceQuote.v2.PQByCityAreaEntity, Bikewale.DTO.PriceQuote.Version.v2.PQByCityAreaDTOV2>();
                cfg.CreateMap<Bikewale.Entities.BikeBooking.PQCustomerDetail, Bikewale.DTO.PriceQuote.CustomerDetails.PQCustomer>();
                cfg.CreateMap<Bikewale.Entities.BikeBooking.PQOutputEntity, Bikewale.DTO.PriceQuote.PQOutput>();
                cfg.CreateMap<Bikewale.Entities.BikeBooking.v2.PQOutputEntity, Bikewale.DTO.PriceQuote.v2.PQOutput>();
                cfg.CreateMap<Bikewale.DTO.NewBikeSearch.PriceRangeDTO, Bikewale.Entities.NewBikeSearch.PriceRangeEntity>();
                cfg.CreateMap<QuestionsAnswers.Entities.Question, Bikewale.Entities.QuestionAndAnswers.Question>();
                cfg.CreateMap<Bikewale.Entities.QuestionAndAnswers.Question, QuestionsAnswers.Entities.Question>();
                cfg.CreateMap<Bikewale.Entities.QuestionAndAnswers.QuestionAnswer, Bikewale.DTO.QuestionAndAnswers.QuestionAnswerDTO>();
                cfg.CreateMap<Bikewale.Entities.QuestionAndAnswers.QuestionBase, Bikewale.DTO.QuestionAndAnswers.QuestionDTO>();
                cfg.CreateMap<Bikewale.DTO.QuestionAndAnswers.QuestionDTO, Bikewale.Entities.QuestionAndAnswers.Question>();
                cfg.CreateMap<Bikewale.Entities.QuestionAndAnswers.Questions, Bikewale.DTO.QuestionAndAnswers.QuestionsDTO>();
                cfg.CreateMap<QuestionsAnswers.Entities.AnswerBase, Bikewale.Entities.QuestionAndAnswers.Answer>();
                cfg.CreateMap<QuestionsAnswers.Entities.Customer, Bikewale.Entities.Customer.CustomerEntityBase>();
                cfg.CreateMap<QuestionsAnswers.Entities.Question, Bikewale.Entities.QuestionAndAnswers.Question>();
                cfg.CreateMap<QuestionsAnswers.Entities.Tag, Bikewale.Entities.QuestionAndAnswers.Tag>();
                cfg.CreateMap<Bikewale.Entities.QuestionAndAnswers.QuestionUrl, Bikewale.DTO.QuestionAndAnswers.QuestionURLDTO>();
                cfg.CreateMap<Bikewale.DTO.NewBikeSearch.RangeDTO, Bikewale.Entities.NewBikeSearch.RangeEntity>();
                cfg.CreateMap<Bikewale.Entities.UserReviews.ReviewDetailsEntity, Bikewale.Entities.DTO.ReviewDetails>();
                cfg.CreateMap<Bikewale.Entities.UserReviews.ReviewEntity, Bikewale.DTO.UserReviews.v2.Review>();
                cfg.CreateMap<Bikewale.Entities.UserReviews.ReviewEntity, Bikewale.Entities.DTO.Review>();
                cfg.CreateMap<Bikewale.Entities.UserReviews.ReviewEntityBase, Bikewale.Entities.DTO.ReviewBase>();
                cfg.CreateMap<Bikewale.Entities.UserReviews.ReviewRatingEntity, Bikewale.Entities.DTO.ReviewRating>();
                cfg.CreateMap<Bikewale.Entities.UserReviews.ReviewRatingEntityBase, Bikewale.DTO.UserReviews.v2.ReviewRatingBase>();
                cfg.CreateMap<Bikewale.Entities.UserReviews.ReviewRatingEntityBase, Bikewale.Entities.DTO.ReviewRatingBase>();
                cfg.CreateMap<Bikewale.Entities.UserReviews.ReviewsListEntity, Bikewale.Entities.DTO.ReviewsList>();
                cfg.CreateMap<Bikewale.Entities.UserReviews.ReviewTaggedBikeEntity, Bikewale.Entities.DTO.ReviewTaggedBike>();
                cfg.CreateMap<Bikewale.DTO.NewBikeSearch.SearchFilterDTO, Bikewale.Entities.NewBikeSearch.SearchFilters>();
                cfg.CreateMap<Bikewale.Entities.NewBikeSearch.SearchOutputEntity, Bikewale.DTO.NewBikeSearch.SearchOutput>();
                cfg.CreateMap<Bikewale.Entities.NewBikeSearch.SearchOutputEntityBase, Bikewale.DTO.NewBikeSearch.SearchOutputBase>();
                cfg.CreateMap<Bikewale.Entities.Used.SellBikeAd, Bikewale.DTO.UsedBikes.SellBikeAdDTO>();
                cfg.CreateMap<Bikewale.DTO.UsedBikes.SellBikeAdDTO, Bikewale.Entities.Used.SellBikeAd>();
                cfg.CreateMap<Bikewale.Entities.Used.SellBikeAdOtherInformation, Bikewale.DTO.UsedBikes.SellBikeAdOtherInformationDTO>();
                cfg.CreateMap<Bikewale.DTO.UsedBikes.SellBikeAdOtherInformationDTO, Bikewale.Entities.Used.SellBikeAdOtherInformation>();
                cfg.CreateMap<Bikewale.Entities.Used.SellBikeAdStatusEntity, Bikewale.DTO.UsedBikes.SellBikeAdStatusDTO>();
                cfg.CreateMap<Bikewale.Entities.Used.SellBikeImageUploadResultBase, Bikewale.DTO.Used.SellBikeImageUploadResultDTOBase>();
                cfg.CreateMap<Bikewale.Entities.Used.SellBikeImageUploadResultEntity, Bikewale.DTO.Used.SellBikeImageUploadResultDTO>();
                cfg.CreateMap<Bikewale.DTO.UsedBikes.SellBikeInquiryResultDTO, Bikewale.Entities.Used.SellBikeInquiryResultEntity>();
                cfg.CreateMap<Bikewale.Entities.Used.SellBikeInquiryResultEntity, Bikewale.DTO.UsedBikes.SellBikeInquiryResultDTO>();
                cfg.CreateMap<Bikewale.DTO.UsedBikes.SellerDTO, Bikewale.Entities.Used.SellerEntity>();
                cfg.CreateMap<Bikewale.Entities.Used.SellerEntity, Bikewale.DTO.UsedBikes.SellerDTO>();
                cfg.CreateMap<Bikewale.Entities.BikeData.SimilarBikeEntity, Bikewale.DTO.BikeData.SimilarBike>();
                cfg.CreateMap<Bikewale.Entities.BikeData.SpecsItem, DTO.BikeData.v2.VersionMinSpecs>();
                cfg.CreateMap<Bikewale.Entities.SplashScreenEntity, Bikewale.DTO.SplashScreen>();
                cfg.CreateMap<Bikewale.Entities.Location.StateEntityBase, Bikewale.DTO.PriceQuote.DetailedDealerQuotation.DDQStateBase>();
                cfg.CreateMap<Bikewale.Entities.Location.StateEntityBase, Bikewale.DTO.PriceQuote.DealerPriceQuote.DPQStateBase>();
                cfg.CreateMap<Bikewale.Entities.Location.StateEntityBase, Bikewale.DTO.State.StateBase>();
                cfg.CreateMap<QuestionsAnswers.Entities.Tag, Bikewale.Entities.QuestionAndAnswers.Tag>();
                cfg.CreateMap<Bikewale.Entities.BikeData.UpcomingBikeEntity, Bikewale.DTO.BikeData.UpcomingBike>();
                cfg.CreateMap<Bikewale.Entities.BikeData.UpcomingBikeEntity, Bikewale.DTO.BikeData.Upcoming.UpcomingBikeDTOBase>();
                cfg.CreateMap<Bikewale.Entities.BikeData.UpcomingBikeResult, Bikewale.DTO.BikeData.UpcomingBikeResultDTO>();
                cfg.CreateMap<Bikewale.Entities.BikeData.UpcomingBikesListInputEntity, Bikewale.DTO.BikeData.Upcoming.InputFilterDTO>();
                cfg.CreateMap<Bikewale.Entities.Used.UsedBikeSellerBase, Bikewale.DTO.UsedBikes.UsedBikeSellerBaseDTO>();
                cfg.CreateMap<Bikewale.Entities.UserReviews.UserReviewOverallRating, Bikewale.DTO.UserReviews.UserReviewOverallRatingDto>();
                cfg.CreateMap<Bikewale.Entities.UserReviews.UserReviewQuestion, Bikewale.DTO.UserReviews.UserReviewQuestionDto>();
                cfg.CreateMap<Bikewale.Entities.UserReviews.UserReviewRating, Bikewale.DTO.UserReviews.UserReviewRatingDto>();
                cfg.CreateMap<Bikewale.Entities.UserReviews.UserReviewRatingObject, Bikewale.DTO.UserReviews.RatingReviewInput>();
                cfg.CreateMap<Bikewale.Entities.UserReviews.UserReviewSummary, Bikewale.DTO.UserReviews.UserReviewSummaryDto>();
                cfg.CreateMap<Bikewale.Entities.CMS.Articles.VehicleTag, Bikewale.DTO.CMS.Articles.CMSVehicleTag>();
                cfg.CreateMap<Bikewale.Entities.BikeData.VersionColor, Bikewale.DTO.PriceQuote.CustomerDetails.PQColor>();
                cfg.CreateMap<Bikewale.Entities.PriceQuote.VersionPriceEntity, DTO.PriceQuote.VersionPriceBase>();
                cfg.CreateMap<Bikewale.DTO.UserReviews.WriteReviewInput, Bikewale.Entities.UserReviews.ReviewSubmitData>();
                cfg.CreateMap<Entities.PriceQuote.v3.PQByCityAreaEntity, Bikewale.DTO.PriceQuote.Version.v4.PQByCityAreaDTO>();
                cfg.CreateMap<Entities.PriceQuote.v2.PQCustomerDetailOutputEntity, DTO.PriceQuote.v4.PQCustomerDetailOutput>();
                cfg.CreateMap<DTO.PriceQuote.v2.PQCustomerDetailInput, Entities.PriceQuote.v2.PQCustomerDetailInput>();
                cfg.CreateMap<Bikewale.DTO.MaskingNumber.MaskingNumberLeadInputDto, Bikewale.Entities.MaskingNumber.MaskingNumberLeadEntity>();
                cfg.CreateMap<Bikewale.Entities.PriceQuote.BikeQuotationEntity, Bikewale.Entities.PriceQuote.v2.BikeQuotationEntity>();
                #endregion
                #region ForMemberMapping

                cfg.CreateMap<Bikewale.Entities.PriceQuote.DealerQuotationEntity, Bikewale.DTO.DealerLocator.DealerBase>().ForMember(d => d.Area, opt => opt.MapFrom(s => s.DealerDetails.objArea.AreaName))
                .ForMember(d => d.DealerId, opt => opt.MapFrom(s => s.DealerDetails.DealerId))
                .ForMember(d => d.DealerPkgType, opt => opt.MapFrom(s => s.DealerDetails.DealerPackageType))
                .ForMember(d => d.MaskingNumber, opt => opt.MapFrom(s => s.DealerDetails.MaskingNumber))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.DealerDetails.Organization));

                cfg.CreateMap<Entities.BikeBooking.NewBikeDealers, DTO.PriceQuote.v2.PQPrimaryDealer>().ForMember(d => d.Address, opt => opt.MapFrom(s => s.Address))
                .ForMember(d => d.ContactNo, opt => opt.MapFrom(s => s.MaskingNumber))
                .ForMember(d => d.DealerType, opt => opt.MapFrom(s => s.DealerPackageType))
                .ForMember(d => d.Email, opt => opt.MapFrom(s => s.EmailId))
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.DealerId))
                .ForMember(d => d.Latitude, opt => opt.MapFrom(s => s.objArea.Latitude))
                .ForMember(d => d.Longitude, opt => opt.MapFrom(s => s.objArea.Longitude))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Organization));

                cfg.CreateMap<Entities.BikeBooking.PQ_Price, DTO.PriceQuote.v2.DPQ_Price>().ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.CategoryName))
                .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.CategoryName))
                .ForMember(d => d.Price, opt => opt.MapFrom(s => s.Price))
                .ForMember(d => d.Price, opt => opt.MapFrom(s => s.Price));

                cfg.CreateMap<Entities.BikeData.MostPopularBikesBase, DTO.DealerLocator.v2.DealerBikeBase>().ForMember(d => d.Bike, opt => opt.MapFrom(s => s.BikeName)).ForMember(d => d.VersionId, opt => opt.MapFrom(s => s.objVersion.VersionId));

                cfg.CreateMap<Entities.DealerBenefitEntity, DTO.PriceQuote.v2.DPQBenefit>().ForMember(d => d.CategoryId, opt => opt.MapFrom(s => s.CatId))
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.BenefitId))
                .ForMember(d => d.Text, opt => opt.MapFrom(s => s.BenefitText));

                cfg.CreateMap<Entities.DealerLocator.DealersList, DTO.Dealer.v2.NewBikeDealerBase>().ForMember(d => d.Address, opt => opt.MapFrom(s => s.Address))
                .ForMember(d => d.CampaignId, opt => opt.MapFrom(s => s.CampaignId))
                .ForMember(d => d.ContactNo, opt => opt.MapFrom(s => s.MaskingNumber))
                .ForMember(d => d.DealerType, opt => opt.MapFrom(s => s.DealerType))
                .ForMember(d => d.Email, opt => opt.MapFrom(s => s.EMail))
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.DealerId))
                .ForMember(d => d.IsFeatured, opt => opt.MapFrom(s => s.IsFeatured))
                .ForMember(d => d.Latitude, opt => opt.MapFrom(s => s.objArea.Latitude))
                .ForMember(d => d.Longitude, opt => opt.MapFrom(s => s.objArea.Longitude))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name));
                cfg.CreateMap<Entities.OfferEntityBase, DTO.PriceQuote.v2.DPQOffer>().ForMember(d => d.Id, opt => opt.MapFrom(s => s.OfferId))
                .ForMember(d => d.OfferCategoryId, opt => opt.MapFrom(s => s.OfferCategoryId))
                .ForMember(d => d.Text, opt => opt.MapFrom(s => s.OfferText));

                cfg.CreateMap<Bikewale.ManufacturerCampaign.Entities.ManufacturerCampaignLeadConfiguration, Bikewale.DTO.Campaign.ManufacturerLeadCampaignDto>().ForMember(output => output.PopupSuccessMessage, opt => opt.MapFrom(input => string.Format(input.PopupSuccessMessage, input.Organization))).ForMember(output => output.LeadsButtonTextMobile, opt => opt.MapFrom(input => string.Format(input.LeadsButtonTextMobile, input.Organization)));
                cfg.CreateMap<Nest.SuggestOption<Bikewale.Entities.AutoComplete.SuggestionOutput>, Bikewale.DTO.AutoComplete.SuggestionList>().ForMember(d => d.Text, opt => opt.MapFrom(s => s.Source.output)).ForMember(d => d.Payload, opt => opt.MapFrom(s => s.Source.Payload));

                cfg.CreateMap<Bikewale.Entities.BikeBooking.PQ_BikeVarient, DTO.PriceQuote.v2.DPQVersionBase>().ForMember(d => d.PriceList, opt => opt.MapFrom(s => s.PriceList))
                .ForMember(d => d.VersionId, opt => opt.MapFrom(s => s.objVersion.VersionId))
                .ForMember(d => d.VersionName, opt => opt.MapFrom(s => s.objVersion.VersionName));

                cfg.CreateMap<Bikewale.Entities.UserReviews.UserReviewSummary, Bikewale.DTO.UserReviews.UserReviewSummaryDto>().ForMember(x => x.ReviewAge, opt => opt.MapFrom(s => !string.IsNullOrEmpty(s.ReviewAge) ? string.Format("{0} ago", s.ReviewAge) : ""));

                cfg.CreateMap<QuestionsAnswers.Entities.Customer, Bikewale.Entities.Customer.CustomerEntityBase>().ForMember(d => d.CustomerId, opt => opt.MapFrom(s => s.Id)).ForMember(d => d.CustomerName, opt => opt.MapFrom(s => s.Name)).ForMember(d => d.CustomerEmail, opt => opt.MapFrom(s => s.Email));

                cfg.CreateMap<Bikewale.Entities.Customer.CustomerEntityBase, QuestionsAnswers.Entities.Customer>().ForMember(d => d.Id, opt => opt.MapFrom(s => s.CustomerId)).ForMember(d => d.Name, opt => opt.MapFrom(s => s.CustomerName)).ForMember(d => d.Email, opt => opt.MapFrom(s => s.CustomerEmail));

                cfg.CreateMap<Bikewale.Entities.BikeData.ModelColorImage, Bikewale.DTO.Model.ModelColorPhoto>().ForMember(dest => dest.ModelColorId, opt => opt.MapFrom(src => src.Id));

                cfg.CreateMap<Bikewale.Entities.BikeBooking.NewBikeDealers, Bikewale.DTO.Campaign.DealerCampaignDto>().ForMember(output => output.Area, opt => opt.MapFrom(input => input.objArea.AreaName)).ForMember(output => output.AreaId, opt => opt.MapFrom(input => input.objArea.AreaId)).ForMember(output => output.City, opt => opt.MapFrom(input => input.objCity.CityName)).ForMember(output => output.CityId, opt => opt.MapFrom(input => input.objCity.CityId));

                cfg.CreateMap<Questions, Bikewale.DTO.QuestionAndAnswers.QuestionsDTO>().ForMember(d => d.QuestionsAnswersList, opt => opt.MapFrom(s => s.QuestionList));


                cfg.CreateMap<Bikewale.ManufacturerCampaign.Entities.ManufacturerCampaignEMIConfiguration, Bikewale.DTO.Campaign.ManufacturerEmiCampaignDto>().ForMember(output => output.PopupSuccessMessage, opt => opt.MapFrom(input => string.Format(input.PopupSuccessMessage, input.Organization)));
                #endregion
            });

        }
    }
}