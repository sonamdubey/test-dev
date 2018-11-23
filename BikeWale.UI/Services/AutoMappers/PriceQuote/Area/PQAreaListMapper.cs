using AutoMapper;
using Bikewale.DTO.PriceQuote.Area;
using Bikewale.Entities.Location;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.PriceQuote.Area
{
    public class PQAreaListMapper
    {
        internal static IEnumerable<DTO.PriceQuote.Area.PQAreaBase> Convert(IEnumerable<Entities.Location.AreaEntityBase> objAreaList)
        {
            return Mapper.Map<IEnumerable<AreaEntityBase>, IEnumerable<PQAreaBase>>(objAreaList);
        }

        internal static IEnumerable<DTO.PriceQuote.Area.v2.PQAreaBase> ConvertV2(IEnumerable<AreaEntityBase> objAreaList)
        {
            return Mapper.Map<IEnumerable<AreaEntityBase>, IEnumerable<DTO.PriceQuote.Area.v2.PQAreaBase>>(objAreaList);
        }
    }
}