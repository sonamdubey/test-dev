﻿using AutoMapper;
using Bikewale.DTO.PriceQuote.CustomerDetails;
using Bikewale.DTO.PriceQuote.DetailedDealerQuotation;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Customer;
using Bikewale.Entities.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.BookingSummary
{
    public class BookingSummaryMapper
    {
        internal static DTO.PriceQuote.CustomerDetails.PQCustomer Convert(Entities.BikeBooking.PQCustomerDetail objCustomer)
        {
            Mapper.CreateMap<CustomerEntityBase, PQCustomerBase>();
            Mapper.CreateMap<VersionColor, PQColor>();
            Mapper.CreateMap<PQCustomerDetail, PQCustomer>();
            return Mapper.Map<PQCustomerDetail, PQCustomer>(objCustomer);
        }

        internal static DTO.PriceQuote.DetailedDealerQuotation.DDQDealerDetailBase Convert(Entities.BikeBooking.PQ_DealerDetailEntity dealerDetailEntity)
        {
            Mapper.CreateMap<BikeMakeEntityBase, DDQMakeBase>();
            Mapper.CreateMap<BikeModelEntityBase, DDQModelBase>();
            Mapper.CreateMap<BikeVersionEntityBase, DDQVersionBase>();
            Mapper.CreateMap<StateEntityBase, DDQStateBase>();
            Mapper.CreateMap<CityEntityBase, DDQCityBase>();
            Mapper.CreateMap<Bikewale.Entities.BikeBooking.AreaEntityBase, DDQAreaBase>();
            Mapper.CreateMap<Bikewale.Entities.BikeBooking.PQ_Price, Bikewale.DTO.PriceQuote.DetailedDealerQuotation.DDQPQ_Price>();
            Mapper.CreateMap<NewBikeDealers, DDQNewBikeDealer>();
            Mapper.CreateMap<OfferEntity, DDQOfferBase>();
            Mapper.CreateMap<FacilityEntity, DDQFacilityBase>();
            Mapper.CreateMap<PQ_QuotationEntity, DDQQuotationBase>();
            Mapper.CreateMap<Bikewale.Entities.BikeBooking.EMI, Bikewale.DTO.PriceQuote.DetailedDealerQuotation.DDQEMI>();
            Mapper.CreateMap<BookingAmountEntityBase, DDQBookingAmountBase>();
            Mapper.CreateMap<PQ_DealerDetailEntity, DDQDealerDetailBase>();
            return Mapper.Map<PQ_DealerDetailEntity, DDQDealerDetailBase>(dealerDetailEntity);
        }
    }
}