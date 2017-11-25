using AutoMapper;
using Bikewale.DTO.BikeBooking.Area;
using Bikewale.Entities.Location;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.Bikebooking.Area
{
    public class BBAreaListMapper
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