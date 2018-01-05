

using System.Linq;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Location;
using Bikewale.Notifications;
using Bikewale.Utility;
namespace Bikewale.Models
{
    public class MoreAboutScootersWidget
    {
        private readonly IBikeModelsCacheRepository<int> _objBestBikes = null;
        private readonly IBikeVersionCacheRepository<BikeVersionEntity, uint> _versionCache = null;
        private readonly ICityCacheRepository _objCityCache;
        private readonly IBikeInfo _bikeInfo = null;
        private readonly BikeInfoTabType _pageId;
        public uint modelId { get; set; }


        public MoreAboutScootersWidget(IBikeModelsCacheRepository<int> objBestBikes, ICityCacheRepository objCityCache, IBikeVersionCacheRepository<BikeVersionEntity, uint> versionCache, IBikeInfo bikeInfo, BikeInfoTabType pageId)
        {
            _objBestBikes = objBestBikes;
            _versionCache = versionCache;
            _bikeInfo = bikeInfo;
            _objCityCache = objCityCache;
            _pageId = pageId;
        }

        public MoreAboutScootersWidgetVM GetData()
        {

            MoreAboutScootersWidgetVM objVM = null;
            try
            {
                objVM = new MoreAboutScootersWidgetVM();
                GlobalCityAreaEntity location = GlobalCityArea.GetGlobalCityArea();

                uint cityId = location != null ? location.CityId : 0;
                uint topCount = 5;
                objVM.objBestBikesList = _objBestBikes.GetBestBikesByCategory(EnumBikeBodyStyles.Scooter, cityId);
                objVM.objBestBikesList = objVM.objBestBikesList.Reverse().Take((int)topCount);
                objVM.BikeInfo = (new BikeInfoWidget(_bikeInfo, _objCityCache, modelId, cityId, topCount, _pageId)).GetData();
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