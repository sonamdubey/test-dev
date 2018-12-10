using AutoMapper;
using Carwale.Adapters.Mappers;
using Carwale.DTOs.CarData;
using Carwale.DTOs.Classified.PopularUC;
using Carwale.DTOs.CMS;
using Carwale.DTOs.CMS.Articles;
using Carwale.DTOs.CMS.ThreeSixtyView;
using Carwale.DTOs.CMS.UserReviews;
using Carwale.DTOs.Deals;
using Carwale.DTOs.Finance;
using Carwale.DTOs.Geolocation;
using Carwale.DTOs.Insurance;
using Carwale.DTOs.IPToLocation;
using Carwale.DTOs.NewCars;
using Carwale.DTOs.Offers;
using Carwale.DTOs.PriceQuote;
using Carwale.DTOs.ViewModels;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.Classification;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.CMS.UserReviews;
using Carwale.Entity.CompareCars;
using Carwale.Entity.Dealers;
using Carwale.Entity.Deals;
using Carwale.Entity.Elastic;
using Carwale.Entity.Enum;
using Carwale.Entity.Finance;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Insurance;
using Carwale.Entity.Offers;
using Carwale.Entity.PriceQuote;
using Carwale.Entity.Vernam;
using Carwale.Entity.ViewModels;
using Carwale.Service.Mappers;
using Carwale.Utility;
using FluentValidation.WebApi;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using vs = VerNam.ProtoClass;

namespace Carwale.Service
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            Mapper.Initialize(p =>
            {
                p.AddProfile<GeoLocationProfile>();
                p.AddProfile<CampaignProfile>();
                p.AddProfile<DealerProfile>();
                p.AddProfile<PriceQuoteProfile>();
                p.AddProfile<PricesProfile>();
                p.AddProfile<OfferProfile>();
                p.AddProfile<CustomerVerificationProfile>();
                Carwale.Adapters.AdapterMappers.CreateMaps(p);
            });
            //MAPPERS

            Mapper.CreateMap<CarMakeEntityBase, CarMakesDTOV1>();
            Mapper.CreateMap<Carwale.Entity.CompareCars.Item, Carwale.DTOs.CarData.Item>()
                .ForMember(x => x.Name, o => o.MapFrom(s => s.Name.Replace("~", string.Empty)))
                .ForMember(x => x.Value, o => o.MapFrom(s => s.Values.Select(y => y.Replace("~", string.Empty)).ToList()));
            Mapper.CreateMap<Carwale.Entity.CompareCars.SubCategory, Carwale.DTOs.CarData.SubCategory>()
                .ForMember(x => x.Name, o => o.MapFrom(s => s.Name.Replace("~", string.Empty)));
            Mapper.CreateMap<Carwale.Entity.CompareCars.Color, Carwale.DTOs.CarData.Color>();
            Mapper.CreateMap<Carwale.Entity.CompareCars.Color, VersionColorDto>()
                .ForMember(x => x.HexCodes, o => o.MapFrom(s => new List<string>(s.Value.ConvertStringToList<string>())));
            Mapper.CreateMap<Carwale.Entity.CarWithImageEntity, Carwale.DTOs.CarData.CarWithImageEntityDTO>();

            Mapper.CreateMap<NewCarVersionsDTO, NewCarVersionsDTOV2>();
            Mapper.CreateMap<CarModelDetails, SimilarCarModels>()
                .ForMember(dest => dest.ModelImageOriginal, o => o.MapFrom(src => src.OriginalImage))
                .ForMember(dest => dest.PopularVersionId, o => o.MapFrom(src => src.PopularVersion))
                .ForMember(dest => dest.ReviewRate, o => o.MapFrom(src => src.ModelRating))
                .ForMember(dest => dest.LaunchDate, o => o.MapFrom(src => src.ModelLaunchDate));
            Mapper.CreateMap<SimilarCarModels, SimilarCarModelsDTOV2>();
            Mapper.CreateMap<CarModelDetails, CarModelDetailsDTO>();
            Mapper.CreateMap<CarModelDetails, CarModelDetailsDTO_V1>();
            Mapper.CreateMap<CarModelDetails, CarModelDetailsDtoV2>();

            Mapper.CreateMap<SimilarCarModels, SimilarCarModelsDTO>()
                .ForMember(x => x.MinPriceNew, o => o.MapFrom(s => s.MinPrice))
                .ForMember(x => x.ReviewRateNew, o => o.MapFrom(s => s.ReviewRate));
            Mapper.CreateMap<SimilarCarModels, SimilarCarModelsDTO_V1>();
            Mapper.CreateMap<SimilarCarModels, SimilarCarModelsDtoV3>();
            Mapper.CreateMap<CarModelDetails, SimilarCarModelsDtoV3>()
                .ForMember(d => d.ModelImageOriginal, o => o.MapFrom(s => s.OriginalImage))
                .ForMember(d => d.HostUrl, o => o.MapFrom(s => CWConfiguration._imgHostUrl));
            Mapper.CreateMap<CarVersions, CarVersionDTO_V1>().ForMember(d => d.TransmissionType, o => o.MapFrom(s => s != null ? s.TransmissionType ?? string.Empty : string.Empty));
            Mapper.CreateMap<CarVersions, CarVersionDtoV3>().ForMember(d => d.TransmissionType, o => o.MapFrom(s => s != null ? s.TransmissionType ?? string.Empty : string.Empty));
            Mapper.CreateMap<CarVersions, CarVersionDTO>();
            Mapper.CreateMap<MileageDataEntity, MileageDataDTO>()
                .ForMember(d => d.Transmission, o => o.MapFrom(s => s != null ? s.Transmission ?? string.Empty : string.Empty))
                .ForMember(d => d.FuelUnit, o => o.MapFrom(s => s.MileageUnit ?? string.Empty))
                .ForMember(d => d.FinalAverage, o => o.MapFrom(s => s.Arai));
            Mapper.CreateMap<MileageDataEntity, MileageDataDTO_V1>()
                .ForMember(d => d.Transmission, o => o.MapFrom(s => s != null ? s.Transmission ?? string.Empty : string.Empty))
                .ForMember(d => d.FuelUnit, o => o.MapFrom(s => s.MileageUnit ?? string.Empty))
                .ForMember(d => d.Displacement, o => o.MapFrom(s => Format.TrimCcFromDisplacement(s.Displacement)))
                .ForMember(d => d.FinalAverage, o => o.MapFrom(s => s.Arai));
            Mapper.CreateMap<CarModelSummary, CarModelSummaryDTOV2>();
            Mapper.CreateMap<ModelSummary, CarModelSummaryDTOV2>();
            Mapper.CreateMap<Cities, IPToLocationDTO>();
            Mapper.CreateMap<Carwale.Entity.CarData.CarModelSummary, Carwale.DTOs.CarData.CarModelSummaryDTOV2>();
            Mapper.CreateMap<CCarData, CompareCarsDetailModel>();
            Mapper.CreateMap<CCarData, CCarDataDto>();

            Mapper.CreateMap<Carwale.Entity.CarData.CarModelSummary, Carwale.DTOs.CarData.CarMakesDTO>();
            Mapper.CreateMap<Carwale.Entity.CarData.CarModelSummary, Carwale.DTOs.CarData.CarModelsDTO>();
            Mapper.CreateMap<Carwale.Entity.CarData.CarModelSummary, Carwale.DTOs.CarData.CarPricesDTO>();
            Mapper.CreateMap<Carwale.Entity.CarData.CarModelSummary, Carwale.DTOs.CarData.CarImageBaseDTO>()
                .ForMember(x => x.OriginalImgPath, o => o.MapFrom(s => s.OriginalImage));

            Mapper.CreateMap<Carwale.Entity.CarData.CarModelSummary, Carwale.DTOs.CarData.CarModelSummaryDTO>()
                .ForMember(x => x.CarMake, o => o.MapFrom(s => s))
                .ForMember(x => x.CarModel, o => o.MapFrom(s => s))
                .ForMember(x => x.CarPrices, o => o.MapFrom(s => s))
                .ForMember(x => x.CarImageBase, o => o.MapFrom(s => s));

            //EDIT CMS API DTOS
            Mapper.CreateMap<Carwale.Entity.CMS.Articles.MakeAndModelDetail, Carwale.DTOs.CMS.Articles.MakeAndModelDetail>();

            Mapper.CreateMap<Carwale.Entity.CarData.CarMakeEntityBase, Carwale.DTOs.CarData.CarMakesDTO>();
            Mapper.CreateMap<Carwale.Entity.CarData.CarModelEntityBase, Carwale.DTOs.CarData.CarModelsDTO>();
            Mapper.CreateMap<Carwale.Entity.CarData.CarVersionEntity, Carwale.DTOs.CarData.CarVersionsDTO>();

            Mapper.CreateMap<Carwale.Entity.CarData.CarModelDetails, Carwale.Entity.CarData.CarVersionDetails>()
                .ForMember(x => x.New, y => y.MapFrom(z => Convert.ToInt32(z.New)));

            Mapper.CreateMap<DiscountSummary, DiscountSummaryDTO_Android>();

            //Deals Recommendation API
            Mapper.CreateMap<DealsStock, DealsRecommendationDTO>()
                 .ForMember(x => x.CityId, o => o.MapFrom(s => s.City.CityId))
                 .ForMember(x => x.CityName, o => o.MapFrom(s => s.City.CityName));

            Mapper.CreateMap<DealsStock, DealsStockDTO>();

            //Advantage Product Details
            Mapper.CreateMap<DealsInquiryDetailDTO, DealsInquiryDetail>();
            Mapper.CreateMap<DealsTestimonialEntity, DealsTestimonialDTO>();

            //Advantage booking
            Mapper.CreateMap<MakeEntity, CarMakesDTO>();
            Mapper.CreateMap<ModelEntity, CarModelsDTO>();
            Mapper.CreateMap<CarVersionEntity, Versions>()
                .ForMember(dest => dest.ID,
            opts => opts.MapFrom(
               src => src.ID))
               .ForMember(dest => dest.Name,
            opts => opts.MapFrom(
               src => src.Name));
            Mapper.CreateMap<VersionBase, CarVersionsDTO>()
                .ForMember(dest => dest.ID,
            opts => opts.MapFrom(
               src => src.VersionId))
               .ForMember(dest => dest.Name,
            opts => opts.MapFrom(
               src => src.VersionName));
            Mapper.CreateMap<ColorEntity, CarColorDTO>();

            Mapper.CreateMap<DealsStock, DealsStockAndroid_DTO>();
            Mapper.CreateMap<Entity.Deals.ProductDetails, ProductDetailsDTO_Android>();
            Mapper.CreateMap<DealsStock, BookingAndroid_DTO>();

            // Added for Mapping dealsSummary to dealsStock 
            Mapper.CreateMap<DealsStock, DealsSummaryDesktop_DTO>()
                .ForMember(x => x.IsOfferAvailable, o => o.MapFrom(s => (!string.IsNullOrWhiteSpace(s.Offers))));
            Mapper.CreateMap<DealsStock, DealsSummaryMobile_DTO>();

            // Added for Mapping versionDTO to Entity
            Mapper.CreateMap<Carwale.Entity.CarData.CarVersions, Carwale.DTOs.NewCars.NewCarVersionsDTO>();
            Mapper.CreateMap<Carwale.Entity.Deals.DiscountSummary, Carwale.DTOs.Deals.DiscountSummaryDTO>();

            //Advantage Landing page Make-Model For prefilling make and model dropdowns in find cars section
            Mapper.CreateMap<MakeEntity, CarMakesDTO>();
            Mapper.CreateMap<ModelEntity, CarModelsDTO>();
            Mapper.CreateMap<CompareCarsDetails, CompareCarsDetailsDTO>();
            Mapper.CreateMap<CompareCarOverview, CompareCarOverviewDTO>();
            Mapper.CreateMap<CompareCarVersionInfo, CompareCarVersionInfoDTO>();
            Mapper.CreateMap<CarModelDetails, CarModelsDTO>();
            Mapper.CreateMap<CarVersionDetails, CarOverviewDTOV2>();
            Mapper.CreateMap<CarModelDetails, CarImageBaseDTO>()
                .ForMember(x => x.OriginalImgPath, o => o.MapFrom(s => s.OriginalImage));
            Mapper.CreateMap<CarModelDetails, BodyTypeBaseDTO>()
                .ForMember(x => x.Id, o => o.MapFrom(s => s.BodyStyleId));
            Mapper.CreateMap<CarModelDetails, CarMakesDTO>();
            Mapper.CreateMap<CarModelDetails, Carwale.DTOs.CarData.CarDetailsDTO>()
              .ForMember(x => x.CarMake, opt => opt.MapFrom(s => Mapper.Map<CarMakesDTO>(s)))
              .ForMember(x => x.CarModel, opt => opt.MapFrom(s => Mapper.Map<CarModelsDTO>(s)))
              .ForMember(x => x.CarImageBase, opt => opt.MapFrom(s => Mapper.Map<CarImageBaseDTO>(s)))
              .ForMember(x => x.BodyType, opt => opt.MapFrom(s => Mapper.Map<BodyTypeBaseDTO>(s)));
            Mapper.CreateMap<CarVersionDetails, Carwale.DTOs.CarData.CarDetailsDTO>()
              .ForMember(x => x.CarMake, opt => opt.MapFrom(s => Mapper.Map<CarMakesDTO>(s)))
              .ForMember(x => x.CarModel, opt => opt.MapFrom(s => Mapper.Map<CarModelsDTO>(s)))
              .ForMember(x => x.CarVersion, opt => opt.MapFrom(s => Mapper.Map<PQCarVersionDTO>(s)))
              .ForMember(x => x.CarImageBase, opt => opt.MapFrom(s => Mapper.Map<CarImageBaseDTO>(s)));
            Mapper.CreateMap<Carwale.DTOs.PriceQuote.ModelPriceDTO, CarPricesDTO>();
            Mapper.CreateMap<DiscountSummary, Carwale.DTOs.CarData.DiscountSummaryDTO>();
            Mapper.CreateMap<AdvantageSearchResults, AdvantageSearchResultsDTO>();
            Mapper.CreateMap<FilterCountEntity, FilterCountDTO>();
            Mapper.CreateMap<Carwale.Entity.Classified.StockMake, StockMakeDTO>();
            Mapper.CreateMap<Carwale.DTOs.Elastic.CityPayLoad, Carwale.DTOs.CityResultsDTO>();
            Mapper.CreateMap<Carwale.DTOs.Elastic.Autocomplete.Area.AreaPayLoad, Carwale.DTOs.AreaResultsDTO>();
            //Mapper for CarVersionDetails to CarMakesDTO, CarModelsDTO, CarVersionDTO, CarImageBaseDTO
            Mapper.CreateMap<CarVersionDetails, CarMakesDTO>()
                .ForMember(d => d.MakeId, o => o.MapFrom(s => s.MakeId))
                .ForMember(d => d.MakeName, o => o.MapFrom(s => s.MakeName));
            Mapper.CreateMap<CarVersionDetails, CarModelsDTO>()
                .ForMember(d => d.ModelId, o => o.MapFrom(s => s.ModelId))
                .ForMember(d => d.ModelName, o => o.MapFrom(s => s.ModelName))
                .ForMember(d => d.MaskingName, o => o.MapFrom(s => s.MaskingName));
            Mapper.CreateMap<CarVersionDetails, PQCarVersionDTO>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.VersionId))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.VersionName))
                .ForMember(d => d.SpecsSummary, o => o.MapFrom(s => s.SpecSummery))
                .ForMember(d => d.ReviewRate, o => o.MapFrom(s => s.ReviewRate));
            Mapper.CreateMap<CarVersionDetails, CarImageBaseDTO>()
                .ForMember(d => d.HostUrl, o => o.MapFrom(s => s.HostURL))
                .ForMember(d => d.OriginalImgPath, o => o.MapFrom(s => s.OriginalImgPath));
            Mapper.CreateMap<CarVersionEntity, Versions>();
            Mapper.CreateMap<PQOfferEntity, PQOffersDTO>();
            Mapper.CreateMap<SimilarCarModelsDTO, SimilarCarsAndroidDTO>()
                .ForMember(d => d.largePicUrl, o => o.MapFrom(s => s.LargePic))
                .ForMember(d => d.minPrice, o => o.MapFrom(s => s.MinPrice))
                .ForMember(d => d.maxPrice, o => o.MapFrom(s => s.MaxPrice))
                .ForMember(d => d.make, o => o.MapFrom(s => s.MakeName))
                .ForMember(d => d.model, o => o.MapFrom(s => s.ModelName))
                .ForMember(d => d.modelId, o => o.MapFrom(s => s.ModelId))
                .ForMember(d => d.reviewRate, o => o.MapFrom(s => s.ReviewRate))
                .ForMember(d => d.reviewCount, o => o.MapFrom(s => s.ReviewCount))
                .ForMember(d => d.smallPicUrl, o => o.MapFrom(s => s.SmallPic))
                .ForMember(d => d.versionId, o => o.MapFrom(s => s.PopularVersionId))
                .ForMember(d => d.OriginalImgPath, o => o.MapFrom(s => s.ModelImageOriginal));
            Mapper.CreateMap<Sponsored_CarLink, AdLinkDTO>();
            Mapper.CreateMap<CarIdEntity, CarVersionDetails>();

            //Coverfox
            Mapper.CreateMap<Carwale.Entity.Insurance.Coverfox.Model, ModelBase>()
                .ForMember(d => d.ModelId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.ModelName, o => o.MapFrom(s => s.Name));
            Mapper.CreateMap<Carwale.Entity.Insurance.Coverfox.Version, VersionBase>()
                .ForMember(d => d.VersionId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.VersionName, o => o.MapFrom(s => s.Name));
            Mapper.CreateMap<Carwale.Entity.Insurance.Coverfox.RTO, InsuranceCity>()
                .ForMember(d => d.CityName, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.StateName, o => o.MapFrom(s => s.Name));

            Mapper.CreateMap<CarMakeEntityBase, MakeEntity>().ForMember(src => src.MakeId, dest => dest.MapFrom(y => y.MakeId))
                                                                    .ForMember(src => src.MakeName, dest => dest.MapFrom(y => y.MakeName));

            Mapper.CreateMap<CarModelEntityBase, ModelBase>().ForMember(src => src.ModelName, dest => dest.MapFrom(y => y.ModelName))
                                                                     .ForMember(src => src.ModelId, dest => dest.MapFrom(y => y.ModelId));

            Mapper.CreateMap<CarVersionEntity, VersionBase>().ForMember(src => src.VersionId, dest => dest.MapFrom(y => y.ID))
                                                                     .ForMember(src => src.VersionName, dest => dest.MapFrom(y => y.Name));

            Mapper.CreateMap<CarVersionEntity, VersionPrice>().ForMember(src => src.VersionBase, dest => dest.MapFrom(y => y));

            Mapper.CreateMap<Cities, InsuranceCity>().ForMember(src => src.CityId, dest => dest.MapFrom(y => y.CityId))
                                                         .ForMember(src => src.CityName, dest => dest.MapFrom(y => y.CityName))
                                                         .ForMember(src => src.StateName, dest => dest.MapFrom(y => y.StateName));

            Mapper.CreateMap<InsuranceCity, InsuranceCityDTO>().ForMember(src => src.Id, dest => dest.MapFrom(y => y.CityId))
                                                         .ForMember(src => src.Name, dest => dest.MapFrom(y => y.CityName))
                                                         .ForMember(src => src.StateName, dest => dest.MapFrom(y => y.StateName));

            Mapper.CreateMap<MakeModelEntity, CarEntityDTO>();
            Mapper.CreateMap<SimilarModelRecommendation, MakeModelEntity>()
                .ForMember(src => src.MakeId, dest => dest.MapFrom(y => y.CarMake.MakeId))
                .ForMember(src => src.MakeName, dest => dest.MapFrom(y => y.CarMake.MakeName))
                .ForMember(src => src.ModelName, dest => dest.MapFrom(y => y.CarModel.ModelName))
                .ForMember(src => src.ModelId, dest => dest.MapFrom(y => y.CarModel.ModelId));

            Mapper.CreateMap<CarModelDetails, CarModelSummary>();

            Mapper.CreateMap<CarVersionDetails, VersionDetailsDTO>();

            Mapper.CreateMap<CarVersionDetails, VersionListDTO>()
                .ForMember(src => src.Id, o => o.MapFrom(dest => dest.VersionId))
                .ForMember(src => src.Name, o => o.MapFrom(dest => dest.VersionName))
                .ForMember(src => src.TransmissionType, o => o.MapFrom(dest => dest.CarTransmission > 0 ? ((CarTransmissionType)dest.CarTransmission).ToString() : string.Empty));

            Mapper.CreateMap<Entity.CarData.TopSellingCarModel, TopSellingCarModelV2>()
                .ForMember(x => x.MakeId, o => o.MapFrom(s => s.Make.MakeId))
                .ForMember(x => x.MakeName, o => o.MapFrom(s => s.Make.MakeName))
                .ForMember(x => x.ModelId, o => o.MapFrom(s => s.Model.ModelId))
                .ForMember(x => x.ModelName, o => o.MapFrom(s => s.Model.ModelName))
                .ForMember(x => x.MaskingName, o => o.MapFrom(s => s.Model.MaskingName))
                .ForMember(x => x.ReviewCount, o => o.MapFrom(s => s.Review.ReviewCount))
                .ForMember(x => x.OverallRating, o => o.MapFrom(s => s.Review.OverallRating))
                .ForMember(x => x.OriginalImgPath, o => o.MapFrom(s => s.Image.ImagePath))
                .ForMember(x => x.HostUrl, o => o.MapFrom(s => s.Image.HostUrl))
                .ForMember(x => x.MaskingName, o => o.MapFrom(s => s.Model.MaskingName))
                .ForMember(x => x.Price, o => o.MapFrom(s => s.PriceOverview));

            Mapper.CreateMap<Entity.CarData.LaunchedCarModel, LaunchedCarModelV2>()
                .ForMember(x => x.MakeId, o => o.MapFrom(s => s.Make.MakeId))
                .ForMember(x => x.MakeName, o => o.MapFrom(s => s.Make.MakeName))
                .ForMember(x => x.ModelId, o => o.MapFrom(s => s.Model.ModelId))
                .ForMember(x => x.ModelName, o => o.MapFrom(s => s.Model.ModelName))
                .ForMember(x => x.Version, o => o.MapFrom(s => s.Version))
                .ForMember(x => x.MakeName, o => o.MapFrom(s => s.Make.MakeName))
                .ForMember(x => x.MaskingName, o => o.MapFrom(s => s.Model.MaskingName))
                .ForMember(x => x.ReviewCount, o => o.MapFrom(s => s.Review.ReviewCount))
                .ForMember(x => x.OverallRating, o => o.MapFrom(s => s.Review.OverallRating))
                .ForMember(x => x.OriginalImgPath, o => o.MapFrom(s => s.Image.ImagePath))
                .ForMember(x => x.HostUrl, o => o.MapFrom(s => s.Image.HostUrl))
                .ForMember(x => x.MaskingName, o => o.MapFrom(s => s.Model.MaskingName))
                .ForMember(x => x.Price, o => o.MapFrom(s => s.PriceOverview));

            Mapper.CreateMap<Entity.CarData.UpcomingCarModel, UpcomingModelV2>()
                .ForMember(x => x.MakeName, o => o.MapFrom(s => s.MakeName))
                .ForMember(x => x.MakeId, o => o.MapFrom(s => s.MakeId))
                .ForMember(x => x.ModelName, o => o.MapFrom(s => s.ModelName))
                .ForMember(x => x.ModelId, o => o.MapFrom(s => s.ModelId))
                .ForMember(x => x.HostUrl, o => o.MapFrom(s => s.Image.HostUrl))
                .ForMember(x => x.OriginalImgPath, o => o.MapFrom(s => s.Image.ImagePath))
                .ForMember(x => x.ExpectedLaunch, o => o.MapFrom(s => s.ExpectedLaunch))
                .ForMember(x => x.Price, o => o.MapFrom(s => s.Price))
                .ForMember(x => x.ExpectedLaunchId, o => o.MapFrom(s => s.ExpectedLaunchId))
                .ForMember(x => x.LaunchLabel, o => o.MapFrom(s => ConfigurationManager.AppSettings["UpcomingPriceDate"]));

            Mapper.CreateMap<BodyType, DTOs.Classification.BodyTypeDTO>();
            Mapper.CreateMap<FuelTypes, FuelTypesDto>();
            Mapper.CreateMap<MakeLogoEntity, CarMakeLogoV2>();

            Mapper.CreateMap<Carwale.Entity.CompareCars.HotCarComparison, HotCarComparisonV2>();
            Mapper.CreateMap<Carwale.Entity.CompareCars.ComparisonCarModel, ComparisonCarModelV2>();

            Mapper.CreateMap<Video, Carwale.DTOs.YouTubeVideoV2>()
                .ForMember(x => x.Url, o => o.MapFrom(s => s.VideoUrl))
                .ForMember(x => x.Title, o => o.MapFrom(s => s.VideoTitle))
                .ForMember(x => x.Views, o => o.MapFrom(s => s.Views))
                .ForMember(x => x.Likes, o => o.MapFrom(s => s.Likes))
                .ForMember(x => x.PublishedDate, o => o.MapFrom(s => s.DisplayDate))
                .ForMember(x => x.VideoId, o => o.MapFrom(s => s.VideoId))
                .ForMember(x => x.HostUrl, o => o.MapFrom(s => s.ImgHost))
                .ForMember(x => x.OriginalImgPath, o => o.MapFrom(s => s.ImagePath));

            Mapper.CreateMap<Video, CarImageBaseDTO>()
                 .ForMember(x => x.HostUrl, o => o.MapFrom(s => s.ImgHost))
                 .ForMember(x => x.OriginalImgPath, o => o.MapFrom(s => s.ImagePath));

            Mapper.CreateMap<PopularUCModelApp, PopularUCModelAppV2>()
                 .ForMember(x => x.carName, o => o.MapFrom(s => s.carName))
                .ForMember(x => x.appSimilarUrl, o => o.MapFrom(s => s.appSimilarUrl))
                .ForMember(x => x.appTitleUrl, o => o.MapFrom(s => s.appTitleUrl))
                .ForMember(x => x.HostUrl, o => o.MapFrom(s => s.Image.HostUrl))
                .ForMember(x => x.ImagePath, o => o.MapFrom(s => s.Image.ImagePath))
                .ForMember(x => x.price, o => o.MapFrom(s => s.price))
                .ForMember(x => x.bodyType, o => o.MapFrom(s => s.bodyType));

            Mapper.CreateMap<CarPrice, PriceOverviewDTO>()
                .ForMember(x => x.Price, o => o.MapFrom(s => ConfigurationManager.AppSettings["RupeeSymbol"] + Format.PriceLacCr(s.MinPrice.ToString("0.")) + ConfigurationManager.AppSettings["PriceSeperator"] + Format.PriceLacCr(s.MaxPrice.ToString("0."))))
                .ForMember(x => x.PriceLabel, o => o.MapFrom(s => ConfigurationManager.AppSettings["UpcomingPriceText"]));

            Mapper.CreateMap<MakeEntity, CarMakesDTO>()
                .ForMember(x => x.MakeId, o => o.MapFrom(s => s.MakeId))
                .ForMember(x => x.MakeName, o => o.MapFrom(s => s.MakeName));

            Mapper.CreateMap<ModelEntity, CarModelsDTO>()
                .ForMember(d => d.ModelId, o => o.MapFrom(s => s.ModelId))
                .ForMember(d => d.ModelName, o => o.MapFrom(s => s.ModelName));

            Mapper.CreateMap<CarVersionDetails, PQCarDetails>()
                .ForMember(x => x.LargePic, o => o.MapFrom(s => s.ModelImageLarge))
                .ForMember(x => x.SmallPic, o => o.MapFrom(s => s.ModelImageSmall))
                .ForMember(x => x.XtraLargePic, o => o.MapFrom(s => s.ModelImageXtraLarge));

            Mapper.CreateMap<DiscountSummaryDTO_Android, DiscountSummaryDTO_AndroidV1>();
            Mapper.CreateMap<RelatedArticles, RelatedArticlesDTO>();
            //UserReviews
            Mapper.CreateMap<UserReviewDetail, UserReviewDetailDTO>();
            Mapper.CreateMap<UserReviewEntity, UserReviewDTO>();
            Mapper.CreateMap<UserReviewDetail, UserReviewDetailDesktopDto>();
            Mapper.CreateMap<SponsoredDealer, SponsoredDealerDTO>();

            //Added for mapping QuotationDto to InsuranceResponse, CW.cs
            Mapper.CreateMap<QuotationDto, InsuranceResponse>()
                  .ForMember(dest => dest.QuoteId, opt => opt.MapFrom(src => src.UniqueId.ToNullSafeString()))
                  .ForMember(dest => dest.Total, opt => opt.MapFrom(src => CustomParser.parseIntObject(src.Quotation)))
                  .ForMember(dest => dest.Success, opt => opt.MapFrom(src => CustomParser.parseBoolObject(src.ConfirmationStatus)));

            //user profiling
            Mapper.CreateMap<ModelDetails, ModelSpecificationsEntity>()
                .ForMember(dest => dest.BodyStyle, opt => opt.MapFrom(src => src.BodyStyle ?? "NA"))
                .ForMember(dest => dest.SubSegment, opt => opt.MapFrom(src => src.SubSegment ?? "NA"))
                .ForMember(dest => dest.AvgPrice, opt =>
                    opt.MapFrom(src => src.IsUpcoming ? src.EstimatedPrice : 0));

            Mapper.CreateMap<ModelSpecificationsEntity, ModelSpecificationsDTO>()
                .ForMember(dest => dest.MakeMaskingName, opt => opt.MapFrom(src => Format.FormatSpecial(src.MakeName)));
            Mapper.CreateMap<VersionSubSegment, VersionSubSegmentDTO>();

            //Added for mapping FinanceLinkData Entity to FinanceLinkDto, FinanceLinkDataController.cs
            Mapper.CreateMap<FinanceLinkData, FinanceLinkDto>();
            Mapper.CreateMap<CarMakeModelAdEntityBase, CarMakeModelAdEntityBase>();

            Mapper.CreateMap<VersionEntity, CarVersionsDTO>()
                .ForMember(x => x.ID, opt => opt.MapFrom(src => CustomParser.parseIntObject(src.VersionId)))
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.VersionName.ToNullSafeString()));

            Mapper.CreateMap<ProtoBufClass.Common.Item, CityDTO>();
            Mapper.CreateMap<AEPLCore.Entity.Geolocation.City, Carwale.Entity.Geolocation.City>();
            Mapper.CreateMap<AEPLCore.Entity.Geolocation.States, Carwale.Entity.Geolocation.States>();
            // mapper to map trending models
            Mapper.CreateMap<CarMakeModelAdEntityBase, CarMakeModelDTO>()
                                .ForMember(x => x.MakeId, o => o.MapFrom(s => s.MakeId))
                                .ForMember(x => x.MakeName, o => o.MapFrom(s => s.MakeName))
                                .ForMember(x => x.ModelId, o => o.MapFrom(s => s.ModelId))
                                .ForMember(x => x.ModelName, o => o.MapFrom(s => s.ModelName))
                                .ForMember(x => x.MaskingName, o => o.MapFrom(s => s.MaskingName));

            // mapper to map trending sponsored models
            Mapper.CreateMap<GlobalSearchSponsoredModelEntity, CarMakeModelAdDTO>()
                        .ForMember(x => x.MakeId, o => o.MapFrom(s => s.MakeId))
                        .ForMember(x => x.MakeName, o => o.MapFrom(s => s.MakeName))
                        .ForMember(x => x.ModelId, o => o.MapFrom(s => s.ModelId))
                        .ForMember(x => x.ModelName, o => o.MapFrom(s => s.ModelName))
                        .ForMember(x => x.MaskingName, o => o.MapFrom(s => s.MaskingName))
                        .ForMember(x => x.StartDate, o => o.MapFrom(s => s.StartDate))
                        .ForMember(x => x.EndDate, o => o.MapFrom(s => s.EndDate))
                        .ForMember(x => x.AdPosition, o => o.MapFrom(s => s.AdPosition))
                        .ForMember(x => x.Priority, o => o.MapFrom(s => s.Priority));

            Mapper.CreateMap<ProtoBufClass.Campaigns.SponsoredCarComparisonData, FeaturedCarDataEntity>();
            Mapper.CreateMap<ModelsByRootAndYear, ModelsByRootAndYearDTO>();
            Mapper.CreateMap<BodyType, BodyTypesDto>();

            Mapper.CreateMap<CustLocation, CityDTO>()
                .ForMember(x => x.Id, o => o.MapFrom(s => s.CityId))
                .ForMember(x => x.Name, o => o.MapFrom(s => s.CityName));
            Mapper.CreateMap<CarVersions, CarVersionsV1>();
            Mapper.CreateMap<CarModelSummary, CarModelDetails>();

            Mapper.CreateMap<Cities, VersionCityPricesObj>()
                .ForMember(x => x.Location, o => o.MapFrom(s => new Entity.Elastic.Location { lat = CustomParser.parseDoubleObject(s.Lattitude), lon = CustomParser.parseDoubleObject(s.Longitude) }));
            Mapper.CreateMap<DTOs.Suggest, DTOs.AmpSuggest>();
            Mapper.CreateMap<CarModelDetails, ThreeSixtyAvailabilityDTO>();
            Mapper.CreateMap<VersionPrices, VersionPriceQuote>();
            Mapper.CreateMap<PQCarDetails, CarIdEntity>();
            Mapper.CreateMap<CarDetailsDTO, CarIdEntity>()
                .ForMember(d => d.MakeId, o => o.MapFrom(s => s.CarMake.MakeId))
                .ForMember(d => d.ModelId, o => o.MapFrom(s => s.CarModel.ModelId))
                .ForMember(d => d.VersionId, o => o.MapFrom(s => s.CarVersion.Id));
            Mapper.CreateMap<EMIInformation, EmiInformationDto_V1>();
            Mapper.CreateMap<EMIInformation, EmiInformationDtoV2>();
            Mapper.CreateMap<CarMakeEntityBase, CarMakesDtoV1>();
            Mapper.CreateMap<Cities, CitiesDTO>();
            Mapper.CreateMap<CarOverviewDTO, CarOverviewDTOV2>().ReverseMap();
            Mapper.CreateMap<PriceBreakUpModel, CarOverviewDTOV2>();
            AutoMapperCollection.PQAutoMapper();
            StockMappers.CreateMaps();
            UsedSearchMappers.CreateMaps();
            UsedLeadMappers.CreateMaps();
            UsedValuationMappers.CreateMaps();
            TyresMapping.CreateMap();
            ESMapping.CreateMap();
            CmsMappers.CreateMaps();
            NewCarFinder.CreateMaps();
            SellcarListingMapper.CreateMaps();
            BlockedCommunicationMapper.CreateMaps();
            CarDataMicroserviceMapper.CreateMaps();
            UserReviewMapper.CreateMaps();
            UsedFeatureListMapper.CreateMaps();
            UsedSpecificationListMapper.CreateMaps();

            config.DependencyResolver = new UnityResolver(UnityBootstrapper.Initialise());
            config.EnableCors();

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "carwale.services",
                routeTemplate: "webapi/{controller}/{action}"
            );

            config.Routes.MapHttpRoute(
               name: "carwale.services_new",
               routeTemplate: "api/{controller}/{action}",
               defaults: new { id = RouteParameter.Optional }
           );

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/csp-report"));
            FluentValidationModelValidatorProvider.Configure(config);

            //userreviews
            Mapper.CreateMap<UserReviewCustomerInfo, UserReviewCustomerDto>();

            Mapper.CreateMap<AreaCode, AreaDto>();
        }
    }
}
