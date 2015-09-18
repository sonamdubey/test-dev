using AutoMapper;
using Bikewale.DTO.PriceQuote.Make;
using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.PriceQuote.Make
{
    public class PQMakeListMapper
    {
        internal static IEnumerable<DTO.PriceQuote.Make.PQMakeBase> Convert(List<Entities.BikeData.BikeMakeEntityBase> objMakeList)
        {
            Mapper.CreateMap<BikeMakeEntityBase, PQMakeBase>();
            return Mapper.Map<List<BikeMakeEntityBase>, List<PQMakeBase>>(objMakeList);
        }
    }
}