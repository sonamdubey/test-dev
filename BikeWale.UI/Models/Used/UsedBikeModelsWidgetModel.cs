
using Bikewale.Common;
using Bikewale.Interfaces.Used;
namespace Bikewale.Models
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 27-Mar-2017
    /// Summary: Used bike models widget
    /// </summary>
    public class UsedBikeModelsWidgetModel
    {
        private uint _topCount = 0;
        public uint makeId { get; set; }
        public uint cityId { get; set; }
        private readonly IUsedBikeDetailsCacheRepository _objUsedCache = null;

        public UsedBikeModelsWidgetModel(uint topCount, IUsedBikeDetailsCacheRepository cachedUsedBikes)
        {

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
                if (makeId > 0)
                {
                    if (cityId > 0)
                        objData.UsedBikeModelList = _objUsedCache.GetUsedBikeByModelCountInCity(makeId, cityId, _topCount);
                    else
                        objData.UsedBikeModelList = _objUsedCache.GetPopularUsedModelsByMake(makeId, _topCount);

                }
                else
                {
                    if (cityId > 0)
                        objData.UsedBikeModelList = _objUsedCache.GetUsedBikeCountInCity(cityId, _topCount);
                    else
                        objData.UsedBikeModelList = _objUsedCache.GetUsedBike(_topCount);
                }
                if (cityId > 0)
                    objData.CityDetails = new CityHelper().GetCityById(cityId); ;
            }
            catch (System.Exception ex)
            {
                Bikewale.Notifications.ErrorClass er = new Bikewale.Notifications.ErrorClass(ex, "UsedBikeModelsWidgetModel.GetData()");
            }
            return objData;
        }
    }
}