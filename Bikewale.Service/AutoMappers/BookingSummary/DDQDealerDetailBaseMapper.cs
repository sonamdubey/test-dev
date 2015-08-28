﻿using AutoMapper;
using Bikewale.DTO.PriceQuote.DetailedDealerQuotation;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entity.BikeBooking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.BookingSummary
{
    public class DDQDealerDetailBaseMapper
    {
        internal static DTO.PriceQuote.DetailedDealerQuotation.DDQDealerDetailBase Convert(Entity.BikeBooking.PQ_DealerDetailEntity dealerDetailEntity)
        {
            Mapper.CreateMap<BikeMakeEntityBase, DDQMakeBase>();
            Mapper.CreateMap<BikeModelEntityBase, DDQModelBase>();
            Mapper.CreateMap<BikeVersionEntityBase, DDQVersionBase>();
            Mapper.CreateMap<StateEntityBase, DDQStateBase>();
            Mapper.CreateMap<CityEntityBase, DDQCityBase>();
            Mapper.CreateMap<Bikewale.Entity.BikeBooking.AreaEntityBase, DDQAreaBase>();
            Mapper.CreateMap<Bikewale.Entity.BikeBooking.PQ_Price, Bikewale.DTO.PriceQuote.DetailedDealerQuotation.DDQPQ_Price>();
            Mapper.CreateMap<NewBikeDealers, DDQNewBikeDealer>();
            Mapper.CreateMap<OfferEntity, DDQOfferBase>();
            Mapper.CreateMap<FacilityEntity, DDQFacilityBase>();
            Mapper.CreateMap<PQ_QuotationEntity, DDQQuotationBase>();
            Mapper.CreateMap<Bikewale.Entity.BikeBooking.EMI, Bikewale.DTO.PriceQuote.DetailedDealerQuotation.DDQEMI>();
            Mapper.CreateMap<BookingAmountEntityBase, DDQBookingAmountBase>();
            Mapper.CreateMap<PQ_DealerDetailEntity, DDQDealerDetailBase>();
            return Mapper.Map<PQ_DealerDetailEntity, DDQDealerDetailBase>(dealerDetailEntity);
        }
    }
}