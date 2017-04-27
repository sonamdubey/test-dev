using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.Compare;
using System;
using System.Collections.Generic;

namespace Bikewale.Models.CompareBikes
{
    /// <summary>
    /// Created By Sajal Gupta on 25-03-2017
    /// This class is created to fetch data for similar comparison widget (Desktop + Mobile)
    /// </summary>
    public class PopularModelCompareWidget
    {
        private string _versionList;
        private uint _topCount, _cityId;
        private readonly IBikeCompareCacheRepository _objCompare;

        public PopularModelCompareWidget(IBikeCompareCacheRepository objCompare, uint topCount, uint cityId, string versionList)
        {
            _versionList = versionList;
            _topCount = topCount;
            _cityId = cityId;
            _objCompare = objCompare;
        }

        /// <summary>
        /// Modified by : Aditi Srivastava on 27 Apr 2017
        /// Summary     : Changed return type to PopularComparisonVM
        /// </summary>
        public PopularComparisonsVM GetData()
        {
            PopularComparisonsVM objComparison = new PopularComparisonsVM();            
            try
            {
                objComparison.IsDataAvailable = true;
                objComparison.CompareBikes = _objCompare.GetSimilarCompareBikes(_versionList, (ushort)_topCount, (int)_cityId);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.CompareBikes.PopularModelCompareWidget.GetData()");
            }

            return objComparison;
        }
    }
}