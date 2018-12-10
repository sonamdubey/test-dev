using Carwale.DTOs.IPToLocation;
using Carwale.Entity.IPToLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.IPToLocation
{
    public interface IIPToLocation
    {
        IPToLocationEntity GetCity();
        IPToLocationDTO GetCity_v1();
    }
}
