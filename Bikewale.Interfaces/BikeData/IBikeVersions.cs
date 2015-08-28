using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.BikeData;

namespace Bikewale.Interfaces.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// Summary : Interface for bike versions data
    /// </summary>
    /// <typeparam name="T">Generic type (need to specify type while implementing this interface)</typeparam>
    /// <typeparam name="U">Generic type (need to specify type while implementing this interface)</typeparam>
    public interface IBikeVersions<T, U> : IRepository<T, U>
    {
        /// <summary>
        /// Modified By : Sadhana Upadhyay on 25 Aug 2014
        /// Summary : Changed return type to get price
        /// </summary>
        /// <param name="requestType"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        List<BikeVersionsListEntity> GetVersionsByType(EnumBikeType requestType, int modelId, int? cityId = null);
        BikeSpecificationEntity GetSpecifications(U versionId);
        List<BikeVersionMinSpecs> GetVersionMinSpecs(uint modelId, bool isNew);
        List<SimilarBikeEntity> GetSimilarBikesList(U versionId,uint topCount,uint percentDeviation);
        List<VersionColor> GetColorByVersion(U versionId);
    }
}
