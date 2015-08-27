using Bikewale.Entities.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Interfaces.Location
{
    public interface IArea
    {
        IEnumerable<AreaEntityBase> GetAreasByCity(UInt16 cityId);
    }
}
