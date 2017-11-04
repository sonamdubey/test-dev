using Bikewale.Entities.Location;
using System;
using System.Collections.Generic;

namespace Bikewale.Interfaces.Location
{
    public interface IArea
    {
        List<AreaEntityBase> GetAreas(string cityId);
        IEnumerable<AreaEntityBase> GetAreasByCity(UInt16 cityId);
    }
}
