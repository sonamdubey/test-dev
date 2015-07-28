using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Interfaces.BikeData
{
    public interface IBikeMaskingCacheRepository<T,U>
    {
        ModelMaskingResponse GetModelMaskingResponse(string maskingName);
    }
}
