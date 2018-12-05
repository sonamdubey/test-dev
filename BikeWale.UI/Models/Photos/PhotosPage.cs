﻿using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.GenericBikes;
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
    /// <summary>
    /// Created by  : Sushil Kumar on 30th Sep 2017
    /// Description :  To bind photo gallery page
    /// </summary>
    public class PhotosPage
    {
        private readonly IBikeModelsCacheRepository<int> _objModelCache = null;
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _objModelMaskingCache = null;
        private readonly IBikeModels<BikeModelEntity, int> _objModelEntity = null;
        private readonly ICityCacheRepository _objCityCache = null;
        private readonly IBikeInfo _objGenericBike = null;
        private readonly IBikeVersions<BikeVersionEntity, uint> _objVersion;
        private readonly IVideos _objVideos = null;
        private readonly IBikeSeries _bikeSeries = null;

        private uint _modelId, _cityId;
        private string _cityName;
        private PhotosPageVM _objData = null;
        private string _returnUrl;
        private uint _selectedColorImageId;
        private uint _selectedImageId;

        public StatusCodes Status { get; internal set; }
        public string RedirectUrl { get; internal set; }
        public bool IsMobile { get; set; }

        private readonly String _adPath_Mobile = "/1017752/Bikewale_Mobile_Image";
        private readonly String _adId_Mobile = "1516082576550";
        private readonly String _adPath_Desktop = "/1017752/Bikewale_Image";
        private readonly String _adId_Desktop = "1516082384256";

        /// <summary>
        /// Created by  : Sushil Kumar on 30th Sep 2017
        /// Description :  To resolve depedencies for photo gallery page
        /// </summary>
        /// <param name="makeMaskingName"></param>
        /// <param name="modelMaskingName"></param>
        /// <param name="objModelCache"></param>
        /// <param name="objModelMaskingCache"></param>
        /// <param name="objModelEntity"></param>
        /// <param name="objCityCache"></param>
        /// <param name="objGenericBike"></param>
        /// <param name="objVersionCache"></param>
        /// <param name="objVideos"></param>
        public PhotosPage(string makeMaskingName, string modelMaskingName, IBikeModelsCacheRepository<int> objModelCache,
            IBikeMaskingCacheRepository<BikeModelEntity, int> objModelMaskingCache, IBikeModels<BikeModelEntity, int> objModelEntity, ICityCacheRepository objCityCache,
            IBikeInfo objGenericBike, IBikeVersions<BikeVersionEntity, uint> objVersion, IVideos objVideos, IBikeSeries bikeSeries)
        {
            _objModelCache = objModelCache;
            _objModelMaskingCache = objModelMaskingCache;
            _objModelEntity = objModelEntity;
            _objCityCache = objCityCache;
            _objGenericBike = objGenericBike;
            _objVersion = objVersion;
            _objVideos = objVideos;
            _bikeSeries = bikeSeries;
            ParseQueryString(makeMaskingName, modelMaskingName);
        }

        /// <summary>
        /// Created by  : Sushil Kumar on 30th Sep 2017
        /// Description :  To bind photo gallery page
        /// Modified by : Vivek Singh Tomar on 28th Nov 2017
        /// Description : Get series entity to create bread crumb list
        /// Modified by : Snehal Dange on 20th Dec 2017
        /// Description: Added BindMoreAboutScootersWidget
        /// Modified by : Snehal Dange on 29th Nov 2017
        /// Descritpion : Added ga for page
        /// Modified by : Snehal Dange on 20th March 2018
        /// Description: Added BindVideosAndColourImages()
        /// </summary>
        /// <param name="gridSize"></param>
        /// <param name="noOfGrid"></param>
        /// <param name="qstr"></param>
        /// <returns></returns>
        public PhotosPageVM GetData(uint gridSize, uint noOfGrid, string qstr)
        {
            _objData = new PhotosPageVM();

            try
            {

                _objData.GridSize = gridSize;
                _objData.NoOfGrid = noOfGrid;
                _objData.BodyStyle = 0;
                GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
                _cityId = currentCityArea.CityId;
                _cityName = currentCityArea.City;
                ProcessQueryStringVariables(qstr);
                _objData.Series = _objModelEntity.GetSeriesByModelId(_modelId);
                BindPhotos();
                BindPageWidgets();
                SetPageMetas();
                BindVideosAndColourImages(_objData);
                if (_objData.BodyStyle.Equals((sbyte)EnumBikeBodyStyles.Scooter))
                {
                    BindMoreAboutScootersWidget(_objData);

                }
                BindAdSlots(_objData);
                
                _objData.Page = Entities.Pages.GAPages.Model_Images_Page;



            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("Bikewale.Models.Photos.PhotosPage.GetData : GetData({0})", _modelId));
            }

            return _objData;
        }

        /// <summary>
        /// Created by  : Sushil Kumar on 30th Sep 2017
        /// Description : To bind photo gallery images and other details related to it
        /// </summary>
        private void BindPhotos()
        {
            try
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
                        _objData.ModelImage = _objData.PhotoGallery.ImageList.First();

                        _objData.TotalPhotos = (uint)_objData.ModelImages.Count();
                        _objData.NonGridPhotoCount = (_objData.TotalPhotos % _objData.NoOfGrid);
                        _objData.GridPhotoCount = _objData.TotalPhotos - _objData.NonGridPhotoCount;


                    }

                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("Bikewale.Models.Photos.PhotosPage.BindPhotos : BindPhotos({0})", _modelId));
            }
        }

        /// <summary>
        /// Created by  : Sushil Kumar on 30th Sep 2017
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
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("Bikewale.Models.Photos.PhotosPage.ProcessQueryStringVariables : ProcessQueryStringVariables({0})", _modelId));
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
                    _objData.BikeInfo = (new BikeInfoWidget(_objGenericBike, _objCityCache, _modelId, _cityId, 4, Entities.GenericBikes.BikeInfoTabType.Image, _objModelEntity, _bikeSeries)).GetData();
                    if (IsMobile)
                        _objData.Videos = new RecentVideos(1, 2, (uint)_objData.Make.MakeId, _objData.Make.MakeName, _objData.Make.MaskingName, (uint)_objData.Model.ModelId, _objData.Model.ModelName, _objData.Model.MaskingName, _objVideos).GetData();
                    else
                        _objData.Videos = new RecentVideos(1, 4, (uint)_objData.Make.MakeId, _objData.Make.MakeName, _objData.Make.MaskingName, (uint)_objData.Model.ModelId, _objData.Model.ModelName, _objData.Model.MaskingName, _objVideos).GetData();

                    if (_objData.PhotoGallery != null && _objData.PhotoGallery.ImageList != null)
                    {
                        var modelgallery = new ModelGalleryWidget(_objData.Make, _objData.Model, _objData.PhotoGallery.ImageList, _objData.ModelVideos, _objData.BikeInfo);
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

                    var similarBikes = new SimilarBikesWithPhotosWidget(_objModelMaskingCache, _modelId, _cityId);
                    similarBikes.BikeName = _objData.BikeName;
                    _objData.SimilarBikes = similarBikes.GetData();
                    _objData.SimilarBikes.City = _cityName;

                    if (_objData.SimilarBikes != null && _objData.SimilarBikes.Bikes != null && _objData.SimilarBikes.Bikes.Any())
                    {
                        var firstModel = _objData.SimilarBikes.Bikes.First();
                        _objData.BodyStyle = firstModel.BodyStyle;
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("Bikewale.Models.Photos.PhotosPage.BindPageWidgets : BindPageWidgets({0})", _modelId));
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

                    if (BWConfiguration.Instance.MetasMakeId.Split(',').Contains(_objData.Make.MakeId.ToString()))
                    {
                        _objData.PageMetaTags.Title = String.Format(" Images of {0}| Photos of {0}- BikeWale", _objData.BikeName);
                        _objData.Page_H1 = string.Format("Images of {0}", _objData.BikeName);
                    }
                    else
                    {
                        _objData.PageMetaTags.Title = String.Format("{0} Images | {1} Photos - BikeWale", _objData.BikeName, _objData.Model.ModelName);
                        _objData.Page_H1 = string.Format("{0} Images", _objData.BikeName);
                    }

                    _objData.PageMetaTags.Keywords = string.Format("{0} photos, {0} pictures, {0} images, {1} {0} photos", _objData.Model.ModelName, _objData.Make.MakeName);
                    _objData.PageMetaTags.Description = string.Format("View images of {0} in different colours and angles. Check out {2} photos of {1} on BikeWale", _objData.Model.ModelName, _objData.BikeName, _objData.TotalPhotos);
                    _objData.PageMetaTags.CanonicalUrl = string.Format("{0}/{1}-bikes/{2}/images/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, _objData.Make.MaskingName, _objData.Model.MaskingName);
                    _objData.PageMetaTags.AlternateUrl = string.Format("{0}/m/{1}-bikes/{2}/images/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, _objData.Make.MaskingName, _objData.Model.MaskingName);


                    SetBreadcrumList();
                    SetPageJSONLDSchema(_objData.PageMetaTags);

                    _objData.AdTags.TargetedModel = _objData.Model.ModelName;
                    _objData.AdTags.TargetedMakes = _objData.Make.MakeName;

                    if (!String.IsNullOrEmpty(_cityName))
                    {
                        _objData.AdTags.TargetedCity = _cityName;
                    }

                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("Bikewale.Models.Photos.PhotosPage.SetPageMetas : SetPageMetas({0})", _modelId));
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
                WebPage webpage = SchemaHelper.GetWebpageSchema(objPageMeta, _objData.SchemaBreadcrumbList);

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
                        ContentUrl = Utility.Image.GetPathToShowImages(_objData.ModelImage.OriginalImgPath, _objData.ModelImage.HostUrl, ImageSize._1280x720),
                        Caption = _objData.ModelImage.ImageTitle
                    };

                    IList<ImageObject> objImages = new List<ImageObject>();

                    foreach (var image in _objData.PhotoGallery.ImageList)
                    {
                        objImages.Add(new ImageObject()
                        {
                            ThumbnailUrl = Utility.Image.GetPathToShowImages(image.OriginalImgPath, image.HostUrl, ImageSize._370x208),
                            ContentUrl = Utility.Image.GetPathToShowImages(image.OriginalImgPath, image.HostUrl, ImageSize._1280x720),
                            Caption = _objData.ModelImage.ImageTitle
                        });
                    }

                    gallery.Images = objImages;

                    _objData.PageMetaTags.PageSchemaJSON = SchemaHelper.JsonSerialize(gallery);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("Bikewale.Models.Photos.PhotosPage.SetPageJSONLDSchema : SetPageJSONLDSchema({0})", _modelId));
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 12th Sep 2017
        /// Description : Function to create page level schema for breadcrum
        /// Modified by : Snehal Dange on 28th Dec 2017
        /// Descritption : Added 'New Bikes' in Breadcrumb
        /// </summary>
        private void SetBreadcrumList()
        {
            IList<BreadcrumbListItem> BreadCrumbs = new List<BreadcrumbListItem>();
            string bikeUrl, scooterUrl, seriesUrl;
            bikeUrl = scooterUrl = seriesUrl = string.Format("{0}/", Utility.BWConfiguration.Instance.BwHostUrl);
            ushort position = 1;
            if (IsMobile)
            {
                bikeUrl += "m/";
            }

            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, "Home"));
            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, string.Format("{0}new-bikes-in-india/", bikeUrl), "New Bikes"));


            if (_objData.Make != null)
            {
                bikeUrl = string.Format("{0}{1}-bikes/", bikeUrl, _objData.Make.MaskingName);

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, string.Format("{0} Bikes", _objData.Make.MakeName)));
            }

            if (_objData.Model != null && _objData.BodyStyle.Equals((sbyte)EnumBikeBodyStyles.Scooter) && !(_objData.Make.IsScooterOnly))
            {
                if (IsMobile)
                {
                    scooterUrl += "m/";
                }
                scooterUrl = string.Format("{0}{1}-scooters/", scooterUrl, _objData.Make.MaskingName);

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, scooterUrl, string.Format("{0} Scooters", _objData.Make.MakeName)));
            }

            if (_objData.Series != null && _objData.Series.IsSeriesPageUrl)
            {
                if (IsMobile)
                {
                    seriesUrl += "m/";
                }

                seriesUrl = string.Format("{0}{1}-bikes/{2}/", seriesUrl, _objData.Make.MaskingName, _objData.Series.MaskingName);

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, seriesUrl, _objData.Series.SeriesName));
            }

            if (_objData.Model != null)
            {
                bikeUrl = string.Format("{0}{1}/", bikeUrl, _objData.Model.MaskingName);

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, _objData.BikeName));
            }

            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null, "Images"));

            _objData.BreadcrumbList.BreadcrumListItem = BreadCrumbs;
            _objData.SchemaBreadcrumbList.BreadcrumListItem = BreadCrumbs.Take(BreadCrumbs.Count - 1);

        }


        /// <summary>
        /// Created By:- Sushil Kumar on 29-Mar-2017 
        /// Summary:- Process the input query
        /// </summary>
        /// <param name="modelMasking"></param>
        private void ParseQueryString(string makeMasking, string modelMasking)
        {
            ModelMaskingResponse objResponse = null;
            string newMakeMasking = string.Empty;
            bool isMakeRedirection = false;
            try
            {
                newMakeMasking = ProcessMakeMaskingName(makeMasking, out isMakeRedirection);

                if (!string.IsNullOrEmpty(newMakeMasking) && !string.IsNullOrEmpty(makeMasking) && !string.IsNullOrEmpty(modelMasking))
                {
                    objResponse = _objModelMaskingCache.GetModelMaskingResponse(string.Format("{0}_{1}", makeMasking, modelMasking));

                    if (objResponse != null)
                    {
                        if (objResponse.StatusCode == 200)
                        {
                            _modelId = objResponse.ModelId;
                            Status = StatusCodes.ContentFound;
                        }
                        else if (objResponse.StatusCode == 301 || isMakeRedirection)
                        {
                            RedirectUrl = HttpContext.Current.Request.RawUrl.Replace(makeMasking, newMakeMasking).Replace(modelMasking, objResponse.MaskingName);
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
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("Bikewale.Models.Photos.PhotosPage.GetData : ParseQueryString({0})", _modelId));
                Status = StatusCodes.ContentNotFound;
            }
        }


        /// <summary>
        /// Created By: Snehal Dange on 20th Dec 2017
        /// Summary : Bind more about scooter widget
        /// </summary>
        /// <param name="objData"></param>
        private void BindMoreAboutScootersWidget(PhotosPageVM objData)
        {
            try
            {
                MoreAboutScootersWidget obj = new MoreAboutScootersWidget(_objModelCache, _objCityCache, _objVersion, _objGenericBike, Entities.GenericBikes.BikeInfoTabType.Image, _objModelEntity, _bikeSeries);
                obj.modelId = _modelId;
                _objData.objMoreAboutScooter = obj.GetData();
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("Bikewale.Models.Photos.PhotosPage.BindMoreAboutScootersWidget : ModelId {0}", _modelId));
            }
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 11th Dec 2017
        /// Description : Process make masking name for redirection
        /// </summary>
        /// <param name="make"></param>
        /// <param name="isMakeRedirection"></param>
        /// <returns></returns>
        private string ProcessMakeMaskingName(string make, out bool isMakeRedirection)
        {
            MakeMaskingResponse makeResponse = null;
            Common.MakeHelper makeHelper = new Common.MakeHelper();
            isMakeRedirection = false;
            if (!string.IsNullOrEmpty(make))
            {
                makeResponse = makeHelper.GetMakeByMaskingName(make);
            }
            if (makeResponse != null)
            {
                if (makeResponse.StatusCode == 200)
                {
                    return makeResponse.MaskingName;
                }
                else if (makeResponse.StatusCode == 301)
                {
                    isMakeRedirection = true;
                    return makeResponse.MaskingName;
                }
                else
                {
                    return "";
                }
            }

            return "";
        }

        /// <summary>
        /// Created by : Snehal Dange on 19th March 2018
        /// Description: Bind Videos and Colour tabs data
        /// </summary>
        /// <param name="objData"></param>
        private void BindVideosAndColourImages(PhotosPageVM objData)
        {
            try
            {
                if (objData != null && objData.ModelImages != null && objData.ModelImages.Any())
                {
                    IEnumerable<ColorImageBaseEntity> colorImages = null;
                    colorImages = objData.ModelImages.Where(m => m.ImageType.Equals(ImageBaseType.ModelColorImage));
                    if (colorImages != null && colorImages.Any())
                    {
                        objData.ColorImagesCount = colorImages.Count();
                    }
                    objData.IsColorAvailable = (objData.ColorImagesCount > 0);
                    objData.IsVideosAvailable = (objData.ModelVideos != null && objData.ModelVideos.Any());
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("Bikewale.Models.Photos.PhotosPage.BindVideosAndColourImages : ModelId {0}", _modelId));
            }
        }

        /// <summary>
        /// Created By : Deepak Israni on 22 May 2018
        /// Description: To bind ad slots for lazy loading implementation on page.
        /// Modified by: Dhruv Joshi
        /// Dated: 27th September 2018
        /// Description: Adslots for desktop
        /// </summary>
        /// <param name="_objData"></param>
        private void BindAdSlots(PhotosPageVM _objData)
        {
            NameValueCollection adInfo = new NameValueCollection();
            AdTags adTagsObj = _objData.AdTags;
            if (IsMobile)
            {
                adTagsObj.AdId = _adId_Mobile;
                adTagsObj.AdPath = _adPath_Mobile;
                adTagsObj.Ad_320x50Top = true;
                adTagsObj.Ad_300x250BTF = true;
                adInfo["adId"] = _adId_Mobile;
                adInfo["adPath"] = _adPath_Mobile;
            }
            else
            {
                adTagsObj.AdId = _adId_Desktop;
                adTagsObj.AdPath = _adPath_Desktop;
                adTagsObj.Ad_970x90Body = true;                
                adTagsObj.Ad_970x90Bottom = true;
                adInfo["adId"] = _adId_Desktop;
                adInfo["adPath"] = _adPath_Desktop;
            }
            IDictionary<string, AdSlotModel> ads = new Dictionary<string, AdSlotModel>();

            if (adTagsObj.Ad_320x50Top)
            {
                ads.Add(String.Format("{0}-0", _adId_Mobile), GoogleAdsHelper.SetAdSlotProperties(adInfo, ViewSlotSize.ViewSlotSizes[AdSlotSize._320x50], 0, 320, AdSlotSize._320x50, "Top", true));
            }
            if (adTagsObj.Ad_300x250BTF)
            {
                ads.Add(String.Format("{0}-14", _adId_Mobile), GoogleAdsHelper.SetAdSlotProperties(adInfo, ViewSlotSize.ViewSlotSizes[AdSlotSize._300x250], 14, 300, AdSlotSize._300x250, "BTF"));
            }
            if (adTagsObj.Ad_970x90Body)
            {
                ads.Add(string.Format("{0}-19", _adId_Desktop), GoogleAdsHelper.SetAdSlotProperties(adInfo, ViewSlotSize.ViewSlotSizes[AdSlotSize._970x90 + "_C"], 19, 970, AdSlotSize._970x90, "Top", true));
            }
            if (adTagsObj.Ad_970x90Bottom)
            {
                ads.Add(String.Format("{0}-5", _adId_Desktop), GoogleAdsHelper.SetAdSlotProperties(adInfo, ViewSlotSize.ViewSlotSizes[AdSlotSize._970x90 + "_C"], 5, 970, AdSlotSize._970x90, "Bottom"));
            }
            _objData.AdSlots = ads;
        }

    }
}