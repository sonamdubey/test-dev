﻿using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;

namespace Bikewale.Interfaces.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// Summary : Interface for bike versions data
    /// Modified By : Rajan Chauhan on 17 Apr 2018
    /// Description : Changed signature of GetSimilarBikesList to include maxTorqueRequired
    /// Modifier    : Kartik Rathod on 11 May 2018, added GetSimilarBikesForEMI
    /// </summary>
    /// <typeparam name="T">Generic type (need to specify type while implementing this interface)</typeparam>
    /// <typeparam name="U">Generic type (need to specify type while implementing this interface)</typeparam>
    public interface IBikeVersions<T, U> : IRepository<T, U>
    {
        /// <summary>
        /// Modified By : Sadhana Upadhyay on 25 Aug 2014
        /// Summary : Changed return type to get price
        /// Modified By : Aditi Srivastava on 17 Oct 2016
        /// Summary : Added function to get version colors by version id
        /// </summary>
        /// <param name="requestType"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        List<BikeVersionsListEntity> GetVersionsByType(EnumBikeType requestType, int modelId, int? cityId = null);
        IEnumerable<BikeVersionMinSpecs> GetVersionMinSpecs(uint modelId, bool isNew);
        IEnumerable<SimilarBikeEntity> GetSimilarBikesList(U versionId, uint topCount, uint cityId, bool maxTorqueRequired);
        IEnumerable<SimilarBikeEntity> GetSimilarBikesByModel(U modelId, uint topCount, uint cityId);
        IEnumerable<SimilarBikeEntity> GetSimilarBudgetBikes(U modelId, uint topCount, uint cityId);

        List<VersionColor> GetColorByVersion(U versionId);
        IEnumerable<BikeColorsbyVersion> GetColorsbyVersionId(uint versionId);
        IEnumerable<BikeVersionsSegment> GetModelVersionsDAL(); // Added by sajal gupta
        IEnumerable<BikeModelVersionsDetails> GetModelVersions(); // Added by sajal gupta
        IEnumerable<BikeVersionWithMinSpec> GetDealerVersionsByModel(uint dealerId, uint modelId);
        IEnumerable<SimilarBikesForEMIEntity> GetSimilarBikesForEMI(int versionId, short topcount, int cityId);

    }
}
