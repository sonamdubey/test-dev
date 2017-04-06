using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Videos;
using Bikewale.Utility;
using System;

namespace Bikewale.Models.Videos
{
    /// <summary>
    /// Created by Sajal Gupta on 01-04-2017
    /// Description : Model for fetching Model wise video page data.
    /// </summary>
    public class ModelWiseVideosPage
    {
        private readonly IVideosCacheRepository _objVideosCache = null;
        private readonly IBikeMakesCacheRepository<int> _bikeMakesCache = null;
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _bikeModelsCache = null;
        private readonly IBikeInfo _bikeInfo = null;
        private readonly ICityCacheRepository _cityCacheRepo = null;


        private string _makeMaskingName = string.Empty, _modelMaskingName = string.Empty;

        private ushort _maxVideoCount = 50, _pageNo = 1;
        private uint _makeId, _modelId;
        private uint _cookieCityId;

        public MakeMaskingResponse objMakeResponse;
        public ModelMaskingResponse objModelResponse;

        public StatusCodes makeStatus;
        public StatusCodes modelStatus;

        public ushort SimilarBikeWidgetTopCount { get; set; }

        public ModelWiseVideosPage(string makeMaskingName, string modelMaskingName, ICityCacheRepository cityCacheRepo, IBikeInfo bikeInfo, IVideosCacheRepository objVideosCache, IBikeMakesCacheRepository<int> bikeMakesCache, IBikeMaskingCacheRepository<BikeModelEntity, int> bikeModelsCache)
        {
            _makeMaskingName = makeMaskingName;
            _modelMaskingName = modelMaskingName;
            _objVideosCache = objVideosCache;
            _bikeMakesCache = bikeMakesCache;
            _bikeModelsCache = bikeModelsCache;
            _cityCacheRepo = cityCacheRepo;
            _bikeInfo = bikeInfo;

            ProcessQuery(_makeMaskingName, _modelMaskingName);
        }

        public ModelWiseVideoPageVM GetData()
        {
            ModelWiseVideoPageVM objVM = null;
            try
            {
                objVM = new ModelWiseVideoPageVM();

                if (_makeId > 0)
                    objVM.Make = new MakeHelper().GetMakeNameByMakeId(_makeId);

                if (_modelId > 0)
                    objVM.Model = new ModelHelper().GetModelDataById(_modelId);


                GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
                _cookieCityId = currentCityArea.CityId;

                objVM.CityId = _cookieCityId;

                objVM.VideosList = _objVideosCache.GetVideosByMakeModel(_pageNo, _maxVideoCount, _makeId, (uint?)_modelId);

                objVM.BikeInfoWidgetData = new BikeInfoWidget(_bikeInfo, _cityCacheRepo, _modelId, _cookieCityId, 4, BikeInfoTabType.Videos).GetData();

                objVM.SimilarBikeVideoList = _bikeModelsCache.GetSimilarBikesVideos(_modelId, SimilarBikeWidgetTopCount);

                BindPageMetas(objVM);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ModelWiseVideosPage.GetData");
            }
            return objVM;
        }

        private void BindPageMetas(ModelWiseVideoPageVM objPageVM)
        {
            try
            {
                if (objPageVM != null && objPageVM.PageMetaTags != null && objPageVM.Make != null && objPageVM.Model != null)
                {
                    objPageVM.PageMetaTags.Title = String.Format("{0} {1} Videos - BikeWale", objPageVM.Make.MakeName, objPageVM.Model.ModelName);
                    objPageVM.PageMetaTags.Keywords = string.Format("{0},{1},{0} {1},{0} {1} videos", objPageVM.Make.MakeName, objPageVM.Model.ModelName);
                    objPageVM.PageMetaTags.Description = string.Format("Check latest {0} {1} videos, watch BikeWale expert's take on {0} {1} - features, performance, price, fuel economy, handling and more.", objPageVM.Make.MakeName, objPageVM.Model.ModelName);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ModelWiseVideosPage.BindPageMetas");
            }
        }

        private void ProcessQuery(string makeMaskingName, string modelMaskingName)
        {
            objMakeResponse = _bikeMakesCache.GetMakeMaskingResponse(makeMaskingName);

            if (objMakeResponse != null)
            {
                if (objMakeResponse.StatusCode == 200)
                {
                    _makeId = objMakeResponse.MakeId;
                    makeStatus = StatusCodes.ContentFound;
                }
                else if (objMakeResponse.StatusCode == 301)
                {
                    makeStatus = StatusCodes.RedirectPermanent;
                }
                else
                {
                    makeStatus = StatusCodes.ContentNotFound;
                }
            }
            else
            {
                makeStatus = StatusCodes.ContentNotFound;
            }

            if (_makeId > 0)
            {
                objModelResponse = _bikeModelsCache.GetModelMaskingResponse(modelMaskingName);
                if (objModelResponse != null)
                {
                    if (objModelResponse.StatusCode == 200)
                    {
                        _modelId = objModelResponse.ModelId;
                        modelStatus = StatusCodes.ContentFound;
                    }
                    else if (objModelResponse.StatusCode == 301)
                    {
                        modelStatus = StatusCodes.RedirectPermanent;
                    }
                    else
                    {
                        modelStatus = StatusCodes.ContentNotFound;
                    }
                }
                else
                {
                    modelStatus = StatusCodes.ContentNotFound;
                }
            }
        }
    }
}