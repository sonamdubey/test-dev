using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.Compare;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by : Aditi Srivastava on 25 Apr 2017
    /// Summary    : To get list of popular comparisons
    /// </summary>
    public class ComparePopularBikes
    {
        #region Private variables
        private readonly IBikeCompareCacheRepository _objCompare=null;
        #endregion

        #region Public properties
        public uint TopCount;
        public uint CityId;
        #endregion

        #region Contructor
        public ComparePopularBikes(IBikeCompareCacheRepository objCompare)
        {
            _objCompare = objCompare;
        }
        #endregion

        #region Functions
        /// <summary>
        /// Created by : Aditi Srivastava on 25 Apr 2017
        /// Summary    : To get list of popular comparisons
        /// </summary>
        public IEnumerable<SimilarCompareBikeEntity> GetData()
        {
            IEnumerable<SimilarCompareBikeEntity> compareBikesList = null;
            try
            {
                compareBikesList = _objCompare.GetPopularCompareList(CityId);
                if (compareBikesList != null && compareBikesList.Count() > 0)
                {
                    compareBikesList = compareBikesList.Take((int)TopCount);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.CompareBikes.CompareSimilarBikes.GetData");
            }
            return compareBikesList;
        }
        #endregion
    }
}