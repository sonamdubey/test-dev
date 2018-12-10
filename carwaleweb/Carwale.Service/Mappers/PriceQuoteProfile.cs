using AutoMapper;
using Carwale.DTOs.CarData;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity.CarData;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Price;
using Carwale.Entity.PriceQuote;

namespace Carwale.Service.Mappers
{
    public class PriceQuoteProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<PQItem, PQItemDTO>().ReverseMap();
            CreateMap<LeadSource, LeadSourceDTO>().ReverseMap();
            CreateMap<CarPriceQuote, CarPriceQuoteDTO>();
            CreateMap<VersionPriceQuote, VersionPriceQuoteDTO>();
            CreateMap<PQItemList, PQItemListDTO>()
                .ForMember(src => src.Id, dest => dest.MapFrom(y => y.PQItemId))
                .ForMember(src => src.Name, dest => dest.MapFrom(y => y.PQItemName))
                .ForMember(src => src.Value, dest => dest.MapFrom(y => y.PQItemValue));
            CreateMap<PQ, PQCarDetails>();
            CreateMap<EMIInformation, EMIInformationDTO>();
            CreateMap<PriceQuoteInput, CustLocation>();
            CreateMap<PriceQuoteInput, CarDataTrackingEntity>()
                .ForMember(src => src.Location, dest => dest.MapFrom(y => y));
            CreateMap<PQInput, CustLocation>();
            CreateMap<PQCarDetails, EmiCalculatorModelData>();
            CreateMap<PQCarDetails, CarDetailsDTO>();
            CreateMap<ThirdPartyEmiDetails, ThirdPartyEmiDetailsDto>();
            CreateMap<EmiCalculatorModelData, EmiCalculatorModelDataDto>();
            CreateMap<ThirdPartyEmiDetailsDto, ThirdPartyEmiDetailsDtoV2>();
        }
    }
}
