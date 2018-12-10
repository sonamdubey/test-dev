using AutoMapper;
using Carwale.DTOs.Classified.CarValuation;
using Carwale.Entity.Classified.CarValuation;
using Carwale.Utility;
using System.Globalization;

namespace Carwale.Service.Mappers
{
    public static class UsedValuationMappers
    {
        public static void CreateMaps()
        {
            Mapper.CreateMap<ValuationReport, ValuationReportAmp>()
                .ForMember(d => d.SellerAskingPrice,
                           o => o.MapFrom(s => s.SellerAskingPrice != null ? Format.FormatFullPrice(s.SellerAskingPrice.ToString(), true) : null));

            Mapper.CreateMap<Valuation, ValuationAmp>()
                .ForMember(d => d.GoodPrice, o => o.MapFrom(s => s.GoodPrice > 0 ? Format.FormatFullPrice(s.GoodPrice.ToString(), true) : null))
                .ForMember(d => d.FairPrice, o => o.MapFrom(s => s.FairPrice > 0 ? Format.FormatFullPrice(s.FairPrice.ToString(), true) : null));

            Mapper.CreateMap<ValuationPrices, ValuationPricesAmp>()
                .ForMember(d => d.GoodPrice, o => o.MapFrom(s => s.GoodPrice > 0 ? string.Format(new CultureInfo("EN-in"), "{0:c0}", s.GoodPrice) : null))
                .ForMember(d => d.FairPrice, o => o.MapFrom(s => s.FairPrice > 0 ? string.Format(new CultureInfo("EN-in"), "{0:c0}", s.FairPrice) : null));
        }
    }
}
