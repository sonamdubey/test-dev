using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Compare;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Compare;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Interfaces.Videos;
using Bikewale.Models.CompareBikes;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 31-Mar-2017
    /// Model for scooters make page
    /// Modified by : Aditi Srivastava on 5 June 2017
    /// Summary     : Added BL instance instead of cache for comparison carousel
    /// </summary>
    public class ScootersMakePageModel
    {
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeCompare _compareScooters = null;
        private readonly IBikeMakesCacheRepository _objMakeCache = null;
        private readonly IDealerCacheRepository _objDealerCache = null;
        private readonly IServiceCenter _objService = null;
        private readonly ICMSCacheContent _articles = null;
        private readonly IVideos _videos = null;
        private ScootersMakePageVM objData = null;


        public StatusCodes Status { get; set; }
        private MakeMaskingResponse objResponse;
        private uint _makeId;
        private string _makeName;
        private readonly string _makeMaskingName;
        public string RedirectUrl { get; set; }
        public bool IsMobile { get; set; }
        /// <summary>
        /// Created by  :   Sumit Kate on 30 Mar 2017
        /// Description :   Constructor to initialize the member variables
        /// </summary>
        public ScootersMakePageModel(
            string makeMaskingName,
            IBikeModels<BikeModelEntity, int> bikeModels,
            IUpcoming upcoming,
            IBikeCompare compareScooters,
            IBikeMakesCacheRepository objMakeCache,
            IDealerCacheRepository objDealerCache,
            IServiceCenter objServices,
            ICMSCacheContent articles,
            IVideos videos
            )
        {
            _makeMaskingName = makeMaskingName;
            _bikeModels = bikeModels;
            _upcoming = upcoming;
            _compareScooters = compareScooters;
            _objMakeCache = objMakeCache;
            _objDealerCache = objDealerCache;
            _objService = objServices;
            _articles = articles;
            _videos = videos;
            ProcessQuery(makeMaskingName);
        }

        public uint CityId { get { return GlobalCityArea.GetGlobalCityArea().CityId; } }
        public ushort BrandTopCount { get; set; }
        public PQSourceEnum PqSource { get; set; }
        public CompareSources CompareSource { get; set; }
        public uint EditorialTopCount { get; set; }

        /// <summary>
        /// Created by  :   Sumit Kate on 30 Mar 2017
        /// Description :   Returns the Scooters Index Page view model
        /// Modified by : snehal Dange on 28th Nov 2017
        /// Descritpion : Added ga for page
        /// Modified by : Pratibha Verma on 4 April 2018
        /// Description : Added grpc call to get minSpecs
        /// </summary>
        /// <returns></returns>
        public ScootersMakePageVM GetData()
        {
            try
            {
                objData = new ScootersMakePageVM();
                CityEntityBase cityEntity = null;
                var cityBase = GlobalCityArea.GetGlobalCityArea();
                if (cityBase != null && cityBase.CityId > 0)
                {
                    cityEntity = new CityHelper().GetCityById(cityBase.CityId);
                    objData.Location = cityEntity.CityName;
                    objData.LocationMasking = cityEntity.CityMaskingName;
                }
                else
                {
                    objData.Location = "India";
                    objData.LocationMasking = "india";
                }
                objData.PageCatId = 8;

                objData.Make = _objMakeCache.GetMakeDetails(_makeId);
                objData.Description = _objMakeCache.GetScooterMakeDescription(objResponse.MakeId);
                objData.Scooters = _bikeModels.GetMostPopularScooters(_makeId);
                if (objData.Make != null)
                {
                    _makeName = objData.Make.MakeName;
                }

                BindUpcomingBikes();
                BindDealersServiceCenters(cityEntity);
                BindOtherScooterBrands(_makeId, 9);
                BindCompareScootes(CompareSource);
                BindEditorialWidget();
                SetFlags(CityId);
                objData.ScooterNewsUrl = UrlFormatter.FormatScootersNewsUrl(objData.News.MakeMasking, objData.News.ModelMasking);

                BindPageMetaTags();
                objData.Page = Entities.Pages.GAPages.Make_Scooters;

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "ScootersIndexPageModel.GetData()");
            }
            return objData;
        }

        /// <summary>
        /// Modified by : Aditi Srivastava on 25 Apr 2017
        /// Summary  :  Moved the comparison logic to common model
        /// Modified by : Aditi Srivastava on 27 Apr 2017
        /// Summary  : Added source for comparisons
        /// </summary>
        private void BindCompareScootes(CompareSources CompareSource)
        {
            try
            {
                string versionList = string.Join(",", objData.Scooters.Select(m => m.objVersion.VersionId));
                PopularModelCompareWidget objCompare = new PopularModelCompareWidget(_compareScooters, 1, CityId, versionList);
                objData.SimilarCompareScooters = objCompare.GetData();
                objData.SimilarCompareScooters.CompareSource = CompareSource;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ScootersIndexPageModel.BindCompareScootes()");
            }
        }

        private void BindOtherScooterBrands(uint _makeId, int topCount)
        {
            var scooterBrand = _objMakeCache.GetScooterMakes();
            objData.OtherBrands = scooterBrand.Where(x => x.MakeId != _makeId).Take(topCount);
        }

        /// <summary>
        /// Modified by : Aditi Srivastava on 15 June 2017
        /// Summary     : Added flags for editorial section (News, expert reviews and videos)
        /// </summary>
        private void SetFlags(uint cityId)
        {
            if (objData != null)
            {

                objData.IsScooterDataAvailable = objData.Scooters != null && objData.Scooters.Any();
                objData.IsCompareDataAvailable = objData.SimilarCompareScooters != null && objData.SimilarCompareScooters.CompareBikes != null && objData.SimilarCompareScooters.CompareBikes.Any();
                objData.IsUpComingBikesAvailable = objData.UpcomingScooters != null && objData.UpcomingScooters.UpcomingBikes != null && objData.UpcomingScooters.UpcomingBikes.Any();
                objData.IsDealerAvailable = objData.Dealers != null && objData.Dealers.Dealers != null && objData.Dealers.Dealers.Any();
                objData.IsDealerServiceDataAvailable = cityId > 0 && objData.IsDealerAvailable;
                objData.IsDealerServiceDataInIndiaAvailable = cityId == 0 && objData.DealersServiceCenter != null && objData.DealersServiceCenter.DealerServiceCenters != null && objData.DealersServiceCenter.DealerServiceCenters.DealerDetails != null && objData.DealersServiceCenter.DealerServiceCenters.DealerDetails.Any();
                objData.IsNewsAvailable = objData.News != null && objData.News.ArticlesList != null && objData.News.ArticlesList.Any();
                objData.IsExpertReviewsAvailable = objData.ExpertReviews != null && objData.ExpertReviews.ArticlesList != null && objData.ExpertReviews.ArticlesList.Any();
                objData.IsVideosAvailable = objData.Videos != null && objData.Videos.VideosList != null && objData.Videos.VideosList.Any();
                objData.IsMakeTabsDataAvailable = (objData.Description != null && objData.Description.FullDescription.Length > 0 || objData.IsDealerServiceDataAvailable || objData.IsDealerServiceDataInIndiaAvailable || objData.IsNewsAvailable || objData.IsExpertReviewsAvailable || objData.IsVideosAvailable);
            }

        }

        /// <summary>
        /// Binds the dealers service centers.
        /// Modified by : Vivek Singh Tomar on 12th Oct 2017
        /// Summary : Removed initialization of service centers 
        /// </summary>
        /// <param name="objVM">The object vm.</param>
        /// <param name="cityEntity">The city entity.</param>
        private void BindDealersServiceCenters(CityEntityBase cityEntity)
        {
            try
            {
                if (cityEntity != null && cityEntity.CityId > 0)
                {
                    var dealerData = new DealerCardWidget(_objDealerCache, cityEntity.CityId, _makeId);
                    dealerData.TopCount = 3;
                    objData.Dealers = dealerData.GetData();
                }
                else
                {
                    objData.DealersServiceCenter = new DealersServiceCentersIndiaWidgetModel(_makeId, _makeName, _makeMaskingName, _objDealerCache).GetData();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ScootersMakePageModel.BindDealersServiceCenters()");
            }

        }

        /// <summary>
        /// Binds the page meta tags.
        /// </summary>
        /// <param name="pageMetaTags">The page meta tags.</param>
        /// <param name="make">The make.</param>
        private void BindPageMetaTags()
        {
            try
            {
                objData.PageMetaTags.CanonicalUrl = string.Format("https://www.bikewale.com/{0}-scooters/", objData.Make.MaskingName);
                objData.PageMetaTags.AlternateUrl = string.Format("https://www.bikewale.com/m/{0}-scooters/", objData.Make.MaskingName);
                objData.PageMetaTags.Keywords = string.Format("{0} Scooter, {0} Scooty, Scooter {0}, Scooty {0}, Scooters, Scooty", objData.Make.MakeName);
                objData.PageMetaTags.Description = string.Format("Check {0} Scooty prices in India. Know more about new and upcoming {0} scooters, their prices, performance and mileage.", objData.Make.MakeName);
                objData.PageMetaTags.Title = string.Format("{0} Scooters in India | Scooty Prices, Mileage & Images - BikeWale", objData.Make.MakeName);
                objData.Page_H1 = string.Format("{0} Scooters", objData.Make.MakeName);

                SetBreadcrumList();
                SetPageJSONLDSchema();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ScootersMakePageModel.BindPageMetaTags()");
            }
        }

        /// <summary>
        /// Created By  : Sushil Kumar on 14th Sep 2017
        /// Description : Added breadcrum and webpage schema
        /// </summary>
        private void SetPageJSONLDSchema()
        {
            //set webpage schema for the model page
            WebPage webpage = SchemaHelper.GetWebpageSchema(objData.PageMetaTags, objData.BreadcrumbList);

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
            url += "scooters/";
            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, url, "Scooters"));

            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null, objData.Page_H1));

            objData.BreadcrumbList.BreadcrumListItem = BreadCrumbs;

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
                objResponse = _objMakeCache.GetMakeMaskingResponse(makeMaskingName);
                if (objResponse != null)
                {
                    Status = (StatusCodes)objResponse.StatusCode;
                    if (objResponse.StatusCode == 200)
                    {
                        _makeId = objResponse.MakeId;
                    }
                    else if (objResponse.StatusCode == 301)
                    {
                        RedirectUrl = HttpContext.Current.Request.RawUrl.Replace(makeMaskingName, objResponse.MaskingName);
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
                ErrorClass.LogError(ex, string.Format("ScootersMakePageModel.ProcessQuery() makeMaskingName:{0}", makeMaskingName));
            }
        }

        /// <summary>
        /// Binds the upcoming bikes.
        /// </summary>
        /// <param name="objData">The object data.</param>
        private void BindUpcomingBikes()
        {
            UpcomingBikesWidget objUpcoming = new UpcomingBikesWidget(_upcoming);
            objUpcoming.Filters = new Bikewale.Entities.BikeData.UpcomingBikesListInputEntity()
            {
                PageSize = 9,
                PageNo = 1,
                BodyStyleId = 5
            };
            objUpcoming.SortBy = EnumUpcomingBikesFilter.Default;
            objData.UpcomingScooters = objUpcoming.GetData();
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 15 June 2017
        /// Summary    : Bind make scooter related editorial content
        /// </summary>
        private void BindEditorialWidget()
        {
            RecentNews objNews = new RecentNews(EditorialTopCount, _makeId, _makeName, _makeMaskingName, string.Format("News about {0} Scooters", _makeName), _articles);
            objNews.IsScooter = true;
            objData.News = objNews.GetData();

            RecentExpertReviews objReviews = new RecentExpertReviews(EditorialTopCount, _makeId, _makeName, _makeMaskingName, _articles, string.Format("{0} Reviews", _makeName));
            objReviews.IsScooter = true;
            objData.ExpertReviews = objReviews.GetData();

            RecentVideos objVideos = new RecentVideos(1, (ushort)EditorialTopCount, _makeId, _makeName, _makeMaskingName, _videos);
            objVideos.IsScooter = true;
            objData.Videos = objVideos.GetData();
        }
    }
}