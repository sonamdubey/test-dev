using AutoMapper;
using Carwale.DTOs.Advertisment;
using Carwale.DTOs.ES;
using Carwale.Entity.Advertizings.Apps;
using Carwale.Entity.CarData;
using Carwale.Entity.ES;

namespace Carwale.Service.Mappers
{
    public static class ESMapping
    {
        public static void CreateMap()
        {
            Mapper.CreateMap<ESVersionColors, ESVersionColorsDto>();
            Mapper.CreateMap<ExteriorColor, ExteriorColorDto>();
            Mapper.CreateMap<InteriorColor, InteriorColorDto>();
            Mapper.CreateMap<CarVersionEntity, ESVersionSummaryDto>()
                .ForMember(dto => dto.Id, map => map.MapFrom(src => src.ID));
            Mapper.CreateMap<SponsoredNavigation, SponsoredNavigationDto>();
            Mapper.CreateMap<Pages, PagesDto>();
            Mapper.CreateMap<PropertiesEntity, PropertiesDto>();
            Mapper.CreateMap<Sponsored_Car, SponsoredAdCampaignDto>()
                .ForMember(dest => dest.AdHtml, map => map.MapFrom(src => src.Ad_Html))
                .ForMember(dest => dest.MaskingName, map => map.MapFrom(src => src.ModelMaskingName))
                .ForMember(dest => dest.Position, map => map.MapFrom(src => src.Postion))
                .ForMember(dest => dest.Title, map => map.MapFrom(src => src.SponsoredTitle));
            Mapper.CreateMap<SplashScreenBanner, SplashScreenBannerDto>();
            Mapper.CreateMap<SplashScreenBanner, CustomSplashDTO>()
                .ForMember(dto => dto.SplashImgUrl, map => map.MapFrom(src => src.Splashurl));
        }
    }
}
