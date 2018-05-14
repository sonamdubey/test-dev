using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.EditorialWidgets;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.Pages;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.PWA.Articles;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.PWA.CMS;
using Bikewale.Memcache;
using Bikewale.Models.EditorialPages;
using Bikewale.Notifications;
using Bikewale.PWA.Utils;
using Bikewale.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by : Aditi Srivastava on 30 Mar 2017
    /// Summary    : Model to populate view model for expert review detail page
    /// Modified by : Rajan Chauhan on 26 Feb 2017
    /// Description : Added private variable CityName and IPWACMSCacheRepository _renderedArticles
    /// Modified by: Dhruv Joshi
    /// Dated: 19th April 2018
    /// Description: Added variables to be set before calling parent class's GetEditorialWidgetData() function
    /// </summary>
    public class ExpertReviewsDetailPage : EditorialBasePage
    {
        #region Variables for dependency injection
        private readonly ICMSCacheContent _cmsCache = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly IBikeModelsCacheRepository<int> _models = null;
        private readonly IBikeInfo _bikeInfo;
        private readonly ICityCacheRepository _cityCacheRepo;
        private readonly IUpcoming _upcoming = null;
        private readonly string _basicId;
        private readonly IPWACMSCacheRepository _renderedArticles = null;
        private readonly IBikeMakesCacheRepository _bikeMakesCacheRepository = null;
        private readonly IBikeVersions<BikeVersionEntity, uint> _objBikeVersions;
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _bikeMasking = null;
        private readonly IBikeSeriesCacheRepository _seriesCache = null;
        private readonly IBikeSeries _series;
        #endregion

        #region Page level variables
        public StatusCodes status;
        public string mappedCWId;
        public string redirectUrl;
        private string CityName;
        private GlobalCityAreaEntity currentCityArea;
        private uint CityId, MakeId, ModelId, pageCatId = 0;
        private uint _totalTabCount = 3;
        private EnumBikeType bikeType = EnumBikeType.All;
        private bool showCheckOnRoadCTA = false;
        private uint basicId;
        private PQSourceEnum pqSource = 0;
        public BikeSeriesEntityBase bikeSeriesEntityBase;
        private static string pageName = "Editorial Details";

        private bool isModelTagged;
        private bool isMakeTagged;
        private bool isMakeLive;
        private EnumBikeBodyStyles bodyStyle;
        private bool isSeriesAvailable;
        private bool isScooterOnlyMake;
        #endregion

        #region Public properties
        public bool IsMobile { get; set; }
        public bool IsAMPPage { get; set; }
        public ControllerContext RefControllerContext { get; set; }
        #endregion

        #region Constructor
        public ExpertReviewsDetailPage(ICMSCacheContent cmsCache, IBikeModelsCacheRepository<int> models, IBikeModels<BikeModelEntity, int> bikeModels, IUpcoming upcoming, IBikeInfo bikeInfo, ICityCacheRepository cityCacheRepo,
            IBikeMakesCacheRepository bikeMakesCacheRepository, IBikeVersions<BikeVersionEntity, uint> objBikeVersions, IBikeMaskingCacheRepository<BikeModelEntity, int> bikeMasking, string basicId,
            IPWACMSCacheRepository renderedArticles, IBikeSeriesCacheRepository seriesCache, IBikeSeries series)
            : base(bikeMakesCacheRepository, models, bikeModels, upcoming, series)
        {
            _cmsCache = cmsCache;
            _models = models;
            _bikeModels = bikeModels;
            _upcoming = upcoming;
            _bikeInfo = bikeInfo;
            _cityCacheRepo = cityCacheRepo;
            _basicId = basicId;
            _renderedArticles = renderedArticles;
            _bikeMakesCacheRepository = bikeMakesCacheRepository;
            _objBikeVersions = objBikeVersions;
            _bikeMasking = bikeMasking;
            _seriesCache = seriesCache;
            _series = series;
            ProcessCityArea();
            ProcessQueryString();
        }
        #endregion

        #region Functions

        /// <summary>
        /// Created by  : Rajan Chauhan on 26 Feb 2018
        /// Description : To set currentCityArea, CityName and CityId
        /// </summary>
        private void ProcessCityArea()
        {
            try
            {
                currentCityArea = GlobalCityArea.GetGlobalCityArea();
                if (currentCityArea != null)
                {
                    CityId = currentCityArea.CityId;
                    CityName = currentCityArea.City;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.ExpertReviews.ExpertReviewsDetailPage.ProcessCityArea");
            }
        }

        private void CheckSeriesData(ExpertReviewsDetailPageVM objdata)
        {
            try
            {
                bikeSeriesEntityBase = _models.GetSeriesByModelId(ModelId);
                if (null != bikeSeriesEntityBase && bikeSeriesEntityBase.IsSeriesPageUrl)
                {
                    objdata.IsSeriesAvailable = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.ExpertReviewsDetailPage.CheckSeriesData");
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 31 Mar 2017
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
                    redirectUrl = string.Format("/expert-reviews/{0}-{1}.html", request["t"], mappedCWId);
                }
                if (uint.TryParse(qsBasicId, out basicId) && basicId > 0)
                    status = StatusCodes.ContentFound;
                else
                    status = StatusCodes.ContentNotFound;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.ExpertReviewsDetailPage.ProcessQueryString - BasicId : " + _basicId);
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 31 Mar 2017
        /// Summary    : Get entire data for expert reviews detail page
        /// Modified by : Ashutosh Sharma on 27 Oct 2017
        /// Description : Added call to BindAmpJsTags.
        /// Modified by : snehal Dange on 28th Nov 2017
        /// Descritpion : Added ga for page
        /// Modified by: Dhruv Joshi
        /// Dated: 19th April 2018
        /// Description: GetWidgetData called if mobile or amp otherwise base.GetEditorialWidgetData()
        /// </summary>
        public ExpertReviewsDetailPageVM GetData(int widgetTopCount)
        {
            ExpertReviewsDetailPageVM objData = new ExpertReviewsDetailPageVM();
            try
            {
                objData.ArticleDetails = _cmsCache.GetArticlesDetails(basicId);

                if (objData.ArticleDetails != null)
                {
                    status = StatusCodes.ContentFound;
                    GetTaggedBikeListByMake(objData);
                    GetTaggedBikeListByModel(objData);
                    SetPageMetas(objData);
                    CheckSeriesData(objData);

                    PopulateBikeInfo(objData);

                    #region Maintain the order of the following two lines
                    SetAdditionalVariables(objData);
                    objData.PageWidgets = base.GetEditorialWidgetData(EnumEditorialPageType.Detail);
                    #endregion

                    PopulatePhotoGallery(objData);
                    BindSimilarBikes(objData);
                    SetBikeTested(objData);
                    InsertBikeInfoWidgetIntoContent(objData);
                    objData.Page = GAPages.Editorial_Details_Page;
                    if (IsAMPPage)
                    {
                        BindAmpJsTags(objData);
                    }
                }
                else
                    status = StatusCodes.ContentNotFound;
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "Bikewale.Models.ExpertReviewsDetailPage.GetData - BasicId : " + _basicId);
            }
            return objData;
        }

        /// <summary>
        /// Created by: Dhruv Joshi
        /// Dated: 19th April 2018
        /// Description: Setting variables before calling base class function
        /// </summary>
        /// <param name="objData"></param>
        private void SetAdditionalVariables(ExpertReviewsDetailPageVM objData)
        {
            objData.PageName = pageName;
            isMakeLive = !(objData.BikeInfo != null && (objData.BikeInfo.IsUpcoming || objData.BikeInfo.IsDiscontinued));

            bodyStyle = EnumBikeBodyStyles.AllBikes;

            IEnumerable<BikeVersionMinSpecs> objVersionsList = _objBikeVersions.GetVersionMinSpecs(ModelId, false);

            if (objVersionsList != null && objVersionsList.Any())
            {
                bodyStyle = objVersionsList.FirstOrDefault().BodyStyle;
            }

            objData.BodyStyle = bodyStyle;

            isSeriesAvailable = objData.IsSeriesAvailable;
            if (objData.Make != null)
            {
                isScooterOnlyMake = objData.Make.IsScooterOnly;
            }
            EditorialWidgetEntity editorialWidgetData = new EditorialWidgetEntity
            {
                IsMobile = IsMobile,
                IsMakeLive = isMakeLive,
                IsModelTagged = isModelTagged,
                IsSeriesAvailable = isSeriesAvailable,
                IsScooterOnlyMake = isScooterOnlyMake,
                BodyStyle = bodyStyle,
                CityId = CityId,
                Make = objData.Make,
                Series = bikeSeriesEntityBase
            };
            base.SetAdditionalData(editorialWidgetData);
        }

        /// <summary>
        /// Created by  : Rajan Chauhan on 26 Feb 2018
        /// Description : Function to get PWA data for expert reviews
        /// Modified by : Ashutosh Sharma on 01 Mar 2018.
        /// Description : Added logic to split article html content into two parts to insert bikeinfo card at 25% height of article content.
        /// Modified by : Sanskar Gupta on 21 April 2018
        /// Description : Added call for `SetAddtionalVariables()`, populate `objData.PageWidgets` and changed method call to populate `newsDetailReducer.NewBikesListData.NewBikesList` and `newsDetailReducer.NewBikesListData.BikeMakeList`
        /// </summary>
        /// <param name="widgetTopCount"></param>
        /// <returns></returns>
        public ExpertReviewsDetailPageVM GetPwaData(int widgetTopCount)
        {
            ExpertReviewsDetailPageVM objData = new ExpertReviewsDetailPageVM();
            try
            {
                objData.ArticleDetails = _cmsCache.GetArticlesDetails(basicId);
                if (objData.ArticleDetails != null)
                {
                    status = StatusCodes.ContentFound;
                    GetTaggedBikeListByMake(objData);
                    GetTaggedBikeListByModel(objData);
                    SetPageMetas(objData);
                    CheckSeriesData(objData);


                    SetAdditionalVariables(objData);
                    objData.PageWidgets = base.GetEditorialWidgetData(EnumEditorialPageType.Detail);

                    PopulatePhotoGallery(objData);
                    BindSimilarBikes(objData);
                    SetBikeTested(objData);
                    int matchedPage = InsertBikeInfoWidgetIntoContentPwa(objData.ArticleDetails);
                    objData.ReduxStore = new PwaReduxStore();
                    var newsDetailReducer = objData.ReduxStore.News.NewsDetailReducer;
                    newsDetailReducer.ArticleDetailData.ArticleDetail = ConverterUtility.MapArticleDetailsToPwaExpertReviewDetails(objData.ArticleDetails, matchedPage);
                    newsDetailReducer.ArticleDetailData.ArticleDetail.ImageGallery = ConverterUtility.MapPhotoGalleryToPwaImageList(objData.PhotoGallery);
                    newsDetailReducer.RelatedModelObject.ModelObject = ConverterUtility.MapGenericBikeInfoToPwaBikeInfo(objData.BikeInfo);

                    if (objData.PageWidgets != null)
                    {
                        newsDetailReducer.NewBikesListData.NewBikesList = ConverterUtility.MapPopularAndUpcomingWidgetDataToPwa(objData.PageWidgets);
                        newsDetailReducer.NewBikesListData.BikeMakeList = ConverterUtility.MapOtherBrandsWidgetDataToPWA(objData.PageWidgets);
                    }
                    var storeJson = JsonConvert.SerializeObject(objData.ReduxStore);
                    objData.ServerRouterWrapper = _renderedArticles.GetNewsDetails(PwaCmsHelper.GetSha256Hash(storeJson), objData.ReduxStore.News.NewsDetailReducer,
                                newsDetailReducer.ArticleDetailData.ArticleDetail.ArticleUrl, "root", "ServerRouterWrapper", "Expert Reviews");
                    objData.WindowState = storeJson;
                    objData.Page = GAPages.Editorial_Details_Page;
                    if (IsAMPPage)
                    {
                        BindAmpJsTags(objData);
                    }
                }
                else
                {
                    status = StatusCodes.ContentNotFound;
                }

            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "Bikewale.Models.ExpertReviewsDetailPage.GetPwaData - BasicId : " + _basicId);
            }
            return objData;

        }

        /// <summary>
        /// Created By :- Subodh Jain 13-12-2017
        /// Description :- Bind Similar Bikes Only for desktop
        /// Modified by : Deepak Israni on 1st Feb 2018
        /// Descritpion : Added a null check for Model object in onjData
        /// </summary>
        /// <param name="objData"></param>
        private void BindSimilarBikes(ExpertReviewsDetailPageVM objData)
        {
            try
            {
                if (objData.Model != null && objData.Model.ModelId > 0)
                {
                    var objSimilarBikes = new SimilarBikesWidget(_objBikeVersions, (uint)objData.Model.ModelId, true, PQSourceEnum.Desktop_NewsDetailsPage);

                    objSimilarBikes.TopCount = 9;
                    objSimilarBikes.CityId = CityId;
                    objSimilarBikes.IsNew = objData.BikeInfo != null && (objData.BikeInfo.IsUpcoming || objData.BikeInfo.IsDiscontinued) ? false : true;
                    objSimilarBikes.IsUpcoming = objData.BikeInfo != null ? objData.BikeInfo.IsUpcoming : false;
                    objSimilarBikes.IsDiscontinued = objData.BikeInfo != null ? objData.BikeInfo.IsDiscontinued : false;
                    objData.SimilarBikes = objSimilarBikes.GetData();
                    if (objData.SimilarBikes != null && objData.SimilarBikes.Bikes != null && objData.SimilarBikes.Bikes.Any())
                    {
                        objData.SimilarBikes.Make = objData.Make;
                        objData.SimilarBikes.Model = objData.Model;

                        objData.SimilarBikes.Page = GAPages.Editorial_Details_Page;
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.Models.NewsDetailPage.BindSimilarBikes({0}) - BasicId : {1}", objData.Model.ModelId, _basicId));
            }
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 27 Oct 2017
        /// Description : Method to bind required JS for AMP page.
        /// </summary>
        /// <param name="objData"></param>
        private void BindAmpJsTags(ExpertReviewsDetailPageVM objData)
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
                objData.AmpJsTags.IsIframe = objData.ArticleDetails != null && objData.ArticleDetails.PageList != null && objData.ArticleDetails.PageList.Any(p => p.Content.Contains("<iframe"));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BindAmpJsTags_{0}", objData));
            }
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 31 Mar 2017
        /// Summary    : Set expert reviews details page metas
        /// Modified by :- Subodh Jain 19 june 2017
        /// Summary :- Added TargetModels and Target Make
        /// </summary>
        private void SetPageMetas(ExpertReviewsDetailPageVM objData)
        {
            try
            {
                objData.PageMetaTags.CanonicalUrl = string.Format("{0}/expert-reviews/{1}-{2}.html", BWConfiguration.Instance.BwHostUrl, objData.ArticleDetails.ArticleUrl, objData.ArticleDetails.BasicId);
                objData.PageMetaTags.AmpUrl = string.Format("{0}/m/expert-reviews/{1}-{2}/amp/", BWConfiguration.Instance.BwHostUrl, objData.ArticleDetails.ArticleUrl, objData.ArticleDetails.BasicId);
                objData.PageMetaTags.AlternateUrl = string.Format("{0}/m/expert-reviews/{1}-{2}.html", BWConfiguration.Instance.BwHostUrl, objData.ArticleDetails.ArticleUrl, objData.ArticleDetails.BasicId);
                objData.PageMetaTags.Title = string.Format("{0} - BikeWale", objData.ArticleDetails.Title);
                objData.PageMetaTags.ShareImage = Image.GetPathToShowImages(objData.ArticleDetails.OriginalImgUrl, objData.ArticleDetails.HostUrl, ImageSize._468x263);
                if (objData.Make != null)
                    objData.AdTags.TargetedMakes = objData.Make.MakeName;
                if (objData.Model != null)
                    objData.AdTags.TargetedModel = objData.Model.ModelName;
                objData.PageMetaTags.Keywords = string.Format("{0},road test, road tests, roadtests, roadtest, bike reviews, expert bike reviews, detailed bike reviews, test-drives, comprehensive bike tests, bike preview, first drives", (objData.Model != null) ? objData.Model.ModelName : "");
                if (IsMobile)
                    objData.PageMetaTags.Description = string.Format("BikeWale tests {0}, Read the complete road test report to know how it performed.", (objData.Model != null) ? objData.Model.ModelName : "");
                else
                    objData.PageMetaTags.Description = "Learn about the trending stories related to bike and bike products. Know more about features, do's and dont's of different bike products exclusively on BikeWale";

                SetPageJSONSchema(objData);
                SetBreadcrumList(objData);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.ExpertReviewsDetailPage.SetPageMetas - BasicId : " + _basicId);
            }
        }

        /// <summary>
        /// Created By  : Sushil Kumar on 25th Aug 2017
        /// Description : To load json schema for the expert reviews articles
        /// </summary>
        /// <param name="objData"></param>
        private void SetPageJSONSchema(ExpertReviewsDetailPageVM objData)
        {
            var objSchema = new NewsArticle();
            objSchema.HeadLine = objData.ArticleDetails.Title;
            objSchema.DateModified = objData.ArticleDetails.DisplayDate.ToString();
            objSchema.DatePublished = objSchema.DateModified;
            objSchema.Description = FormatDescription.SanitizeHtml(objData.ArticleDetails.Description);
            if (objData.ArticleDetails.PageList != null && objData.ArticleDetails.PageList.Any())
            {
                objSchema.ArticleBody = Bikewale.Utility.FormatDescription.SanitizeHtml(Convert.ToString(objData.ArticleDetails.PageList.First().Content));
            }
            objSchema.ArticleImage = new ImageObject()
            {
                ImageUrl = objData.PageMetaTags.ShareImage,
                Height = "348",
                Width = "640"
            };
            objSchema.Author = new Author()
            {
                Name = objData.ArticleDetails.AuthorName
            };
            objSchema.MainEntityOfPage = new MainEntityOfPage() { PageUrlId = objData.PageMetaTags.CanonicalUrl };

            objData.PageMetaTags.SchemaJSON = Newtonsoft.Json.JsonConvert.SerializeObject(objSchema);
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 31 Mar 2017
        /// Summary    : Get tagged make in article
        /// Modified by: Dhruv Joshi
        /// Dated: 19th April 2018
        /// Description: Setting Value for isMakeTagged
        /// </summary>
        private void GetTaggedBikeListByMake(ExpertReviewsDetailPageVM objData)
        {
            try
            {
                if (objData.ArticleDetails.VehiclTagsList != null && objData.ArticleDetails.VehiclTagsList.Any())
                {

                    var taggedMakeObj = objData.ArticleDetails.VehiclTagsList.FirstOrDefault(m => !string.IsNullOrEmpty(m.MakeBase.MaskingName));
                    if (taggedMakeObj != null)
                    {
                        objData.Make = taggedMakeObj.MakeBase;
                    }
                    else
                    {
                        objData.Make = objData.ArticleDetails.VehiclTagsList.FirstOrDefault().MakeBase;
                        if (objData.Make != null)
                            objData.Make = new Bikewale.Common.MakeHelper().GetMakeNameByMakeId((uint)objData.Make.MakeId);
                    }
                    MakeId = (uint)objData.Make.MakeId;
                    isMakeTagged = MakeId > 0;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.ExpertReviewsDetailPage.GetTaggedBikeListByMake - BasicId : " + _basicId);
            }
        }


        /// <summary>
        /// Created by : Aditi Srivastava on 31 Mar 2017
        /// Summary    : Get tagged model in article
        /// Modified by: Dhruv Joshi
        /// Dated: 19th April 2018
        /// Description: Setting Value for isModelTagged
        /// </summary>
        private void GetTaggedBikeListByModel(ExpertReviewsDetailPageVM objData)
        {
            try
            {
                if (objData.ArticleDetails.VehiclTagsList != null && objData.ArticleDetails.VehiclTagsList.Any())
                {
                    objData.TaggedBikes = objData.ArticleDetails.VehiclTagsList.Where(bike => !string.IsNullOrEmpty(bike.MakeBase.MaskingName) && !string.IsNullOrEmpty(bike.ModelBase.MaskingName));

                    var taggedModelObj = objData.ArticleDetails.VehiclTagsList.FirstOrDefault(m => !string.IsNullOrEmpty(m.ModelBase.MaskingName));
                    if (taggedModelObj != null)
                    {
                        objData.Model = taggedModelObj.ModelBase;
                    }
                    else
                    {
                        objData.Model = objData.ArticleDetails.VehiclTagsList.FirstOrDefault().ModelBase;
                        if (objData.Model != null)
                            objData.Model = new Bikewale.Common.ModelHelper().GetModelDataById((uint)objData.Model.ModelId);
                    }
                    ModelId = (uint)objData.Model.ModelId;
                    isModelTagged = ModelId > 0;

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.ExpertReviewsDetailPage.GetTaggedBikeListByModel - BasicId : " + _basicId);
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 31 Mar 2017
        /// Summary    : Populate view model for photo gallery
        /// </summary>
        private void PopulatePhotoGallery(ExpertReviewsDetailPageVM objData)
        {
            try
            {
                objData.PhotoGallery = new EditCMSPhotoGalleryVM();
                objData.PhotoGallery.Images = _cmsCache.GetArticlePhotos(Convert.ToInt32(basicId));
                if (objData.PhotoGallery.Images != null && objData.PhotoGallery.Images.Any())
                {
                    objData.PhotoGallery.ImageCount = objData.PhotoGallery.Images.Count();
                }
                if (objData.Make != null && objData.Model != null)
                    objData.PhotoGallery.BikeName = string.Format("{0} {1}", objData.Make.MakeName, objData.Model.ModelName);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.ExpertReviewsDetailPage.PopulatePhotoGallery - BasicId : " + _basicId);
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 31 Mar 2017
        /// Summary    : Add bike tested if models are tagged  
        /// </summary>
        /// <param name="objData"></param>
        private void SetBikeTested(ExpertReviewsDetailPageVM objData)
        {
            try
            {
                if (objData.ArticleDetails.VehiclTagsList != null && objData.ArticleDetails.VehiclTagsList.Count > 0 && objData.ArticleDetails.VehiclTagsList.Any(m => (m.MakeBase != null && !String.IsNullOrEmpty(m.MakeBase.MaskingName))))
                {
                    objData.BikeTested = new StringBuilder();

                    objData.BikeTested.Append("Bike Tested: ");

                    IEnumerable<int> ids = objData.ArticleDetails.VehiclTagsList
                           .Select(e => e.ModelBase.ModelId)
                           .Distinct();
                    int iTemp = 1;
                    foreach (var i in ids)
                    {
                        VehicleTag item = objData.ArticleDetails.VehiclTagsList.FirstOrDefault(e => e.ModelBase.ModelId == i);
                        if (item != null && !String.IsNullOrEmpty(item.MakeBase.MaskingName))
                        {
                            objData.BikeTested.Append(string.Format("<a title={0} {1} Bikes href=/m/{2}-bikes/{3}/>{0} {1}</a>", item.MakeBase.MakeName, item.ModelBase.ModelName, item.MakeBase.MaskingName, item.ModelBase.MaskingName));
                            if (iTemp < ids.Count()) { objData.BikeTested.Append(", "); }
                            iTemp++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.ExpertReviewsDetailPage.SetBikeTested - BasicId : " + _basicId);
            }
        }

        /// <summary>
        /// Modified by : Rajan Chauhan on 24 Apr 2018
        /// Desciption  : Corrected the condition for setting trucatingIndex on matched page 
        /// </summary>
        /// <param name="objData"></param>
        private void InsertBikeInfoWidgetIntoContent(ExpertReviewsDetailPageVM objData)
        {
            try
            {
                if (objData.ArticleDetails != null && objData.ArticleDetails.PageList != null && objData.BikeInfo != null)
                {
                    int totalStrippedHTMLLength = 0, matchedPage = 0, currentPageLength = 0, requiredLength = 0, totalPages = objData.ArticleDetails.PageList.Count;
                    string inputString = null, viewName = null;
                    IList<Tuple<int, int>> objPagesInfo = new List<Tuple<int, int>>();
                    Bikewale.Models.Shared.BikeInfo ampBikeInfo = null;

                    if (IsMobile && !IsAMPPage)
                    {
                        viewName = "~/views/BikeModels/_minBikeInfoCard_Mobile.cshtml";
                    }
                    else if (IsAMPPage)
                    {
                        ampBikeInfo = new Bikewale.Models.Shared.BikeInfo()
                        {
                            Info = objData.BikeInfo.BikeInfo,
                            Bike = objData.BikeInfo.BikeName,
                            Url = objData.BikeInfo.BikeUrl
                        };
                        viewName = "~/views/BikeModels/_BikeInfoCard_AMP_Mobile.cshtml";
                    }
                    else viewName = "~/views/BikeModels/_minBikeInfoCard.cshtml";


                    //get length of each pages with stripped html
                    for (int i = 0; i < totalPages; i++)
                    {
                        var tuple = StringHtmlHelpers.StripHtmlTagsWithLength(objData.ArticleDetails.PageList[i].Content);
                        totalStrippedHTMLLength += tuple.Item2;
                        objPagesInfo.Add(Tuple.Create(i, tuple.Item2));
                    }


                    requiredLength = Convert.ToInt32(totalStrippedHTMLLength * 0.25);
                    foreach (var item in objPagesInfo)
                    {
                        currentPageLength = item.Item2;
                        if (currentPageLength < requiredLength)
                        {
                            requiredLength = requiredLength - currentPageLength;
                        }
                        else
                        {
                            matchedPage = item.Item1;
                            break;
                        }
                    }

                    if (RefControllerContext != null)
                    {
                        if (IsAMPPage)
                        {
                            inputString = MvcHelper.RenderViewToString(RefControllerContext, viewName, ampBikeInfo);
                        }
                        else
                        {
                            inputString = MvcHelper.RenderViewToString(RefControllerContext, viewName, objData.BikeInfo);
                        }
                    }

                    if (!string.IsNullOrEmpty(inputString) && totalPages > 0)
                    {
                        string output = StringHtmlHelpers.InsertHTMLBetweenHTML(objData.ArticleDetails.PageList[matchedPage].Content, inputString, requiredLength);

                        objData.ArticleDetails.PageList[matchedPage].Content = output;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.ExpertReviewsDetailPage.InsertBikeInfoWidgetIntoContent - BasicId : " + _basicId);
            }
        }
        /// <summary>
        /// Created by : Ashutosh Sharma on 01 Mar 2018.
        /// Description : Method to split article html content into two parts to insert bikeinfo card at 25% height of article content, if there
        ///                 is only one page in article pagelist, than one more page is added if bottomContent is not empty.
        /// Modified by : Rajan Chauhan on 24 Apr 2018
        /// Desciption  : Corrected the condition for setting trucatingIndex on matched page 
        /// </summary>
        /// <param name="articleDetails"></param>
        /// <returns></returns>
        private int InsertBikeInfoWidgetIntoContentPwa(ArticlePageDetails articleDetails)
        {
            int matchedPage = 0;
            try
            {
                if (articleDetails != null && articleDetails.PageList != null)
                {
                    int totalStrippedHTMLLength = 0, currentPageLength = 0, requiredLength = 0, totalPages = articleDetails.PageList.Count;
                    IList<Tuple<int, int>> objPagesInfo = new List<Tuple<int, int>>();



                    //get length of each pages with stripped html
                    for (int i = 0; i < totalPages; i++)
                    {
                        var tuple = StringHtmlHelpers.StripHtmlTagsWithLength(articleDetails.PageList[i].Content);
                        totalStrippedHTMLLength += tuple.Item2;
                        objPagesInfo.Add(Tuple.Create(i, tuple.Item2));
                    }


                    requiredLength = Convert.ToInt32(totalStrippedHTMLLength * 0.25);

                    foreach (var item in objPagesInfo)
                    {
                        currentPageLength = item.Item2;
                        if (currentPageLength < requiredLength)
                        {
                            requiredLength = requiredLength - currentPageLength;
                        }
                        else
                        {
                            matchedPage = item.Item1;
                            break;
                        }
                    }




                    string topContentInPage = string.Empty, bottomContentInPage = string.Empty;
                    StringHtmlHelpers.InsertHTMLBetweenHTMLPwa(articleDetails.PageList[matchedPage].Content, requiredLength, out topContentInPage, out bottomContentInPage);

                    articleDetails.PageList[matchedPage].Content = topContentInPage;
                    if (matchedPage != articleDetails.PageList.Count - 1)
                    {
                        articleDetails.PageList.Insert(matchedPage + 1, new Page()
                        {
                            Content = bottomContentInPage,
                            PageName = string.Empty,
                            pageId = articleDetails.PageList[matchedPage + 1].pageId,
                            Priority = articleDetails.PageList[matchedPage + 1].Priority
                        });
                    }
                    else
                    {
                        articleDetails.PageList.Add(new Page()
                        {
                            Content = bottomContentInPage,
                            PageName = string.Empty,
                            pageId = articleDetails.PageList[matchedPage].pageId + 1,
                            Priority = articleDetails.PageList.Max(p => p.Priority)
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.ExpertReviewsDetailPage.InsertBikeInfoWidgetIntoContentPwa : {0}", articleDetails));
            }
            return matchedPage;
        }
        /// <summary>
        /// Created By : Snehal Dange on 8th NOV 2017
        /// Description : Function to create page level schema for breadcrum
        /// </summary>
        private void SetBreadcrumList(ExpertReviewsDetailPageVM objData)
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
                url += "expert-reviews/";
                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, url, "Expert Reviews"));

                if (objData.ArticleDetails != null)
                {
                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null, objData.ArticleDetails.Title));
                }


                objData.BreadcrumbList.BreadcrumListItem = BreadCrumbs;
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.Models.ExpertReviewsDetailPage.SetBreadcrumList");
            }

        }

        /// <summary>
        /// Created by: Dhruv Joshi
        /// Dated: 20th April 2018
        /// Description: Populate BikeInfo object of objData
        /// </summary>
        /// <param name="objData"></param>
        private void PopulateBikeInfo(ExpertReviewsDetailPageVM objData)
        {
            try
            {
                BikeInfoWidget objBikeInfo = new BikeInfoWidget(_bikeInfo, _cityCacheRepo, ModelId, CityId, _totalTabCount, BikeInfoTabType.ExpertReview);
                objData.BikeInfo = objBikeInfo.GetData();
                objData.BikeInfo.IsSmallSlug = true;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception: Bikewale.Models.News.NewsIndexPage.PopulateBikeInfo");
            }
        }
        #endregion
    }
}