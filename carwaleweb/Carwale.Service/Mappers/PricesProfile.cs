using AutoMapper;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Price;
using Carwale.Entity.PriceQuote;
using Carwale.Utility;
using System;
using System.Configuration;

namespace Carwale.Service.Mappers
{
    public class PricesProfile : Profile
    {
        private static string appsGstText = ConfigurationManager.AppSettings["AppsGSTText"] ?? string.Empty;
        private static string priceWithGstText = ConfigurationManager.AppSettings["PriceWithGSTText"] ?? string.Empty;
        private static string priceEstimatedGstText = ConfigurationManager.AppSettings["PriceEstimatedGSTText"] ?? string.Empty;
        private static string rupeeSymbol = ConfigurationManager.AppSettings["rupeeSymbol"] ?? string.Empty;
        private static string priceSuffix = ConfigurationManager.AppSettings["PriceSuffix"] ?? string.Empty;
        private static string pricePrefix = ConfigurationManager.AppSettings["PricePrefix"] ?? string.Empty;
        private static string priceNotAvailableText = ConfigurationManager.AppSettings["PriceNotAvailableText"] ?? string.Empty;

        protected override void Configure()
        {
            CreateMap<PriceOverview, PriceOverviewDTO>()
                .ForMember(d => d.Price, o => o.MapFrom(s => rupeeSymbol + Format.PriceLacCr(s.Price.ToString())))
                .ForMember(d => d.PriceLabel, o => o.MapFrom(s => s.PriceLabel.Replace(priceWithGstText, appsGstText).Replace(priceEstimatedGstText, appsGstText)))
                .ForMember(d => d.ReasonText, o => o.MapFrom(s => s.ReasonText))
                .ForMember(x => x.PriceSuffix, o => o.MapFrom(s => s.PriceVersionCount > 1 ? priceSuffix : string.Empty))
                .ForMember(x => x.PricePrefix, o => o.MapFrom(s => s.PriceVersionCount > 1 ? pricePrefix : string.Empty))
                .ForMember(x => x.LabelColor, o => o.MapFrom(s => (s.PriceStatus == (int)PriceBucket.PriceNotAvailable ? "#c1a611 " : s.PriceStatus == (int)PriceBucket.CarNotSold ? "#ef3f30 " : "#82888b ")))
                .ForMember(d => d.City, o => o.MapFrom(s => s.PriceStatus == (int)PriceBucket.HaveUserCity ? s.City : new City()))
                .ForMember(d => d.CityColor, o => o.MapFrom(s => string.Empty))
                .ForMember(d => d.PriceStatus, o => o.MapFrom(s => s.PriceStatus))
                .ForMember(d => d.PriceForSorting, o => o.MapFrom(s => s.Price))
                .ForMember(d => d.FormattedFullPrice, o => o.MapFrom(s => s.Price > 0 ? (rupeeSymbol + Format.FormatNumericCommaSep(s.Price.ToString())) : priceNotAvailableText));

            CreateMap<PriceOverview, PriceOverviewDTOV2>()
               .ForMember(d => d.Price, o => o.MapFrom(s => rupeeSymbol + Format.GetFormattedPriceV2(Convert.ToString(s.Price), null, null, false)))
               .ForMember(d => d.PriceLabel, o => o.MapFrom(s => s.PriceLabel))
               .ForMember(d => d.ReasonText, o => o.MapFrom(s => s.ReasonText))
               .ForMember(x => x.PriceSuffix, o => o.MapFrom(s => s.PriceVersionCount > 1 ? priceSuffix : string.Empty))
               .ForMember(x => x.PricePrefix, o => o.MapFrom(s => s.PriceVersionCount > 1 ? pricePrefix : string.Empty))
               .ForMember(x => x.LabelColor, o => o.MapFrom(s => (s.PriceStatus == (int)PriceBucket.PriceNotAvailable ? "oliveText " : s.PriceStatus == (int)PriceBucket.CarNotSold ? "oliveText " : " ")))
               .ForMember(d => d.City, o => o.MapFrom(s => s.PriceStatus == (int)PriceBucket.HaveUserCity ? s.City : new City()))
               .ForMember(d => d.CityColor, o => o.MapFrom(s => string.Empty))
               .ForMember(d => d.PriceStatus, o => o.MapFrom(s => s.PriceStatus))
               .ForMember(d => d.PriceForSorting, o => o.MapFrom(s => s.Price));

            CreateMap<PriceOverview, PriceOverviewDtoV3>()
              .ForMember(d => d.Price, o => o.MapFrom(s => rupeeSymbol + Format.PriceLacCr(s.Price.ToString())))
                .ForMember(d => d.PriceLabel, o => o.MapFrom(s => s.PriceLabel.Replace(priceWithGstText, appsGstText).Replace(priceEstimatedGstText, appsGstText)))
                .ForMember(d => d.ReasonText, o => o.MapFrom(s => s.ReasonText))
                .ForMember(x => x.PriceSuffix, o => o.MapFrom(s => s.PriceVersionCount > 1 ? priceSuffix : string.Empty))
                .ForMember(x => x.PricePrefix, o => o.MapFrom(s => s.PriceVersionCount > 1 ? pricePrefix : string.Empty))
                .ForMember(x => x.LabelColor, o => o.MapFrom(s => (s.PriceStatus == (int)PriceBucket.PriceNotAvailable ? "#c1a611 " 
                    : s.PriceStatus == (int)PriceBucket.CarNotSold ? "#ef3f30 " : "#82888b ")))
                .ForMember(d => d.PriceStatus, o => o.MapFrom(s => s.PriceStatus))
                .ForMember(d => d.PriceForSorting, o => o.MapFrom(s => s.Price))
                .ForMember(d => d.FormattedFullPrice, o => o.MapFrom(s => s.Price > 0 ? (rupeeSymbol + Format.FormatNumericCommaSep(s.Price.ToString())) : priceNotAvailableText));

            CreateMap<VersionPrice, PriceOverview>();

            CreateMap<NearByCity, NearByCityDto>();
            CreateMap<NearByCityDetails, NearByCityDetailsDto>();
            CreateMap<Charge, ChargeBase>();
            CreateMap<PQItemList, Charge>();
            CreateMap<ChargeGroup, ChargeGroupPrice>();
            CreateMap<VersionPriceQuote, VersionPriceQuoteDTOV2>();
            CreateMap<PQItemList, PQItemListDTOV2>()
                .ForMember(src => src.Id, dest => dest.MapFrom(y => y.ChargePrice.Charge.Id))
                .ForMember(src => src.Name, dest => dest.MapFrom(y => y.ChargePrice.Charge.Name))
                .ForMember(src => src.Value, dest => dest.MapFrom(y => y.ChargePrice.Price))
                .ForMember(src => src.Type, dest => dest.MapFrom(y => y.ChargeGroupPrice.Type));
            CreateMap<VehiclePriceDto, VehiclePrice>();
            CreateMap<CarVersionDetails, EmiCalculatorModelData>();
        }
    }
}
