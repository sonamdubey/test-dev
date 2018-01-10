using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Location;
using Bikewale.Memcache;
using Bikewale.Models.BestBikes;
using Bikewale.Models.BikeModels;
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
    /// </summary>
    public class DetailPage
    {
        #region Variables for dependency injection and constructor
        private readonly ICMSCacheContent _cache = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly IBikeModelsCacheRepository<int> _models = null;
        private GlobalCityAreaEntity currentCityArea;
        private readonly IBikeVersionCacheRepository<BikeVersionEntity, uint> _objBikeVersionsCache = null;
        private readonly bool showCheckOnRoadCTA = false;
        private readonly IBikeInfo _bikeInfo;
        private readonly ICityCacheRepository _cityCacheRepo;
        private readonly IBikeMakesCacheRepository _bikeMakesCacheRepository = null;
        #endregion

        #region Page level variables
        private uint basicId;
        public StatusCodes status;
        public string mappedCWId;
        public string redirectUrl;
        public bool IsMobile { get; set; }
        public bool IsAMPPage { get; set; }
        private readonly uint _totalTabCount = 3;
        private BikeInfoTabType _pageId = BikeInfoTabType.Features;
        private PQSourceEnum pqSource = 0;
        private string _basicId;
        private uint CityId, MakeId, ModelId, pageCatId = 0;
        private EnumBikeType bikeType = EnumBikeType.All;
        #endregion

        #region Constructor
        public DetailPage(ICMSCacheContent cache, IUpcoming upcoming, IBikeModels<BikeModelEntity, int> bikeModels, IBikeModelsCacheRepository<int> models, string basicId, IBikeVersionCacheRepository<BikeVersionEntity, uint> objBikeVersionsCache, IBikeInfo bikeInfo, ICityCacheRepository cityCacheRepo, IBikeMakesCacheRepository bikeMakesCacheRepository)
        {
            _cache = cache;
            _upcoming = upcoming;
            _bikeModels = bikeModels;
            _models = models;
            _basicId = basicId;
            _objBikeVersionsCache = objBikeVersionsCache;
            _bikeMakesCacheRepository = bikeMakesCacheRepository;
            _cityCacheRepo = cityCacheRepo;
            _bikeInfo = bikeInfo;
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
                    GetWidgetData(objDetailsVM, widgetTopCount);
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

                if (IsMobile || IsAMPPage)
                {

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
                            objData.PopularBodyStyle.CityId = CityId;
                            objData.PopularBodyStyle.ReturnUrlForAmpPages = string.Format("{0}/m/features/{1}-{2}/", BWConfiguration.Instance.BwHostUrl, objData.objFeature.ArticleUrl, objData.objFeature.BasicId);
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
                    MostPopularBikesWidget objPopularBikes = new MostPopularBikesWidget(_bikeModels, bikeType, false, false, 0, 0, (objData.objMake != null) ? (uint)objData.objMake.MakeId : 0);
                    objPopularBikes.TopCount = topCount;
                    objPopularBikes.CityId = CityId;
                    objData.MostPopularBikes = objPopularBikes.GetData();
                    if (objData.MostPopularBikes != null)
                    {
                        objData.MostPopularBikes.CityId = CityId;
                        objData.MostPopularBikes.ReturnUrlForAmpPages = string.Format("{0}/m/features/{1}-{2}/", BWConfiguration.Instance.BwHostUrl, objData.objFeature.ArticleUrl, objData.objFeature.BasicId);
                    }
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
                else
                {
                    MostPopularBikeWidgetVM MostPopularBikes = null;
                    MostPopularBikeWidgetVM MostPopularScooters = null;
                    UpcomingBikesWidgetVM UpcomingBikes = null;
                    UpcomingBikesWidgetVM UpcomingScooters = null;

                    MostPopularBikesWidget objPopularBikes = new MostPopularBikesWidget(_bikeModels, EnumBikeType.All, false, false);
                    objPopularBikes.TopCount = topCount > 6 ? topCount : 6;
                    objPopularBikes.CityId = CityId;
                    MostPopularBikes = objPopularBikes.GetData();

                    MostPopularBikesWidget objPopularScooters = new MostPopularBikesWidget(_bikeModels, EnumBikeType.Scooters, false, false);
                    objPopularScooters.TopCount = topCount > 6 ? topCount : 6;
                    objPopularScooters.CityId = CityId;
                    MostPopularScooters = objPopularScooters.GetData();

                    UpcomingBikesWidget objUpcomingBikes = new UpcomingBikesWidget(_upcoming);
                    objUpcomingBikes.Filters = new UpcomingBikesListInputEntity();
                    objUpcomingBikes.Filters.PageNo = 1;
                    objUpcomingBikes.Filters.PageSize = topCount > 6 ? topCount : 6;
                    objUpcomingBikes.SortBy = EnumUpcomingBikesFilter.Default;
                    UpcomingBikes = objUpcomingBikes.GetData();
                    objData.UpcomingBikes = new UpcomingBikesWidgetVM
                    {
                        UpcomingBikes = UpcomingBikes.UpcomingBikes.Take(topCount)
                    };
                    objUpcomingBikes.Filters.BodyStyleId = (uint)EnumBikeBodyStyles.Scooter;
                    UpcomingScooters = objUpcomingBikes.GetData();

                    objData.UpcomingBikesAndUpcomingScootersWidget = new MultiTabsWidgetVM();

                    objData.UpcomingBikesAndUpcomingScootersWidget.TabHeading1 = "Upcoming bikes";
                    objData.UpcomingBikesAndUpcomingScootersWidget.TabHeading2 = "Upcoming scooters";
                    objData.UpcomingBikesAndUpcomingScootersWidget.ViewPath1 = "~/Views/Upcoming/_UpcomingBikes_Vertical.cshtml";
                    objData.UpcomingBikesAndUpcomingScootersWidget.ViewPath2 = "~/Views/Upcoming/_UpcomingBikes_Vertical.cshtml";
                    objData.UpcomingBikesAndUpcomingScootersWidget.TabId1 = "UpcomingBikes";
                    objData.UpcomingBikesAndUpcomingScootersWidget.TabId2 = "UpcomingScooters";
                    objData.UpcomingBikesAndUpcomingScootersWidget.UpcomingBikes = UpcomingBikes;

                    if (UpcomingBikes != null)
                        objData.UpcomingBikesAndUpcomingScootersWidget.UpcomingBikes.UpcomingBikes = objData.UpcomingBikesAndUpcomingScootersWidget.UpcomingBikes.UpcomingBikes.Take(6);

                    objData.UpcomingBikesAndUpcomingScootersWidget.UpcomingScooters = UpcomingScooters;

                    if (UpcomingScooters != null)
                        objData.UpcomingBikesAndUpcomingScootersWidget.UpcomingScooters.UpcomingBikes = objData.UpcomingBikesAndUpcomingScootersWidget.UpcomingScooters.UpcomingBikes.Take(6);

                    objData.UpcomingBikesAndUpcomingScootersWidget.ViewAllHref1 = "/upcoming-bikes/";
                    objData.UpcomingBikesAndUpcomingScootersWidget.ViewAllTitle1 = "View all upcoming bikes";
                    objData.UpcomingBikesAndUpcomingScootersWidget.ViewAllText1 = "View all upcoming bikes";
                    objData.UpcomingBikesAndUpcomingScootersWidget.ShowViewAllLink1 = true;
                    objData.UpcomingBikesAndUpcomingScootersWidget.ShowViewAllLink2 = false;
                    objData.UpcomingBikesAndUpcomingScootersWidget.Pages = MultiTabWidgetPagesEnum.UpcomingBikesAndUpcomingScooters;
                    objData.UpcomingBikesAndUpcomingScootersWidget.PageName = "Features";

                    objData.PopularBikesAndPopularScootersWidget = new MultiTabsWidgetVM();

                    objData.PopularBikesAndPopularScootersWidget.TabHeading1 = "Popular bikes";
                    objData.PopularBikesAndPopularScootersWidget.TabHeading2 = "Popular scooters";
                    objData.PopularBikesAndPopularScootersWidget.ViewPath1 = "~/Views/BikeModels/_MostPopularBikesSideBar.cshtml";
                    objData.PopularBikesAndPopularScootersWidget.ViewPath2 = "~/Views/BikeModels/_MostPopularBikesSideBar.cshtml";
                    objData.PopularBikesAndPopularScootersWidget.TabId1 = "PopularBikes";
                    objData.PopularBikesAndPopularScootersWidget.TabId2 = "PopularScooters";
                    objData.PopularBikesAndPopularScootersWidget.MostPopularBikes = MostPopularBikes;

                    if (MostPopularBikes != null)
                        objData.PopularBikesAndPopularScootersWidget.MostPopularBikes.Bikes = objData.PopularBikesAndPopularScootersWidget.MostPopularBikes.Bikes.Take(6);

                    objData.PopularBikesAndPopularScootersWidget.MostPopularScooters = MostPopularScooters;

                    if (MostPopularScooters != null)
                        objData.PopularBikesAndPopularScootersWidget.MostPopularScooters.Bikes = objData.PopularBikesAndPopularScootersWidget.MostPopularScooters.Bikes.Take(6);

                    objData.PopularBikesAndPopularScootersWidget.ViewAllHref2 = "/best-scooters-in-india/";
                    objData.PopularBikesAndPopularScootersWidget.ViewAllHref1 = "/best-bikes-in-india/";
                    objData.PopularBikesAndPopularScootersWidget.ViewAllTitle1 = "View all bikes";
                    objData.PopularBikesAndPopularScootersWidget.ViewAllTitle2 = "View all scooters";
                    objData.PopularBikesAndPopularScootersWidget.ViewAllText1 = "View all bikes";
                    objData.PopularBikesAndPopularScootersWidget.ViewAllText2 = "View all scooters";
                    objData.PopularBikesAndPopularScootersWidget.ShowViewAllLink1 = true;
                    objData.PopularBikesAndPopularScootersWidget.ShowViewAllLink2 = true;
                    objData.PopularBikesAndPopularScootersWidget.Pages = MultiTabWidgetPagesEnum.PopularBikesAndPopularScooters;
                    objData.PopularBikesAndPopularScootersWidget.PageName = "Features";
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.Features.DetailPage.GetWidgetData");
            }
        }

        private void SetPopularBikeByBodyStyleId(DetailFeatureVM objData, int topCount)
        {
            if (objData != null && topCount > 0)
            {
                PopularBikesByBodyStyle objPopularStyle = new PopularBikesByBodyStyle(_models);
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
                objPage.PageMetaTags.CanonicalUrl = string.Format("{0}/features/{1}-{2}/", BWConfiguration.Instance.BwHostUrl, objPage.objFeature.ArticleUrl, objPage.objFeature.BasicId);
                objPage.PageMetaTags.AlternateUrl = string.Format("{0}/m/features/{1}-{2}/", BWConfiguration.Instance.BwHostUrl, objPage.objFeature.ArticleUrl, objPage.objFeature.BasicId);
                objPage.PageMetaTags.AmpUrl = string.Format("{0}/m/features/{1}-{2}/amp/", BWConfiguration.Instance.BwHostUrl, objPage.objFeature.ArticleUrl, objPage.objFeature.BasicId);
                objPage.PageMetaTags.Title = string.Format("{0} - Bikewale ", objPage.objFeature.Title);
                objPage.PageMetaTags.Description = string.Format("Read about {0}. Read through more bike care tips to learn more about your bike maintenance.", objPage.objFeature.Title);
                objPage.PageMetaTags.Keywords = string.Format("features, stories, travelogues, specials, drives.");
                objPage.PageMetaTags.ShareImage = Bikewale.Utility.Image.GetPathToShowImages(objPage.objFeature.OriginalImgUrl, objPage.objFeature.HostUrl, ImageSize._640x348);
                objPage.Page_H1 = objPage.objFeature.Title;

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
            objData.PageMetaTags.PageSchemaJSON = Newtonsoft.Json.JsonConvert.SerializeObject(objSchema);

            WebPage webpage = SchemaHelper.GetWebpageSchema(objData.PageMetaTags, objData.BreadcrumbList);

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
        #endregion
    }
}