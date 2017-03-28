
using Bikewale.Interfaces.Used;
namespace Bikewale.Models
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 27-Mar-2017
    /// Summary: Used bike models widget
    /// </summary>
    public class UsedBikeModelsWidgetModel
    {
        private uint _cityId, _topCount = 0;
        private readonly IUsedBikeDetailsCacheRepository _objUsedCache = null;

        public UsedBikeModelsWidgetModel(uint cityId, uint topCount, IUsedBikeDetailsCacheRepository cachedUsedBikes)
        {
            _cityId = cityId;
            _topCount = topCount;
            _objUsedCache = cachedUsedBikes;
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <returns>
        /// Created by : Sangram Nandkhile on 27-Mar-2017 
        /// </returns>
        public UsedBikeModelsWidgetVM GetData()
        {
            UsedBikeModelsWidgetVM objData = new UsedBikeModelsWidgetVM();
            if (_cityId > 0)
                objData.UsedBikeModelList = _objUsedCache.GetUsedBikeCountInCity(_cityId, _topCount);
            else
                objData.UsedBikeModelList = _objUsedCache.GetUsedBike(_topCount);
            return objData;
        }
    }
}