using AutoMapper;
using Carwale.DTOs.CustomerVerification;
using Carwale.Entity.Enum;
using Carwale.Entity.Vernam;
using Carwale.Utility;
using System;

namespace Carwale.Service.Mappers
{
    public class CustomerVerificationProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<VerifyMobileOtpDto, RequestData>()
                .ForMember(dest => dest.VerificationValue, opt => opt.MapFrom(src => src.MobileNumber))
                .ForMember(dest => dest.VerificationType, opt => opt.UseValue(VerificationType.Mobile));

            CreateMap<InitiateVerificationDto, RequestData>()
                .ForMember(dest => dest.VerificationType, opt => opt.UseValue(VerificationType.Mobile))
                .ForMember(dest => dest.VerificationValue, opt => opt.MapFrom(src => src.Mobile))
                .ForMember(dest => dest.SourceModule, opt => opt.MapFrom(src => src.SourceModule))
                .ForMember(dest => dest.Validity, opt => opt.MapFrom(src => src.ValidityInMinutes > 0 ? src.ValidityInMinutes : 30))
                .ForMember(dest => dest.DeviceId, opt => opt.MapFrom(src => GetDeviceId()))
                .ForMember(dest => dest.ApplicationId, opt => opt.UseValue(Application.CarWale))
                .ForMember(dest => dest.PlatformId, opt => opt.MapFrom(src => GetPlatformIdForRequestData()))
                .ForMember(dest => dest.ClientIp, opt => opt.ResolveUsing(src => UserTracker.GetUserIp()?.Split(',')[0].Trim()))
                .ForMember(dest => dest.OtpLength, opt => opt.MapFrom(src => src.OtpLength > 0 ? src.OtpLength : 4)); //set default length of otp to 4 in case missing
        }

        private static Platform GetPlatformIdForRequestData()
        {
            Entity.Enum.Platform platform;
            var sourceId = HttpContextUtils.GetHeader<string>("sourceId");
            if (!string.IsNullOrEmpty(sourceId))
            {
                Enum.TryParse(sourceId, out platform);
            }
            else
            {
                platform = Entity.Enum.Platform.CarwaleDesktop;
            }
            return platform;
        }

        private static string GetDeviceId()
        {
            var deviceId = HttpContextUtils.GetCookie("CWC");
            return !string.IsNullOrEmpty(deviceId) ? deviceId : HttpContextUtils.GetHeader<string>("IMEI");
        }
    }
}
