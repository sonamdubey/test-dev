
using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Used;
using Bikewale.Interfaces.Videos;
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
        public StatusCodes Status;
        private IDealerCacheRepository _dealerServiceCenters;
        private readonly IBikeModelsCacheRepository<int> _bikeModelsCache;
        private readonly IBikeMakesCacheRepository<int> _bikeMakesCache;
        private readonly ICMSCacheContent _articles = null;
        private readonly ICMSCacheContent _expertReviews = null;
        private readonly IVideos _videos = null;
        private readonly IUsedBikeDetailsCacheRepository _cachedBikeDetails = null;
        private readonly IDealerCacheRepository _cacheDealers = null;
        private readonly IUpcoming _upcoming = null;
        public StatusCodes status;
        public MakeMaskingResponse objResponse;
        //public BikeMakeEntityBase objMake;
        public string redirectUrl;

        public MakePageModel(string makeMaskingName, uint topCount, IDealerCacheRepository dealerServiceCenters, IBikeModelsCacheRepository<int> bikeModelsCache, IBikeMakesCacheRepository<int> bikeMakesCache, ICMSCacheContent articles, ICMSCacheContent expertReviews, IVideos videos, IUsedBikeDetailsCacheRepository cachedBikeDetails, IDealerCacheRepository cacheDealers, IUpcoming upcoming)
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
                GlobalCityAreaEntity location = GlobalCityArea.GetGlobalCityArea();
                if (location != null && location.CityId > 0)
                {
                    cityId = location.CityId;
                    cityName = location.City;
                    var cityEntity = new CityHelper().GetCityById(cityId);
                    cityMaskingName = cityEntity != null ? cityEntity.CityMaskingName : string.Empty;
                    objData.Location = cityName;
                    objData.LocationMasking = cityMaskingName;
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
                var baseDetails = _dealerServiceCenters.GetPopularCityDealer(this._makeId, this._topCount);
                if (baseDetails != null)
                {
                    objData.DealerServiceCenters = new DealerServiceCenterWidgetVM();
                    objData.DealerServiceCenters.DealerDetails = baseDetails.DealerDetails;
                    objData.DealerServiceCenters.TotalDealerCount = baseDetails.TotalDealerCount;
                    objData.DealerServiceCenters.TotalServiceCenterCount = baseDetails.TotalServiceCenterCount;
                }
                //UpcomingBikesWidget objUpcoming = new UpcomingBikesWidget(_bikeModelsCache)
                // {
                //     MakeId = this._makeId,
                //     TopCount = 9
                // };
                //objData.UpcomingBikes = objUpcoming.GetData();


                UpcomingBikesWidget objUpcoming = new UpcomingBikesWidget(_upcoming);

                objUpcoming.Filters = new Bikewale.Entities.BikeData.UpcomingBikesListInputEntity()
                {
                    EndIndex = 9,
                    StartIndex = 1,
                    MakeId = (int)this._makeId
                };
                objUpcoming.SortBy = Bikewale.Entities.BikeData.EnumUpcomingBikesFilter.Default;
                objData.UpcomingBikes = objUpcoming.GetData();



                objData.BikeDescription = _bikeMakesCache.GetMakeDescription((int)_makeId);
                objData.News = new RecentNews(2, _makeId, _makeName, _makeMaskingName, string.Format("{0} News", _makeName), _articles).GetData();

                objData.ExpertReviews = new RecentExpertReviews(2, _makeId, _makeName, _makeMaskingName, _expertReviews, string.Format("{0} Reviews", _makeName)).GetData();

                objData.Videos = new RecentVideos(1, 2, _videos).GetData();

                objData.UsedModels = new UsedBikeModelsWidgetModel(cityId, 9, _makeId, objData.Location, objData.LocationMasking, _cachedBikeDetails).GetData();

                objData.Showrooms = new ShowroomsWidgetModel(6, 3, cityId, cityName, _makeId, _makeName, _makeMaskingName, _cacheDealers).GetData();
                if (objData.Showrooms != null)
                    objData.IsDealerServiceDataAvailable = objData.Showrooms.IsDataAvailable;

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
                    objData.IsDealerServiceDataAvailable = objData.DealerServiceCenters != null && objData.DealerServiceCenters.DealerDetails.Count > 0;
                    objData.IsMakeTabsDataAvailable = (objData.BikeDescription != null && objData.BikeDescription.FullDescription.Length > 0 || objData.IsNewsAvailable ||
                        objData.IsExpertReviewsAvailable || objData.IsVideosAvailable || objData.IsUsedModelsBikeAvailable || objData.IsDealerServiceDataAvailable);

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