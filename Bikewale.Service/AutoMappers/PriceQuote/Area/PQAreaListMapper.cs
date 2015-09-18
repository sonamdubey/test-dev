using AutoMapper;
using Bikewale.DTO.PriceQuote.Area;
using Bikewale.Entities.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.PriceQuote.Area
{
    public class PQAreaListMapper
    {
        internal static IEnumerable<DTO.PriceQuote.Area.PQAreaBase> Convert(List<Entities.Location.AreaEntityBase> objAreaList)
        {
            Mapper.CreateMap<AreaEntityBase, PQAreaBase>();
            return Mapper.Map<List<AreaEntityBase>, List<PQAreaBase>>(objAreaList);
        }
    }
}