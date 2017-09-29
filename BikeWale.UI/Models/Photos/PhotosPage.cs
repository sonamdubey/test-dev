﻿using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Videos;
using Bikewale.Models.Gallery;
using Bikewale.Notifications;
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
        private PhotosPageVM _objData = null;
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
            _objData = new PhotosPageVM();
            _objData.GridSize = gridSize;
            _objData.NoOfGrid = noOfGrid;
            GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
            _cityId = currentCityArea.CityId;
            ProcessQueryStringVariables(qstr);

            BindPhotos();
            SetPageMetas();
            BindPageWidgets();

            return _objData;
        }

        private void BindPhotos()
        {
            _objData.PhotoGallery = _objModelEntity.GetPhotoGalleryData(Convert.ToInt32(_modelId));

            if (_objData.PhotoGallery != null)
            {

                if (_objData.PhotoGallery.ObjModelEntity != null)
                {
                    _objData.objModel = _objData.PhotoGallery.ObjModelEntity;
                    _objData.Make = _objData.objModel.MakeBase;
                    _objData.Model = new BikeModelEntityBase()
                    {
                        ModelId = _objData.objModel.ModelId,
                        ModelName = _objData.objModel.ModelName,
                        MaskingName = _objData.objModel.MaskingName
                    };

                    _objData.BikeName = string.Format("{0} {1}", _objData.Make.MakeName, _objData.Model.ModelName);
                }

                _objData.ModelImages = _objData.PhotoGallery.ImageList;
                _objData.ModelVideos = _objData.PhotoGallery.VideosList;

                if (_objData.ModelImages != null)
                {
                    //modelImage = Utility.Image.GetPathToShowImages(objImageList[0].OriginalImgPath, objImageList[0].HostUrl, Bikewale.Utility.ImageSize._476x268);
                    _objData.ModelImage = _objData.PhotoGallery.ImageList.First();

                    if (!IsMobile)
                    {
                        _objData.ModelImages = _objData.ModelImages.Skip(1);
                    }
                    _objData.TotalPhotos = (uint)_objData.ModelImages.Count();
                    _objData.NonGridPhotoCount = (_objData.TotalPhotos % _objData.NoOfGrid);
                    _objData.GridPhotoCount = _objData.TotalPhotos - _objData.NonGridPhotoCount;


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

                    _objData.IsPopupOpen = !string.IsNullOrEmpty(_returnUrl);
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
                if (_modelId > 0 && _objData.Make != null && _objData.Model != null)
                {
                    _objData.BikeInfo = (new BikeInfoWidget(_objGenericBike, _objCityCache, _modelId, _cityId, 4, Entities.GenericBikes.BikeInfoTabType.Image)).GetData();

                    _objData.Videos = new RecentVideos(1, 3, (uint)_objData.Make.MakeId, _objData.Make.MakeName, _objData.Make.MaskingName, (uint)_objData.Model.ModelId, _objData.Model.ModelName, _objData.Model.MaskingName, _objVideos).GetData();

                    var similarBikes = new SimilarBikesWithPhotosWidget(_objModelMaskingCache, _modelId, _cityId);
                    similarBikes.BikeName = _objData.BikeName;
                    _objData.SimilarBikes = similarBikes.GetData();

                    var modelgallery = new ModelGalleryWidget(_objData.Make, _objData.Model, _objData.ModelImages, _objData.ModelVideos, _objData.BikeInfo);
                    modelgallery.IsGalleryDataAvailable = true;
                    modelgallery.IsJSONRequired = true;
                    _objData.ModelGallery = modelgallery.GetData();
                    if (_objData.ModelGallery != null)
                    {
                        _objData.ModelGallery.ReturnUrl = _returnUrl;
                        _objData.ModelGallery.SelectedColorImageId = _selectedColorImageId;
                        _objData.ModelGallery.SelectedImageId = _selectedImageId;
                        _objData.ModelGallery.BikeName = _objData.BikeName;
                        _objData.ModelGallery.IsDiscontinued = !_objData.objModel.Futuristic && !_objData.objModel.New;
                        _objData.ModelGallery.IsUpcoming = _objData.objModel.Futuristic;
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
                if (_objData.Make != null && _objData.Model != null)
                {
                    _objData.PageMetaTags.Title = String.Format("{0} Images | {1} Photos - BikeWale", _objData.BikeName, _objData.Model.ModelName);
                    _objData.PageMetaTags.Keywords = string.Format("{0} photos, {0} pictures, {0} images, {1} {0} photos", _objData.Model.ModelName, _objData.Make.MakeName);
                    _objData.PageMetaTags.Description = string.Format("View images of {0} in different colours and angles. Check out {2} photos of {1} on BikeWale", _objData.Model.ModelName, _objData.BikeName, _objData.TotalPhotos);
                    _objData.PageMetaTags.CanonicalUrl = string.Format("{0}/{1}-bikes/{2}/images/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, _objData.Make.MaskingName, _objData.Model.MaskingName);
                    _objData.PageMetaTags.AlternateUrl = string.Format("{0}/m/{1}-bikes/{2}/images/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, _objData.Make.MaskingName, _objData.Model.MaskingName);
                    _objData.Page_H1 = string.Format("{0} Images", _objData.BikeName);

                    SetBreadcrumList();
                    SetPageJSONLDSchema(_objData.PageMetaTags);

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
            try
            {
                //set webpage schema for the model page
                WebPage webpage = SchemaHelper.GetWebpageSchema(objPageMeta, _objData.BreadcrumbList);

                if (webpage != null)
                {
                    _objData.PageMetaTags.SchemaJSON = SchemaHelper.JsonSerialize(webpage);
                }

                if (_objData.PhotoGallery.ImageList != null && _objData.PhotoGallery.ImageList.Any())
                {
                    ImageGallery gallery = new ImageGallery();

                    gallery.Description = objPageMeta.Description;
                    gallery.Name = objPageMeta.Title;
                    gallery.Headline = objPageMeta.Title;

                    gallery.PrimaryImageOfPage = new ImageObject()
                    {
                        ThumbnailUrl = Utility.Image.GetPathToShowImages(_objData.ModelImage.OriginalImgPath, _objData.ModelImage.HostUrl, ImageSize._370x208),
                        ContentUrl = Utility.Image.GetPathToShowImages(_objData.ModelImage.OriginalImgPath, _objData.ModelImage.HostUrl, "0x0"),
                        Caption = _objData.ModelImage.ImageTitle
                    };

                    IList<ImageObject> objImages = new List<ImageObject>();

                    foreach (var image in _objData.PhotoGallery.ImageList)
                    {
                        objImages.Add(new ImageObject()
                        {
                            ThumbnailUrl = Utility.Image.GetPathToShowImages(image.OriginalImgPath, image.HostUrl, ImageSize._370x208),
                            ContentUrl = Utility.Image.GetPathToShowImages(image.OriginalImgPath, image.HostUrl, "0x0"),
                            Caption = _objData.ModelImage.ImageTitle
                        });
                    }

                    gallery.Images = objImages;

                    _objData.PageMetaTags.PageSchemaJSON = SchemaHelper.JsonSerialize(gallery);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.PhotosPage.SetPageJSONLDSchema => BikeName: {0}", _objData.BikeName));
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


            if (_objData.Make != null)
            {
                url = string.Format("{0}{1}-bikes/", url, _objData.Make.MaskingName);

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, string.Format("{0} Bikes", _objData.Make.MakeName)));
            }

            if (_objData.Model != null)
            {
                url = string.Format("{0}{1}/", url, _objData.Model.MaskingName);

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, _objData.BikeName));
            }

            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, null, "Images"));

            _objData.BreadcrumbList.BreadcrumListItem = BreadCrumbs;

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