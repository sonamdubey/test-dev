using AutoMapper;
using Bikewale.DTO.PriceQuote.Make;
using Bikewale.Entities.BikeData;
using System.Collections.Generic;

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