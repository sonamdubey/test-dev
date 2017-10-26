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
using Bikewale.Interfaces.Videos;
using Bikewale.Models.CompareBikes;
using Bikewale.Models.ServiceCenters;
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
    /// </summary>
    public class MakePageModel
    {
        private readonly string _makeMaskingName;
        private uint _makeId;
        private readonly IBikeModelsCacheRepository<int> _bikeModelsCache;
        private readonly IBikeMakesCacheRepository<int> _bikeMakesCache;
        private readonly ICMSCacheContent _articles = null;
        private readonly ICMSCacheContent _expertReviews = null;
        private readonly IVideos _videos = null;
        private readonly IUsedBikeDetailsCacheRepository _cachedBikeDetails = null;
        private readonly IDealerCacheRepository _cacheDealers = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeCompare _compareBikes = null;
        private readonly IServiceCenter _objSC;
        public StatusCodes Status { get; set; }
        public MakeMaskingResponse objResponse { get; set; }
        public string RedirectUrl { get; set; }
        public CompareSources CompareSource { get; set; }
        public bool IsMobile { get; set; }

        public MakePageModel(string makeMaskingName, IBikeModelsCacheRepository<int> bikeModelsCache, IBikeMakesCacheRepository<int> bikeMakesCache, ICMSCacheContent articles, ICMSCacheContent expertReviews, IVideos videos, IUsedBikeDetailsCacheRepository cachedBikeDetails, IDealerCacheRepository cacheDealers, IUpcoming upcoming, IBikeCompare compareBikes, IServiceCenter objSC)
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
            ProcessQuery(this._makeMaskingName);
        }

        /// <summary>
        /// Gets the data for homepage
        /// Modified by : Ashutosh Sharma on 05 Oct 2017
        /// Description : Replaced call to method 'GetMostPopularBikesByMake' with 'GetMostPopularBikesByMakeWithCityPrice' to get city price when city is selected.
        /// Modified by : Vivek Singh Tomar on 11th Oct 2017
        /// Summary : Removed unnecessary arguments from BindDealerSserviceData which was required for fetchin service center details
        /// </summary>
        /// <returns>
        /// Created by : Sangram Nandkhile on 25-Mar-2017 
        /// </returns>
        public MakePageVM GetData()
        {
            MakePageVM objData = new MakePageVM();

            try
            {
                #region Variable initialization

                uint cityId = 0;
                string cityName = string.Empty, cityMaskingName = string.Empty;
                CityEntityBase cityBase = null;
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
                BikeMakeEntityBase makeBase = _bikeMakesCache.GetMakeDetails(_makeId);
                objData.BikeDescription = _bikeMakesCache.GetMakeDescription((int)_makeId);
                if (makeBase != null)
                {
                    objData.MakeMaskingName = makeBase.MaskingName;
                    objData.MakeName = makeBase.MakeName;
                }
                BindPageMetaTags(objData, objData.Bikes, makeBase);
                BindUpcomingBikes(objData);
                BindCompareBikes(objData, CompareSource, cityId);
                BindDealerServiceData(objData, cityId);
                BindCMSContent(objData);
                objData.UsedModels = BindUsedBikeByModel(_makeId, cityId);
                BindDiscontinuedBikes(objData);
                BindOtherMakes(objData);
                #region Set Visible flags

                if (objData != null)
                {
                    objData.IsUpComingBikesAvailable = objData.UpcomingBikes != null && objData.UpcomingBikes.UpcomingBikes != null && objData.UpcomingBikes.UpcomingBikes.Any();
                    objData.IsNewsAvailable = objData.News != null && objData.News.ArticlesList != null && objData.News.ArticlesList.Any();
                    objData.IsExpertReviewsAvailable = objData.News != null && objData.ExpertReviews.ArticlesList != null && objData.ExpertReviews.ArticlesList.Any();
                    objData.IsVideosAvailable = objData.Videos != null && objData.Videos.VideosList != null && objData.Videos.VideosList.Any();
                    objData.IsUsedModelsBikeAvailable = objData.UsedModels != null && objData.UsedModels.UsedBikeModelList != null && objData.UsedModels.UsedBikeModelList.Any();

                    objData.IsDealerAvailable = objData.Dealers != null && objData.Dealers.Dealers != null && objData.Dealers.Dealers.Any();
                    objData.IsDealerServiceDataAvailable = cityId > 0 && objData.IsDealerAvailable;
                    objData.IsDealerServiceDataInIndiaAvailable = cityId == 0 && objData.DealersServiceCenter != null && objData.DealersServiceCenter.DealerServiceCenters != null && objData.DealersServiceCenter.DealerServiceCenters.DealerDetails != null && objData.DealersServiceCenter.DealerServiceCenters.DealerDetails.Any();

                    objData.IsMakeTabsDataAvailable = (objData.BikeDescription != null && objData.BikeDescription.FullDescription.Length > 0 || objData.IsNewsAvailable ||
                        objData.IsExpertReviewsAvailable || objData.IsVideosAvailable || objData.IsUsedModelsBikeAvailable || objData.IsDealerServiceDataAvailable || objData.IsDealerServiceDataInIndiaAvailable);
                }
                #endregion
            }
            catch (Exception ex)
            {
                ErrorClass er = new ErrorClass(ex, string.Format("MakePageModel.GetData() => MakeId: {0}", _makeId));
            }

            return objData;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 24 Aug 2017
        /// Description :   Bind Other Make list
        /// </summary>
        /// <param name="objData"></param>
        private void BindOtherMakes(MakePageVM objData)
        {
            try
            {
                var makes = _bikeMakesCache.GetMakesByType(EnumBikeType.New);
                if (makes != null && makes.Any())
                {
                    objData.OtherMakes = makes.Where(m => m.MakeId != _makeId).Take(9);
                }
            }
            catch (Exception ex)
            {
                ErrorClass er = new ErrorClass(ex, string.Format("MakePageModel.BindOtherMakes() => MakeId: {0}", _makeId));
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

                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "MakePageModel.BindUsedBikeByModel()");
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
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "MakePageModel.BindCompareBikes");
            }
        }

        private void BindCMSContent(MakePageVM objData)
        {
            objData.News = new RecentNews(2, _makeId, objData.MakeName, _makeMaskingName, string.Format("{0} News", objData.MakeName), _articles).GetData();
            objData.ExpertReviews = new RecentExpertReviews(2, _makeId, objData.MakeName, _makeMaskingName, _expertReviews, string.Format("{0} Reviews", objData.MakeName)).GetData();
            objData.Videos = new RecentVideos(1, 2, _makeId, objData.MakeName, _makeMaskingName, _videos).GetData();

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
        private void BindPageMetaTags(MakePageVM objData, IEnumerable<MostPopularBikesBase> objModelList, BikeMakeEntityBase objMakeBase)
        {
            long minPrice = 0;
            long MaxPrice = 0;
            try
            {
                if (objModelList != null && objModelList.Any())
                {
                    minPrice = objModelList.Min(bike => bike.VersionPrice);
                    MaxPrice = objModelList.Max(bike => bike.VersionPrice);
                }

                objData.PageMetaTags.Title = string.Format("{0} Bikes | {1} {0} Models- Prices, Dealers, & Images- BikeWale", objData.MakeName, objData.Bikes.Count());
                objData.PageMetaTags.Description = string.Format("{0} Price in India - Rs. {1} - Rs. {2}. Check out {0} on road price, reviews, mileage, versions, news & images at Bikewale.", objData.MakeName, Bikewale.Utility.Format.FormatPrice(minPrice.ToString()), Bikewale.Utility.Format.FormatPrice(MaxPrice.ToString()));
                objData.PageMetaTags.CanonicalUrl = string.Format("{0}/{1}-bikes/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, _makeMaskingName);
                objData.PageMetaTags.AlternateUrl = string.Format("{0}/m/{1}-bikes/", BWConfiguration.Instance.BwHostUrl, _makeMaskingName);
                objData.PageMetaTags.AmpUrl = string.Format("{0}/m/{1}-bikes/amp/", BWConfiguration.Instance.BwHostUrl, _makeMaskingName);
                objData.PageMetaTags.Keywords = string.Format("{0}, {0} Bikes , {0} Bikes prices, {0} Bikes reviews, {0} Images, new {0} Bikes", objData.MakeName);
                objData.AdTags.TargetedMakes = objData.MakeName;
                objData.Page_H1 = string.Format("{0} Bikes", objData.MakeName);

                SetBreadcrumList(ref objData);

                CheckCustomPageMetas(objData, objMakeBase);

                SetPageJSONLDSchema(objData);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "MakePageModel.BindPageMetaTags()");
            }

        }

        /// <summary>
        /// Created By  : Sushil Kumar on 14th Sep 2017
        /// Description : Added breadcrum and webpage schema and added brand schema
        /// </summary>
        private void SetPageJSONLDSchema(MakePageVM objPageMeta)
        {
            //set webpage schema for the model page
            WebPage webpage = SchemaHelper.GetWebpageSchema(objPageMeta.PageMetaTags, objPageMeta.BreadcrumbList);

            if (webpage != null)
            {
                objPageMeta.PageMetaTags.SchemaJSON = SchemaHelper.JsonSerialize(webpage);
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 12th Sep 2017
        /// Description : Function to create page level schema for breadcrum
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
            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, null, objData.Page_H1));


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
                ErrorClass objErr = new ErrorClass(ex, string.Format("MakePageModel.CheckCustomPageMetas() makeId:{0}", _makeId));
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
                ErrorClass objErr = new ErrorClass(ex, string.Format("MakePageModel.ProcessQuery() makeMaskingName:{0}", makeMaskingName));
            }
        }
    }
}