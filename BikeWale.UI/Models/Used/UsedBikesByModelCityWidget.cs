using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.UsedBikes;
using Bikewale.Interfaces.UsedBikes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models.Used
{
    /// <summary>
    /// Created by Sajal Gupta on 27-03-2017
    /// This class is created to give data to usedBikesByModelCity widget (Desktop + Model)
    /// </summary>
    public class UsedBikesByModelCityWidget
    {
        private uint _topCount, _makeId, _modelId, _cityId;
        private IUsedBikesCache _objCache;

        public UsedBikesByModelCityWidget(IUsedBikesCache objCache, uint topCount, uint makeId, uint modelId, uint cityId)
        {
            _topCount = topCount;
            _objCache = objCache;
            _makeId = makeId;
            _modelId = modelId;
            _cityId = cityId;
        }

        public UsedBikeByModelCityVM GetData()
        {
            UsedBikeByModelCityVM objUsed = null;

            try
            {
                if (_topCount <= 0) { _topCount = 6; }

                IEnumerable<MostRecentBikes> objMostRecentBikes = null;

                objUsed = new UsedBikeByModelCityVM();

                objMostRecentBikes = _objCache.GetUsedBikes(_makeId, _modelId, _cityId, _topCount);

                if (objMostRecentBikes != null && objMostRecentBikes.Count() > 0)
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