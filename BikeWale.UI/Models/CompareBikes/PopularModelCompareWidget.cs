using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.Compare;
using System;
using System.Collections.Generic;

namespace Bikewale.Models.CompareBikes
{
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

        public ICollection<SimilarCompareBikeEntity> GetData()
        {
            ICollection<SimilarCompareBikeEntity> objSimilarBikes = null;

            try
            {
                objSimilarBikes = _objCompare.GetSimilarCompareBikes(_versionList, (ushort)_topCount, (int)_cityId);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.CompareBikes.PopularModelCompareWidget.GetData()");
            }

            return objSimilarBikes;
        }
    }
}