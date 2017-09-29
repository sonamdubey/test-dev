using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Videos;
using Bikewale.Models.Gallery;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Bikewale.Models.Photos
{
    public class PhotosPage
    {
        private readonly IBikeModelsCacheRepository<int> _objModelCache = null;
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _objModelMaskingCache = null;
        private readonly IBikeModels<BikeModelEntity, int> _objModelEntity = null;
        private readonly ICityCacheRepository _objCityCache = null;
        private readonly IBikeInfo _objGenericBike = null;
        private readonly IBikeVersionCacheRepository<BikeVersionEntity, uint> _objVersionCache = null;
        private readonly IVideos _objVideos = null;

        private uint _modelId, _cityId;
        private PhotosPageVM objData = null;
        private string _returnUrl;
        private uint _selectedColorImageId;
        private uint _selectedImageId;

        public StatusCodes Status { get; internal set; }
        public string RedirectUrl { get; internal set; }
        public bool IsMobile { get; set; }

        public PhotosPage(string makeMaskingName, string modelMaskingName, IBikeModelsCacheRepository<int> objModelCache, IBikeMaskingCacheRepository<BikeModelEntity, int> objModelMaskingCache, IBikeModels<BikeModelEntity, int> objModelEntity, ICityCacheRepository objCityCache, IBikeInfo objGenericBike, IBikeVersionCacheRepository<BikeVersionEntity, uint> objVersionCache, IVideos objVideos)
        {
            _objModelCache = objModelCache;
            _objModelMaskingCache = objModelMaskingCache;
            _objModelEntity = objModelEntity;
            _objCityCache = objCityCache;
            _objGenericBike = objGenericBike;
            _objVersionCache = objVersionCache;
            _objVideos = objVideos;
            ParseQueryString(modelMaskingName);
        }

        public PhotosPageVM GetData(uint gridSize, uint noOfGrid, string qstr)
        {
            objData = new PhotosPageVM();
            objData.GridSize = gridSize;
            objData.NoOfGrid = noOfGrid;
            GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
            _cityId = currentCityArea.CityId;
            ProcessQueryStringVariables(qstr);

            BindPhotos();
            SetPageMetas();
            BindPageWidgets();

            return objData;
        }

        private void BindPhotos()
        {
            objData.PhotoGallery = _objModelEntity.GetPhotoGalleryData(Convert.ToInt32(_modelId));

            if (objData.PhotoGallery != null)
            {

                if (objData.PhotoGallery.ObjModelEntity != null)
                {
                    objData.objModel = objData.PhotoGallery.ObjModelEntity;
                    objData.Make = objData.objModel.MakeBase;
                    objData.Model = new BikeModelEntityBase()
                    {
                        ModelId = objData.objModel.ModelId,
                        ModelName = objData.objModel.ModelName,
                        MaskingName = objData.objModel.MaskingName
                    };

                    objData.BikeName = string.Format("{0} {1}", objData.Make.MakeName, objData.Model.ModelName);
                }

                objData.ModelImages = objData.PhotoGallery.ImageList;
                objData.ModelVideos = objData.PhotoGallery.VideosList;

                if (objData.ModelImages != null)
                {
                    //modelImage = Utility.Image.GetPathToShowImages(objImageList[0].OriginalImgPath, objImageList[0].HostUrl, Bikewale.Utility.ImageSize._476x268);
                    objData.ModelImage = objData.PhotoGallery.ImageList.First();

                    if (!IsMobile)
                    {
                        objData.ModelImages = objData.ModelImages.Skip(1);
                    }
                    objData.TotalPhotos = (uint)objData.ModelImages.Count();
                    objData.NonGridPhotoCount = (objData.TotalPhotos % objData.NoOfGrid);
                    objData.GridPhotoCount = objData.TotalPhotos - objData.NonGridPhotoCount;


                }

            }
        }

        /// <summary>
        /// Created by Sajal Gupta on 27-04-2017
        /// Description : Function to get query string variables
        /// </summary>
        private void ProcessQueryStringVariables(string qstr)
        {
            try
            {
                uint imageIndex, colorImageId;
                if (!String.IsNullOrEmpty(qstr))
                {
                    string queryString = EncodingDecodingHelper.DecodeFrom64(qstr);

                    NameValueCollection queryCollection = HttpUtility.ParseQueryString(queryString);
                    uint.TryParse(queryCollection["imageindex"], out imageIndex);
                    uint.TryParse(queryCollection["colorimageid"], out colorImageId);

                    _returnUrl = queryCollection["returl"];
                    _selectedColorImageId = colorImageId;
                    _selectedImageId = imageIndex;

                    objData.IsPopupOpen = !string.IsNullOrEmpty(_returnUrl);
                }


            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.New.Photos : ProcessQueryStringVariables");
            }
        }


        /// <summary>
        /// Created By : Sajal Gupta on 09-02-2017
        /// Description : bind photos page widgets
        /// Modified By :Snehal Dange on 07-09-2017
        /// Description : Added CityId and City ,SimilarMakeName,SimilarModelName
        /// </summary>
        private void BindPageWidgets()
        {
            PQSourceEnum pqSource = IsMobile ? PQSourceEnum.Mobile_Photos_Page : PQSourceEnum.Desktop_Photos_page;
            try
            {
                if (_modelId > 0 && objData.Make != null && objData.Model != null)
                {
                    objData.BikeInfo = (new BikeInfoWidget(_objGenericBike, _objCityCache, _modelId, _cityId, 4, Entities.GenericBikes.BikeInfoTabType.Image)).GetData();

                    objData.Videos = new RecentVideos(1, 3, (uint)objData.Make.MakeId, objData.Make.MakeName, objData.Make.MaskingName, (uint)objData.Model.ModelId, objData.Model.ModelName, objData.Model.MaskingName, _objVideos).GetData();

                    var similarBikes = new SimilarBikesWithPhotosWidget(_objModelMaskingCache, _modelId, _cityId);
                    similarBikes.BikeName = objData.BikeName;
                    objData.SimilarBikes = similarBikes.GetData();

                    var modelgallery = new ModelGalleryWidget(objData.Make, objData.Model, objData.ModelImages, objData.ModelVideos, objData.BikeInfo);
                    modelgallery.IsGalleryDataAvailable = true;
                    modelgallery.IsJSONRequired = true;
                    objData.ModelGallery = modelgallery.GetData();
                    if (objData.ModelGallery != null)
                    {
                        objData.ModelGallery.ReturnUrl = _returnUrl;
                        objData.ModelGallery.SelectedColorImageId = _selectedColorImageId;
                        objData.ModelGallery.SelectedImageId = _selectedImageId;
                        objData.ModelGallery.BikeName = objData.BikeName;
                        objData.ModelGallery.IsDiscontinued = !objData.objModel.Futuristic && !objData.objModel.New;
                        objData.ModelGallery.IsUpcoming = objData.objModel.Futuristic;
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, string.Format("Bikewale.New.Photos : BindPageWidgets for modelId {0}", _modelId));
            }
        }


        /// <summary>
        /// Created By : Sushil Kumar on 6th Jan 2017
        /// Description : Set mode photos page metas
        /// </summary>
        private void SetPageMetas()
        {
            try
            {
                if (objData.Make != null && objData.Model != null)
                {
                    objData.PageMetaTags.Title = String.Format("{0} Images | {1} Photos - BikeWale", objData.BikeName, objData.Model.ModelName);
                    objData.PageMetaTags.Keywords = string.Format("{0} photos, {0} pictures, {0} images, {1} {0} photos", objData.Model.ModelName, objData.Make.MakeName);
                    objData.PageMetaTags.Description = string.Format("View images of {0} in different colours and angles. Check out {2} photos of {1} on BikeWale", objData.Model.ModelName, objData.BikeName, objData.TotalPhotos);
                    objData.PageMetaTags.CanonicalUrl = string.Format("{0}/{1}-bikes/{2}/images/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, objData.Make.MaskingName, objData.Model.MaskingName);
                    objData.PageMetaTags.AlternateUrl = string.Format("{0}/m/{1}-bikes/{2}/images/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, objData.Make.MaskingName, objData.Model.MaskingName);
                    objData.Page_H1 = string.Format("{0} Images", objData.BikeName);

                    SetBreadcrumList();
                    SetPageJSONLDSchema(objData.PageMetaTags);

                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.BindViewModels.Webforms.BindModelPhotos : SetPageMetas");
            }

        }


        /// <summary>
        /// Created By  : Sushil Kumar on 14th Sep 2017
        /// Description : Added breadcrum and webpage schema
        /// </summary>
        private void SetPageJSONLDSchema(PageMetaTags objPageMeta)
        {
            //set webpage schema for the model page
            WebPage webpage = SchemaHelper.GetWebpageSchema(objPageMeta, objData.BreadcrumbList);

            if (webpage != null)
            {
                objData.PageMetaTags.SchemaJSON = SchemaHelper.JsonSerialize(webpage);
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 12th Sep 2017
        /// Description : Function to create page level schema for breadcrum
        /// </summary>
        private void SetBreadcrumList()
        {
            IList<BreadcrumbListItem> BreadCrumbs = new List<BreadcrumbListItem>();
            string url = string.Format("{0}/", Utility.BWConfiguration.Instance.BwHostUrl);
            ushort position = 1;
            if (IsMobile)
            {
                url += "m/";
            }

            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, "Home"));


            if (objData.Make != null)
            {
                url = string.Format("{0}{1}-bikes/", url, objData.Make.MaskingName);

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, string.Format("{0} Bikes", objData.Make.MakeName)));
            }

            if (objData.Model != null)
            {
                url = string.Format("{0}{1}/", url, objData.Model.MaskingName);

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, objData.Model.ModelName));
            }

            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, null, "Images"));

            objData.BreadcrumbList.BreadcrumListItem = BreadCrumbs;

        }


        /// <summary>
        /// Created By:- Sushil Kumar on 29-Mar-2017 
        /// Summary:- Process the input query
        /// </summary>
        /// <param name="modelMasking"></param>
        private void ParseQueryString(string modelMasking)
        {
            ModelMaskingResponse objResponse = null;
            try
            {
                if (!string.IsNullOrEmpty(modelMasking))
                {
                    objResponse = _objModelMaskingCache.GetModelMaskingResponse(modelMasking);

                    if (objResponse != null)
                    {
                        if (objResponse.StatusCode == 200)
                        {
                            _modelId = objResponse.ModelId;
                            Status = StatusCodes.ContentFound;
                        }
                        else if (objResponse.StatusCode == 301)
                        {
                            RedirectUrl = HttpContext.Current.Request.RawUrl.Replace(modelMasking, objResponse.MaskingName);
                            Status = StatusCodes.RedirectPermanent;
                        }
                        else
                        {
                            Status = StatusCodes.ContentNotFound;
                        }
                    }
                    else
                    {
                        Status = StatusCodes.ContentNotFound;
                    }
                }
                else
                {
                    Status = StatusCodes.ContentNotFound;
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, String.Format("ParseQueryString({0} - {1})", modelMasking, _modelId));
                Status = StatusCodes.ContentNotFound;
            }
        }

    }
}