﻿

using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Location;
using Bikewale.Notifications;
using Bikewale.Utility;
using System.Linq;
namespace Bikewale.Models
{
    public class MoreAboutScootersWidget
    {
        private readonly IBikeModelsCacheRepository<int> _objBestBikes = null;
        private readonly IBikeVersionCacheRepository<BikeVersionEntity, uint> _versionCache = null;
        private readonly ICityCacheRepository _objCityCache;
        private readonly IBikeInfo _bikeInfo = null;


        public uint modelId { get; set; }


        public MoreAboutScootersWidget(IBikeModelsCacheRepository<int> objBestBikes, ICityCacheRepository objCityCache, IBikeVersionCacheRepository<BikeVersionEntity, uint> versionCache, IBikeInfo bikeInfo)
        {
            _objBestBikes = objBestBikes;
            _versionCache = versionCache;
            _bikeInfo = bikeInfo;
            _objCityCache = objCityCache;
        }

        public MoreAboutScootersWidgetVM GetData()
        {

            MoreAboutScootersWidgetVM objVM = null;
            try
            {
                objVM = new MoreAboutScootersWidgetVM();
                GlobalCityAreaEntity location = GlobalCityArea.GetGlobalCityArea();

                uint cityId = location != null ? location.CityId : 0;
                uint topCount = 4;
                objVM.objBestBikesList = _objBestBikes.GetBestBikesByCategory(EnumBikeBodyStyles.Scooter, cityId);
                objVM.objBestBikesList = objVM.objBestBikesList.Where(x => x.Model.ModelId != modelId).Reverse().Take((int)topCount);
                objVM.BikeInfo = (new BikeInfoWidget(_bikeInfo, _objCityCache, modelId, cityId, topCount, Entities.GenericBikes.BikeInfoTabType.PriceInCity)).GetData();
                objVM.SimilarBikes = _versionCache.GetSimilarBikesByMinPriceDiff(modelId, topCount, cityId);
                objVM.objBikeData = new Bikewale.Common.ModelHelper().GetModelDataById(modelId);
            }
            catch (System.Exception ex)
            {

                ErrorClass.LogError(ex, "Bikewale.MoreAboutScootersWidget.GetData");
            }
            return objVM;
        }
    }
}