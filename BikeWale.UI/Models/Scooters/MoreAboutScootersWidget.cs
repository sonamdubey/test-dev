

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
        private readonly IBikeVersions<BikeVersionEntity, uint> _version;
        private readonly ICityCacheRepository _objCityCache;
        private readonly IBikeInfo _bikeInfo = null;
        private readonly BikeInfoTabType _pageId;
        private readonly IBikeSeries _bikeSeries = null;
        private readonly IBikeModels<BikeModelEntity, int> _models;
        public uint modelId { get; set; }


        public MoreAboutScootersWidget(IBikeModelsCacheRepository<int> objBestBikes, ICityCacheRepository objCityCache, IBikeVersions<BikeVersionEntity, uint> version, IBikeInfo bikeInfo, BikeInfoTabType pageId, IBikeModels<BikeModelEntity, int> models, IBikeSeries bikeSeries)
        {
            _objBestBikes = objBestBikes;
            _version = version;
            _bikeInfo = bikeInfo;
            _objCityCache = objCityCache;
            _pageId = pageId;
            _models = models;
            _bikeSeries = bikeSeries;
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
                objVM.BikeInfo = (new BikeInfoWidget(_bikeInfo, _objCityCache, modelId, cityId, topCount, _pageId, _models, _bikeSeries)).GetData();
                objVM.SimilarBikes = _version.GetSimilarBudgetBikes(modelId, topCount, cityId);
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