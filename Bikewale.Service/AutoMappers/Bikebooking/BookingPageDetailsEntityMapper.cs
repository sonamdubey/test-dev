using AutoMapper;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.PriceQuote.BikeBooking;
using Bikewale.DTO.Version;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.Bikebooking
{
    public class BookingPageDetailsEntityMapper
    {
        internal static DTO.PriceQuote.BikeBooking.BookingPageDetailsDTO Convert(Entities.PriceQuote.BookingPageDetailsEntity objBookingPageDetailsEntity)
        {
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeVersionMinSpecs, VersionMinSpecs>();
            Mapper.CreateMap<DealerVersionPriceItemEntity, DealerVersionPriceItemDTO>();
            Mapper.CreateMap<BikeDealerPriceDetail, BikeDealerPriceDetailDTO>();
            Mapper.CreateMap<BookingPageDetailsEntity, BookingPageDetailsDTO>();
            Mapper.CreateMap<DealerDetails,DealerDetailsDTO>();
            Mapper.CreateMap<DealerOfferEntity,DealerOfferDTO>();
            return Mapper.Map<BookingPageDetailsEntity, BookingPageDetailsDTO>(objBookingPageDetailsEntity);
        }
    }
}