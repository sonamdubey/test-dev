
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.UsedBikes;
using Bikewale.Interfaces.UsedBikes;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.Models.Used
{
    public class UserBikeSeriesModelsWidget
    {
        uint _cityId, _makeId, _modelId, _seriesid;
        private readonly IUsedBikesCache _objCache;
        public UserBikeSeriesModelsWidget(IUsedBikesCache objCache, uint seriesid, uint cityId)
        {
            _objCache = objCache;
            _seriesid = seriesid;
            _cityId = cityId;
        }
        public UsedBikeByModelCityVM GetData()
        {

            UsedBikeByModelCityVM objUsed = null;

            try
            {


                IEnumerable<MostRecentBikes> objMostRecentBikes = null;

                objUsed = new UsedBikeByModelCityVM();

                objMostRecentBikes = _objCache.GetUsedBikesSeries(_seriesid, _cityId);

                if (objMostRecentBikes != null && objMostRecentBikes.Any())
                {
                    MostRecentBikes objMostRecentUsedBikes = null;
                    objMostRecentUsedBikes = objMostRecentBikes.FirstOrDefault();

                    objUsed.RecentUsedBikesList = objMostRecentBikes;

                    if (objMostRecentUsedBikes != null)
                    {
                        objUsed.Make = new BikeMakeEntityBase();
                        objUsed.Make.MakeId = (int)_makeId;
                        objUsed.Make.MakeName = objMostRecentUsedBikes.MakeName;
                        objUsed.Make.MaskingName = objMostRecentUsedBikes.MakeMaskingName;

                        objUsed.Model = new BikeModelEntityBase();
                        objUsed.Model.ModelId = (int)_modelId;
                        objUsed.Model.ModelName = objMostRecentUsedBikes.ModelName;
                        objUsed.Model.MaskingName = objMostRecentUsedBikes.ModelMaskingName;

                        objUsed.LinkHeading = String.Format("{0} {1}", objUsed.Make.MakeName, objUsed.Model.ModelName);

                        objUsed.City = new CityEntityBase();
                        objUsed.City.CityId = _cityId;
                        objUsed.City.CityName = objMostRecentUsedBikes.CityName;
                        objUsed.City.CityMaskingName = objMostRecentUsedBikes.CityMaskingName;
                    }
                }

            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Bikewale.Models.Used.UsedBikesByModelCityWidget.GetData()");
            }

            return objUsed;

        }

    }
}