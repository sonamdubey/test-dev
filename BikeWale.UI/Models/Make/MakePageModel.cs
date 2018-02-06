using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Compare;
using Bikewale.Entities.Location;
using Bikewale.Entities.Pages;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Compare;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Interfaces.Used;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Interfaces.Videos;
using Bikewale.Models.CompareBikes;
using Bikewale.Models.Make;
using Bikewale.Models.UserReviews;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 27-Mar-2017
    /// Model for make page
    /// Modified by : Aditi Srivastava on 5 June 2017
    /// Summary     : Added BL instance instead of cache for comaprison carousel
    /// Modified by : Ashutosh Sharma on 29 Oct 2017
    /// Description : Added property IsAmpPage.
    /// Modified By :Snehal Dange on 21st Nov 2017
    /// Description: Added IUserReviewsCache _cacheUserReviews
    /// Modified By : Deepak Israni on 6th Feb 2018
    /// Description : Added TopCountNews property
    /// </summary>
    public class MakePageModel
    {
        private readonly string _makeMaskingName;
        private uint _makeId;
        private readonly IBikeModelsCacheRepository<int> _bikeModelsCache;
        private readonly IBikeMakesCacheRepository _bikeMakesCache;
        private readonly ICMSCacheContent _articles = null;
        private readonly ICMSCacheContent _expertReviews = null;
        private readonly IVideos _videos = null;
        private readonly IUsedBikeDetailsCacheRepository _cachedBikeDetails = null;
        private readonly IDealerCacheRepository _cacheDealers = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeCompare _compareBikes = null;
        private readonly IServiceCenter _objSC;
        private readonly IUserReviewsCache _cacheUserReviews;
        public StatusCodes Status { get; set; }
        public MakeMaskingResponse objResponse { get; set; }
        public string RedirectUrl { get; set; }
        public CompareSources CompareSource { get; set; }
        public bool IsMobile { get; set; }
        public bool IsAmpPage { get; set; }
        private CityEntityBase cityBase = null;
        public uint TopCountNews { get; set; }

        public MakePageModel(string makeMaskingName, IBikeModelsCacheRepository<int> bikeModelsCache, IBikeMakesCacheRepository bikeMakesCache, ICMSCacheContent articles, ICMSCacheContent expertReviews, IVideos videos, IUsedBikeDetailsCacheRepository cachedBikeDetails, IDealerCacheRepository cacheDealers, IUpcoming upcoming, IBikeCompare compareBikes, IServiceCenter objSC, IUserReviewsCache cacheUserReviews)
        {
            this._makeMaskingName = makeMaskingName;
            this._bikeModelsCache = bikeModelsCache;
            this._bikeMakesCache = bikeMakesCache;
            this._articles = articles;
            this._expertReviews = expertReviews;
            this._videos = videos;
            this._cachedBikeDetails = cachedBikeDetails;
            this._cacheDealers = cacheDealers;
            this._upcoming = upcoming;
            this._compareBikes = compareBikes;
            this._objSC = objSC;
            this._cacheUserReviews = cacheUserReviews;
            ProcessQuery(this._makeMaskingName);
        }

        /// <summary>
        /// Gets the data for homepage
        /// Modified by : Ashutosh Sharma on 05 Oct 2017
        /// Description : Replaced call to method 'GetMostPopularBikesByMake' with 'GetMostPopularBikesByMakeWithCityPrice' to get city price when city is selected.
        /// Modified by : Vivek Singh Tomar on 11th Oct 2017
        /// Summary : Removed unnecessary arguments from BindDealerSserviceData which was required for fetchin service center details
        /// Modified by sajal Gupta on 06-11-2017
        /// Descriptition :  Chaged default sorting of bikes on page for particuaklar makes
        /// Modified by : Ashutosh Sharma on 27 Oct 2017
        /// Description : Added call to BindAmpJsTags.
        /// Modified by : Snehal Dange on 21st Nov 2017
        /// Description : Added BindUserReviews() method.
        /// Modified by: Snehal Dange on 23rd Nov 2017
        /// Description : Added BindMakeFooterCategoriesandPriceWidget() method
        /// Modified by sajal Gupta on 24-11-2017
        /// Descriptition :  Added BikeCityPopup
        /// Modified BY: Snehal Dange on 23rd Nov 2017
        /// Description: Added IsFooterDescriptionAvailable ,IsPriceListingAvailable checks
        /// Modified by : Snehal Dange on 17th Jan 2018
        /// Description: Added BindResearchMoreMakeWidget()
        /// Modified by: Deepak Israni on 30th Jan 2018
        /// Description: Removed ShowCheckOnRoadpriceBtn property
        /// </summary>         
        /// <returns>
        /// Created by : Sangram Nandkhile on 25-Mar-2017 
        /// Modified by : Rajan Chauhan on 3 Jan 2017
        /// Description : Bind MakeId to objData
        /// </returns>
        public MakePageVM GetData()
        {
            MakePageVM objData = new MakePageVM();

            try
            {
                #region Variable initialization

                uint cityId = 0;
                string cityName = string.Empty, cityMaskingName = string.Empty;

                GlobalCityAreaEntity location = GlobalCityArea.GetGlobalCityArea();
                objData.City = location;
                if (location != null && location.CityId > 0)
                {
                    cityId = location.CityId;
                    cityName = location.City;
                    var cityEntity = new CityHelper().GetCityById(cityId);
                    cityMaskingName = cityEntity != null ? cityEntity.CityMaskingName : string.Empty;
                    objData.Location = cityName;
                    objData.LocationMasking = cityMaskingName;
                    cityBase = new CityEntityBase()
                    {
                        CityId = cityId,
                        CityMaskingName = cityMaskingName,
                        CityName = cityName
                    };
                }
                else
                {
                    objData.Location = "India";
                    objData.LocationMasking = "india";
                }

                #endregion

                objData.Bikes = _bikeModelsCache.GetMostPopularBikesByMakeWithCityPrice((int)_makeId, cityId);

                if (objData.Bikes!=null && objData.Bikes.Count() > 5)
                {
                    objData.TopPopularBikes = objData.Bikes.OrderBy(x => x.BikePopularityIndex).Take(4);
                }
                
                BikeMakeEntityBase makeBase = _bikeMakesCache.GetMakeDetails(_makeId);
                objData.BikeDescription = _bikeMakesCache.GetMakeDescription(_makeId);
                objData.SelectedSortingId = 0;
                objData.SelectedSortingText = "Price: Low to High";

                if (makeBase != null)
                {
                    objData.MakeMaskingName = makeBase.MaskingName;
                    objData.MakeName = makeBase.MakeName;
                    objData.MakeId = makeBase.MakeId;
                }

                if (!string.IsNullOrEmpty(BWConfiguration.Instance.PopularityOrderForMake) && BWConfiguration.Instance.PopularityOrderForMake.Split(',').Contains(_makeId.ToString()))
                {
                    objData.Bikes = objData.Bikes.OrderBy(x => x.BikePopularityIndex);
                    objData.SelectedSortingId = 1;
                    objData.SelectedSortingText = "Popular";
                }
                BindUpcomingBikes(objData);
                BindPageMetaTags(objData, objData.Bikes, makeBase);
                BindCompareBikes(objData, CompareSource, cityId);
                BindDealerServiceData(objData, cityId);
                BindCMSContent(objData);
                objData.UsedModels = BindUsedBikeByModel(_makeId, cityId);
                BindDiscontinuedBikes(objData);
                BindOtherMakes(objData);
                BindUserReviews(objData);
                BindMakeFooterCategoriesandPriceWidget(objData);
                objData.Page = GAPages.Make_Page;
                objData.BikeCityPopup = new PopUp.BikeCityPopup()
                {
                    ApiUrl = "/api/v2/DealerCity/?makeId=" + _makeId,
                    PopupShowButtonMessage = "Show showrooms",
                    PopupSubHeading = "See Showrooms in your city!",
                    FetchDataPopupMessage = "Fetching showrooms for ",
                    RedirectUrl = string.Format("/dealer-showrooms/{0}/", _makeMaskingName),
                    IsCityWrapperPresent = 1
                };

                BindShowroomPopularCityWidget(objData);
                BindResearchMoreMakeWidget(objData);
                GetEMIDetails(objData);
                #region Set Visible flags

                if (objData != null)
                {
                    objData.IsUpComingBikesAvailable = objData.UpcomingBikes != null && objData.UpcomingBikes.UpcomingBikes != null && objData.UpcomingBikes.UpcomingBikes.Any();
                    objData.IsNewsAvailable = objData.News != null && objData.News.ArticlesList != null && objData.News.ArticlesList.Any();
                    objData.IsExpertReviewsAvailable = objData.ExpertReviews != null && objData.ExpertReviews.ArticlesList != null && objData.ExpertReviews.ArticlesList.Any();
                    objData.IsVideosAvailable = objData.Videos != null && objData.Videos.VideosList != null && objData.Videos.VideosList.Any();
                    objData.IsUsedModelsBikeAvailable = objData.UsedModels != null && objData.UsedModels.UsedBikeModelList != null && objData.UsedModels.UsedBikeModelList.Any();

                    objData.IsDealerAvailable = objData.Dealers != null && objData.Dealers.Dealers != null && objData.Dealers.Dealers.Any();
                    objData.IsDealerServiceDataAvailable = cityId > 0 && objData.IsDealerAvailable;
                    objData.IsDealerServiceDataInIndiaAvailable = cityId == 0 && objData.DealersServiceCenter != null && objData.DealersServiceCenter.DealerServiceCenters != null && objData.DealersServiceCenter.DealerServiceCenters.DealerDetails != null && objData.DealersServiceCenter.DealerServiceCenters.DealerDetails.Any();
                    objData.IsUserReviewsAvailable = (objData.PopularBikesUserReviews != null && objData.PopularBikesUserReviews.BikesReviewsList != null && objData.PopularBikesUserReviews.BikesReviewsList.Any() && objData.PopularBikesUserReviews.BikesReviewsList.FirstOrDefault().MostRecent != null);

                    objData.IsMakeTabsDataAvailable = (objData.BikeDescription != null && objData.BikeDescription.FullDescription.Length > 0 || objData.IsNewsAvailable ||
                        objData.IsExpertReviewsAvailable || objData.IsVideosAvailable || objData.IsUsedModelsBikeAvailable || objData.IsDealerServiceDataAvailable || objData.IsDealerServiceDataInIndiaAvailable || objData.IsUserReviewsAvailable);

                    objData.IsFooterDescriptionAvailable = objData.SubFooter != null && objData.SubFooter.FooterContent != null && objData.SubFooter.FooterContent.FooterDescription != null && objData.SubFooter.FooterContent.FooterDescription.Any();

                    objData.IsPriceListingAvailable = objData.IsFooterDescriptionAvailable && objData.SubFooter.FooterContent.ModelPriceList != null && objData.SubFooter.FooterContent.ModelPriceList.Any();

                }

                if (IsAmpPage)
                {
                    BindAmpJsTags(objData);
                }
                #endregion
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("MakePageModel.GetData() => MakeId: {0}", _makeId));
            }

            return objData;
        }
        /// <summary>
        /// Created by : Ashutosh Sharma on 27 Oct 2017
        /// Description : Method to bind required JS for AMP page.
        /// </summary>
        /// <param name="objData"></param>
        private void BindAmpJsTags(MakePageVM objData)
        {
            try
            {
                objData.AmpJsTags = new Entities.Models.AmpJsTags();
                objData.AmpJsTags.IsAccordion = true;
                objData.AmpJsTags.IsAd = true;
                objData.AmpJsTags.IsAnalytics = true;
                objData.AmpJsTags.IsBind = true;
                objData.AmpJsTags.IsCarousel = true;
                objData.AmpJsTags.IsSidebar = true;

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BindAmpJsTags_{0}", objData));
            }
        }

        /// <summary>
        /// Created by Sajal on 24-11-2017
        /// Desc : Widget Bind Showroom Popular City 
        /// </summary>
        /// <param name="objMakePage"></param>
        private void BindShowroomPopularCityWidget(MakePageVM objMakePage)
        {
            DealersServiceCentersIndiaWidgetVM objData = new DealersServiceCentersIndiaWidgetVM();
            try
            {
                uint topCount = 8;
                objData.DealerServiceCenters = _cacheDealers.GetPopularCityDealer(_makeId, topCount);
                objData.MakeMaskingName = objMakePage.MakeMaskingName;
                objData.MakeName = objMakePage.MakeName;
                objData.CityCardTitle = "showrooms in";
                objData.CityCardLink = "dealer-showrooms";
                objData.IsServiceCenterPage = false;
                objMakePage.DealersServiceCenterPopularCities = objData;
                if (objData.DealerServiceCenters.DealerDetails.Any())
                {
                    objMakePage.DealersServiceCenterPopularCities.DealerServiceCenters.DealerDetails = objMakePage.DealersServiceCenterPopularCities.DealerServiceCenters.DealerDetails.Where(m => !m.CityId.Equals(cityBase != null ? cityBase.CityId : 0)).ToList();
                }
                objData.IsIndiaCardNeeded = true;
            }
            catch (System.Exception ex)
            {

                ErrorClass er = new ErrorClass(ex, "ServiceCenterDetailsPage.BindShowroomPopularCityWidget");
            }

        }

        /// <summary>
        /// Created by  :   Sumit Kate on 24 Aug 2017
        /// Description :   Bind Other Make list
        /// Modifiwed by Sajal Gupta on 15-11-2017
        /// Dewsc : Added makecategory sorting logic
        /// Modified by: Snehal Dange on 14th Dec 2017
        /// Desc: Added logic for cardtext and PageLinkFormat
        /// </summary>
        /// <param name="objData"></param>
        private void BindOtherMakes(MakePageVM objData)
        {
            try
            {
                IEnumerable<BikeMakeEntityBase> makes = _bikeMakesCache.GetMakesByType(EnumBikeType.New);

                var popularBrandsList = Utility.BikeFilter.FilterMakesByCategory(_makeId, makes);

                if (popularBrandsList != null && popularBrandsList.Any())
                {
                    var otherMakes = new OtherMakesVM();
                    otherMakes.Makes = popularBrandsList.Take(9);
                    objData.OtherMakes = otherMakes;

                    if (objData.OtherMakes != null)
                    {
                        objData.OtherMakes.CardText = "bike";
                        objData.OtherMakes.PageLinkFormat = "/{0}-bikes/";
                        objData.OtherMakes.PageTitleFormat = "{0} Bikes";
                    }


                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("MakePageModel.BindOtherMakes() => MakeId: {0}", _makeId));
            }
        }

        private UsedBikeModelsWidgetVM BindUsedBikeByModel(uint makeId, uint cityId)
        {
            UsedBikeModelsWidgetVM UsedBikeModel = new UsedBikeModelsWidgetVM();
            try
            {

                UsedBikeModelsWidgetModel objUsedBike = new UsedBikeModelsWidgetModel(9, _cachedBikeDetails);
                if (makeId > 0)
                    objUsedBike.makeId = makeId;
                if (cityId > 0)
                    objUsedBike.cityId = cityId;
                UsedBikeModel = objUsedBike.GetData();
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, "MakePageModel.BindUsedBikeByModel()");
            }

            return UsedBikeModel;

        }

        /// <summary>
        /// Modified by : Vivek Singh Tomar on 11th Oct 2017
        /// Summary : Removed call for getting service centers details
        /// </summary>
        /// <param name="objData"></param>
        /// <param name="cityId"></param>
        private void BindDealerServiceData(MakePageVM objData, uint cityId)
        {
            if (cityId > 0)
            {
                var dealerData = new DealerCardWidget(_cacheDealers, cityId, _makeId);
                dealerData.TopCount = 3;
                objData.Dealers = dealerData.GetData();
            }
            else
            {
                objData.DealersServiceCenter = new DealersServiceCentersIndiaWidgetModel(_makeId, objData.MakeName, _makeMaskingName, _cacheDealers).GetData();
            }
        }

        private void BindUpcomingBikes(MakePageVM objData)
        {
            UpcomingBikesWidget objUpcoming = new UpcomingBikesWidget(_upcoming);
            objUpcoming.Filters = new Bikewale.Entities.BikeData.UpcomingBikesListInputEntity()
            {
                PageSize = 9,
                PageNo = 1,
                MakeId = (int)this._makeId
            };
            objUpcoming.SortBy = Bikewale.Entities.BikeData.EnumUpcomingBikesFilter.Default;
            objData.UpcomingBikes = objUpcoming.GetData();
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 24 Apr 2017
        /// Summary  :  Function to bind popular comparison carousel
        /// Modified by : Aditi Srivastava on 27 Apr 2017
        /// Summary  : Added source for comparisons
        /// </summary>
        private void BindCompareBikes(MakePageVM objViewModel, CompareSources compareSource, uint cityId)
        {
            try
            {
                string versionList = string.Join(",", objViewModel.Bikes.OrderBy(m => m.BikePopularityIndex).Select(m => m.objVersion.VersionId).Take(9));
                PopularModelCompareWidget objCompare = new PopularModelCompareWidget(_compareBikes, 1, cityId, versionList);
                objViewModel.CompareSimilarBikes = objCompare.GetData();
                objViewModel.IsCompareBikesAvailable = (objViewModel.CompareSimilarBikes != null && objViewModel.CompareSimilarBikes.CompareBikes != null && objViewModel.CompareSimilarBikes.CompareBikes.Any());
                objViewModel.CompareSimilarBikes.CompareSource = compareSource;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "MakePageModel.BindCompareBikes");
            }
        }

        /// <summary>
        /// Modified By: Deepak Israni on 5th Feb 2018
        /// Description: Bind more news articles on mobile page.
        /// </summary>
        /// <param name="objData"></param>
        private void BindCMSContent(MakePageVM objData)
        {

            objData.News = new RecentNews(TopCountNews, _makeId, objData.MakeName, _makeMaskingName, string.Format("{0} News", objData.MakeName), _articles).GetData();
            
            objData.ExpertReviews = new RecentExpertReviews(2, _makeId, objData.MakeName, _makeMaskingName, _expertReviews, string.Format("{0} Reviews", objData.MakeName)).GetData();
            if (IsMobile)
            {
                objData.Videos = new RecentVideos(1, 2, _makeId, objData.MakeName, _makeMaskingName, _videos).GetData();
            }
            else
            {
                objData.Videos = new RecentVideos(1, 4, _makeId, objData.MakeName, _makeMaskingName, _videos).GetData();
            }

        }

        private void BindDiscontinuedBikes(MakePageVM objData)
        {
            objData.DiscontinuedBikes = _bikeMakesCache.GetDiscontinuedBikeModelsByMake(_makeId);
            objData.IsDiscontinuedBikeAvailable = objData.DiscontinuedBikes != null && objData.DiscontinuedBikes.Any();

            if (objData.IsDiscontinuedBikeAvailable)
            {
                foreach (var bike in objData.DiscontinuedBikes)
                {
                    bike.Href = string.Format("/{0}-bikes/{1}/", _makeMaskingName, bike.ModelMasking);
                    bike.BikeName = string.Format("{0} {1}", objData.MakeName, bike.ModelName);
                }
            }
        }

        /// Modified by :- Subodh Jain 19 june 2017
        /// Summary :- Added Target Make
        /// Modified By : Sushil Kumar on 23rd Aug 2017
        /// Description : Added null check for min and max price
        /// Modified by : Snehal Dange on 11th Jan 2017
        /// Decription: Changed meta title and description
        private void BindPageMetaTags(MakePageVM objData, IEnumerable<MostPopularBikesBase> objModelList, BikeMakeEntityBase objMakeBase)
        {
            long minPrice = 0;
            long MaxPrice = 0;
            IEnumerable<MostPopularBikesBase> objTopModelList = null;
            IList<string> topBikeList = null;
            int topModelCount = 4;
            string topModelsName = null;
            try
            {
                topBikeList = new List<string>();
                if (objModelList != null)
                {
                    if (objModelList.Any())
                    {
                        minPrice = objModelList.Min(bike => bike.VersionPrice);
                        MaxPrice = objModelList.Max(bike => bike.VersionPrice);
                    }
                    if (objModelList.Count() < topModelCount)
                    {
                        topModelCount = objModelList.Count();
                    }

                    objTopModelList = objModelList.OrderBy(x => x.BikePopularityIndex).Take(topModelCount);

                    if (objTopModelList != null)
                    {
                        foreach (var objBike in objTopModelList.Take(topModelCount - 1))
                        {
                            topBikeList.Add(objBike.BikeName);
                        }

                        if (objTopModelList.Last() != null)
                        {
                            topModelsName = string.Format("{0} and {1}", string.Join(",", topBikeList), objTopModelList.Last().BikeName);
                        }
                    }

                }
                objData.PageMetaTags.Title = string.Format("{0} Bikes in India- {0} New Bikes Prices, Specs, & Images - BikeWale", objData.MakeName);

                objData.PageMetaTags.Description = string.Format("{0} has a total of {1} models. The top 4 {0} models are- {2}. BikeWale offers history, prices, specs, and images for all {0} models in India.{3}", objData.MakeName, objModelList.Count(), topModelsName, (objData.UpcomingBikes.UpcomingBikes.Count() > 0 ? string.Format("There are {0} {1} upcoming models as well.", objData.UpcomingBikes.UpcomingBikes.Count(), objData.MakeName) : ""));

                objData.PageMetaTags.CanonicalUrl = string.Format("{0}/{1}-bikes/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, _makeMaskingName);
                objData.PageMetaTags.AlternateUrl = string.Format("{0}/m/{1}-bikes/", BWConfiguration.Instance.BwHostUrl, _makeMaskingName);
                objData.PageMetaTags.AmpUrl = string.Format("{0}/m/{1}-bikes/amp/", BWConfiguration.Instance.BwHostUrl, _makeMaskingName);
                objData.PageMetaTags.Keywords = string.Format("{0}, {0} Bikes , {0} Bikes prices, {0} Bikes reviews, {0} Images, new {0} Bikes", objData.MakeName);
                objData.AdTags.TargetedMakes = objData.MakeName;
                objData.Page_H1 = string.Format("{0} Bikes", objData.MakeName);
                objData.ReturlUrl = string.Format("/m/{0}-bikes/", objData.MakeMaskingName);

                SetBreadcrumList(ref objData);

                CheckCustomPageMetas(objData, objMakeBase);

                SetPageJSONLDSchema(objData, objMakeBase);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "MakePageModel.BindPageMetaTags()");
            }

        }

        /// <summary>
        /// Created By  : Sushil Kumar on 6th Nov Sep 2017
        /// Description : Added breadcrum and webpage schema and added brand schema
        /// </summary>
        private void SetPageJSONLDSchema(MakePageVM objPageMeta, BikeMakeEntityBase objMakeBase)
        {
            WebPage webpage = SchemaHelper.GetWebpageSchema(objPageMeta.PageMetaTags, objPageMeta.BreadcrumbList);

            if (webpage != null)
            {
                if (objMakeBase != null)
                {
                    Brand brand = new Brand();
                    brand.Logo = Image.GetPathToShowImages(objMakeBase.LogoUrl, objMakeBase.HostUrl, "0x0");
                    brand.Url = string.Format("{0}/{1}-bikes/", BWConfiguration.Instance.BwHostUrl, objMakeBase.MaskingName);
                    brand.Name = objMakeBase.MakeName;
                    brand.Description = objPageMeta.BikeDescription != null ? objPageMeta.BikeDescription.SmallDescription : objPageMeta.PageMetaTags.Description;
                    objPageMeta.PageMetaTags.PageSchemaJSON = SchemaHelper.JsonSerialize(brand);
                }
                objPageMeta.PageMetaTags.SchemaJSON = SchemaHelper.JsonSerialize(webpage);
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 12th Sep 2017
        /// Description : Function to create page level schema for breadcrum
        /// Modified by : Snehal Dange on 27th Dec 2017
        /// Description: Added 'new bikes' in breadcrumb
        /// </summary>
        private void SetBreadcrumList(ref MakePageVM objData)
        {
            IList<BreadcrumbListItem> BreadCrumbs = new List<BreadcrumbListItem>();
            string url = string.Format("{0}/", Utility.BWConfiguration.Instance.BwHostUrl);
            ushort position = 1;
            if (IsMobile)
            {
                url += "m/";
            }

            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, "Home"));
            url = string.Format("{0}new-bikes-in-india/", url);
            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, "New Bikes"));
            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null, objData.Page_H1));


            objData.BreadcrumbList.BreadcrumListItem = BreadCrumbs;

        }


        /// <summary>
        /// Created By : Sushil Kumar on 13th Aug 2017
        /// Description : Function to check and set custom page metas and summary for the page
        /// </summary>
        /// <param name="objData"></param>
        /// <param name="objMakeBase"></param>
        private void CheckCustomPageMetas(MakePageVM objData, BikeMakeEntityBase objMakeBase)
        {
            try
            {
                if (objMakeBase != null && objMakeBase.Metas != null)
                {
                    var metas = objMakeBase.Metas.FirstOrDefault(m => m.PageId == (int)(IsMobile ? BikewalePages.Mobile_MakePage : BikewalePages.Desktop_MakePage));
                    if (metas != null)
                    {
                        if (!string.IsNullOrEmpty(metas.Title))
                        {
                            objData.PageMetaTags.Title = metas.Title;
                        }
                        if (!string.IsNullOrEmpty(metas.Description))
                        {
                            objData.PageMetaTags.Description = metas.Description;
                        }
                        if (!string.IsNullOrEmpty(metas.Keywords))
                        {
                            objData.PageMetaTags.Keywords = metas.Keywords;
                        }
                        if (!string.IsNullOrEmpty(metas.Heading))
                        {
                            objData.Page_H1 = metas.Heading;
                        }
                        if (!string.IsNullOrEmpty(metas.Summary))
                        {
                            if (objData.BikeDescription == null)
                            {
                                objData.BikeDescription = new BikeDescriptionEntity();
                            }

                            objData.BikeDescription.FullDescription = objData.BikeDescription.SmallDescription = metas.Summary;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("MakePageModel.CheckCustomPageMetas() makeId:{0}", _makeId));
            }
        }

        /// <summary>
        /// Created By:- Sangram Nandkhile on 29-Mar-2017 
        /// Summary:- Process the input query
        /// </summary>
        /// <returns></returns>
        private void ProcessQuery(string makeMaskingName)
        {
            try
            {
                objResponse = _bikeMakesCache.GetMakeMaskingResponse(makeMaskingName);
                if (objResponse != null)
                {
                    Status = (StatusCodes)objResponse.StatusCode;
                    if (objResponse.StatusCode == 200)
                    {
                        _makeId = objResponse.MakeId;
                    }
                    else if (objResponse.StatusCode == 301)
                    {
                        RedirectUrl = HttpContext.Current.Request.RawUrl.Replace(_makeMaskingName, objResponse.MaskingName);
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
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("MakePageModel.ProcessQuery() makeMaskingName:{0}", makeMaskingName));
            }
        }

        /// <summary>
        /// Created By:Snehal Dange on 20th Nov 2017
        /// Description: Get most recent and helpful reviews by make
        /// </summary>
        /// <param name="objData"></param>
        private void BindUserReviews(MakePageVM objData)
        {
            try
            {
                if (_makeId > 0 && objData != null && _cacheUserReviews != null)
                {
                    objData.PopularBikesUserReviews = new BikesWithReviewsByMakeVM();
                    if (objData.PopularBikesUserReviews != null)
                    {
                        objData.PopularBikesUserReviews.BikesReviewsList = _cacheUserReviews.GetBikesWithReviewsByMake(_makeId);
                    }
                }

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("MakePageModel.BindUserReviews() makeId:{0}", _makeId));
            }
        }

        /// <summary>
        /// Created By: Snehal Dange on 23rd Nov 2017
        /// Description: Created BindMakeFooterCategoriesandPriceWidget() to bind SubFooter on make page
        /// </summary>
        /// <param name="objData"></param>
        private void BindMakeFooterCategoriesandPriceWidget(MakePageVM objData)
        {
            try
            {
                if (_makeId > 0 && objData != null && _bikeMakesCache != null)
                {
                    objData.SubFooter = new MakeFooterCategoriesandPriceVM();
                    if (objData.SubFooter != null)
                    {
                        objData.SubFooter.FooterContent = _bikeMakesCache.GetMakeFooterCategoriesandPrice(_makeId);
                        if (objData.SubFooter.FooterContent != null && objData.SubFooter.FooterContent.ModelPriceList != null && objData.SubFooter.FooterContent.ModelPriceList.Any())
                        {
                            objData.SubFooter.Make = objData.SubFooter.FooterContent.ModelPriceList.First().Make;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("MakePageModel.BindMakeFooterCategoriesandPriceWidget() makeId:{0}", _makeId));
            }
        }

        /// <summary>
        /// Created by :  Snehal Dange on 17th Jan 2018
        /// Description: Method to bind research more about make widget data
        /// </summary>
        /// <param name="objData"></param>
        private void BindResearchMoreMakeWidget(MakePageVM objData)
        {

            try
            {
                if (_makeId > 0 && objData != null)
                {
                    objData.ResearchMoreMakeWidget = new ResearchMoreAboutMakeVM();

                    if (objData.ResearchMoreMakeWidget != null)
                    {
                        if (cityBase != null && cityBase.CityId > 0)
                        {
                            objData.ResearchMoreMakeWidget.WidgetObj = _bikeMakesCache.ResearchMoreAboutMakeByCity(_makeId, cityBase.CityId);
                            if (objData.ResearchMoreMakeWidget.WidgetObj != null)
                            {
                                objData.ResearchMoreMakeWidget.WidgetObj.City = cityBase;
                            }

                        }
                        else
                        {
                            objData.ResearchMoreMakeWidget.WidgetObj = _bikeMakesCache.ResearchMoreAboutMake(_makeId);
                        }
                        if (objData.ResearchMoreMakeWidget.WidgetObj != null)
                        {
                            objData.ResearchMoreMakeWidget.WidgetObj.ShowServiceCenterLink = true;
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("MakePageModel.BindResearchMoreMakeWidget() makeId:{0} , cityId:{1}", _makeId, (cityBase != null ? cityBase.CityId.ToString() : "0")));
            }
        }

        /// <summary>
        /// Created by : Snehal Dange on 1st Feb 2018
        /// Descritpion: Method created to set emi details for bikelist models
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        private void GetEMIDetails(MakePageVM objData)
        {

            try
            {
                if (objData != null)
                {
                    if (cityBase != null && cityBase.CityId > 0) // when city is selected
                    {
                        foreach (var bike in objData.Bikes)
                        {
                            if (bike != null)
                            {
                                if (bike.ExShowroomPrice > 0)
                                {
                                    bike.EMIDetails = EMICalculation.SetDefaultEMIDetails((uint)bike.ExShowroomPrice);
                                }
                                else if (bike.AvgPrice > 0)
                                {
                                    bike.EMIDetails = EMICalculation.SetDefaultEMIDetails((uint)bike.AvgPrice);
                                }
                            }


                        }

                    }
                    else // when city is not selected
                    {
                        foreach (var bike in objData.Bikes)
                        {
                            if (bike != null)
                            {
                                bike.EMIDetails = EMICalculation.SetDefaultEMIDetails((uint)bike.ExShowroomPrice);
                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("MakePageModel.GetEMIDetails: Make : {0} , City : {1}", _makeId, cityBase.CityId));
            }

        }
    }
}