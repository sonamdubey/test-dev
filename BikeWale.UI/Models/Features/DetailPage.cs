﻿using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Memcache;
using Bikewale.Models.BestBikes;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Linq;
using System.Web;
namespace Bikewale.Models.Features
{
    public class DetailPage
    {
        #region Variables for dependency injection and constructor
        private readonly ICMSCacheContent _cache = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly IBikeModelsCacheRepository<int> _models = null;
        private string _basicId;
        private EnumBikeType bikeType = EnumBikeType.All;
        #endregion

        #region Page level variables
        private uint basicId;
        public StatusCodes status;
        public string mappedCWId;
        public string redirectUrl;
        #endregion

        #region Constructor
        public DetailPage(ICMSCacheContent cache, IUpcoming upcoming, IBikeModels<BikeModelEntity, int> bikeModels, IBikeModelsCacheRepository<int> models, string basicId)
        {
            _cache = cache;
            _upcoming = upcoming;
            _bikeModels = bikeModels;
            _models = models;
            _basicId = basicId;
            ProcessQueryString();

        }
        #endregion

        #region Functions
        /// <summary>
        /// Created by : Subodh Jain  on 30 March 2017
        /// Summary    : Get all feature details
        /// </summary>
        public DetailFeatureVM GetData(int widgetTopCount)
        {
            DetailFeatureVM objDetailsVM = new DetailFeatureVM();
            try
            {

                objDetailsVM.objFeature = _cache.GetArticlesDetails(basicId);
                if (objDetailsVM.objFeature != null)
                {
                    status = Entities.StatusCodes.ContentFound;
                    GetTaggedBikeListByMake(objDetailsVM);
                    GetTaggedBikeListByModel(objDetailsVM);
                    BindPageMetas(objDetailsVM);
                    GetWidgetData(objDetailsVM, widgetTopCount);
                    PopulatePhotoGallery(objDetailsVM);
                }
                else
                {
                    status = StatusCodes.ContentNotFound;
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Bikewale.Models.Features.DetailPage.GetData");
            }
            return objDetailsVM;
        }
        /// <summary>
        /// Modified By Sajal Gupta on 25-04-20187
        /// Descrition : Call most popular bike widget by body type
        /// </summary>
        private void GetWidgetData(DetailFeatureVM objData, int topCount)
        {
            try
            {
                GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
                uint CityId = 0;
                if (currentCityArea != null)
                    CityId = currentCityArea.CityId;

                if (objData.objModel != null && objData.objModel.ModelId > 0)
                {
                    PopularBikesByBodyStyle objPopularStyle = new PopularBikesByBodyStyle(_models);
                    objPopularStyle.ModelId = (uint)objData.objModel.ModelId;
                    objPopularStyle.CityId = CityId;
                    objPopularStyle.TopCount = topCount;
                    objData.PopularBodyStyle = objPopularStyle.GetData();
                    if (objData.PopularBodyStyle != null)
                    {
                        objData.PopularBodyStyle.WidgetHeading = string.Format("Popular {0}", objData.PopularBodyStyle.BodyStyleText);
                        objData.PopularBodyStyle.WidgetLinkTitle = string.Format("Best {0} in India", objData.PopularBodyStyle.BodyStyleLinkTitle);
                        objData.PopularBodyStyle.WidgetHref = UrlFormatter.FormatGenericPageUrl(objData.PopularBodyStyle.BodyStyle);
                        bikeType = objData.PopularBodyStyle.BodyStyle == EnumBikeBodyStyles.Scooter ? EnumBikeType.Scooters : EnumBikeType.All;
                    }
                }
                else
                {
                    UpcomingBikesWidget objUpcomingBikes = new UpcomingBikesWidget(_upcoming);
                    objUpcomingBikes.Filters = new UpcomingBikesListInputEntity();
                    objUpcomingBikes.Filters.PageNo = 1;
                    objUpcomingBikes.Filters.PageSize = topCount;

                    if (objData.objMake != null && objData.objMake.MakeId > 0)
                    {
                        objUpcomingBikes.Filters.MakeId = objData.objMake.MakeId;
                    }
                    objUpcomingBikes.SortBy = EnumUpcomingBikesFilter.Default;
                    objData.UpcomingBikes = objUpcomingBikes.GetData();

                    if (objData.UpcomingBikes != null)
                    {
                        if (objData.objMake != null)
                        {
                            objData.UpcomingBikes.WidgetHeading = string.Format("Upcoming {0} bikes", objData.objMake.MakeName);
                            objData.UpcomingBikes.WidgetHref = string.Format("/{0}-bikes/upcoming/", objData.objMake.MaskingName);
                        }
                        else
                        {
                            objData.UpcomingBikes.WidgetHeading = "Upcoming bikes";
                            objData.UpcomingBikes.WidgetHref = "/upcoming-bikes/";
                        }
                        objData.UpcomingBikes.WidgetLinkTitle = "Upcoming Bikes in India";
                    }

                }

                MostPopularBikesWidget objPopularBikes = new MostPopularBikesWidget(_bikeModels, bikeType, false, false, 0, 0, (uint)objData.objMake.MakeId);
                objPopularBikes.TopCount = topCount;
                objPopularBikes.CityId = CityId;
                objData.MostPopularBikes = objPopularBikes.GetData();

                MostPopularBikeWidgetVM PopularBikesWidget = objData.MostPopularBikes;

                if (PopularBikesWidget != null)
                {
                    if (objData.objMake != null && objData.objMake.MakeId > 0)
                    {
                        if (bikeType.Equals(EnumBikeType.Scooters))
                        {
                            PopularBikesWidget.WidgetHeading = string.Format("Popular {0} scooters", objData.objMake.MakeName);
                            if (objData.objMake.IsScooterOnly)
                                PopularBikesWidget.WidgetHref = string.Format("/{0}-bikes/", objData.objMake.MaskingName);
                            else
                                PopularBikesWidget.WidgetHref = string.Format("/{0}-scooters/", objData.objMake.MaskingName);
                            PopularBikesWidget.WidgetLinkTitle = string.Format("{0} Scooters", objData.objMake.MakeName);
                            PopularBikesWidget.CtaText = "View all scooters";
                        }
                        else
                        {
                            PopularBikesWidget.WidgetHeading = string.Format("Popular {0} bikes", objData.objMake.MakeName);
                            PopularBikesWidget.WidgetHref = string.Format("/{0}-bikes/", objData.objMake.MaskingName);
                            PopularBikesWidget.WidgetLinkTitle = string.Format("{0} Bikes", objData.objMake.MakeName);
                            PopularBikesWidget.CtaText = "View all bikes";
                        }
                    }
                    else
                    {
                        PopularBikesWidget.WidgetHeading = "Popular bikes";
                        PopularBikesWidget.WidgetHref = "/best-bikes-in-india/";
                        PopularBikesWidget.WidgetLinkTitle = "Best Bikes in India";
                        PopularBikesWidget.CtaText = "View all bikes";
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.Features.DetailPage.GetWidgetData");
            }
        }
        /// <summary>
        /// Created By:- Subodh Jain 23 March 2017
        /// Summary:- Fetching Meta Tags
        /// </summary>
        /// <returns></returns>
        private void BindPageMetas(DetailFeatureVM objPage)
        {
            try
            {
                objPage.PageMetaTags.CanonicalUrl = string.Format("{0}/features/{1}-{2}/", BWConfiguration.Instance.BwHostUrl, objPage.objFeature.ArticleUrl, objPage.objFeature.BasicId);
                objPage.PageMetaTags.AlternateUrl = string.Format("{0}/m/features/{1}-{2}/", BWConfiguration.Instance.BwHostUrl, objPage.objFeature.ArticleUrl, objPage.objFeature.BasicId);
                objPage.PageMetaTags.AmpUrl = string.Format("{0}/m/features/{1}-{2}/amp/", BWConfiguration.Instance.BwHostUrl, objPage.objFeature.ArticleUrl, objPage.objFeature.BasicId);
                objPage.PageMetaTags.Title = string.Format("{0} - Bikewale ", objPage.objFeature.Title);
                objPage.PageMetaTags.Description = string.Format("Read about {0}. Read through more bike care tips to learn more about your bike maintenance.", objPage.objFeature.Title);
                objPage.PageMetaTags.Keywords = string.Format("features, stories, travelogues, specials, drives.");
                objPage.PageMetaTags.ShareImage = Bikewale.Utility.Image.GetPathToShowImages(objPage.objFeature.OriginalImgUrl, objPage.objFeature.HostUrl, ImageSize._640x348);

                SetPageJSONSchema(objPage);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.Features.DetailPage.BindPageMetas");
            }
        }

        /// <summary>
        /// Created By  : Sushil Kumar on 25th Aug 2017
        /// Description : To load json schema for the featured articles
        /// </summary>
        /// <param name="objData"></param>
        private void SetPageJSONSchema(DetailFeatureVM objData)
        {
            var objSchema = new NewsArticle();
            objSchema.HeadLine = objData.objFeature.Title;
            objSchema.DateModified = objData.objFeature.DisplayDate.ToString();
            objSchema.DatePublished = objSchema.DateModified;
            objSchema.Description = FormatDescription.SanitizeHtml(objData.objFeature.Description);
            if (objData.objFeature.PageList != null && objData.objFeature.PageList.Any())
            {
                objSchema.ArticleBody = Bikewale.Utility.FormatDescription.SanitizeHtml(Convert.ToString(objData.objFeature.PageList.First().Content));
            }

            objSchema.ArticleImage = new ImageObject()
            {
                ImageUrl = objData.PageMetaTags.ShareImage,
                Height = "348",
                Width = "640"
            };
            objSchema.Author = new Author()
            {
                Name = objData.objFeature.AuthorName
            };
            objSchema.MainEntityOfPage = new MainEntityOfPage() { PageUrlId = objData.PageMetaTags.CanonicalUrl };
            objSchema.Url = objData.PageMetaTags.CanonicalUrl;
            objData.PageMetaTags.SchemaJSON = Newtonsoft.Json.JsonConvert.SerializeObject(objSchema);
        }

        /// <summary>
        /// Created by : SubodhJain  on 30 March 2017
        /// Summary    : Get tagged make in feature
        /// </summary>
        private void GetTaggedBikeListByMake(DetailFeatureVM objDetailsVM)
        {
            try
            {
                if (objDetailsVM != null && objDetailsVM.objFeature != null && objDetailsVM.objFeature.VehiclTagsList != null && objDetailsVM.objFeature.VehiclTagsList.Count > 0)
                {

                    var _taggedMakeObj = objDetailsVM.objFeature.VehiclTagsList.FirstOrDefault(m => !string.IsNullOrEmpty(m.MakeBase.MaskingName));
                    if (_taggedMakeObj != null)
                    {
                        objDetailsVM.objMake = _taggedMakeObj.MakeBase;
                    }
                    else
                    {
                        objDetailsVM.objMake = objDetailsVM.objFeature.VehiclTagsList.FirstOrDefault().MakeBase;
                        if (objDetailsVM.objMake != null && objDetailsVM.objMake.MakeId > 0)
                        {
                            objDetailsVM.objMake = new Bikewale.Common.MakeHelper().GetMakeNameByMakeId((uint)objDetailsVM.objMake.MakeId);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Bikewale.Models.Features.DetailPage.GetTaggedBikeListByMake");

            }
        }

        private void PopulatePhotoGallery(DetailFeatureVM objData)
        {
            try
            {
                objData.PhotoGallery = new EditCMSPhotoGalleryVM();
                objData.PhotoGallery.Images = _cache.GetArticlePhotos(Convert.ToInt32(basicId));
                if (objData.PhotoGallery.Images != null && objData.PhotoGallery.Images.Any())
                {
                    objData.PhotoGallery.ImageCount = objData.PhotoGallery.Images.Count();
                }
                if (objData.objMake != null && objData.objModel != null)
                    objData.PhotoGallery.BikeName = string.Format("{0} {1}", objData.objMake.MakeName, objData.objModel.ModelName);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.Features.DetailPage.PopulatePhotoGallery");
            }
        }

        /// <summary>
        /// Created by : Subodh Jain  on 30 March 2017
        /// Summary    : Get model details if model is tagged
        /// </summary>
        private void GetTaggedBikeListByModel(DetailFeatureVM objDetailsVM)
        {
            try
            {
                if (objDetailsVM != null && objDetailsVM.objFeature != null && objDetailsVM.objFeature.VehiclTagsList != null && objDetailsVM.objFeature.VehiclTagsList.Count > 0)
                {

                    var _taggedModelObj = objDetailsVM.objFeature.VehiclTagsList.FirstOrDefault(m => !string.IsNullOrEmpty(m.ModelBase.MaskingName));
                    if (_taggedModelObj != null)
                    {
                        objDetailsVM.objModel = _taggedModelObj.ModelBase;
                    }
                    else
                    {
                        objDetailsVM.objModel = objDetailsVM.objFeature.VehiclTagsList.FirstOrDefault().ModelBase;
                        if (objDetailsVM.objModel != null)
                        {
                            objDetailsVM.objModel = new Bikewale.Common.ModelHelper().GetModelDataById((uint)objDetailsVM.objModel.ModelId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.Features.DetailPage.GetTaggedBikeListByModel");
            }
        }

        /// <summary>
        /// Created by : Subodh Jain  on 30 March 2017
        /// Summary    : Process query string
        /// </summary>
        private void ProcessQueryString()
        {
            var request = HttpContext.Current.Request;
            string qsBasicId = _basicId;
            try
            {
                qsBasicId = BasicIdMapping.GetCWBasicId(qsBasicId);
                if (!qsBasicId.Equals(_basicId))
                {
                    status = StatusCodes.RedirectPermanent;
                    mappedCWId = qsBasicId;
                    redirectUrl = string.Format("/features/{0}-{1}.html", request["t"], mappedCWId);
                }
                if (uint.TryParse(qsBasicId, out basicId) && basicId > 0)
                    status = StatusCodes.ContentFound;
                else
                    status = StatusCodes.ContentNotFound;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.Features.DetailPage.ProcessQueryString");
            }
        }
        #endregion
    }
}