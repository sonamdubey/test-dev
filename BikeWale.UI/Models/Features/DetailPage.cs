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
using Bikewale.Models.Scooters;
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
                currentCityArea = GlobalCityArea.GetGlobalCityArea();
                if (currentCityArea != null)
                    CityId = currentCityArea.CityId;

                EnumBikeBodyStyles bodyStyle = EnumBikeBodyStyles.AllBikes;

                List<BikeVersionMinSpecs> objVersionsList = _objBikeVersionsCache.GetVersionMinSpecs(ModelId, false);

                if (objVersionsList != null && objVersionsList.Count > 0)
                    bodyStyle = objVersionsList.FirstOrDefault().BodyStyle;

                MostPopularBikesWidget objPopularBikes = new MostPopularBikesWidget(_bikeModels, bodyStyle.Equals(EnumBikeBodyStyles.Scooter) ? EnumBikeType.Scooters : EnumBikeType.All, showCheckOnRoadCTA, false, pqSource, pageCatId, MakeId);
                objPopularBikes.TopCount = topCount;
                objPopularBikes.CityId = CityId;
                objData.MostPopularBikes = objPopularBikes.GetData();
                if (objData.MostPopularBikes != null)
                {
                    objData.MostPopularBikes.CityId = CityId;
                    objData.MostPopularBikes.ReturnUrlForAmpPages = string.Format("{0}/m/features/{1}-{2}/", BWConfiguration.Instance.BwHostUrl, objData.objFeature.ArticleUrl, objData.objFeature.BasicId);

                }
                MostPopularBikeWidgetVM PopularBikesWidget = objData.MostPopularBikes;

                if (ModelId > 0)
                {
                    MostPopularBikeWidgetVM MostPopularBikes = null;
                    MostPopularBikeWidgetVM MostPopularMakeBikes = null;
                    MostPopularBikeWidgetVM MostPopularScooters = null;
                    MostPopularBikeWidgetVM MostPopularMakeScooters = null;
                    UpcomingBikesWidgetVM UpcomingBikes = null;
                    UpcomingBikesWidgetVM UpcomingScooters = null;
                    IEnumerable<BikeMakeEntityBase> PopularScooterMakes = null;
                    PopularBodyStyleVM BodyStyleVM = null;
                    if (IsMobile)
                    {
                        if (bodyStyle.Equals(EnumBikeBodyStyles.Scooter))
                        {
                            PopularScooterBrandsWidget objPopularScooterBrands = new PopularScooterBrandsWidget(_bikeMakesCacheRepository);
                            objPopularScooterBrands.TopCount = 4;
                            objData.PopularScooterMakesWidget = objPopularScooterBrands.GetData();
                            bikeType = EnumBikeType.Scooters;
                        }
                        else
                        {
                            SetPopularBikeByBodyStyleId(objData, topCount);
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
                        objData.MostPopularMakeBikes = new MostPopularBikeWidgetVM() { Bikes = MostPopularMakeBikes.Bikes.Take(4), WidgetHref = string.Format("/{0}-bikes/", objData.objMake.MaskingName), WidgetLinkTitle = "View all Bikes" };


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

                        objData.UpcomingBikes = new UpcomingBikesWidgetVM
                        {
                            UpcomingBikes = UpcomingBikes.UpcomingBikes.Take(topCount)
                        };
                        objUpcomingBikes.Filters.BodyStyleId = (uint)EnumBikeBodyStyles.Scooter;

                        UpcomingScooters = objUpcomingBikes.GetData();

                        if (bodyStyle.Equals(EnumBikeBodyStyles.Scooter))
                        {
                            PopularScooterBrandsWidget objPopularScooterBrands = new PopularScooterBrandsWidget(_bikeMakesCacheRepository);
                            objPopularScooterBrands.TopCount = 6;
                            PopularScooterMakes = objPopularScooterBrands.GetData();
                            objData.PopularScooterMakesWidget = PopularScooterMakes.Take(4);
                            bikeType = EnumBikeType.Scooters;
                        }
                        else
                        {
                            PopularBikesByBodyStyle BodyStyleBikes = new PopularBikesByBodyStyle(_models);
                            BodyStyleBikes.ModelId = ModelId;
                            BodyStyleBikes.CityId = CityId;
                            BodyStyleBikes.TopCount = topCount > 6 ? topCount : 6;
                            BodyStyleVM = BodyStyleBikes.GetData();

                            objData.PopularBodyStyle = new PopularBodyStyleVM() { PopularBikes = BodyStyleVM.PopularBikes.Take(topCount), BodyStyleText = BodyStyleVM.BodyStyleText, BodyStyleLinkTitle = BodyStyleVM.BodyStyleLinkTitle, BodyStyle = BodyStyleVM.BodyStyle };

                            if (objData.PopularBodyStyle != null)
                            {
                                objData.PopularBodyStyle.WidgetHeading = string.Format("Popular {0}", objData.PopularBodyStyle.BodyStyleText);
                                objData.PopularBodyStyle.WidgetLinkTitle = string.Format("Best {0} in India", objData.PopularBodyStyle.BodyStyleLinkTitle);
                                objData.PopularBodyStyle.WidgetHref = UrlFormatter.FormatGenericPageUrl(objData.PopularBodyStyle.BodyStyle);
                            }
                        }



                        if (bodyStyle.Equals(EnumBikeBodyStyles.Scooter))
                        {
                            objData.PopularMakeScootersAndOtherBrandsWidget = new MultiTabsWidgetVM();

                            objData.PopularMakeScootersAndOtherBrandsWidget.TabHeading1 = string.Format("{0} scooters", objData.objMake.MakeName);
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

                            objData.PopularMakeScootersAndOtherBrandsWidget.ViewAllHref1 = string.Format("/{0}-scooters/", objData.objMake.MaskingName);
                            objData.PopularMakeScootersAndOtherBrandsWidget.ViewAllHref2 = "/scooters/";
                            objData.PopularMakeScootersAndOtherBrandsWidget.ViewAllTitle1 = "View all scooters";
                            objData.PopularMakeScootersAndOtherBrandsWidget.ViewAllTitle2 = "View other brands";
                            objData.PopularMakeScootersAndOtherBrandsWidget.ViewAllText1 = "View all scooters";
                            objData.PopularMakeScootersAndOtherBrandsWidget.ViewAllText2 = "View other brands";
                            objData.PopularMakeScootersAndOtherBrandsWidget.ShowViewAllLink1 = true;
                            objData.PopularMakeScootersAndOtherBrandsWidget.ShowViewAllLink2 = true;
                            objData.PopularMakeScootersAndOtherBrandsWidget.Pages = MultiTabWidgetPagesEnum.PopularMakeScootersAndOtherBrands;
                            objData.PopularMakeScootersAndOtherBrandsWidget.PageName = "News";

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
                        else if (bodyStyle.Equals(EnumBikeBodyStyles.Sports) || bodyStyle.Equals(EnumBikeBodyStyles.Cruiser))
                        {
                            objData.PopularMakeBikesAndBodyStyleBikesWidget = new MultiTabsWidgetVM();

                            objData.PopularMakeBikesAndBodyStyleBikesWidget.TabHeading1 = string.Format("{0} bikes", objData.objMake.MakeName);
                            objData.PopularMakeBikesAndBodyStyleBikesWidget.TabHeading2 = bodyStyle.Equals(EnumBikeBodyStyles.Sports) ? "Sports bikes" : "Cruisers";
                            objData.PopularMakeBikesAndBodyStyleBikesWidget.ViewPath1 = "~/Views/BikeModels/_MostPopularBikesSideBar.cshtml";
                            objData.PopularMakeBikesAndBodyStyleBikesWidget.ViewPath2 = "~/Views/BestBikes/_PopularBodyStyle_Vertical.cshtml";
                            objData.PopularMakeBikesAndBodyStyleBikesWidget.TabId1 = "PopularMakeBikes";
                            objData.PopularMakeBikesAndBodyStyleBikesWidget.TabId2 = bodyStyle.Equals(EnumBikeBodyStyles.Sports) ? "PopularSportsBikes" : "PopularCruisers";
                            objData.PopularMakeBikesAndBodyStyleBikesWidget.MostPopularMakeBikes = MostPopularMakeBikes;

                            if (MostPopularMakeBikes != null)
                                objData.PopularMakeBikesAndBodyStyleBikesWidget.MostPopularMakeBikes.Bikes = objData.PopularMakeBikesAndBodyStyleBikesWidget.MostPopularMakeBikes.Bikes.Take(6);

                            objData.PopularMakeBikesAndBodyStyleBikesWidget.PopularBodyStyle = new PopularBodyStyleVM() { PopularBikes = objData.PopularBodyStyle.PopularBikes.Take(6) };
                            objData.PopularMakeBikesAndBodyStyleBikesWidget.ViewAllHref1 = string.Format("/{0}-bikes/", objData.objMake.MaskingName);
                            objData.PopularMakeBikesAndBodyStyleBikesWidget.ViewAllHref2 = !bodyStyle.Equals(EnumBikeBodyStyles.Sports) ? "/best-cruiser-bikes-in-india/" : "/best-sports-bikes-in-india/";
                            objData.PopularMakeBikesAndBodyStyleBikesWidget.ViewAllTitle1 = "View all bikes";
                            objData.PopularMakeBikesAndBodyStyleBikesWidget.ViewAllTitle2 = bodyStyle.Equals(EnumBikeBodyStyles.Sports) ? "View all Sports bikes" : "View all Cruisers";
                            objData.PopularMakeBikesAndBodyStyleBikesWidget.ViewAllText1 = "View all bikes";
                            objData.PopularMakeBikesAndBodyStyleBikesWidget.ViewAllText2 = bodyStyle.Equals(EnumBikeBodyStyles.Sports) ? "View all Sports bikes" : "View all Cruisers";
                            objData.PopularMakeBikesAndBodyStyleBikesWidget.ShowViewAllLink1 = true;
                            objData.PopularMakeBikesAndBodyStyleBikesWidget.ShowViewAllLink2 = true;
                            objData.PopularMakeBikesAndBodyStyleBikesWidget.Pages = MultiTabWidgetPagesEnum.PopularMakeBikesAndBodyStyleWidget;
                            objData.PopularMakeBikesAndBodyStyleBikesWidget.PageName = "News";

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
                            objData.PopularBikesAndUpcomingBikesWidget.PageName = "News";
                        }
                        else
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
                    }
                }
                PopularBikesWidget.WidgetHeading = "Popular bikes";
                PopularBikesWidget.WidgetHref = "/best-bikes-in-india/";
                PopularBikesWidget.WidgetLinkTitle = "Best Bikes in India";
                PopularBikesWidget.CtaText = "View all bikes";

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