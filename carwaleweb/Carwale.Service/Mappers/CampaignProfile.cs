using AutoMapper;
using Carwale.DTOs.Campaigns;
using Carwale.Entity;
using Carwale.Entity.Campaigns;
using Carwale.Entity.CarData;
using Carwale.Entity.CrossSell;
using Carwale.Entity.Dealers;
using Carwale.Entity.Template;
using Carwale.Entity.Dealers.URI;
using Carwale.Utility;
using Predictive;
using Carwale.DTOs.Template;
using Carwale.Entity.PageProperty;
using Carwale.DTOs.PageProperty;
using Carwale.Entity.Enum;
using Carwale.Entity.Customers;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Leads;
using Carwale.DTOs.LeadForm;
using Carwale.DTOs.Dealer;
using Carwale.DTOs.CarData;

namespace Carwale.Service.Mappers
{
    public class CampaignProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Campaign, CampaignDTO>();
            CreateMap<Campaign, CampaignDTOv2>()
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.ContactName))
                .ForMember(d => d.MaskingNumber, o => o.MapFrom(s => s.ContactNumber))
                .ForMember(d => d.LinkText, o => o.MapFrom(s => s.ActionText));
            CreateMap<ProtoBufClass.Campaigns.Campaign, Campaign>()
              .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id.Id))
              .ForMember(x => x.ContactName, opt => opt.MapFrom(src => src.DisplayName))
              .ForMember(x => x.ContactEmail, opt => opt.MapFrom(src => src.NotificationEmailId))
              .ForMember(x => x.ShowOnDesktop, opt => opt.MapFrom(src => src.IsDesktop))
              .ForMember(x => x.ShowOnMobile, opt => opt.MapFrom(src => src.IsMobile))
              .ForMember(x => x.ShowOnAndroid, opt => opt.MapFrom(src => src.IsAndroid))
              .ForMember(x => x.ShowOniOS, opt => opt.MapFrom(src => src.IsIPhone))
              .ForMember(x => x.ActionText, opt => opt.MapFrom(src => src.LinkText))
              .ForMember(x => x.NotifyUserByEmail, opt => opt.MapFrom(src => src.EnableUserEmail))
              .ForMember(x => x.NotifyUserBySMS, opt => opt.MapFrom(src => src.EnableUserSMS))
              .ForMember(x => x.NotifyDealerByEmail, opt => opt.MapFrom(src => src.EnableDealerEmail))
              .ForMember(x => x.NotifyDealerBySMS, opt => opt.MapFrom(src => src.EnableDealerSMS))
              .ForMember(x => x.IsEmailRequired, opt => opt.MapFrom(src => src.ShowEmail))
              .ForMember(x => x.ContactNumber, opt => opt.MapFrom(src => src.PhoneNumber))
              .ForMember(x => x.Type, opt => opt.MapFrom(src => (src.LeadPanel == (short)LeadPanel.Autobiz) ? (short)0 : (short)1));
            CreateMap<ProtoBufClass.Campaigns.Campaign, CvlDetails>()
               .ForMember(x => x.IsCvl, opt => opt.MapFrom(src => src.IsCVL));
            CreateMap<ProtoBufClass.Campaigns.Campaign, Carwale.Entity.Campaigns.Campaign>()
                .ForMember(x => x.CvlDetails, opt => opt.MapFrom(s => Mapper.Map<ProtoBufClass.Campaigns.Campaign, CvlDetails>(s)));
            CreateMap<CvlDetails, CvlDetailsDTO>();
            CreateMap<Campaign, SponsoredDealer>()
                   .ForMember(i => i.DealerId, o => o.MapFrom(j => j.Id))
                   .ForMember(i => i.ActualDealerId, o => o.MapFrom(j => j.DealerId))
                   .ForMember(i => i.DealerName, o => o.MapFrom(j => j.ContactName))
                   .ForMember(i => i.DealerEmail, o => o.MapFrom(j => j.ContactEmail))
                   .ForMember(i => i.DealerMobile, o => o.MapFrom(j => j.ContactNumber))
                   .ForMember(i => i.ShowEmail, o => o.MapFrom(j => j.IsEmailRequired))
                   .ForMember(i => i.LinkText, o => o.MapFrom(j => j.ActionText))
                   .ForMember(i => i.DealerLeadBusinessType, o => o.MapFrom(j => j.Type)).ReverseMap();
            CreateMap<CrossSellDetail, CrossSellCampaign>()
                .ForMember(i => i.DealerName, o => o.MapFrom(j => j.CampaignDetail.ContactName))
                .ForMember(i => i.CampaignId, o => o.MapFrom(j => j.CampaignDetail.Id))
                .ForMember(i => i.MaskingNumber, o => o.MapFrom(j => j.CampaignDetail.ContactNumber))
                .ForMember(i => i.ShowEmail, o => o.MapFrom(j => j.CampaignDetail.IsEmailRequired))
                .ForMember(i => i.DealerLeadBusinessType, o => o.MapFrom(j => j.CampaignDetail.Type))
                .ForMember(i => i.LeadPanel, o => o.MapFrom(j => j.CampaignDetail.LeadPanel))
                .ForMember(i => i.VersionId, o => o.MapFrom(j => j.CarVersionDetail.VersionId))
                .ForMember(i => i.VersionName, o => o.MapFrom(j => j.CarVersionDetail.VersionName))
                .ForMember(i => i.HostURL, o => o.MapFrom(j => j.CarVersionDetail.HostURL))
                .ForMember(i => i.MaskingName, o => o.MapFrom(j => j.CarVersionDetail.MaskingName))
                .ForMember(i => i.OriginalImgPath, o => o.MapFrom(j => j.CarVersionDetail.OriginalImgPath))
                .ForMember(i => i.MakeId, o => o.MapFrom(j => j.CarVersionDetail.MakeId))
                .ForMember(i => i.ModelId, o => o.MapFrom(j => j.CarVersionDetail.ModelId))
                .ForMember(i => i.MakeName, o => o.MapFrom(j => j.CarVersionDetail.MakeName))
                .ForMember(i => i.ModelName, o => o.MapFrom(j => j.CarVersionDetail.ModelName));
            CreateMap<PredictionModelRequest, ModelRequest>();
            CreateMap<PredictionCampaignRequest, CampaignRequest>();
            CreateMap<PredictionModelRequestLocation, Predictive.Location>();
            CreateMap<ModelResponse, PredictionModelResponse>();
            CreateMap<CampaignRecommendationEntity, MakeModelEntity>()
               .ForMember(src => src.MakeId, dest => dest.MapFrom(y => y.CarMake.MakeId))
               .ForMember(src => src.MakeName, dest => dest.MapFrom(y => y.CarMake.MakeName))
               .ForMember(src => src.ModelName, dest => dest.MapFrom(y => y.CarModel.ModelName))
               .ForMember(src => src.ModelId, dest => dest.MapFrom(y => y.CarModel.ModelId));
            CreateMap<ProtoBufClass.Campaigns.CrossSellCampaign, FeaturedVersion>()
               .ForMember(dest => dest.VersionId, opt => opt.MapFrom(src => src.Version.Id));
            CreateMap<ProtoBufClass.Campaigns.Dealer, NewCarDealer>()
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Dealer_.Name))
                .ForMember(x => x.Website, opt => opt.MapFrom(src => src.WebsiteUrl))
                .ForMember(x => x.CityName, opt => opt.MapFrom(src => src.City.Name))
                .ForMember(x => x.CityId, opt => opt.MapFrom(src => src.City.Id))
                .ForMember(x => x.State, opt => opt.MapFrom(src => src.State.Name))
                .ForMember(x => x.DealerId, opt => opt.MapFrom(src => src.Dealer_.Id))
                .ForMember(x => x.MakeId, opt => opt.MapFrom(src => src.Make.Id))
                .ForMember(x => x.CampaignId, opt => opt.MapFrom(src => src.CampaignId.Id))
                .ForMember(x => x.StateId, opt => opt.MapFrom(src => src.State.Id))
                .ForMember(x => x.NewCarDealerId, opt => opt.MapFrom(src => src.Dealer_.Id));
            CreateMap<ProtoBufClass.Campaigns.Dealer, Entity.Dealers.DealerDetails>()
                .ForMember(x => x.MakeId, opt => opt.MapFrom(src => src.Make.Id))
                .ForMember(x => x.CityId, opt => opt.MapFrom(src => src.City.Id))
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Dealer_.Name))
                .ForMember(x => x.EMailId, opt => opt.MapFrom(src => src.EmailId))
                .ForMember(x => x.WebSite, opt => opt.MapFrom(src => src.WebsiteUrl))
                .ForMember(x => x.ShowroomStartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(x => x.ShowroomEndTime, opt => opt.MapFrom(src => src.EndTime))
                .ForMember(x => x.ContactNo, opt => opt.MapFrom(src => src.MobileNo))
                .ForMember(x => x.Mobile, opt => opt.MapFrom(src => src.ContactNumber))
                .ForMember(x => x.Pincode, opt => opt.MapFrom(src => src.PinCode))
                .ForMember(x => x.PrimaryMobileNo, opt => opt.MapFrom(src => src.LandLineNo))
                .ForMember(x => x.DealerMobileNo, opt => opt.MapFrom(src => src.LandLineNo))
                .ForMember(x => x.SecondaryMobileNo, opt => opt.MapFrom(src => src.MobileNo))
                .ForMember(x => x.LandLineNo, opt => opt.MapFrom(src => src.MobileNo))
                .ForMember(x => x.CityName, opt => opt.MapFrom(src => src.City.Name))
                .ForMember(x => x.StateId, opt => opt.MapFrom(src => src.State.Id))
                .ForMember(x => x.StateName, opt => opt.MapFrom(src => src.State.Name))
                .ForMember(x => x.DealerId, opt => opt.MapFrom(src => src.Dealer_.Id))
                .ForMember(x => x.CampaignId, opt => opt.MapFrom(src => src.CampaignId.Id))
                .ForMember(x => x.Latitude, opt => opt.MapFrom(src => CustomParser.parseDoubleObject(src.Latitude)))
                .ForMember(x => x.Longitude, opt => opt.MapFrom(src => CustomParser.parseDoubleObject(src.Longitude)));
            CreateMap<ProtoBufClass.Campaigns.Campaign, SponsoredDealer>()
                .ForMember(x => x.DealerName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(x => x.DealerEmail, opt => opt.MapFrom(src => src.NotificationEmailId));
            CreateMap<ModelResponse, PredictionModelResponse>();
            CreateMap<PredictionModelResponse, PredictionData>().ReverseMap();
            CreateMap<CarVersionDetails, CarIdEntity>();
            CreateMap<SponsoredDealer, Templates>()
                .ForMember(x => x.UniqueName, opt => opt.MapFrom(src => src.TemplateName))
                .ForMember(x => x.Html, opt => opt.MapFrom(src => src.TemplateHtml));
            CreateMap<CampaignInputURI, CarIdEntity>();
            CreateMap<CampaignInputv2, CarIdEntity>();
            CreateMap<CampaignInputURI, Carwale.Entity.Geolocation.Location>()
                .ForMember(x => x.ZoneId, opt => opt.MapFrom(src => CustomParser.parseIntObject(src.ZoneId)));
            CreateMap<CampaignInputv2, Carwale.Entity.Geolocation.Location>();
            CreateMap<CarVersionDetails, CarIdWithImageDto>()
                .ForMember(x => x.HostUrl, opt => opt.MapFrom(src => src.HostURL))
                .ForMember(x => x.OriginalImage, opt => opt.MapFrom(src => src.OriginalImgPath));
            CreateMap<DealerAd, DealerAdDTO>();
            CreateMap<PageProperty, PagePropertyDTO>();
            CreateMap<Templates, TemplateDTO>();
            CreateMap<CampaignInputv2, ProtoBufClass.Campaigns.TemplateInput>();
            CreateMap<ProtoBufClass.Campaigns.PageTemplate, PageTemplates>();
            CreateMap<Campaign, CampaignBaseDTO>();
            CreateMap<CampaignRecommendation, CampaignRecommendationDTO>()
                .ForMember(x => x.CarData, opt => opt.MapFrom(s => (s.CarData)))
                .ForMember(x => x.Campaign, opt => opt.MapFrom(s => (s.Campaign)))
                .ForMember(x => x.PricesOverview, opt => opt.MapFrom(s => (s.PricesOverview)));
            CreateMap<Entity.Geolocation.Location,ProtoBufClass.Campaigns.GeoLocation>();
            CreateMap<DealerInquiry, DealerInquiryDetails>()
                .ForMember(x => x.CityId, opt => opt.MapFrom(s => (s.UserLocation.CityId)))
                .ForMember(x => x.CityName, opt => opt.MapFrom(s => (s.UserLocation.CityName)))
                .ForMember(x => x.ZoneId, opt => opt.MapFrom(s => CustomParser.parseStringObject(s.UserLocation.ZoneId)))
                .ForMember(x => x.AreaId, opt => opt.MapFrom(s => (s.UserLocation.AreaId)))
                .ForMember(x => x.InquirySourceId, opt => opt.MapFrom(s => (s.LeadSource.InquirySourceId)))
                .ForMember(x => x.LeadClickSource, opt => opt.MapFrom(s => (s.LeadSource.LeadClickSourceId)))
                .ForMember(x => x.PlatformSourceId, opt => opt.MapFrom(s => (s.LeadSource.PlatformId)))
                .ForMember(x => x.ApplicationId, opt => opt.MapFrom(s => (s.LeadSource.ApplicationId)))
                .ForMember(x => x.PageId, opt => opt.MapFrom(s => (s.LeadSource.PageId)))
                .ForMember(x => x.PropertyId, opt => opt.MapFrom(s => (s.LeadSource.PropertyId)))
                .ForMember(x => x.SourceType, opt => opt.MapFrom(s => (s.LeadSource.SourceType)))
                .ForMember(x => x.IsCitySet, opt => opt.MapFrom(s => (s.LeadSource.IsCitySet)))
                .ForMember(x => x.Name, opt => opt.MapFrom(s => (s.UserInfo.Name)))
                .ForMember(x => x.Mobile, opt => opt.MapFrom(s => (s.UserInfo.Mobile)))
                .ForMember(x => x.Email, opt => opt.MapFrom(s => (s.UserInfo.Email)))
                .ForMember(x => x.EncryptedPQDealerAdLeadId, opt => opt.MapFrom(s => (s.EncryptedLeadId)));
            CreateMap<DealerLeadFormInput, LeadFormDto>();
                                
            CreateMap<NewCarDealer, CampaignDTO>()
                .ForMember(x => x.Id, opt => opt.MapFrom(s => s.CampaignId))
                .ForMember(x => x.ContactName, opt => opt.MapFrom(s => s.Name))
                .ForMember(x => x.LeadPanel, opt => opt.MapFrom(s => s.LeadPanel))
                .ForMember(x => x.IsEmailRequired, opt => opt.MapFrom(s => s.ShowEmail))
                .ForMember(x => x.Type, opt => opt.Ignore());
            CreateMap<NewCarDealer, DealerDTO>()
                .ForMember(x => x.DealerId, opt => opt.MapFrom(s => s.DealerId))
                .ForMember(x => x.DealerName, opt => opt.MapFrom(s => s.Name))
                .ForMember(x => x.DealerArea, opt => opt.MapFrom(s => s.Address));
            CreateMap<NewCarDealer, DealerAdDTO>()
                .ForMember(x => x.Campaign, opt => opt.MapFrom(s => Mapper.Map<NewCarDealer, CampaignDTO>(s)))
                .ForMember(x => x.DealerDetails, opt => opt.MapFrom(s => Mapper.Map<NewCarDealer, DealerDTO>(s)))
                .ForMember(x => x.CampaignType, opt => opt.UseValue((int)CampaignAdType.DealerLocator));
            CreateMap<CarModelDetails, CarIdWithImageDto>();
        }
    }
}
