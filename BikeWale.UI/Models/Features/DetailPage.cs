using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.EditorialWidgets;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Location;
using Bikewale.Memcache;
using Bikewale.Models.BestBikes;
using Bikewale.Models.EditorialPages;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Bikewale.Models.Features
{
    /// <summary>
    /// Modified by : Ashutosh Sharma on 28 Oct 2017
    /// Description : Added IsAMPPage
    /// Modified by : Rajan Chauhan on 17 Apr 2018
    /// Description : Removed IBikeModelsCacheRepository dependency
    /// </summary>
    public class DetailPage : EditorialBasePage
    {
        #region Variables for dependency injection and constructor
        private readonly ICMSCacheContent _cache = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly IBikeVersionCacheRepository<BikeVersionEntity, uint> _objBikeVersionsCache = null;
        private readonly IBikeInfo _bikeInfo;
        private readonly ICityCacheRepository _cityCacheRepo;
        private readonly IBikeMakesCacheRepository _bikeMakesCacheRepository = null;
        private readonly IBikeSeries _series;
        #endregion

        #region Page level variables
        private uint basicId;
        public StatusCodes status;
        public string mappedCWId;
        public string redirectUrl;
        public bool IsMobile { get; set; }
        public bool IsAMPPage { get; set; }
        private BikeInfoTabType _pageId = BikeInfoTabType.Features;
        private string _basicId;
        private uint CityId, ModelId = 0;
        private EnumBikeType bikeType = EnumBikeType.All;
        #endregion

        #region Constructor
        public DetailPage(ICMSCacheContent cache, IUpcoming upcoming, IBikeModels<BikeModelEntity, int> bikeModels, IBikeModelsCacheRepository<int> models, string basicId, IBikeVersionCacheRepository<BikeVersionEntity, uint> objBikeVersionsCache, IBikeInfo bikeInfo, ICityCacheRepository cityCacheRepo, IBikeMakesCacheRepository bikeMakesCacheRepository, IBikeSeries series)
            : base(bikeMakesCacheRepository, models, bikeModels, upcoming, series)
        {
            _cache = cache;
            _upcoming = upcoming;
            _bikeModels = bikeModels;
            _basicId = basicId;
            _objBikeVersionsCache = objBikeVersionsCache;
            _bikeMakesCacheRepository = bikeMakesCacheRepository;
            _cityCacheRepo = cityCacheRepo;
            _bikeInfo = bikeInfo;
            _series = series;
            ProcessQueryString();
        }
        #endregion

        #region Functions
        /// <summary>
        /// Created by : Subodh Jain  on 30 March 2017
        /// Summary    : Get all feature details
        /// Modified by : Ashutosh Sharma on 27 Oct 2017
        /// Description : Added call to BindAmpJsTags.
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
                    SetAdditionalVariables(objDetailsVM);
                    objDetailsVM.PageWidgets = base.GetEditorialWidgetData(EnumEditorialPageType.Detail);
                    PopulatePhotoGallery(objDetailsVM);
                    if (IsAMPPage)
                    {
                        BindAmpJsTags(objDetailsVM);
                    }
                    objDetailsVM.Page = Entities.Pages.GAPages.Editorial_Details_Page;
                }
                else
                {
                    status = StatusCodes.ContentNotFound;
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "Bikewale.Models.Features.DetailPage.GetData");
            }
            return objDetailsVM;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 27 Oct 2017
        /// Description : Method to bind required JS for AMP page.
        /// </summary>
        /// <param name="objData"></param>
        private void BindAmpJsTags(DetailFeatureVM objData)
        {
            try
            {
                objData.AmpJsTags = new Entities.Models.AmpJsTags();
                objData.AmpJsTags.IsAccordion = true;
                objData.AmpJsTags.IsAd = true;
                objData.AmpJsTags.IsBind = true;
                objData.AmpJsTags.IsCarousel = true;
                objData.AmpJsTags.IsSidebar = true;
                objData.AmpJsTags.IsSocialShare = true;
                objData.AmpJsTags.IsIframe = objData.objFeature != null && objData.objFeature.PageList != null && objData.objFeature.PageList.Any(p => p.Content.Contains("<iframe"));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BindAmpJsTags_{0}", objData));
            }
        }

        private void SetPopularBikeByBodyStyleId(DetailFeatureVM objData, int topCount)
        {
            if (objData != null && topCount > 0)
            {
                PopularBikesByBodyStyle objPopularStyle = new PopularBikesByBodyStyle(_bikeModels);
                objPopularStyle.ModelId = ModelId;
                objPopularStyle.CityId = CityId;
                objPopularStyle.TopCount = topCount;
                objData.PopularBodyStyle = objPopularStyle.GetData();
                if (objData.PopularBodyStyle != null)
                {
                    objData.PopularBodyStyle.WidgetHeading = string.Format("Popular {0}", objData.PopularBodyStyle.BodyStyleText);
                    objData.PopularBodyStyle.WidgetLinkTitle = string.Format("Best {0} in India", objData.PopularBodyStyle.BodyStyleLinkTitle);
                    objData.PopularBodyStyle.WidgetHref = UrlFormatter.FormatGenericPageUrl(objData.PopularBodyStyle.BodyStyle);
                    objData.PopularBodyStyle.CityId = CityId;
                    objData.PopularBodyStyle.ReturnUrlForAmpPages = string.Format("{0}/m/features/{1}-{2}/", BWConfiguration.Instance.BwHostUrl, objData.objFeature.ArticleUrl, objData.objFeature.BasicId);
                    bikeType = objData.PopularBodyStyle.BodyStyle == EnumBikeBodyStyles.Scooter ? EnumBikeType.Scooters : EnumBikeType.All;
                }
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
                var objPageDetails = objPage.objFeature;
                if (objPageDetails != null)
                {
                    objPage.AdTags.TargetedTags = String.Join(", ", objPageDetails.TagsList);
                    objPage.PageMetaTags.CanonicalUrl = string.Format("{0}/features/{1}-{2}/", BWConfiguration.Instance.BwHostUrl, objPageDetails.ArticleUrl, objPageDetails.BasicId);
                    objPage.PageMetaTags.AlternateUrl = string.Format("{0}/m/features/{1}-{2}/", BWConfiguration.Instance.BwHostUrl, objPageDetails.ArticleUrl, objPageDetails.BasicId);
                    objPage.PageMetaTags.AmpUrl = string.Format("{0}/m/features/{1}-{2}/amp/", BWConfiguration.Instance.BwHostUrl, objPageDetails.ArticleUrl, objPageDetails.BasicId);
                    objPage.PageMetaTags.Title = string.Format("{0} - Bikewale ", objPageDetails.Title);
                    objPage.PageMetaTags.Description = string.Format("Read about {0}. Read through more bike care tips to learn more about your bike maintenance.", objPageDetails.Title);
                    objPage.PageMetaTags.Keywords = string.Format("features, stories, travelogues, specials, drives.");
                    objPage.PageMetaTags.ShareImage = Bikewale.Utility.Image.GetPathToShowImages(objPageDetails.OriginalImgUrl, objPageDetails.HostUrl, ImageSize._640x348);
                    objPage.Page_H1 = objPageDetails.Title;
                }
                SetBreadcrumList(objPage);
                SetPageJSONSchema(objPage);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.Features.DetailPage.BindPageMetas");
            }
        }

        /// <summary>
        /// Created By  : Sushil Kumar on 25th Aug 2017
        /// Description : To load json schema for the featured articles
        /// Modified By : Monika Korrapati on 30 July 2018
        /// Description : Format date into ISO 8601
        /// Modified By : Monika Korrapati on 22 Nov 2018
        /// Description : Added Modified Date.
        /// </summary>
        /// <param name="objData"></param>
        private void SetPageJSONSchema(DetailFeatureVM objData)
        {
            var objSchema = new NewsArticle();
            objSchema.HeadLine = objData.objFeature.Title;
            objSchema.DateModified = Utility.FormatDate.ConvertToISO(objData.objFeature.ModifiedDate);
            objSchema.DatePublished = Utility.FormatDate.ConvertToISO(objData.objFeature.DisplayDate);
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
            objData.PageMetaTags.PageSchemaJSON = Newtonsoft.Json.JsonConvert.SerializeObject(objSchema);

            WebPage webpage = SchemaHelper.GetWebpageSchema(objData.PageMetaTags, objData.SchemaBreadcrumbList);

            if (webpage != null)
            {
                objData.PageMetaTags.SchemaJSON = SchemaHelper.JsonSerialize(webpage);
            }

        }
        /// <summary>
        /// Created By : Snehal Dange on 8th nOV 2017
        /// Description : Function to create page level schema for breadcrum
        /// </summary>
        private void SetBreadcrumList(DetailFeatureVM objData)
        {
            try
            {
                IList<BreadcrumbListItem> BreadCrumbs = new List<BreadcrumbListItem>();
                string url = string.Format("{0}/", Utility.BWConfiguration.Instance.BwHostUrl);
                ushort position = 1;
                if (IsMobile)
                {
                    url += "m/";
                }

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, "Home"));

                url += "features/";
                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, url, "Features"));

                if (objData.objFeature != null)
                {
                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null, objData.objFeature.Title));
                }
                objData.BreadcrumbList.BreadcrumListItem = BreadCrumbs;
                objData.SchemaBreadcrumbList.BreadcrumListItem = BreadCrumbs.Take(BreadCrumbs.Count - 1);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.Models.Features.DetailPage.SetBreadcrumList");
            }

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
                ErrorClass.LogError(err, "Bikewale.Models.Features.DetailPage.GetTaggedBikeListByMake");

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
                ErrorClass.LogError(ex, "Bikewale.Models.Features.DetailPage.PopulatePhotoGallery");
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
                        ModelId = (uint)objDetailsVM.objModel.ModelId;
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
                ErrorClass.LogError(ex, "Bikewale.Models.Features.DetailPage.GetTaggedBikeListByModel");
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
                ErrorClass.LogError(ex, "Bikewale.Models.Features.DetailPage.ProcessQueryString");
            }
        }

        /// <summary>
        /// Created by : Snehal Dange on 25th April 2018
        /// Description: set page level variables before calling base function
        /// Modified By : Deepak Israni on 8 May 2018
        /// Description : Added flag for show on road price button and added GA related information.
        /// </summary>
        /// <param name="objData"></param>
        private void SetAdditionalVariables(DetailFeatureVM objData)
        {
            try
            {
                EditorialWidgetEntity editorialWidgetData = new EditorialWidgetEntity();
                if (objData.objMake != null)
                {
                    BikeMakeEntityBase bikeMakeObj = objData.objMake;
                    editorialWidgetData.IsMakeLive = (bikeMakeObj.IsNew && !bikeMakeObj.IsFuturistic);
                    editorialWidgetData.IsScooterOnlyMake = (bikeMakeObj.IsScooterOnly);
                    editorialWidgetData.Make = bikeMakeObj;

                }
                editorialWidgetData.IsMobile = IsMobile;
                editorialWidgetData.CityId = CityId;
                editorialWidgetData.IsModelTagged = (ModelId > 0);
                editorialWidgetData.ShowOnRoadPriceButton = true;
                editorialWidgetData.GAInfo = new EditorialGAEntity
                {
                    CategoryId = EditorialGACategories.Editorial_Details_Page,
                    PQSourceId = PQSourceEnum.Mobile_Features_Details_Page
                };

                base.SetAdditionalData(editorialWidgetData);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.Features.DetailPage.SetAdditionalVariables");
            }
        }
        #endregion
    }
}