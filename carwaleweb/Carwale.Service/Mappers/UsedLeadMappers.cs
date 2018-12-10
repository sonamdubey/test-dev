using AutoMapper;
using Carwale.BL.Dealers.Used;
using Carwale.BL.Stock;
using Carwale.DTOs.Classified.Leads;
using Carwale.Entity.Classified.CarDetails;
using Carwale.Entity.Classified.Leads;
using Carwale.Utility;

namespace Carwale.Service.Mappers
{
    public static class UsedLeadMappers
    {
        public static void CreateMaps()
        {
            Mapper.CreateMap<BasicCarInfo, LeadStockSummary>()
                .ForMember(dest => dest.IsDealer, map => map.MapFrom(src => src.SellerId == 1))
                .ForMember(dest => dest.CarName, map => map.MapFrom(src => $"{ Format.FilterModelName(src.ModelName) } { src.VersionName}"))
                .ForMember(dest => dest.Make, map => map.MapFrom(src => src.MakeName))
                .ForMember(dest => dest.Model, map => map.MapFrom(src => Format.FilterModelName(src.ModelName)))
                .ForMember(dest => dest.Year, map => map.MapFrom(src => src.MakeYear.Year))
                .ForMember(dest => dest.Price, map => map.MapFrom(src => Format.FormatFullPrice(src.Price, false)))
                .ForMember(dest => dest.Color, map => map.MapFrom(src => src.Color));

            Mapper.CreateMap<LeadDetail, LeadWrapper>()
                .ForMember(dest => dest.ProfileId, map => map.MapFrom(src => StockBL.GetProfileId(src.Stock.InquiryId, src.Stock.IsDealer)))
                .ForMember(dest => dest.CustomerName, map => map.MapFrom(src => src.Buyer.Name))
                .ForMember(dest => dest.CustomerMobile, map => map.MapFrom(src => src.Buyer.Mobile))
                .ForMember(dest => dest.CustomerEmail, map => map.MapFrom(src => src.Buyer.Email))
                .ForMember(dest => dest.AbCookie, map => map.ResolveUsing(src => { int abTestInt; int.TryParse(src.AbTestCookie, out abTestInt); return abTestInt; }))
                .ForMember(dest => dest.DeliveryCity, map => map.MapFrom(src => src.LeadTrackingParams.DeliveryCity))
                .ForMember(dest => dest.LeadType, map => map.MapFrom(src => (int)src.LeadTrackingParams.LeadType));

            Mapper.CreateMap<Seller, SellerDTO>()
                .ForMember(dest => dest.Mobile, map => map.Ignore())
                .ForMember(dest => dest.DealerShowroomPage, map => map.MapFrom(src => src.IsDealerPageAvailable ? UsedDealerShowroomBL.GetDealerShowroomUrl(src.City, src.Name, src.Id) : null));

            Mapper.CreateMap<BuyerInfo, BuyerDTO>()
                .ForMember(dest => dest.ChatUserId, map => map.MapFrom(src => src.IsChatLeadGiven ? src.UserId : null))
                .ForMember(dest => dest.ChatAccessToken, map => map.MapFrom(src => src.IsChatLeadGiven ? src.AccessToken : null));

            Mapper.CreateMap<C2BLead,ClassifiedRequest>();
            Mapper.CreateMap<CarTradeLead,ClassifiedRequest>();
        }
    }
}
