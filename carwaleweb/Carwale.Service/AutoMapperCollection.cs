using AutoMapper;
using Carwale.DTOs.Autocomplete;
using Carwale.DTOs.CarData;
using Carwale.DTOs.Geolocation;
using Carwale.Entity.CarData;
using Carwale.Entity.Geolocation;
using Carwale.Utility;

namespace Carwale.Service
{
    public static class AutoMapperCollection
    {
        public static void PQAutoMapper()
        {
            Mapper.CreateMap<Carwale.Entity.Geolocation.Zone, Carwale.DTOs.Geolocation.Zone>();
            Mapper.CreateMap<CityZones, Carwale.DTOs.Geolocation.CityZonesDTO>();
            Mapper.CreateMap<CarVersionEntity, LabelValueDTO>().ForMember(dest => dest.Label, opt => opt.MapFrom(src => src.Name))
                                                              .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.ID));
            Mapper.CreateMap<Carwale.Entity.Geolocation.City, LabelValueDTO>().ForMember(dest => dest.Label, opt => opt.MapFrom(src => src.CityName))
                                                  .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.CityId));
            Mapper.CreateMap<CityZones, CityZonesDTO>();
            Mapper.CreateMap<PqVersionCitiesEntity, PqVersionCitiesDTO>()
                .ForMember(dest => dest.SmallPicUrl, opt => opt.MapFrom(src => src.SmallPicUrl))
                .ForMember(dest => dest.LargePicUrl, opt => opt.MapFrom(src => src.LargePicUrl))
                .ForMember(dest => dest.MinPrice, opt => opt.MapFrom(src => Format.GetPrice(src.MinPrice.ToString())))
                .ForMember(dest => dest.MaxPrice, opt => opt.MapFrom(src => Format.GetPrice(src.MaxPrice.ToString())))
                .ForMember(dest => dest.CarPrice, opt => opt.MapFrom(src => "₹ " + Format.FormatFullPrice(src.MinPrice.ToString(), src.MaxPrice.ToString())))
                .ForMember(dest => dest.ReviewRate, opt => opt.MapFrom(src => src.ReviewRate))
                .ForMember(dest => dest.OfferExists, opt => opt.MapFrom(src => src.OfferExists))
                .ForMember(dest => dest.ReviewCount, opt => opt.MapFrom(src => src.ReviewCount))
                .ForMember(dest => dest.ExShowroomCity, opt => opt.MapFrom(src => src.ExShowroomCity))
                .ForMember(dest => dest.CarName, opt => opt.MapFrom(src => src.CarName))
                .ForMember(dest => dest.OriginalImgPath, opt => opt.MapFrom(src => src.OriginalImgPath))
                .ForMember(dest => dest.HostUrl, opt => opt.MapFrom(src => src.HostUrl))
                .ForMember(dest => dest.Versions, opt => opt.MapFrom(src => src.Versions))
                .ForMember(dest => dest.Cities, opt => opt.MapFrom(src => src.Cities))
                .ForMember(dest => dest.Zones, opt => opt.MapFrom(src => src.Zones));
        }
    }
}
