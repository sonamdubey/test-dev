using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Interfaces.BikeData
{
    public interface IBikeInfo
    {
       Bikewale.Models.Shared.BikeInfo GetBikeInfo(uint modelId);
    }
}
