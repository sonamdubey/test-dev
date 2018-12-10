using AutoMapper;
using Carwale.DTOs.CarData;
using Carwale.DTOs.OffersV1;
using Carwale.Entity.CarData;
using Carwale.Entity.Geolocation;
using Carwale.Entity.PriceQuote;
using Carwale.Utility;
using Offers.Protos.ProtoFiles;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Carwale.Service.Mappers
{
    public class OfferProfile : Profile
    {
        private readonly static string[] _toolTipText = new string[] {"The offer is subject to car availability at the dealerships", 
                                                        "CarWale has brought this offer information on best effort basis and CarWale is not liable for any consequential (or otherwise) loss / damage caused by the information",
                                                        "All applicable terms and conditions would be as per the dealers and the same may change without any prior intimation", 
                                                        "For the final price, offer availability and exact details of the offers, kindly contact the dealerships"};
        private readonly string _offersCtaText = ConfigurationManager.AppSettings["OffersCtaButtonText"] ?? string.Empty;
        private readonly string _imgHostUrl = Carwale.Utility.CWConfiguration._imgHostUrl;

        protected override void Configure()
        {
            CreateMap<Carwale.Entity.OffersV1.Offer, Carwale.Entity.OffersV1.OfferDetails>();
            CreateMap<CategoryDetail, Carwale.Entity.OffersV1.OfferCategoryDetails>();

            CreateMap<Offer, Carwale.Entity.OffersV1.OfferDetails>();
            CreateMap<OfferWithCategoryDetail, Carwale.Entity.OffersV1.Offer>()
                .ForMember(d => d.OfferDetails, o => o.MapFrom(s => s.OfferDetail));

            CreateMap<CategoryDetail, Carwale.Entity.OffersV1.OfferCategoryDetails>()
                .ForMember(d => d.OfferText, o => o.MapFrom(s => s.CategoryMapping.OfferText))
                .ForMember(d => d.OriginalImgPath, o => o.MapFrom(s => s.Category.OriginalImgPath));

            CreateMap<PQCarDetails, Carwale.Entity.OffersV1.OfferInput>();
            CreateMap<Entity.OffersV1.OfferDetails, OfferDetailsDto>()
                .ForMember(dest => dest.ToolTipText, opt => opt.UseValue<List<string>>(new List<string>(_toolTipText)))
                .ForMember(dest => dest.CtaText, opt => opt.UseValue<string>(_offersCtaText));
            CreateMap<Entity.OffersV1.OfferCategoryDetails, OfferCategoryDetailsDto>()
                .ForMember(dest => dest.HostURL, opt => opt.UseValue<string>(_imgHostUrl));
            CreateMap<Carwale.Entity.OffersV1.Offer, OfferDto>();
            CreateMap<CarVersionDetails, Carwale.Entity.OffersV1.OfferInput>();
            CreateMap<LocationV2, Carwale.Entity.OffersV1.OfferInput>();
            CreateMap<CarDetailsDTO, Carwale.Entity.OffersV1.OfferInput>();
            CreateMap<OfferAvailabiltyDetails, Carwale.Entity.OffersV1.OfferAvailabiltyDetails>();
        }
    }
}
