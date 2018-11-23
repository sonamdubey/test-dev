using AutoMapper;
using Bikewale.DTO.Area;
using Bikewale.Entities.Location;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.Area
{
    public class AreaListMapper
    {
        internal static IEnumerable<DTO.Area.AreaBase> Convert(List<Entities.Location.AreaEntityBase> objAreaList)
        {

            return Mapper.Map<List<AreaEntityBase>, List<AreaBase>>(objAreaList);
        }
    }
}