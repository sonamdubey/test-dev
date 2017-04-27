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
        public uint TopCount { get; set; }
        public uint CityId { get; set; }
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
        public PopularComparisonsVM GetData()
        {
            PopularComparisonsVM objComparison = new PopularComparisonsVM();
            try
            {
                if (TopCount == 0)
                    TopCount = 9;
                objComparison.CompareBikes = _objCompare.GetPopularCompareList(CityId);
                if (objComparison.CompareBikes != null && objComparison.CompareBikes.Count() > 0)
                {
                    objComparison.CompareBikes = objComparison.CompareBikes.Take((int)TopCount);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.CompareBikes.CompareSimilarBikes.GetData");
            }
            return objComparison;
        }
        #endregion
    }
}