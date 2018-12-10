using AutoMapper;
using Carwale.DTOs.CarData;
using Carwale.DTOs.Dealer;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity.Dealers;
using Carwale.Utility;

namespace Carwale.Service.Mappers
{
    public class DealerProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Entity.Dealers.DealerDetails, DTO.Dealers.DealerDetails>();
            CreateMap<Entity.Dealers.NewCarDealer, DTO.Dealers.NewCarDealer>();
            CreateMap<AboutUsImageEntity, CarImageBaseDTO>();
            CreateMap<DealerShowroomDetails, DTOs.Dealers.DealerShowroomDetailsDTO>()
                .ForMember(x => x.DealerDetails, o => o.MapFrom(s => s.objDealerDetails))
                .ForMember(x => x.ImageList, o => o.MapFrom(s => s.objImageList))
                .ForMember(x => x.ModelDetails, o => o.MapFrom(s => s.objModelDetails));
            CreateMap<DealerInquiryDetails, DTOs.Campaigns.CampaignLeadDTO>()
               .ForMember(x => x.UserModel, o => o.MapFrom(s => s.ModelsHistory))
               .ForMember(x => x.ZoneId, o => o.MapFrom(s => CustomParser.parseIntObject(s.ZoneId)));
            CreateMap<DealerInquiryDetails, DTOs.Campaigns.CampaignLeadDTO>()
               .ForMember(x => x.UserModel, o => o.MapFrom(s => s.ModelsHistory))
               .ForMember(x => x.ZoneId, o => o.MapFrom(s => CustomParser.parseIntObject(s.ZoneId)));
            CreateMap<DealerDetails, DealerSummaryDTO>();
            CreateMap<NewCarDealersList, DealerDTO>();
            CreateMap<NewCarDealersList, CTDealerDTO>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.DealerId))
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.DealerName));
            CreateMap<SponsoredDealer, SponsoredDealerDTO>().ReverseMap();
            CreateMap<DealerDetails, DealersDTO>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.DealerId))
                .ForMember(d => d.Area, o => o.MapFrom(s => s.DealerArea));
            CreateMap<DealerInquiryDetailsDTO, DealerInquiryDetails>()
               .ForMember(x => x.ZoneId, opt => opt.MapFrom(x => x.ZoneId.ToString()))
               .ForMember(x => x.InquirySourceId, opt => opt.MapFrom(x => x.InquirySourceId.ToString()));
            CreateMap<NewCarDealer, DealerLocatorDTO>();
            CreateMap<DealerDetails, DealerDTO>()
                .ForMember(x => x.DealerName, opt => opt.MapFrom(x => x.Name));
            CreateMap<NewCarDealer, DealerDetailsV1Dto>()
                .ForMember(x => x.City, opt => opt.MapFrom(x => x.CityName))
                .ForMember(x => x.ContactNo, opt => opt.MapFrom(x => x.MobileNo))
                .ForMember(x => x.Email, opt => opt.MapFrom(x => x.EmailId));
        }
    }
}
