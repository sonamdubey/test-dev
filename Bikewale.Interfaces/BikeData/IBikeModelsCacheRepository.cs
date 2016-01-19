using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Photos;                                       

namespace Bikewale.Interfaces.BikeData
{
    public interface IBikeModelsCacheRepository<U>
    {
        BikeModelPageEntity GetModelPageDetails(U modelId);        
    }
}
