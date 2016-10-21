﻿using Bikewale.Entities.BikeData;
using Bikewale.Entities.Compare;
using System.Collections.Generic;

namespace Bikewale.Interfaces.Compare
{
    /// <summary>
    /// Updated by : Sangram Nandkhile on 12 May 2016
    /// Desc       : Added GetSimilarCompareBikes functions
    /// Modified by :Subodh Jain on 21 oct 2016
    /// Desc : Added cityid as parameter
    /// </summary>
    public interface IBikeCompare
    {
        BikeCompareEntity DoCompare(string versions);
        IEnumerable<TopBikeCompareBase> CompareList(uint topCount);
        IEnumerable<SimilarCompareBikeEntity> GetSimilarCompareBikes(string versionList, uint topCount, int cityid);
    }
}
