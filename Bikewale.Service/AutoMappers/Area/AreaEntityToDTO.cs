using AutoMapper;
using Bikewale.DTO.Area;
using Bikewale.Entities.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.Area
{
    public class AreaEntityToDTO
    {
        internal static IEnumerable<DTO.Area.AreaBase> ConvertAreaList(List<Entities.Location.AreaEntityBase> objAreaList)
        {
            Mapper.CreateMap<AreaEntityBase, AreaBase>();
            return Mapper.Map<List<AreaEntityBase>, List<AreaBase>>(objAreaList);
        }
    }
}