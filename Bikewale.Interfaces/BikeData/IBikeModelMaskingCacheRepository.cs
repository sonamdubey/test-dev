using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Interfaces.BikeData
{
    /// <summary>
    /// Modified By : Lucky Rathore on 07 June 2016
    /// Description : BikeSpecificationEntity MVSpecsFeatures(int versionId) added for caching specification and feature of versions.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public interface IBikeMaskingCacheRepository<T,U>
    {
        ModelMaskingResponse GetModelMaskingResponse(string maskingName);
        BikeSpecificationEntity MVSpecsFeatures(int versionId);
    }
}
