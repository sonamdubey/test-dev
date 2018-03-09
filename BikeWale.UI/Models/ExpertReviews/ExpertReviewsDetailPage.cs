using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
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
using Bikewale.Models.BestBikes;
using Bikewale.Models.BikeModels;
using Bikewale.Models.Scooters;
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
    /// </summary>
    public class ExpertReviewsDetailPage
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
        private readonly IBikeVersionCacheRepository<BikeVersionEntity, uint> _objBikeVersionsCache = null;
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
        #endregion

        #region Public properties
        public bool IsMobile { get; set; }
        public bool IsAMPPage { get; set; }
        public ControllerContext RefControllerContext { get; set; }
        #endregion

        #region Constructor
        public ExpertReviewsDetailPage(ICMSCacheContent cmsCache, IBikeModelsCacheRepository<int> models, IBikeModels<BikeModelEntity, int> bikeModels, IUpcoming upcoming, IBikeInfo bikeInfo, ICityCacheRepository cityCacheRepo,
            IBikeMakesCacheRepository bikeMakesCacheRepository, IBikeVersionCacheRepository<BikeVersionEntity, uint> objBikeVersionsCache, IBikeMaskingCacheRepository<BikeModelEntity, int> bikeMasking, string basicId,
            IPWACMSCacheRepository renderedArticles,IBikeSeriesCacheRepository seriesCache, IBikeSeries series)
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
            _objBikeVersionsCache = objBikeVersionsCache;
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
                    GetWidgetData(objData, widgetTopCount);
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
        /// Created by  : Rajan Chauhan on 26 Feb 2018
        /// Description : Function to get PWA data for expert reviews
        /// Modified by : Ashutosh Sharma on 01 Mar 2018.
        /// Description : Added logic to split article html content into two parts to insert bikeinfo card at 25% height of article content.
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
                    GetWidgetData(objData, widgetTopCount);
                    PopulatePhotoGallery(objData);
                    BindSimilarBikes(objData);
                    SetBikeTested(objData);
                    int matchedPage = InsertBikeInfoWidgetIntoContentPwa(objData.ArticleDetails);
                    objData.ReduxStore = new PwaReduxStore();
                    var newsDetailReducer = objData.ReduxStore.News.NewsDetailReducer;
                    newsDetailReducer.ArticleDetailData.ArticleDetail = ConverterUtility.MapArticleDetailsToPwaExpertReviewDetails(objData.ArticleDetails, matchedPage);
                    newsDetailReducer.ArticleDetailData.ArticleDetail.ImageGallery = ConverterUtility.MapPhotoGalleryToPwaImageList(objData.PhotoGallery);
                    newsDetailReducer.RelatedModelObject.ModelObject = ConverterUtility.MapGenericBikeInfoToPwaBikeInfo(objData.BikeInfo);
                    newsDetailReducer.NewBikesListData.NewBikesList = ConverterUtility.MapNewBikeListToPwaNewBikeList(objData, CityName);
                    newsDetailReducer.NewBikesListData.BikeMakeList = ConverterUtility.MapBikeMakeEntityBaseListToPwaMakeBikeBaseList(objData);
                    var storeJson = JsonConvert.SerializeObject(objData.ReduxStore);
                    objData.ServerRouterWrapper = _renderedArticles.GetNewsDetails(PwaCmsHelper.GetSha256Hash(storeJson), objData.ReduxStore.News.NewsDetailReducer,
                                newsDetailReducer.ArticleDetailData.ArticleDetail.ArticleUrl, "root", "ServerRouterWrapper","Expert Reviews");
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
                    var objSimilarBikes = new SimilarBikesWidget(_objBikeVersionsCache, (uint)objData.Model.ModelId, true, PQSourceEnum.Desktop_NewsDetailsPage);

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
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.ExpertReviewsDetailPage.GetTaggedBikeListByModel - BasicId : " + _basicId);
            }
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 31 Mar 2017
        /// Summary    : Get data for the page widgets
        ///  Modified By Sajal Gupta on 25-04-20187
        /// Descrition : Call most popular bike widget by body type
        /// Modified by Sajal Gupta on 24-08-2017
        /// description : Added Popular Scooter Brands widget
        /// </summary>
        private void GetWidgetData(ExpertReviewsDetailPageVM objData, int topCount)
        {
            try
            {
                currentCityArea = GlobalCityArea.GetGlobalCityArea();
                if (currentCityArea != null)
                    CityId = currentCityArea.CityId;

                objData.BodyStyle = EnumBikeBodyStyles.AllBikes;

                List<BikeVersionMinSpecs> objVersionsList = _objBikeVersionsCache.GetVersionMinSpecs(ModelId, false);

                if (objVersionsList != null && objVersionsList.Count > 0 && objVersionsList.FirstOrDefault() != null)
                {
                    objData.BodyStyle = objVersionsList.FirstOrDefault().BodyStyle;
                }


                objData.IsScooter = objData.BodyStyle.Equals(EnumBikeBodyStyles.Scooter);

                MostPopularBikesWidget objPopularBikes = new MostPopularBikesWidget(_bikeModels, objData.IsScooter ? EnumBikeType.Scooters : EnumBikeType.All, showCheckOnRoadCTA, false, pqSource, pageCatId, MakeId);
                objPopularBikes.TopCount = topCount;
                objPopularBikes.CityId = CityId;
                objData.MostPopularBikes = objPopularBikes.GetData();




                if (objData.MostPopularBikes != null && objData.ArticleDetails != null)
                {
                    objData.MostPopularBikes.CityId = CityId;
                    objData.MostPopularBikes.ReturnUrlForAmpPages = string.Format("{0}/m/expert-reviews/{1}-{2}.html", BWConfiguration.Instance.BwHostUrl, objData.ArticleDetails.ArticleUrl, objData.ArticleDetails.BasicId);
                }
                MostPopularBikeWidgetVM PopularBikesWidget = objData.MostPopularBikes;

                if (ModelId > 0)
                {
                    #region If Model is tagged

                    MostPopularBikeWidgetVM MostPopularBikes = null;
                    MostPopularBikeWidgetVM MostPopularMakeBikes = null;
                    MostPopularBikeWidgetVM MostPopularScooters = null;
                    MostPopularBikeWidgetVM MostPopularMakeScooters = null;
                    UpcomingBikesWidgetVM UpcomingBikes = null;
                    UpcomingBikesWidgetVM UpcomingScooters = null;
                    IEnumerable<BikeMakeEntityBase> PopularScooterMakes = null;
                    PopularBodyStyleVM BodyStyleVM = null;

                    BikeInfoWidget objBikeInfo = new BikeInfoWidget(_bikeInfo, _cityCacheRepo, ModelId, CityId, _totalTabCount, BikeInfoTabType.ExpertReview);
                    objData.BikeInfo = objBikeInfo.GetData();
                    objData.BikeInfo.IsSmallSlug = true;
                    if (objData.IsSeriesAvailable && objData.Make != null)
                    {
                        objData.SeriesBikes = new MostPopularBikeWidgetVM()
                        {
                            Bikes = FetchPopularSeriesBikes(bikeSeriesEntityBase.SeriesId),
                            CityId = CityId,
                            WidgetHeading = string.Format("Popular {0} {1}", bikeSeriesEntityBase.SeriesName,objData.IsScooter ? "Scooters" : "Bikes"),
                            WidgetLinkTitle = string.Format("View all {0} {1}", bikeSeriesEntityBase.SeriesName, objData.IsScooter ? "Scooters" : "Bikes"),
                            WidgetHref = string.Format("/{0}{1}-bikes/{2}/", IsMobile ? "m/" : "",objData.Make.MaskingName, bikeSeriesEntityBase.MaskingName)

                        };
                    }


                    if (IsMobile)
                    {
                        if (objData.BodyStyle.Equals(EnumBikeBodyStyles.Scooter))
                        {
                            PopularScooterBrandsWidget objPopularScooterBrands = new PopularScooterBrandsWidget(_bikeMakesCacheRepository);
                            objPopularScooterBrands.TopCount = 6;
                            objData.PopularScooterMakesWidget = objPopularScooterBrands.GetData();
                            bikeType = EnumBikeType.Scooters;
                        }
                        else
                        {
                            SetPopularBikeByBodyStyleId(objData, topCount);
                        }

                        if (bikeSeriesEntityBase != null)
                        {
                            FetchPopularBikes(objData, bikeSeriesEntityBase.SeriesId);
                            if (objData.IsSeriesAvailable && objData.SeriesWidget != null && objData.Make != null)
                            {
                                objData.MostPopularBikes = new MostPopularBikeWidgetVM()
                                                            {
                                                                Bikes = objData.SeriesWidget.PopularSeriesBikes,
                                                                WidgetHeading = string.Format("Popular {0} {1}", bikeSeriesEntityBase.SeriesName, objData.IsScooter ? "Scooters" : "Bikes"),
                                                                WidgetHref = "/m" +UrlFormatter.BikeSeriesUrl(objData.Make.MaskingName, bikeSeriesEntityBase.MaskingName),
                                                                WidgetLinkTitle = string.Format("View all {0} {1}", bikeSeriesEntityBase.MaskingName, objData.IsScooter ? "Scooters" : "Bikes")
                                                            };
                            }

                        }
                        else if (objData.BodyStyle.Equals(EnumBikeBodyStyles.Scooter) && objData.Make != null)
                        {
                            objData.MostPopularBikes = MostPopularMakeScooters;
                            objData.MostPopularBikes.WidgetHeading = string.Format("Popular {0} Scooters", objData.Make.MakeName);
                            objData.MostPopularBikes.WidgetHref = string.Format("/m/{0}-scooters/", objData.Make.MaskingName);
                            objData.MostPopularBikes.WidgetLinkTitle = string.Format("{0} Scooters", objData.Make.MakeName);
                        }
                    }
                    else
                    {

                        objPopularBikes = new MostPopularBikesWidget(_bikeModels, EnumBikeType.All, showCheckOnRoadCTA, false, pqSource, pageCatId, MakeId);
                        objPopularBikes.TopCount = 9;
                        objPopularBikes.CityId = CityId;

                        MostPopularMakeBikes = objPopularBikes.GetData();

                        objPopularBikes = new MostPopularBikesWidget(_bikeModels, EnumBikeType.All, showCheckOnRoadCTA, false, pqSource, pageCatId);
                        objPopularBikes.TopCount = 9;
                        objPopularBikes.CityId = CityId;
                        MostPopularBikes = objPopularBikes.GetData();

                        if (MostPopularMakeBikes != null && objData.Make != null)
                        {
                            objData.MostPopularMakeBikes = new MostPopularBikeWidgetVM() { Bikes = MostPopularMakeBikes.Bikes.Take(6), WidgetHref = string.Format("/{0}-bikes/", objData.Make.MaskingName), WidgetLinkTitle = "View all Bikes" };
                        }


                        MostPopularBikesWidget objPopularScooters = new MostPopularBikesWidget(_bikeModels, EnumBikeType.Scooters, showCheckOnRoadCTA, false, pqSource, pageCatId, MakeId);
                        objPopularScooters.TopCount = 9;
                        objPopularScooters.CityId = CityId;

                        MostPopularMakeScooters = objPopularScooters.GetData();

                        objPopularScooters = new MostPopularBikesWidget(_bikeModels, EnumBikeType.Scooters, showCheckOnRoadCTA, false, pqSource, pageCatId);
                        objPopularScooters.TopCount = 9;
                        objPopularScooters.CityId = CityId;
                        MostPopularScooters = objPopularScooters.GetData();

                        UpcomingBikesWidget objUpcomingBikes = new UpcomingBikesWidget(_upcoming);
                        objUpcomingBikes.Filters = new UpcomingBikesListInputEntity();
                        objUpcomingBikes.Filters.PageNo = 1;
                        objUpcomingBikes.Filters.PageSize = 6;

                        objUpcomingBikes.SortBy = EnumUpcomingBikesFilter.Default;
                        UpcomingBikes = objUpcomingBikes.GetData();

                        if (UpcomingBikes != null)
                        {
                            objData.UpcomingBikes = new UpcomingBikesWidgetVM
                            {
                                UpcomingBikes = UpcomingBikes.UpcomingBikes.Take(topCount)
                            };
                            objUpcomingBikes.Filters.BodyStyleId = (uint)EnumBikeBodyStyles.Scooter;

                        }

                        UpcomingScooters = objUpcomingBikes.GetData();

                        if (objData.BodyStyle.Equals(EnumBikeBodyStyles.Scooter))
                        {
                            PopularScooterBrandsWidget objPopularScooterBrands = new PopularScooterBrandsWidget(_bikeMakesCacheRepository);
                            objPopularScooterBrands.TopCount = 6;
                            PopularScooterMakes = objPopularScooterBrands.GetData();
                            if (PopularScooterMakes != null)
                            {
                                objData.PopularScooterMakesWidget = PopularScooterMakes.Take(6);
                            }
                            bikeType = EnumBikeType.Scooters;
                        }
                        else
                        {
                            PopularBikesByBodyStyle BodyStyleBikes = new PopularBikesByBodyStyle(_models);
                            BodyStyleBikes.ModelId = ModelId;
                            BodyStyleBikes.CityId = CityId;
                            BodyStyleBikes.TopCount = topCount > 6 ? topCount : 6;
                            BodyStyleVM = BodyStyleBikes.GetData();

                            objData.PopularBodyStyle = BodyStyleVM;

                            if (objData.PopularBodyStyle != null)
                            {
                                objData.PopularBodyStyle.WidgetHeading = string.Format("Popular {0}", objData.PopularBodyStyle.BodyStyleText);
                                objData.PopularBodyStyle.WidgetLinkTitle = string.Format("Best {0} in India", objData.PopularBodyStyle.BodyStyleLinkTitle);
                                objData.PopularBodyStyle.WidgetHref = UrlFormatter.FormatGenericPageUrl(objData.PopularBodyStyle.BodyStyle);
                            }
                        }

                        if (objData.IsScooter)
                        {
                            objData.PopularMakeScootersAndOtherBrandsWidget = new MultiTabsWidgetVM();
                            if (objData.IsSeriesAvailable)
                            {
                                BindSeriesBikesAndOtherBrands(objData, bikeSeriesEntityBase, PopularScooterMakes);
                            }
                            else
                            {
                                BindPopularMakeScootersAndOtherBrandsWidget(objData, MostPopularMakeScooters, PopularScooterMakes, MostPopularScooters, UpcomingScooters);
                            }
                            BindPopularAndUpcomingScooters(objData, MostPopularScooters, UpcomingScooters);
                        }
                        else
                        {

                            BindSportsAndCruisers(objData, MostPopularMakeBikes, objData.BodyStyle);
                            BindPopularAndUpcomingBikes(objData, MostPopularBikes, UpcomingBikes);

                        }

                    }

                    #endregion
                }
                else
                {
                    string urlPrefix = IsMobile ? "m/" : "";
                    UpcomingBikesWidget objUpcomingBikes = new UpcomingBikesWidget(_upcoming);
                    objUpcomingBikes.Filters = new UpcomingBikesListInputEntity();
                    objUpcomingBikes.Filters.PageNo = 1;
                    objUpcomingBikes.Filters.PageSize = topCount;
                    if (MakeId > 0)
                    {
                        objUpcomingBikes.Filters.MakeId = (int)MakeId;
                    }
                    objUpcomingBikes.SortBy = EnumUpcomingBikesFilter.Default;
                    objData.UpcomingBikes = objUpcomingBikes.GetData();

                    if (objData.Make != null)
                    {
                        objData.UpcomingBikes.WidgetHeading = string.Format("Upcoming {0} Bikes", objData.Make.MakeName);
                        objData.UpcomingBikes.WidgetHref = string.Format("/{0}{1}-bikes/upcoming/", urlPrefix, objData.Make.MaskingName);
                    }
                    else
                    {
                        objData.UpcomingBikes.WidgetHeading = "Upcoming Bikes";
                        objData.UpcomingBikes.WidgetHref = string.Format("/{0}upcoming-bikes/", urlPrefix);
                    }
                    objData.UpcomingBikes.WidgetLinkTitle = "Upcoming Bikes in India";
                }



                if (MakeId > 0 && objData.Make != null)
                {
                    BindPopularBikesOrScooters(objData, PopularBikesWidget, objData.BodyStyle);
                }
                else
                {
                    PopularBikesWidget.WidgetHeading = "Popular Bikes";
                    PopularBikesWidget.WidgetHref = string.Format("/{0}best-bikes-in-india/", IsMobile ? "m/" : "");
                    PopularBikesWidget.WidgetLinkTitle = "Best Bikes in India";
                    PopularBikesWidget.CtaText = "View all bikes";
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.ExpertReviewsDetailPage.GetWidgetData");
            }
        }

        /// <summary>
        /// Binds the series bikes and other brands.
        /// </summary>
        /// <param name="objData">The object data.</param>
        /// <param name="bikeSeriesEntityBase">The bike series entity base.</param>
        /// <param name="popularScooterMakes">The popular scooter makes.</param>
        private void BindSeriesBikesAndOtherBrands(ExpertReviewsDetailPageVM objData, BikeSeriesEntityBase bikeSeriesEntityBase, IEnumerable<BikeMakeEntityBase> popularScooterMakes)
        {
            try
            {
                objData.SeriesBikesAndOtherBrands = new MultiTabsWidgetVM();

                objData.SeriesBikesAndOtherBrands.TabHeading1 = string.Format("{0} Scooters", bikeSeriesEntityBase.SeriesName);
                objData.SeriesBikesAndOtherBrands.TabHeading2 = "Other Brands";
                objData.SeriesBikesAndOtherBrands.ViewPath1 = "~/Views/Shared/_EditorialSeriesBikesWidget.cshtml";
                objData.SeriesBikesAndOtherBrands.ViewPath2 = "~/Views/Scooters/_PopularScooterBrandsVerticalWidget.cshtml";
                objData.SeriesBikesAndOtherBrands.TabId1 = "SeriesScooters";
                objData.SeriesBikesAndOtherBrands.TabId2 = "OtherBrands";
                objData.SeriesBikesAndOtherBrands.PopularSeriesBikes = FetchPopularSeriesBikes(bikeSeriesEntityBase.SeriesId);
                if (popularScooterMakes != null)
                    objData.SeriesBikesAndOtherBrands.PopularScooterMakes = popularScooterMakes.Take(6);


                objData.SeriesBikesAndOtherBrands.ViewAllHref1 = UrlFormatter.BikeSeriesUrl(objData.Make.MaskingName, bikeSeriesEntityBase.MaskingName);
                objData.SeriesBikesAndOtherBrands.ViewAllHref2 = "/scooters/";
                objData.SeriesBikesAndOtherBrands.ViewAllTitle1 = string.Format("View all {0} scooters", bikeSeriesEntityBase.SeriesName);
                objData.SeriesBikesAndOtherBrands.ViewAllTitle2 = "View other brands";
                objData.SeriesBikesAndOtherBrands.ViewAllText1 = string.Format("View all {0} scooters", bikeSeriesEntityBase.SeriesName);
                objData.SeriesBikesAndOtherBrands.ViewAllText2 = "View other brands";
                objData.SeriesBikesAndOtherBrands.ShowViewAllLink1 = true;
                objData.SeriesBikesAndOtherBrands.ShowViewAllLink2 = true;
                objData.SeriesBikesAndOtherBrands.Pages = MultiTabWidgetPagesEnum.SeriesBikesAndOtherBrands;
                objData.SeriesBikesAndOtherBrands.PageName = "News";
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.ExpertReviewsDetailPage.BindSeriesBikesAndOtherBrands - BasicId : " + _basicId);
            }
        }

        /// <summary>
        /// Binds the popular bikes or scooters.
        /// </summary>
        /// <param name="objData">The object data.</param>
        /// <param name="PopularBikesWidget">The popular bikes widget.</param>
        /// <param name="bodyStyle">The body style.</param>
        private void BindPopularBikesOrScooters(ExpertReviewsDetailPageVM objData, MostPopularBikeWidgetVM PopularBikesWidget, EnumBikeBodyStyles bodyStyle)
        {
            try
            {
                if (objData.Make != null)
                {
                    string urlPrefix = IsMobile ? "m/" : "";
                    if (bodyStyle.Equals(EnumBikeBodyStyles.Scooter))
                    {
                        PopularBikesWidget.WidgetHeading = string.Format("Popular {0} Scooters", objData.Make.MakeName);
                        if (objData.Make.IsScooterOnly)
                            PopularBikesWidget.WidgetHref = string.Format("/{0}{1}-bikes/", urlPrefix, objData.Make.MaskingName);
                        else
                            PopularBikesWidget.WidgetHref = string.Format("/{0}{1}-scooters/", urlPrefix, objData.Make.MaskingName);
                        PopularBikesWidget.WidgetLinkTitle = string.Format("{0} Scooters", objData.Make.MakeName);
                        PopularBikesWidget.CtaText = "View all scooters";
                    }
                    else
                    {
                        PopularBikesWidget.WidgetHeading = string.Format("Popular {0} Bikes", objData.Make.MakeName);
                        PopularBikesWidget.WidgetHref = string.Format("/{0}{1}-bikes/", urlPrefix, objData.Make.MaskingName);
                        PopularBikesWidget.WidgetLinkTitle = string.Format("{0} Bikes", objData.Make.MakeName);
                        PopularBikesWidget.CtaText = "View all bikes";
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.ExpertReviewsDetailPage.BindPopularBikesOrScooters - BasicId : " + _basicId);
            }
        }

        /// <summary>
        /// Populars the make bikes and body style bikes widget.
        /// </summary>
        /// <param name="objData">The object data.</param>
        /// <param name="MostPopularMakeBikes">The most popular make bikes.</param>
        /// <param name="MostPopularBikes">The most popular bikes.</param>
        /// <param name="UpcomingBikes">The upcoming bikes.</param>
        private void PopularMakeBikesAndBodyStyleBikesWidget(ExpertReviewsDetailPageVM objData, MostPopularBikeWidgetVM MostPopularMakeBikes, MostPopularBikeWidgetVM MostPopularBikes, UpcomingBikesWidgetVM UpcomingBikes, EnumBikeBodyStyles bodyStyle)
        {
            try
            {
                objData.PopularMakeBikesAndBodyStyleBikesWidget.TabHeading1 = string.Format("{0} bikes", objData.Make.MakeName);
                objData.PopularMakeBikesAndBodyStyleBikesWidget.TabHeading2 = bodyStyle.Equals(EnumBikeBodyStyles.Sports) ? "Sports bikes" : "Cruisers";
                objData.PopularMakeBikesAndBodyStyleBikesWidget.ViewPath1 = "~/Views/BikeModels/_MostPopularBikesSideBar.cshtml";
                objData.PopularMakeBikesAndBodyStyleBikesWidget.ViewPath2 = "~/Views/BestBikes/_PopularBodyStyle_Vertical.cshtml";
                objData.PopularMakeBikesAndBodyStyleBikesWidget.TabId1 = "PopularMakeBikes";
                objData.PopularMakeBikesAndBodyStyleBikesWidget.TabId2 = bodyStyle.Equals(EnumBikeBodyStyles.Sports) ? "PopularSportsBikes" : "PopularCruisers";
                objData.PopularMakeBikesAndBodyStyleBikesWidget.MostPopularMakeBikes = MostPopularMakeBikes;

                if (MostPopularMakeBikes != null)
                    objData.PopularMakeBikesAndBodyStyleBikesWidget.MostPopularMakeBikes.Bikes = objData.PopularMakeBikesAndBodyStyleBikesWidget.MostPopularMakeBikes.Bikes.Take(6);

                objData.PopularMakeBikesAndBodyStyleBikesWidget.PopularBodyStyle = new PopularBodyStyleVM() { PopularBikes = objData.PopularBodyStyle.PopularBikes.Take(6) };
                objData.PopularMakeBikesAndBodyStyleBikesWidget.ViewAllHref1 = string.Format("/{0}-bikes/", objData.Make.MaskingName);
                objData.PopularMakeBikesAndBodyStyleBikesWidget.ViewAllHref2 = !bodyStyle.Equals(EnumBikeBodyStyles.Sports) ? "/best-cruiser-bikes-in-india/" : "/best-sports-bikes-in-india/";
                objData.PopularMakeBikesAndBodyStyleBikesWidget.ViewAllTitle1 = "View all bikes";
                objData.PopularMakeBikesAndBodyStyleBikesWidget.ViewAllTitle2 = bodyStyle.Equals(EnumBikeBodyStyles.Sports) ? "View all Sports bikes" : "View all Cruisers";
                objData.PopularMakeBikesAndBodyStyleBikesWidget.ViewAllText1 = "View all bikes";
                objData.PopularMakeBikesAndBodyStyleBikesWidget.ViewAllText2 = bodyStyle.Equals(EnumBikeBodyStyles.Sports) ? "View all Sports bikes" : "View all Cruisers";
                objData.PopularMakeBikesAndBodyStyleBikesWidget.ShowViewAllLink1 = true;
                objData.PopularMakeBikesAndBodyStyleBikesWidget.ShowViewAllLink2 = true;
                objData.PopularMakeBikesAndBodyStyleBikesWidget.Pages = MultiTabWidgetPagesEnum.PopularMakeBikesAndBodyStyleWidget;
                objData.PopularMakeBikesAndBodyStyleBikesWidget.PageName = "ExpertReviews";

                objData.PopularBikesAndUpcomingBikesWidget = new MultiTabsWidgetVM();

                objData.PopularBikesAndUpcomingBikesWidget.TabHeading1 = "Popular bikes";
                objData.PopularBikesAndUpcomingBikesWidget.TabHeading2 = "Upcoming bikes";
                objData.PopularBikesAndUpcomingBikesWidget.ViewPath1 = "~/Views/BikeModels/_MostPopularBikesSideBar.cshtml";
                objData.PopularBikesAndUpcomingBikesWidget.ViewPath2 = "~/Views/Upcoming/_UpcomingBikes_Vertical.cshtml";
                objData.PopularBikesAndUpcomingBikesWidget.TabId1 = "PopularBikes";
                objData.PopularBikesAndUpcomingBikesWidget.TabId2 = "UpcomingBikes";
                objData.PopularBikesAndUpcomingBikesWidget.MostPopularBikes = MostPopularBikes;

                if (MostPopularBikes != null)
                    objData.PopularBikesAndUpcomingBikesWidget.MostPopularBikes.Bikes = objData.PopularBikesAndUpcomingBikesWidget.MostPopularBikes.Bikes.Take(6);

                objData.PopularBikesAndUpcomingBikesWidget.UpcomingBikes = UpcomingBikes;

                if (UpcomingBikes != null)
                    objData.PopularBikesAndUpcomingBikesWidget.UpcomingBikes.UpcomingBikes = objData.PopularBikesAndUpcomingBikesWidget.UpcomingBikes.UpcomingBikes.Take(6);

                objData.PopularBikesAndUpcomingBikesWidget.ViewAllHref1 = "/best-bikes-in-india/";
                objData.PopularBikesAndUpcomingBikesWidget.ViewAllHref2 = "/upcoming-bikes/";
                objData.PopularBikesAndUpcomingBikesWidget.ViewAllTitle1 = "View all bikes";
                objData.PopularBikesAndUpcomingBikesWidget.ViewAllTitle2 = "View all upcoming bikes";
                objData.PopularBikesAndUpcomingBikesWidget.ViewAllText1 = "View all bikes";
                objData.PopularBikesAndUpcomingBikesWidget.ViewAllText2 = "View all upcoming bikes";
                objData.PopularBikesAndUpcomingBikesWidget.ShowViewAllLink1 = true;
                objData.PopularBikesAndUpcomingBikesWidget.ShowViewAllLink2 = true;
                objData.PopularBikesAndUpcomingBikesWidget.Pages = MultiTabWidgetPagesEnum.PopularBikesAndUpcomingBikes;
                objData.PopularBikesAndUpcomingBikesWidget.PageName = "ExpertReviews";
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.ExpertReviewsDetailPage.PopularMakeBikesAndBodyStyleBikesWidget - BasicId : " + _basicId);
            }
        }

        /// <summary>
        /// Binds the popular make scooters and other brands widget.
        /// </summary>
        /// <param name="objData">The object data.</param>
        /// <param name="MostPopularMakeScooters">The most popular make scooters.</param>
        /// <param name="PopularScooterMakes">The popular scooter makes.</param>
        /// <param name="MostPopularScooters">The most popular scooters.</param>
        /// <param name="UpcomingScooters">The upcoming scooters.</param>
        private void BindPopularMakeScootersAndOtherBrandsWidget(ExpertReviewsDetailPageVM objData, MostPopularBikeWidgetVM MostPopularMakeScooters, IEnumerable<BikeMakeEntityBase> PopularScooterMakes, MostPopularBikeWidgetVM MostPopularScooters, UpcomingBikesWidgetVM UpcomingScooters)
        {
            try
            {
                objData.PopularMakeScootersAndOtherBrandsWidget.TabHeading1 = string.Format("{0} Scooters", objData.Make.MakeName);
                objData.PopularMakeScootersAndOtherBrandsWidget.TabHeading2 = "Other Brands";
                objData.PopularMakeScootersAndOtherBrandsWidget.ViewPath1 = "~/Views/BikeModels/_MostPopularBikesSideBar.cshtml";
                objData.PopularMakeScootersAndOtherBrandsWidget.ViewPath2 = "~/Views/Scooters/_PopularScooterBrandsVerticalWidget.cshtml";
                objData.PopularMakeScootersAndOtherBrandsWidget.TabId1 = "PopularMakeScooters";
                objData.PopularMakeScootersAndOtherBrandsWidget.TabId2 = "OtherBrands";
                objData.PopularMakeScootersAndOtherBrandsWidget.MostPopularMakeScooters = MostPopularMakeScooters;

                if (MostPopularMakeScooters != null)
                    objData.PopularMakeScootersAndOtherBrandsWidget.MostPopularMakeScooters.Bikes = objData.PopularMakeScootersAndOtherBrandsWidget.MostPopularMakeScooters.Bikes.Take(6);

                if (PopularScooterMakes != null)
                    objData.PopularMakeScootersAndOtherBrandsWidget.PopularScooterMakes = PopularScooterMakes.Take(6);

                objData.PopularMakeScootersAndOtherBrandsWidget.ViewAllHref1 = string.Format("/{0}-scooters/", objData.Make.MaskingName);
                objData.PopularMakeScootersAndOtherBrandsWidget.ViewAllHref2 = "/scooters/";
                objData.PopularMakeScootersAndOtherBrandsWidget.ViewAllTitle1 = "View all scooters";
                objData.PopularMakeScootersAndOtherBrandsWidget.ViewAllTitle2 = "View other brands";
                objData.PopularMakeScootersAndOtherBrandsWidget.ViewAllText1 = "View all scooters";
                objData.PopularMakeScootersAndOtherBrandsWidget.ViewAllText2 = "View other brands";
                objData.PopularMakeScootersAndOtherBrandsWidget.ShowViewAllLink1 = true;
                objData.PopularMakeScootersAndOtherBrandsWidget.ShowViewAllLink2 = true;
                objData.PopularMakeScootersAndOtherBrandsWidget.Pages = MultiTabWidgetPagesEnum.PopularMakeScootersAndOtherBrands;
                objData.PopularMakeScootersAndOtherBrandsWidget.PageName = "ExpertReviews";

                objData.PopularScootersAndUpcomingScootersWidget = new MultiTabsWidgetVM();

                objData.PopularScootersAndUpcomingScootersWidget.TabHeading1 = "Popular scooters";
                objData.PopularScootersAndUpcomingScootersWidget.TabHeading2 = "Upcoming Scooters";
                objData.PopularScootersAndUpcomingScootersWidget.ViewPath1 = "~/Views/BikeModels/_MostPopularBikesSideBar.cshtml";
                objData.PopularScootersAndUpcomingScootersWidget.ViewPath2 = "~/Views/Upcoming/_UpcomingBikes_Vertical.cshtml";
                objData.PopularScootersAndUpcomingScootersWidget.TabId1 = "PopularScooters";
                objData.PopularScootersAndUpcomingScootersWidget.TabId2 = "UpcomingScooters";
                objData.PopularScootersAndUpcomingScootersWidget.MostPopularScooters = MostPopularScooters;

                if (MostPopularScooters != null)
                    objData.PopularScootersAndUpcomingScootersWidget.MostPopularScooters.Bikes = objData.PopularScootersAndUpcomingScootersWidget.MostPopularScooters.Bikes.Take(6);

                objData.PopularScootersAndUpcomingScootersWidget.UpcomingScooters = UpcomingScooters;

                if (UpcomingScooters != null)
                    objData.PopularScootersAndUpcomingScootersWidget.UpcomingScooters.UpcomingBikes = objData.PopularScootersAndUpcomingScootersWidget.UpcomingScooters.UpcomingBikes.Take(6);
                objData.PopularScootersAndUpcomingScootersWidget.ViewAllHref1 = "/best-scooters-in-india/";
                objData.PopularScootersAndUpcomingScootersWidget.ViewAllTitle1 = "View all scooters";
                objData.PopularScootersAndUpcomingScootersWidget.ViewAllText1 = "View all scooters";
                objData.PopularScootersAndUpcomingScootersWidget.ShowViewAllLink1 = true;
                objData.PopularScootersAndUpcomingScootersWidget.ShowViewAllLink2 = false;
                objData.PopularScootersAndUpcomingScootersWidget.Pages = MultiTabWidgetPagesEnum.PopularScootersAndUpcomingScooters;
                objData.PopularScootersAndUpcomingScootersWidget.PageName = "ExpertReviews";
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.ExpertReviewsDetailPage.BindPopularMakeScootersAndOtherBrandsWidget - BasicId : " + _basicId);
            }
        }

        /// <summary>
        /// Binds the popular and upcoming scooters.
        /// </summary>
        /// <param name="objData">The object data.</param>
        /// <param name="MostPopularScooters">The most popular scooters.</param>
        /// <param name="UpcomingScooters">The upcoming scooters.</param>
        private void BindPopularAndUpcomingScooters(ExpertReviewsDetailPageVM objData, MostPopularBikeWidgetVM MostPopularScooters, UpcomingBikesWidgetVM UpcomingScooters)
        {
            objData.PopularScootersAndUpcomingScootersWidget = new MultiTabsWidgetVM();

            objData.PopularScootersAndUpcomingScootersWidget.TabHeading1 = "Popular scooters";
            objData.PopularScootersAndUpcomingScootersWidget.TabHeading2 = "Upcoming Scooters";
            objData.PopularScootersAndUpcomingScootersWidget.ViewPath1 = "~/Views/BikeModels/_MostPopularBikesSideBar.cshtml";
            objData.PopularScootersAndUpcomingScootersWidget.ViewPath2 = "~/Views/Upcoming/_UpcomingBikes_Vertical.cshtml"; ;
            objData.PopularScootersAndUpcomingScootersWidget.TabId1 = "PopularScooters";
            objData.PopularScootersAndUpcomingScootersWidget.TabId2 = "UpcomingScooters";
            objData.PopularScootersAndUpcomingScootersWidget.MostPopularScooters = MostPopularScooters;

            if (MostPopularScooters != null)
                objData.PopularScootersAndUpcomingScootersWidget.MostPopularScooters.Bikes = objData.PopularScootersAndUpcomingScootersWidget.MostPopularScooters.Bikes.Take(6);

            objData.PopularScootersAndUpcomingScootersWidget.UpcomingScooters = UpcomingScooters;

            if (UpcomingScooters != null)
                objData.PopularScootersAndUpcomingScootersWidget.UpcomingScooters.UpcomingBikes = objData.PopularScootersAndUpcomingScootersWidget.UpcomingScooters.UpcomingBikes.Take(6);
            objData.PopularScootersAndUpcomingScootersWidget.ViewAllHref1 = "/best-scooters-in-india/";
            objData.PopularScootersAndUpcomingScootersWidget.ViewAllTitle1 = "View all scooters";
            objData.PopularScootersAndUpcomingScootersWidget.ViewAllText1 = "View all scooters";
            objData.PopularScootersAndUpcomingScootersWidget.ShowViewAllLink1 = true;
            objData.PopularScootersAndUpcomingScootersWidget.ShowViewAllLink2 = false;
            objData.PopularScootersAndUpcomingScootersWidget.Pages = MultiTabWidgetPagesEnum.PopularScootersAndUpcomingScooters;
            objData.PopularScootersAndUpcomingScootersWidget.PageName = "News";
        }


        /// <summary>
        /// Binds the series bikes and model body style bikes.
        /// </summary>
        /// <param name="objData">The object data.</param>
        /// <param name="bikeSeriesEntityBase">The bike series entity base.</param>
        /// <param name="bodyStyles">The body styles.</param>
        private void BindSeriesBikesAndModelBodyStyleBikes(ExpertReviewsDetailPageVM objData, BikeSeriesEntityBase bikeSeriesEntityBase, EnumBikeBodyStyles bodyStyles)
        {
            try
            {
                if (objData.SeriesWidget != null && objData.Make != null && objData.Make.MaskingName != null && bikeSeriesEntityBase != null)
                {
                    objData.SeriesBikesAndModelBodyStyleBikes = new MultiTabsWidgetVM()
                    {
                        TabHeading1 = string.Format("{0} {1}", bikeSeriesEntityBase.SeriesName, bodyStyles == EnumBikeBodyStyles.Scooter ? "Scooters" : "Bikes"),
                        ViewPath1 = "~/Views/Shared/_EditorialSeriesBikesWidget.cshtml",
                        TabId1 = "SeriesBikes",
                        ViewAllHref1 = string.Format("/{0}-bikes/{1}/", objData.Make.MaskingName, bikeSeriesEntityBase.MaskingName),
                        ViewAllTitle1 = string.Format("View all {0} {1}", bikeSeriesEntityBase.SeriesName, bodyStyles == EnumBikeBodyStyles.Scooter ? "scooters" : "bikes"),
                        ViewAllText1 = string.Format("View all {0} {1}", bikeSeriesEntityBase.SeriesName, bodyStyles == EnumBikeBodyStyles.Scooter ? "scooters" : "bikes"),
                        ShowViewAllLink1 = true,
                        PopularSeriesBikes = objData.SeriesWidget.PopularSeriesBikes,


                        TabHeading2 = string.Format("Popular {0}", Bikewale.Utility.BodyStyleLinks.BodyStyleText(bodyStyles)),
                        ViewPath2 = "~/Views/BestBikes/_PopularBodyStyle_Vertical.cshtml",
                        TabId2 = "PopularBodyStyle",
                        ViewAllHref2 = bodyStyles == EnumBikeBodyStyles.Scooter ? "/best-scooters-in-india/" : (bodyStyles == EnumBikeBodyStyles.Sports ? "/best-sports-bikes-in-india/" : (bodyStyles == EnumBikeBodyStyles.Cruiser ? "/best-cruiser-bikes-in-india/" : "/best-bikes-in-india/")),
                        ViewAllTitle2 = string.Format("View all popular {0}", Bikewale.Utility.BodyStyleLinks.BodyStyleText(bodyStyles).ToLower()),
                        ViewAllText2 = string.Format("View all popular {0}", Bikewale.Utility.BodyStyleLinks.BodyStyleText(bodyStyles).ToLower()),
                        ShowViewAllLink2 = true,
                        PopularBodyStyle = new PopularBodyStyleVM() { PopularBikes = objData.PopularBodyStyle.PopularBikes.Take(6) },
                        Pages = MultiTabWidgetPagesEnum.SeriesBikesAndModelBodyStyleBike,
                        PageName = "News"
                    };
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.ExpertReviews.ExpertReviewsDetailPage.BindSeriesBikesAndModelBodyStyleBikes - BasicId : " + _basicId);
            }
        }

        /// <summary>
        /// Binds the sports and cruisers.
        /// </summary>
        /// <param name="objData">The object data.</param>
        /// <param name="MostPopularMakeBikes">The most popular make bikes.</param>
        /// <param name="bodyStyle">The body style.</param>
        private void BindSportsAndCruisers(ExpertReviewsDetailPageVM objData, MostPopularBikeWidgetVM MostPopularMakeBikes, EnumBikeBodyStyles bodyStyle)
        {
            try
            {
                if (objData.IsSeriesAvailable)
                {
                    FetchBikesByBodyStyle(objData, bodyStyle);
                    BindSeriesBikesAndModelBodyStyleBikes(objData, bikeSeriesEntityBase, bodyStyle);
                }
                else
                {
                    objData.PopularMakeBikesAndBodyStyleBikesWidget = new MultiTabsWidgetVM();

                    objData.PopularMakeBikesAndBodyStyleBikesWidget.TabHeading1 = string.Format("{0} bikes", objData.Make.MakeName);
                    objData.PopularMakeBikesAndBodyStyleBikesWidget.TabHeading2 = bodyStyle.Equals(EnumBikeBodyStyles.Sports) ? "Sports bikes" : "Cruisers";
                    objData.PopularMakeBikesAndBodyStyleBikesWidget.ViewPath1 = "~/Views/BikeModels/_MostPopularBikesSideBar.cshtml";
                    objData.PopularMakeBikesAndBodyStyleBikesWidget.ViewPath2 = "~/Views/BestBikes/_PopularBodyStyle_Vertical.cshtml";
                    objData.PopularMakeBikesAndBodyStyleBikesWidget.TabId1 = "PopularMakeBikes";
                    objData.PopularMakeBikesAndBodyStyleBikesWidget.TabId2 = bodyStyle.Equals(EnumBikeBodyStyles.Sports) ? "PopularSportsBikes" : "PopularCruisers";
                    objData.PopularMakeBikesAndBodyStyleBikesWidget.MostPopularMakeBikes = MostPopularMakeBikes;

                    if (MostPopularMakeBikes != null)
                        objData.PopularMakeBikesAndBodyStyleBikesWidget.MostPopularMakeBikes.Bikes = objData.PopularMakeBikesAndBodyStyleBikesWidget.MostPopularMakeBikes.Bikes.Take(6);

                    objData.PopularMakeBikesAndBodyStyleBikesWidget.PopularBodyStyle = new PopularBodyStyleVM() { PopularBikes = objData.PopularBodyStyle.PopularBikes.Take(6) };
                    objData.PopularMakeBikesAndBodyStyleBikesWidget.ViewAllHref1 = string.Format("/{0}-bikes/", objData.Make.MaskingName);
                    objData.PopularMakeBikesAndBodyStyleBikesWidget.ViewAllHref2 = !bodyStyle.Equals(EnumBikeBodyStyles.Sports) ? "/best-cruiser-bikes-in-india/" : "/best-sports-bikes-in-india/";
                    objData.PopularMakeBikesAndBodyStyleBikesWidget.ViewAllTitle1 = "View all bikes";
                    objData.PopularMakeBikesAndBodyStyleBikesWidget.ViewAllTitle2 = bodyStyle.Equals(EnumBikeBodyStyles.Sports) ? "View all Sports bikes" : "View all Cruisers";
                    objData.PopularMakeBikesAndBodyStyleBikesWidget.ViewAllText1 = "View all bikes";
                    objData.PopularMakeBikesAndBodyStyleBikesWidget.ViewAllText2 = bodyStyle.Equals(EnumBikeBodyStyles.Sports) ? "View all Sports bikes" : "View all Cruisers";
                    objData.PopularMakeBikesAndBodyStyleBikesWidget.ShowViewAllLink1 = true;
                    objData.PopularMakeBikesAndBodyStyleBikesWidget.ShowViewAllLink2 = true;
                    objData.PopularMakeBikesAndBodyStyleBikesWidget.Pages = MultiTabWidgetPagesEnum.PopularMakeBikesAndBodyStyleWidget;
                    objData.PopularMakeBikesAndBodyStyleBikesWidget.PageName = "News";
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.ExpertReviews.ExpertReviewsDetailPage.BindSportsAndCruisers - BasicId : " + _basicId);
            }
        }

        /// <summary>
        /// Binds the popular and upcoming bikes.
        /// 
        /// Modified by : Sanskar Gupta on 22 Jan 2018
        /// Description : Added Newly Launched feature
        /// </summary>
        /// <param name="objData">The object data.</param>
        /// <param name="MostPopularBikes">The most popular bikes.</param>
        /// <param name="UpcomingBikes">The upcoming bikes.</param>
        private void BindPopularAndUpcomingBikes(ExpertReviewsDetailPageVM objData, MostPopularBikeWidgetVM MostPopularBikes, UpcomingBikesWidgetVM UpcomingBikes)
        {
            try
            {
                objData.PopularBikesAndUpcomingBikesWidget = new MultiTabsWidgetVM();

                objData.PopularBikesAndUpcomingBikesWidget.TabHeading1 = "Popular bikes";
                objData.PopularBikesAndUpcomingBikesWidget.TabHeading2 = "Upcoming bikes";
                objData.PopularBikesAndUpcomingBikesWidget.ViewPath1 = "~/Views/BikeModels/_MostPopularBikesSideBar.cshtml";
                objData.PopularBikesAndUpcomingBikesWidget.ViewPath2 = "~/Views/Upcoming/_UpcomingBikes_Vertical.cshtml";
                objData.PopularBikesAndUpcomingBikesWidget.TabId1 = "PopularBikes";
                objData.PopularBikesAndUpcomingBikesWidget.TabId2 = "UpcomingBikes";
                objData.PopularBikesAndUpcomingBikesWidget.MostPopularBikes = MostPopularBikes;

                if (MostPopularBikes != null)
                    objData.PopularBikesAndUpcomingBikesWidget.MostPopularBikes.Bikes = objData.PopularBikesAndUpcomingBikesWidget.MostPopularBikes.Bikes.Take(6);

                BikeFilters obj = new BikeFilters();
                obj.CityId = CityId;
                IEnumerable<MostPopularBikesBase> promotedBikes = _bikeModels.GetAdPromotedBike(obj, true);
                objData.PopularBikesAndUpcomingBikesWidget.MostPopularBikes.Bikes = _bikeModels.GetAdPromoteBikeFilters(promotedBikes, objData.PopularBikesAndUpcomingBikesWidget.MostPopularBikes.Bikes);

                objData.PopularBikesAndUpcomingBikesWidget.UpcomingBikes = UpcomingBikes;

                if (UpcomingBikes != null)
                    objData.PopularBikesAndUpcomingBikesWidget.UpcomingBikes.UpcomingBikes = objData.PopularBikesAndUpcomingBikesWidget.UpcomingBikes.UpcomingBikes.Take(6);

                objData.PopularBikesAndUpcomingBikesWidget.ViewAllHref1 = "/best-bikes-in-india/";
                objData.PopularBikesAndUpcomingBikesWidget.ViewAllHref2 = "/upcoming-bikes/";
                objData.PopularBikesAndUpcomingBikesWidget.ViewAllTitle1 = "View all bikes";
                objData.PopularBikesAndUpcomingBikesWidget.ViewAllTitle2 = "View all upcoming bikes";
                objData.PopularBikesAndUpcomingBikesWidget.ViewAllText1 = "View all bikes";
                objData.PopularBikesAndUpcomingBikesWidget.ViewAllText2 = "View all upcoming bikes";
                objData.PopularBikesAndUpcomingBikesWidget.ShowViewAllLink1 = true;
                objData.PopularBikesAndUpcomingBikesWidget.ShowViewAllLink2 = true;
                objData.PopularBikesAndUpcomingBikesWidget.Pages = MultiTabWidgetPagesEnum.PopularBikesAndUpcomingBikes;
                objData.PopularBikesAndUpcomingBikesWidget.PageName = "News";
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.ExpertReviews.ExpertReviewsDetailPage.BindPopularAndUpcomingBikes - BasicId : " + _basicId);
            }
        }

        private void SetPopularBikeByBodyStyleId(ExpertReviewsDetailPageVM objData, int topCount)
        {
            try
            {
                if (objData != null && topCount > 0)
                {
                    PopularBikesByBodyStyle objPopularStyle = new PopularBikesByBodyStyle(_models);
                    objPopularStyle.ModelId = ModelId;
                    objPopularStyle.CityId = CityId;
                    objPopularStyle.TopCount = topCount;
                    objData.PopularBodyStyle = objPopularStyle.GetData();

                    if (objData.PopularBodyStyle != null && objData.ArticleDetails != null)
                    {
                        objData.PopularBodyStyle.WidgetHeading = string.Format("Popular {0}", objData.PopularBodyStyle.BodyStyleText);
                        objData.PopularBodyStyle.WidgetLinkTitle = string.Format("Best {0} in India", objData.PopularBodyStyle.BodyStyleLinkTitle);
                        objData.PopularBodyStyle.WidgetHref = string.Format("{0}{1}",IsMobile ? "/m" : "", UrlFormatter.FormatGenericPageUrl(objData.PopularBodyStyle.BodyStyle));
                        objData.PopularBodyStyle.CityId = CityId;
                        objData.PopularBodyStyle.ReturnUrlForAmpPages = string.Format("{0}/m/expert-reviews/{1}-{2}.html", BWConfiguration.Instance.BwHostUrl, objData.ArticleDetails.ArticleUrl, objData.ArticleDetails.BasicId);
                        bikeType = objData.PopularBodyStyle.BodyStyle == EnumBikeBodyStyles.Scooter ? EnumBikeType.Scooters : EnumBikeType.All;

                        if (bikeType == EnumBikeType.All && string.Compare(objData.PopularBodyStyle.WidgetHeading, "Popular bikes") == 0)
                        {
                            BikeFilters obj = new BikeFilters();
                            obj.CityId = CityId;
                            IEnumerable<MostPopularBikesBase> promotedBikes = _bikeModels.GetAdPromotedBike(obj, true);
                            objData.PopularBodyStyle.PopularBikes = _bikeModels.GetAdPromoteBikeFilters(promotedBikes, objData.PopularBodyStyle.PopularBikes);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.ExpertReviews.ExpertReviewsDetailPage.SetPopularBikeByBodyStyleId - BasicId : " + _basicId);
            }
        }

        /// <summary>
        /// Fetches the bikes by body style.
        /// Modified by : Ashutosh Sharma on 27 Dec 2017
        /// Description : Added cityId to fetch bikes with city price.
        /// </summary>
        /// <param name="objData">The object data.</param>
        /// <param name="bodyStyle">The body style.</param>
        private void FetchBikesByBodyStyle(ExpertReviewsDetailPageVM objData, EnumBikeBodyStyles bodyStyle)
        {
            objData.SeriesWidget = new EditorialSeriesWidgetVM();
            try
            {
                if (!IsMobile)
                {
                    // Fetch Upcoming bikes
                    UpcomingBikesListInputEntity filters = new UpcomingBikesListInputEntity()
                    {
                        PageNo = 1,
                        PageSize = 6,
                        BodyStyleId = (uint)bodyStyle
                    };
                    objData.SeriesWidget.UpcomingBikesByBodyStyle = _upcoming.GetModels(filters, EnumUpcomingBikesFilter.Default);
                }

                // Popular BodyStyles
                IEnumerable<BestBikeEntityBase> bestBikesByBodyStyle = _models.GetBestBikesByCategory(bodyStyle, CityId);
                if (bestBikesByBodyStyle != null && bestBikesByBodyStyle.Any())
                {
                    objData.SeriesWidget.PopularBikesByBodyStyle = bestBikesByBodyStyle.Take(6);

                }
                objData.SeriesWidget.PopularSeriesBikes = FetchPopularSeriesBikes(bikeSeriesEntityBase.SeriesId);
                objData.SeriesWidget.SeriesName = bikeSeriesEntityBase.SeriesName;
                objData.SeriesWidget.WidgetLink = string.Format("/{0}-bikes/{1}/", objData.Make.MaskingName, bikeSeriesEntityBase.MaskingName);
                objData.SeriesWidget.WidgetLinkTitle = string.Format("View all {0} {1}", bikeSeriesEntityBase.SeriesName, "bikes");
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Models.ExpertReviews.ExpertReviewsDetailPage.FetchBikesByBodyStyle - BasicId : " + _basicId);
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


        private void InsertBikeInfoWidgetIntoContent(ExpertReviewsDetailPageVM objData)
        {
            try
            {
                if (objData.ArticleDetails != null && objData.ArticleDetails.PageList != null && objData.BikeInfo != null)
                {
                    int totalStrippedHTMLLength = 0, matchedPage = 0, currentPageLength = 0, requiredLength = 0, totalPages = objData.ArticleDetails.PageList.Count; string inputString = null, viewName = null;
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
                        currentPageLength += item.Item2;
                        if (currentPageLength >= requiredLength)
                        {
                            matchedPage = item.Item1;
                            requiredLength = currentPageLength - requiredLength;
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
                    int totalStrippedHTMLLength = 0, currentPageLength = 0, requiredLength = 0, totalPages = articleDetails.PageList.Count; string inputString = null, viewName = null;
                    IList<Tuple<int, int>> objPagesInfo = new List<Tuple<int, int>>();
                    Shared.BikeInfo ampBikeInfo = null;

                    

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
                        currentPageLength += item.Item2;
                        if (currentPageLength >= requiredLength)
                        {
                            matchedPage = item.Item1;
                            requiredLength = currentPageLength - requiredLength;
                            break;
                        }

                    }

                    

                    
                        string topContentInPage = string.Empty, bottomContentInPage = string.Empty;
                        StringHtmlHelpers.InsertHTMLBetweenHTMLPwa(articleDetails.PageList[matchedPage].Content, requiredLength,out topContentInPage,out bottomContentInPage);

                        articleDetails.PageList[matchedPage].Content = topContentInPage;
                        if (matchedPage != articleDetails.PageList.Count - 1)
                        {
                            articleDetails.PageList[matchedPage + 1].Content = bottomContentInPage + articleDetails.PageList[matchedPage + 1].Content;
                        }
                        else
                        {
                            articleDetails.PageList.Add(new Page()
                            {
                                Content = bottomContentInPage,
                                PageName = "",
                                pageId = articleDetails.PageList[matchedPage].pageId + 1,
                                Priority = articleDetails.PageList.Max(p => p.Priority)
                            });
                        }
                    
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.ExpertReviewsDetailPage.InsertBikeInfoWidgetIntoContentPwa : {0}" , articleDetails));
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
        /// Fetches the popular series bikes.
        /// </summary>
        /// <param name="seriesId">The series identifier.</param>
        /// <returns></returns>
        private IEnumerable<MostPopularBikesBase> FetchPopularSeriesBikes(uint seriesId)
        {
            IEnumerable<MostPopularBikesBase> popularSeriesBikes = null;
            try
            {
                popularSeriesBikes = _models.GetMostPopularBikesByMakeWithCityPrice((int)MakeId, CityId);
                string modelIds = string.Empty;
                if (seriesId > 0)
                    modelIds = _series.GetModelIdsBySeries(seriesId);
                string[] modelArray = modelIds.Split(',');
                if (modelArray.Length > 0 && popularSeriesBikes != null)
                {
                    popularSeriesBikes = (from bike in popularSeriesBikes
                                          where modelArray.Contains(bike.objModel.ModelId.ToString())
                                          select bike
                                         );
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Models.News.NewsIndexPage.FetchPopularBikes - BasicId : " + _basicId);
            }
            return popularSeriesBikes;
        }

        /// <summary>
        /// Fetches the popular bikes.
        /// Modified by : Rajan Chauhan on 28 Dec 2017
        /// Description : Removed the PopularMakeSeriesBikes from objData SeriesWidget
        /// </summary>
        private void FetchPopularBikes(ExpertReviewsDetailPageVM objData, uint seriesId = 0)
        {
            try
            {
                objData.SeriesWidget = new EditorialSeriesWidgetVM();
                IEnumerable<MostPopularBikesBase> makePopularBikes = _models.GetMostPopularBikesByMake((int)MakeId);
                string modelIds = string.Empty;
                modelIds = _series.GetModelIdsBySeries(seriesId);
                string[] modelArray = modelIds.Split(',');
                if (modelArray.Length > 0)
                {
                    var popularSeries = (from bike in makePopularBikes
                                         where modelArray.Contains(bike.objModel.ModelId.ToString())
                                         select bike
                                         ).ToList<MostPopularBikesBase>();
                    if (popularSeries != null && popularSeries.Any())
                        objData.SeriesWidget.PopularSeriesBikes = popularSeries.Take(6);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Models.News.NewsIndexPage.FetchPopularBikes - BasicId : " + _basicId);
            }

        }
        #endregion
    }
}