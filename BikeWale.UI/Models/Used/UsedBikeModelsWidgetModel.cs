
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Used;
using Bikewale.Notifications;
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
        private readonly CityEntityBase _city;

        public UsedBikeModelsWidgetModel(uint topCount, CityEntityBase city, IUsedBikeDetailsCacheRepository cachedUsedBikes)
        {
            if (city != null && city.CityId > 0)
            {
                _cityId = city.CityId;
            }
            _city = city;
            _topCount = topCount;
            _objUsedCache = cachedUsedBikes;
        }


        public UsedBikeModelsWidgetModel(uint topCount, uint makeId, CityEntityBase city, IUsedBikeDetailsCacheRepository cachedUsedBikes)
        {
            // TODO: Complete member initialization
            _makeId = makeId;
            if (city != null && city.CityId > 0)
            {
                _cityId = city.CityId;
            }
            _city = city;
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
            try
            {
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
                objData.CityDetails = _city;
            }
            catch (System.Exception ex)
            {
                ErrorClass er = new ErrorClass(ex, "UsedBikeModelsWidgetModel.GetData()");
            }
            return objData;
        }
    }
}