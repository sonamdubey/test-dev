
using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Interfaces.Used;
using Bikewale.Interfaces.Videos;
using Bikewale.Models.ServiceCenters;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 27-Mar-2017
    /// Model for make page
    /// </summary>
    public class MakePageModel
    {
        public string RedirectUrl;
        private string _makeName, _makeMaskingName;
        private uint _topCount, _makeId;
        private IDealerCacheRepository _dealerServiceCenters;
        private readonly IBikeModelsCacheRepository<int> _bikeModelsCache;
        private readonly IBikeMakesCacheRepository<int> _bikeMakesCache;
        private readonly ICMSCacheContent _articles = null;
        private readonly ICMSCacheContent _expertReviews = null;
        private readonly IVideos _videos = null;
        private readonly IUsedBikeDetailsCacheRepository _cachedBikeDetails = null;
        private readonly IDealerCacheRepository _cacheDealers = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IServiceCenter _objSC;
        public StatusCodes status;
        public MakeMaskingResponse objResponse;
        public string redirectUrl;

        public MakePageModel(string makeMaskingName, uint topCount, IDealerCacheRepository dealerServiceCenters, IBikeModelsCacheRepository<int> bikeModelsCache, IBikeMakesCacheRepository<int> bikeMakesCache, ICMSCacheContent articles, ICMSCacheContent expertReviews, IVideos videos, IUsedBikeDetailsCacheRepository cachedBikeDetails, IDealerCacheRepository cacheDealers, IUpcoming upcoming, IServiceCenter objSC)
        {
            this._makeMaskingName = makeMaskingName;
            this._dealerServiceCenters = dealerServiceCenters;
            this._topCount = topCount > 0 ? topCount : 9;
            this._bikeModelsCache = bikeModelsCache;
            this._bikeMakesCache = bikeMakesCache;
            this._articles = articles;
            this._expertReviews = expertReviews;
            this._videos = videos;
            this._cachedBikeDetails = cachedBikeDetails;
            this._cacheDealers = cacheDealers;
            this._upcoming = upcoming;
            this._objSC = objSC;
            ProcessQuery(this._makeMaskingName);
        }

        /// <summary>
        /// Gets the data for homepage
        /// </summary>
        /// <returns>
        /// Created by : Sangram Nandkhile on 25-Mar-2017 
        /// </returns>
        public MakePageVM GetData()
        {
            MakePageVM objData = new MakePageVM();

            try
            {
                uint cityId = 0;
                string cityName = string.Empty, cityMaskingName = string.Empty;
                CityEntityBase cityBase = null;
                GlobalCityAreaEntity location = GlobalCityArea.GetGlobalCityArea();
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

                objData.Bikes = _bikeModelsCache.GetMostPopularBikesByMake((int)_makeId);
                BikeMakeEntityBase makeBase = _bikeMakesCache.GetMakeDetails(_makeId);
                if (makeBase != null)
                {
                    objData.MakeMaskingName = makeBase.MaskingName;
                    objData.MakeName = _makeName = makeBase.MakeName;
                }
                BindPageMetaTags(objData.PageMetaTags, objData.Bikes, _makeName);

                UpcomingBikesWidget objUpcoming = new UpcomingBikesWidget(_upcoming);
                objUpcoming.Filters = new Bikewale.Entities.BikeData.UpcomingBikesListInputEntity()
                {
                    EndIndex = 9,
                    StartIndex = 1,
                    MakeId = (int)this._makeId
                };
                objUpcoming.SortBy = Bikewale.Entities.BikeData.EnumUpcomingBikesFilter.Default;
                objData.UpcomingBikes = objUpcoming.GetData();
                if (cityId > 0)
                {
                    var dealerData = new DealerCardWidget(_cacheDealers, cityId, _makeId);
                    dealerData.TopCount = 3;
                    objData.Dealers = dealerData.GetData();
                    objData.ServiceCenters = new ServiceCentersCard(_objSC, 3, makeBase, cityBase).GetData();
                }
                else
                {
                    objData.DealersServiceCenter = new DealersServiceCentersIndiaWidgetModel(_makeId, _makeName, _makeMaskingName, _cacheDealers).GetData();
                }
                objData.BikeDescription = _bikeMakesCache.GetMakeDescription((int)_makeId);

                objData.News = new RecentNews(2, _makeId, _makeName, _makeMaskingName, string.Format("{0} News", _makeName), _articles).GetData();

                objData.ExpertReviews = new RecentExpertReviews(2, _makeId, _makeName, _makeMaskingName, _expertReviews, string.Format("{0} Reviews", _makeName)).GetData();

                objData.Videos = new RecentVideos(1, 2, _videos).GetData();

                objData.UsedModels = new UsedBikeModelsWidgetModel(cityId, 9, _makeId, objData.Location, objData.LocationMasking, _cachedBikeDetails).GetData();

                objData.DiscontinuedBikes = _bikeMakesCache.GetDiscontinuedBikeModelsByMake(_makeId);
                objData.IsDiscontinuedBikeAvailable = objData.DiscontinuedBikes != null && objData.DiscontinuedBikes.Count() > 0;

                if (objData.IsDiscontinuedBikeAvailable)
                {
                    foreach (var bike in objData.DiscontinuedBikes)
                    {
                        bike.Href = string.Format("/{0}-bikes/{1}/", _makeMaskingName, bike.ModelMasking);
                        bike.BikeName = string.Format("{0} {1}", _makeName, bike.ModelName);
                    }
                }

                #region Set Visible flags

                if (objData != null)
                {
                    objData.IsUpComingBikesAvailable = objData.UpcomingBikes != null && objData.UpcomingBikes.UpcomingBikes != null && objData.UpcomingBikes.UpcomingBikes.Count() > 0;
                    objData.IsNewsAvailable = objData.News != null && objData.News.ArticlesList != null && objData.News.ArticlesList.Count() > 0;
                    objData.IsExpertReviewsAvailable = objData.News != null && objData.ExpertReviews.ArticlesList != null && objData.ExpertReviews.ArticlesList.Count() > 0;
                    objData.IsVideosAvailable = objData.Videos != null && objData.Videos.VideosList != null && objData.Videos.VideosList.Count() > 0;
                    objData.IsUsedModelsBikeAvailable = objData.UsedModels != null && objData.UsedModels.UsedBikeModelList != null && objData.UsedModels.UsedBikeModelList.Count() > 0;

                    objData.IsDealerAvailable = objData.Dealers != null && objData.Dealers.Dealers != null && objData.Dealers.Dealers.Count() > 0;
                    objData.IsServiceDataAvailable = objData.ServiceCenters != null && objData.ServiceCenters.ServiceCentersList != null && objData.ServiceCenters.ServiceCentersList.Count() > 0;
                    objData.IsDealerServiceDataAvailable = cityId > 0 && (objData.IsDealerAvailable || objData.IsServiceDataAvailable);
                    objData.IsDealerServiceDataInIndiaAvailable = cityId == 0 && objData.DealersServiceCenter != null && objData.DealersServiceCenter.DealerServiceCenters != null && objData.DealersServiceCenter.DealerServiceCenters.DealerDetails != null && objData.DealersServiceCenter.DealerServiceCenters.DealerDetails.Count() > 0;

                    objData.IsMakeTabsDataAvailable = (objData.BikeDescription != null && objData.BikeDescription.FullDescription.Length > 0 || objData.IsNewsAvailable ||
                        objData.IsExpertReviewsAvailable || objData.IsVideosAvailable || objData.IsUsedModelsBikeAvailable || objData.IsDealerServiceDataAvailable || objData.IsDealerServiceDataInIndiaAvailable);

                }
                #endregion
            }
            catch (Exception ex)
            {
                ErrorClass er = new ErrorClass(ex, string.Format("MakePageModel.GetData() => MakeName: {0}", _makeName));
            }

            return objData;
        }

        private void BindPageMetaTags(PageMetaTags pageMetaTags, IEnumerable<MostPopularBikesBase> objModelList, string makeName)
        {
            long minPrice = objModelList.Min(bike => bike.VersionPrice);
            long MaxPrice = objModelList.Max(bike => bike.VersionPrice);
            pageMetaTags.Title = string.Format("{0} Bikes Prices, Reviews, Mileage & Images - BikeWale", makeName);
            pageMetaTags.Description = string.Format("{0} Price in India - Rs. {1} - Rs. {2}. Check out {0} on road price, reviews, mileage, versions, news & images at Bikewale.", makeName, Bikewale.Utility.Format.FormatPrice(minPrice.ToString()), Bikewale.Utility.Format.FormatPrice(MaxPrice.ToString()));
            pageMetaTags.CanonicalUrl = string.Format("https://www.bikewale.com/{0}-bikes/", makeName);
            pageMetaTags.Keywords = string.Format("{0}, {0} Bikes , {0} Bikes prices, {0} Bikes reviews, {0} Images, new {0} Bikes", makeName);
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
                    status = (StatusCodes)objResponse.StatusCode;
                    if (objResponse.StatusCode == 200)
                    {
                        _makeId = objResponse.MakeId;
                    }
                    else if (objResponse.StatusCode == 301)
                    {
                        status = StatusCodes.RedirectPermanent;
                    }
                    else
                    {
                        status = StatusCodes.ContentNotFound;
                    }
                }
                else
                {
                    status = StatusCodes.ContentNotFound;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("MakePageModel.ProcessQuery() makeMaskingName:{0}", makeMaskingName));
            }
        }
    }
}