
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
        private uint _makeId = 0;
        private readonly IUsedBikeDetailsCacheRepository _objUsedCache = null;
        private uint cityId;
        private string _location, _locationMasking;
        private IUsedBikeDetailsCacheRepository _cachedBikeDetails;

        public UsedBikeModelsWidgetModel(uint cityId, uint topCount, IUsedBikeDetailsCacheRepository cachedUsedBikes)
        {
            _cityId = cityId;
            _topCount = topCount;
            _objUsedCache = cachedUsedBikes;
        }

        public UsedBikeModelsWidgetModel(uint cityId, uint topCount, uint makeId, IUsedBikeDetailsCacheRepository cachedUsedBikes)
        {
            _makeId = makeId;
            _cityId = cityId;
            _topCount = topCount;
            _objUsedCache = cachedUsedBikes;
        }

        public UsedBikeModelsWidgetModel(uint cityId, uint topCount, uint makeId, string location, string locationMasking, IUsedBikeDetailsCacheRepository cachedUsedBikes)
        {
            // TODO: Complete member initialization
            _makeId = makeId;
            _cityId = cityId;
            _topCount = topCount;
            _location = location;
            _locationMasking = locationMasking;
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
            if (_makeId > 0)
            {
                if (_cityId > 0)
                    objData.UsedBikeModelList = _objUsedCache.GetUsedBikeByModelCountInCity(_makeId, _cityId, _topCount);
                else
                    objData.UsedBikeModelList = _objUsedCache.GetPopularUsedModelsByMake(_makeId, _topCount);

            }
            else
            {
                if (_cityId > 0)
                    objData.UsedBikeModelList = _objUsedCache.GetUsedBikeCountInCity(_cityId, _topCount);
                else
                    objData.UsedBikeModelList = _objUsedCache.GetUsedBike(_topCount);
            }
            objData.Location = _location;
            objData.LocationMasking = _locationMasking;
            return objData;
        }
    }
}