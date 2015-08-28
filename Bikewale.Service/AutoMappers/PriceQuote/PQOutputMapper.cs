using AutoMapper;
using Bikewale.Entities.BikeBooking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.PriceQuote
{
    public class PQOutputMapper
    {
        internal static DTO.PriceQuote.PQOutput Convert(Entities.BikeBooking.PQOutputEntity objPQOutput)
        {
            Mapper.CreateMap<PQOutputEntity, Bikewale.DTO.PriceQuote.PQOutput>();
            return Mapper.Map<PQOutputEntity, Bikewale.DTO.PriceQuote.PQOutput>(objPQOutput);
        }
    }
}