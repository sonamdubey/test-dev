using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Videos;
using Bikewale.Utility;
using System;
using System.Collections.Generic;

namespace Bikewale.Models.Videos
{
    /// <summary>
    /// Created by Sajal Gupta on 01-04-2017
    /// Description : Model for fetching Model wise video page data.
    /// </summary>
    public class ModelWiseVideosPage
    {
        private readonly IVideosCacheRepository _objVideosCache = null;
        private readonly IBikeMakesCacheRepository _bikeMakesCache = null;
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _bikeModelsCache = null;
        private readonly IBikeInfo _bikeInfo = null;
        private readonly ICityCacheRepository _cityCacheRepo = null;
        private readonly IBikeSeriesCacheRepository _seriesCache = null;
        private readonly IBikeSeries _series = null;
        private string _makeMaskingName = string.Empty, _modelMaskingName = string.Empty;

        private ushort _maxVideoCount = 50, _pageNo = 1;
        private uint _makeId, _modelId;
        private uint _cookieCityId;

        public MakeMaskingResponse objMakeResponse;
        public ModelMaskingResponse objModelResponse;

        public StatusCodes makeStatus;
        public StatusCodes modelStatus;
        public string redirectUrl;
        public ushort SimilarBikeWidgetTopCount { get; set; }
        public bool IsMobile { get; set; }
        public SeriesMaskingResponse objMaskingResponse;
        public BikeSeriesEntityBase objSeries;

        public ModelWiseVideosPage(string makeMaskingName, string modelMaskingName, ICityCacheRepository cityCacheRepo, IBikeInfo bikeInfo, IVideosCacheRepository objVideosCache, IBikeMakesCacheRepository bikeMakesCache, IBikeMaskingCacheRepository<BikeModelEntity, int> bikeModelsCache, IBikeSeriesCacheRepository seriesCache, IBikeSeries series)
        {
            _makeMaskingName = makeMaskingName;
            _modelMaskingName = modelMaskingName;
            _objVideosCache = objVideosCache;
            _bikeMakesCache = bikeMakesCache;
            _bikeModelsCache = bikeModelsCache;
            _cityCacheRepo = cityCacheRepo;
            _bikeInfo = bikeInfo;
            _seriesCache = seriesCache;
            _series = series;
            ProcessQuery(_makeMaskingName, _modelMaskingName);
        }

        public ModelWiseVideoPageVM GetDataSeries()
        {
            ModelWiseVideoPageVM objVM = new ModelWiseVideoPageVM();
            try
            {
                if (_makeId > 0)
                    objVM.Make = new MakeHelper().GetMakeNameByMakeId(_makeId);

                string modelIds = _series.GetModelIdsBySeries(objMaskingResponse.Id);
                objVM.VideosList = _objVideosCache.GetSimilarVideos(_maxVideoCount, modelIds, 1);
                objVM.objSeries = objSeries;
                BindPageMetasSeries(objVM);
            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "ModelWiseVideosPage.GetDataSeries");
            }
            return objVM;
        }
        private void BindPageMetasSeries(ModelWiseVideoPageVM objPageVM)
        {
            try
            {
                if (objPageVM != null && objPageVM.PageMetaTags != null && objPageVM.Make != null && objPageVM.objSeries != null)
                {
                    objPageVM.PageMetaTags.Title = String.Format("Videos of all {0} {1} bikes | See the latest {0} {1} videos - BikeWale", objPageVM.Make.MakeName, objPageVM.objSeries.SeriesName);
                    objPageVM.PageMetaTags.Keywords = string.Format("{0} {1} Videos, {1} Videos", objPageVM.Make.MakeName, objPageVM.objSeries.SeriesName);
                    objPageVM.PageMetaTags.Description = string.Format("Check latest {0} {1} 3G videos, watch BikeWale expert's take on all {0} {1} bikes", objPageVM.Make.MakeName, objPageVM.objSeries.SeriesName);
                    SetBreadcrumListSeries(objPageVM);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ModelWiseVideosPage.BindPageMetasSeries");
            }
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

                objVM.SimilarBikeVideoList = _bikeModelsCache.GetSimilarBikesVideos(_modelId, SimilarBikeWidgetTopCount, _cookieCityId);

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
                    SetBreadcrumList(objPageVM);
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
                objMaskingResponse = _seriesCache.ProcessMaskingName(modelMaskingName);
                objModelResponse = _bikeModelsCache.GetModelMaskingResponse(modelMaskingName);
                if (objMaskingResponse != null)
                {
                    if (objMaskingResponse.StatusCode == 200)
                    {
                        if (!objMaskingResponse.IsSeriesPageCreated)
                        {


                            _modelId = objMaskingResponse.Id;

                        }
                        else
                        {

                            objSeries = new BikeSeriesEntityBase
                            {
                                SeriesId = objMaskingResponse.Id,
                                SeriesName = objMaskingResponse.Name,
                                MaskingName = objMaskingResponse.MaskingName,
                                IsSeriesPageUrl = true
                            };
                        }
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
        /// <summary>
        /// Created By :Subodh Jain on 11th Nov 2017
        /// Description : Function to create page level schema for breadcrum
        /// </summary>
        private void SetBreadcrumListSeries(ModelWiseVideoPageVM objVM)
        {
            try
            {
                IList<BreadcrumbListItem> BreadCrumbs = new List<BreadcrumbListItem>();
                string bikeUrl;
                bikeUrl = string.Format("{0}/", Utility.BWConfiguration.Instance.BwHostUrl);
                ushort position = 1;
                if (IsMobile)
                {
                    bikeUrl += "m/";
                }

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, "Home"));

                if (objVM.Make != null)
                {
                    bikeUrl += string.Format("{0}-bikes/", objVM.Make.MaskingName);
                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, bikeUrl, string.Format("{0} Bikes", objVM.Make.MakeName)));
                }
                if (objVM.objSeries != null)
                {
                    bikeUrl += objVM.objSeries.MaskingName;
                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, bikeUrl, string.Format("{0} Bikes", objVM.objSeries.SeriesName)));

                }
                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null, "Videos"));

                objVM.BreadcrumbList.BreadcrumListItem = BreadCrumbs;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ModelWiseVideosPage.SetBreadcrumList");
            }

        }
        /// <summary>
        /// Created By :Snehal Dange on 8th Nov 2017
        /// Description : Function to create page level schema for breadcrum
        /// </summary>
        private void SetBreadcrumList(ModelWiseVideoPageVM objVM)
        {
            try
            {
                IList<BreadcrumbListItem> BreadCrumbs = new List<BreadcrumbListItem>();
                string bikeUrl;
                bikeUrl = string.Format("{0}/", Utility.BWConfiguration.Instance.BwHostUrl);
                ushort position = 1;
                if (IsMobile)
                {
                    bikeUrl += "m/";
                }

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, "Home"));

                if (objVM.Make != null)
                {
                    bikeUrl += string.Format("{0}-bikes/", objVM.Make.MaskingName);
                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, bikeUrl, string.Format("{0} Bikes", objVM.Make.MakeName)));
                }
                if (objVM.Model != null)
                {
                    bikeUrl += string.Format("{0}/", objVM.Model.MaskingName);
                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, bikeUrl, string.Format("{0} {1}", objVM.Make.MakeName, objVM.Model.ModelName)));

                }
                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null, "Videos"));

                objVM.BreadcrumbList.BreadcrumListItem = BreadCrumbs;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ModelWiseVideosPage.SetBreadcrumList");
            }

        }
    }
}