using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Videos;
using Bikewale.Models.BikeSeries;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

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
        private readonly IBikeModels<BikeModelEntity, int> _models;
        private string _makeMaskingName = string.Empty, _modelMaskingName = string.Empty;
        private readonly IBikeVersionCacheRepository<BikeVersionEntity, uint> _objBikeVersionsCache = null;
        private readonly IBikeModelsCacheRepository<int> _objModelCache = null;

        private ushort _maxVideoCount = 50, _pageNo = 1;
        private uint _makeId, _modelId;
        private uint _cookieCityId;
        private ModelHelper modelHelper = null;

        public MakeMaskingResponse objMakeResponse;
        public ModelMaskingResponse objModelResponse;

        public StatusCodes makeStatus;
        public StatusCodes modelStatus;
        public string redirectUrl;
        public ushort SimilarBikeWidgetTopCount { get; set; }
        public bool IsMobile { get; set; }
        public SeriesMaskingResponse objMaskingResponse;
        public BikeSeriesEntityBase objSeries;
        public string newMakeMasking = string.Empty, newModelMasking = string.Empty;

        public ModelWiseVideosPage(string makeMaskingName, string modelMaskingName, ICityCacheRepository cityCacheRepo, IBikeInfo bikeInfo, IVideosCacheRepository objVideosCache, IBikeMakesCacheRepository bikeMakesCache, IBikeMaskingCacheRepository<BikeModelEntity, int> bikeModelsCache, IBikeSeriesCacheRepository seriesCache, IBikeSeries series, IBikeModels<BikeModelEntity, int> models, IBikeVersionCacheRepository<BikeVersionEntity, uint> objBikeVersionsCache, IBikeModelsCacheRepository<int> objModelCache)
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
            _models = models;
            _objBikeVersionsCache = objBikeVersionsCache;
            _objModelCache = objModelCache;
            ProcessQuery(_makeMaskingName, _modelMaskingName);
        }

        /// <summary>
        /// Modified by : Ashutosh Sharma on 11 Dec 2017
        /// Description : Removed videoBasicId from call of GetSimilarVideos.
        /// Description : Added call to GetGlobalCityArea and BindPopularSeriesBikes.
        /// </summary>
        /// <returns></returns>
        public ModelWiseVideoPageVM GetDataSeries()
        {
            ModelWiseVideoPageVM objVM = new ModelWiseVideoPageVM();
            try
            {
                GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
                _cookieCityId = currentCityArea.CityId;

                objVM.CityId = _cookieCityId;

                if (_makeId > 0)
                    objVM.Make = new MakeHelper().GetMakeNameByMakeId(_makeId);

                string modelIds = _series.GetModelIdsBySeries(objMaskingResponse.SeriesId);
                objVM.VideosList = _objVideosCache.GetSimilarVideos(_maxVideoCount, modelIds);
                objVM.objSeries = objSeries;
                BindPageMetasSeries(objVM);
                BindPopularSeriesBikes(objVM, currentCityArea.City);
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, "ModelWiseVideosPage.GetDataSeries");
            }
            return objVM;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 11 Dec 2017
        /// Description : Method to bind popular series bikes.
        /// </summary>
        private void BindPopularSeriesBikes(ModelWiseVideoPageVM objVM, string CityName)
        {
            try
            {
                objVM.PopularSeriesBikes = new PopularSeriesBikesVM();
                objVM.PopularSeriesBikes.BikesList = _series.GetNewModels(objMaskingResponse.SeriesId, objVM.CityId);
                if (objVM.PopularSeriesBikes.BikesList != null)
                {
                    objVM.PopularSeriesBikes.BikesList = objVM.PopularSeriesBikes.BikesList.Take(9);
                }
                if (objVM.objSeries != null && objMakeResponse != null)
                {
                    objVM.PopularSeriesBikes.SeriesBase = objSeries;
                    objVM.PopularSeriesBikes.WidgetTitle = string.Format("Popular {0} Bikes", objVM.objSeries.SeriesName);
                    objVM.PopularSeriesBikes.WidgetViewAllUrl = UrlFormatter.BikeSeriesUrl(objMakeResponse.MaskingName, objSeries.MaskingName);
                    objVM.PopularSeriesBikes.CityName = CityName;
                    objVM.PopularSeriesBikes.PQSourceId = (int)(IsMobile ? Entities.PriceQuote.PQSourceEnum.Mobile_Videos_Page_PopularSeries : Entities.PriceQuote.PQSourceEnum.Desktop_Videos_Page_PopularSeries);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ModelWiseVideosPage.BindPopularSeriesBikes");
            }
        }

        private void BindPageMetasSeries(ModelWiseVideoPageVM objPageVM)
        {
            try
            {
                if (objPageVM != null && objPageVM.PageMetaTags != null && objPageVM.Make != null && objPageVM.objSeries != null)
                {
                    objPageVM.PageMetaTags.Title = String.Format("Videos of all {0} {1} bikes | See the latest {0} {1} videos - BikeWale", objPageVM.Make.MakeName, objPageVM.objSeries.SeriesName);
                    objPageVM.PageMetaTags.Keywords = string.Format("{0} {1} Videos, {1} Videos", objPageVM.Make.MakeName, objPageVM.objSeries.SeriesName);
                    objPageVM.PageMetaTags.Description = string.Format("Check latest {0} {1} videos, watch BikeWale expert's take on all {0} {1} bikes", objPageVM.Make.MakeName, objPageVM.objSeries.SeriesName);
                    objPageVM.PageMetaTags.CanonicalUrl = String.Format("https://www.bikewale.com/{0}-bikes/{1}/videos/", objPageVM.Make.MaskingName, objPageVM.objSeries.MaskingName);
                    if (!IsMobile)
                        objPageVM.PageMetaTags.AlternateUrl = String.Format("https://www.bikewale.com/m/{0}-bikes/{1}/videos/", objPageVM.Make.MaskingName, objPageVM.objSeries.MaskingName);
                    SetBreadcrumListSeries(objPageVM);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ModelWiseVideosPage.BindPageMetasSeries");
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

                GetBodyStyle(objVM);

                BindPageMetas(objVM);
                if (objVM.bikeType.Equals(EnumBikeBodyStyles.Scooter))
                {
                    BindMoreAboutScootersWidget(objVM);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ModelWiseVideosPage.GetData");
            }
            return objVM;
        }

        private void BindPageMetas(ModelWiseVideoPageVM objPageVM)
        {
            try
            {
                if (objPageVM != null && objPageVM.PageMetaTags != null && objPageVM.Make != null && objPageVM.Model != null)
                {

                    if (BWConfiguration.Instance.MetasMakeId.Split(',').Contains(objPageVM.Make.MakeId.ToString()))
                    {
                        objPageVM.PageMetaTags.Title = String.Format("Videos of {0} {1} | Videos From Experts on {1}- BikeWale", objPageVM.Make.MakeName, objPageVM.Model.ModelName);
                    }
                    else
                    {
                        objPageVM.PageMetaTags.Title = String.Format("{0} {1} Videos - BikeWale", objPageVM.Make.MakeName, objPageVM.Model.ModelName);
                    }
                    objPageVM.PageMetaTags.Keywords = string.Format("{0},{1},{0} {1},{0} {1} videos", objPageVM.Make.MakeName, objPageVM.Model.ModelName);
                    objPageVM.PageMetaTags.Description = string.Format("Check latest {0} {1} videos, watch BikeWale expert's take on {0} {1} - features, performance, price, fuel economy, handling and more.", objPageVM.Make.MakeName, objPageVM.Model.ModelName);

                    objPageVM.PageMetaTags.CanonicalUrl = String.Format("https://www.bikewale.com/{0}-bikes/{1}/videos/", objPageVM.Make.MaskingName, objPageVM.Model.MaskingName);
                    if (!IsMobile)
                        objPageVM.PageMetaTags.AlternateUrl = String.Format("https://www.bikewale.com/m/{0}-bikes/{1}/videos/", objPageVM.Make.MaskingName, objPageVM.Model.MaskingName);
                    if (objPageVM.Model != null)
                    {
                        objPageVM.objSeries = _models.GetSeriesByModelId(_modelId);
                    }
                    SetBreadcrumList(objPageVM);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ModelWiseVideosPage.BindPageMetas");
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
                }
                else if (objMakeResponse.StatusCode == 301)
                {
                    newMakeMasking = objMakeResponse.MaskingName;
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
                objMaskingResponse = _seriesCache.ProcessMaskingName(String.Format("{0}_{1}", makeMaskingName, modelMaskingName));
                objModelResponse = _bikeModelsCache.GetModelMaskingResponse(string.Format("{0}_{1}", makeMaskingName, modelMaskingName));
                if (objMaskingResponse != null)
                {
                    if (objMaskingResponse.StatusCode == 200)
                    {
                        if (!objMaskingResponse.IsSeriesPageCreated)
                        {


                            _modelId = objMaskingResponse.ModelId;

                        }
                        else
                        {

                            objSeries = new BikeSeriesEntityBase
                            {
                                SeriesId = objMaskingResponse.SeriesId,
                                SeriesName = objMaskingResponse.Name,
                                MaskingName = objMaskingResponse.MaskingName,
                                IsSeriesPageUrl = true
                            };
                        }
                    }
                    else if (objModelResponse.StatusCode == 301)
                    {
                        newModelMasking = objModelResponse.MaskingName;
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
                    bikeUrl = string.Format("{0}{1}/", bikeUrl, objVM.objSeries.MaskingName);
                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, bikeUrl, objVM.objSeries.SeriesName));

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
                string bikeUrl, seriesUrl, scooterUrl;
                scooterUrl = bikeUrl = string.Format("{0}/", Utility.BWConfiguration.Instance.BwHostUrl);
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

                if (objVM.Model != null && objVM.bikeType.Equals(EnumBikeBodyStyles.Scooter))
                {
                    if (IsMobile)
                    {
                        scooterUrl += "m/";
                    }
                    scooterUrl += string.Format("{0}-scooters/", objVM.Make.MaskingName);
                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, scooterUrl, string.Format("{0} Scooters", objVM.Make.MakeName)));
                }

                if (objVM.objSeries != null)
                {
                    seriesUrl = string.Format("{0}{1}/", bikeUrl, objVM.objSeries.MaskingName);
                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, seriesUrl, objVM.objSeries.SeriesName));

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
                Bikewale.Notifications.ErrorClass.LogError(ex, "ModelWiseVideosPage.SetBreadcrumList");
            }

        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 30th Nov 2017
        /// Summary : get body style for given model
        /// </summary>
        /// <param name="objVM"></param>
        private void GetBodyStyle(ModelWiseVideoPageVM objVM)
        {
            try
            {
                if (_modelId > 0)
                {
                    EnumBikeBodyStyles bodyStyle = EnumBikeBodyStyles.AllBikes;

                    List<BikeVersionMinSpecs> objVersionsList = _objBikeVersionsCache.GetVersionMinSpecs(_modelId, false);

                    if (objVersionsList != null && objVersionsList.Count > 0)
                    {
                        bodyStyle = objVersionsList.FirstOrDefault().BodyStyle;
                    }

                    if (bodyStyle.Equals(EnumBikeBodyStyles.Scooter))
                    {
                        objVM.bikeType = bodyStyle;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("ModelWiseVideosPage.GetBodyStyle model id = {0}", _modelId));
            }
        }
        /// <summary>
        /// Created By: Snehal Dange on 21th Dec 2017
        /// Summary : Bind more about scooter widget
        /// </summary>
        /// <param name="objData"></param>
        private void BindMoreAboutScootersWidget(ModelWiseVideoPageVM objData)
        {
            try
            {
                MoreAboutScootersWidget obj = new MoreAboutScootersWidget(_objModelCache, _cityCacheRepo, _objBikeVersionsCache, _bikeInfo, Entities.GenericBikes.BikeInfoTabType.Videos);
                obj.modelId = _modelId;
                objData.ObjMoreAboutScooter = obj.GetData();
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("ModelWiseVideosPage.BindMoreAboutScootersWidget : ModelId {0}", _modelId));
            }
        }
    }
}