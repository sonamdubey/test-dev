using AutoMapper;
using Carwale.Entity.Enum;
using Carwale.Entity.Vernam;
using VerNam.ProtoClass;

namespace Carwale.Adapters.Mappers
{
    public class VerNamProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<RequestData, VerifyOtpRequest>();
            CreateMap<RequestData, IsVerifiedRequest>();
            CreateMap<RequestData, IsVerifiedForDeviceRequest>();
            CreateMap<RequestData, InitiateOtpRequest>()
                .ForMember(dest => dest.VerificationExpiry, opt => opt.MapFrom(src => src.VerificationExpiry != null ? src.VerificationExpiry.Value.ToString() : string.Empty))
                .ForMember(dest => dest.Validity, opt => opt.MapFrom(src => src.Validity > 0 ? src.Validity : 30))
                .ForMember(dest => dest.PlatformId, opt => opt.MapFrom(src => MapCarwalePlatformToVernamPlatform(src.PlatformId)))
                .ForMember(dest => dest.ApplicationId, opt => opt.UseValue(ApplicationId.CarWale));
            CreateMap<RequestData, InitiateMissedCallRequest>()
                .ForMember(dest => dest.VerificationMobile, opt => opt.MapFrom(src => src.VerificationValue))
                .ForMember(dest => dest.VerificationExpiry, opt => opt.MapFrom(src => src.VerificationExpiry != null ? src.VerificationExpiry.Value.ToString() : string.Empty))
                .ForMember(dest => dest.PlatformId, opt => opt.MapFrom(src => MapCarwalePlatformToVernamPlatform(src.PlatformId)))
                .ForMember(dest => dest.ApplicationId, opt => opt.UseValue(ApplicationId.CarWale));
            CreateMap<RequestData, VerifyMissedCallRequest>()
                .ForMember(dest => dest.VerificationMobile, opt => opt.MapFrom(src => src.VerificationValue));
        }

        private static PlatformId MapCarwalePlatformToVernamPlatform(Platform platform)
        {
            PlatformId platformId;
            switch(platform)
            {
                case Platform.BikewaleDesktop:
                case Platform.CarwaleDesktop:
                    {
                        platformId = PlatformId.Desktop;
                        break;
                    }
                case Platform.CarwaleMobile:
                    {
                        platformId = PlatformId.Mobile;
                        break;
                    }

                case Platform.CarwaleiOS:
                    {
                        platformId = PlatformId.Ios;
                        break;
                    }
                case Platform.CarwaleAndroid:
                    {
                        platformId = PlatformId.Android;
                        break;
                    }
                default:
                    {
                        platformId = PlatformId.Mobile;
                        break;
                    }
            }
            return platformId;
        }
    }
}
