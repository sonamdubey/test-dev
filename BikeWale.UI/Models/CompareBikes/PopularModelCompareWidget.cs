using Bikewale.Common;
using Bikewale.Interfaces.Compare;
using System;

namespace Bikewale.Models.CompareBikes
{
    /// <summary>
    /// Created By Sajal Gupta on 25-03-2017
    /// This class is created to fetch data for similar comparison widget (Desktop + Mobile)
    /// Modified by : Aditi Srivastava on 5 June 2017
    /// Summary     : Added BL instance instead of cache
    /// </summary>
    public class PopularModelCompareWidget
    {
        private string _versionList;
        private uint _topCount, _cityId;
        private readonly IBikeCompare _objCompare;

        public PopularModelCompareWidget(IBikeCompare objCompare, uint topCount, uint cityId, string versionList)
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

                objComparison.CompareBikes = _objCompare.GetSimilarCompareBikes(_versionList, (ushort)_topCount, (int)_cityId);
                objComparison.IsDataAvailable = (objComparison.CompareBikes != null);

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.CompareBikes.PopularModelCompareWidget.GetData()");
            }

            return objComparison;
        }
    }
}