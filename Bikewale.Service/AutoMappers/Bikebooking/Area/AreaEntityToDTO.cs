using AutoMapper;
using Bikewale.DTO.BikeBooking.Area;
using Bikewale.Entities.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.Bikebooking.Area
{
    public class AreaEntityToDTO
    {
        internal static IEnumerable<BBAreaBase> Convert(IEnumerable<Entities.Location.AreaEntityBase> objAreaList)
        {
            IEnumerable<BBAreaBase> dto = null;
            Mapper.CreateMap<AreaEntityBase, BBAreaBase>();
            dto = Mapper.Map<IEnumerable<AreaEntityBase>, IEnumerable<BBAreaBase>>(objAreaList);
            return dto;
        }
    }
}