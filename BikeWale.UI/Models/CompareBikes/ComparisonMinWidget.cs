using Bikewale.Common;
using Bikewale.Entities.Compare;
using Bikewale.Interfaces.Compare;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models.CompareBikes
{
    public class ComparisonMinWidget
    {
        /// <summary>
        /// Created by Sajal Gupta on 25-03-2017
        /// This class will get data for comparisson widget (Mobile + Desktop)
        /// </summary>
        private readonly IBikeCompareCacheRepository _objCompareCache;
        private uint _topCount;
        private bool _showComparisonButton;

        public ComparisonMinWidget(IBikeCompareCacheRepository objCompareCache, uint topCount, bool showComparisonButton)
        {
            _topCount = topCount;
            _objCompareCache = objCompareCache;
            _showComparisonButton = showComparisonButton;
        }

        public ComparisonMinWidgetVM GetData()
        {
            ComparisonMinWidgetVM objComparison = null;
            try
            {
                IEnumerable<TopBikeCompareBase> topBikeCompares = _objCompareCache.CompareList(_topCount);

                if (topBikeCompares != null && topBikeCompares.Count() > 0)
                {
                    objComparison = new ComparisonMinWidgetVM();
                    objComparison.TopComparisonRecord = topBikeCompares.First();
                    objComparison.RemainingCompareList = topBikeCompares.Skip(1);
                    objComparison.ShowComparisonButton = _showComparisonButton;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.CompareBikes.ComparisonMinWidget.GetData()");
            }
            return objComparison;
        }
    }
}