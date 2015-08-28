using AutoMapper;
using Bikewale.DTO.PriceQuote.DealerPriceQuote;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entity.BikeBooking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.PriceQuote
{
    public class DPQuotationOutputMapper
    {
        internal static DTO.PriceQuote.DealerPriceQuote.DPQuotationOutput Convert(Entity.BikeBooking.PQ_QuotationEntity objPrice)
        {
            Mapper.CreateMap<BikeMakeEntityBase, DPQMakeBase>();
            Mapper.CreateMap<BikeModelEntityBase, DPQModelBase>();
            Mapper.CreateMap<BikeVersionEntityBase, DPQVersionBase>();
            Mapper.CreateMap<StateEntityBase, DPQStateBase>();
            Mapper.CreateMap<CityEntityBase, DPQCityBase>();
            Mapper.CreateMap<Bikewale.Entity.BikeBooking.AreaEntityBase, DPQAreaBase>();
            Mapper.CreateMap<Bikewale.Entity.BikeBooking.PQ_Price, Bikewale.DTO.PriceQuote.DealerPriceQuote.DPQ_Price>();
            Mapper.CreateMap<NewBikeDealers, DPQNewBikeDealer>();
            Mapper.CreateMap<OfferEntity, DPQOfferBase>();
            Mapper.CreateMap<PQ_QuotationEntity, DPQuotationOutput>();
            return Mapper.Map<PQ_QuotationEntity, DPQuotationOutput>(objPrice);
        }
    }
}