﻿using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.PWA.Articles;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Pager;
using Bikewale.Notifications;
using Bikewale.PWA.Utils;
using Bikewale.Service.Utilities;
using Bikewale.Utility;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.PWA.CMS
{
    /// <summary>
    /// Edit CMS Controller :  All Edit CMS related Operations 
    /// Author : Sushil Kumar
    /// Created On : 24th August 2015
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class PwaCMSController : CompressionApiController//ApiController
    {

        private readonly IPager _pager = null;
        private readonly ICMSCacheContent _CMSCache = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModelEntity = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeMakesCacheRepository _bikeMakesEntity;
        private readonly IBikeModelsCacheRepository<int> _objModelCache;
        private readonly IBikeInfo _bikeInfo;
        private readonly ICityCacheRepository _cityCacheRepository;
        static ILog _logger = LogManager.GetLogger("PwaCMSController");
        private readonly bool _logNewsUrl = BWConfiguration.Instance.LogNewsUrl;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pager"></param>
        public PwaCMSController(IPager pager, ICMSCacheContent cmsCache,
            IBikeModelsCacheRepository<int> objModelCache,
            IBikeModels<BikeModelEntity, int> bikeModelEntity,
            IUpcoming upcoming,
            IBikeMakesCacheRepository bikeMakesEntity,
            IBikeInfo bikeInfo,
            ICityCacheRepository cityCacheRepository)
        {
            _pager = pager;
            _CMSCache = cmsCache;
            _bikeModelEntity = bikeModelEntity;
            _upcoming = upcoming;
            _bikeMakesEntity = bikeMakesEntity;
            _objModelCache = objModelCache;
            _bikeInfo = bikeInfo;
            _cityCacheRepository = cityCacheRepository;
        }


        #region News Details Api
        /// <summary>
        /// Modified By : Ashish G. Kamble
        /// Summary : API to get details of article. This is api is used for the articles single page. e.g. News.
        /// Modified By : Sangram Nandkhile on 04 Mar 2016
        /// Summary : Utility function to fetch shareurl is used
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns>News Details</returns>
        [ResponseType(typeof(PwaArticleDetails)), Route("api/pwa/cms/id/{basicId}/page/")]
        public IHttpActionResult GetArticleDetailsPage(string basicId)
        {
            uint _basicId = default(uint);
            PwaArticleDetails objPwaArticle = null;
            try
            {
                if (!String.IsNullOrEmpty(basicId) && uint.TryParse(basicId, out _basicId))
                {

                    ArticleDetails objNews = _CMSCache.GetNewsDetails(_basicId);

                    if (objNews != null)
                    {
                        objPwaArticle = ConverterUtility.MapArticleDetailsToPwaArticleDetails(objNews);

                        if (_logNewsUrl && !string.IsNullOrEmpty(objPwaArticle.ArticleUrl) && objPwaArticle.ArticleUrl.EndsWith(@".html.html"))
                        {
                            ThreadContext.Properties["NewsUrl"] = objPwaArticle.ArticleUrl;
                            ThreadContext.Properties["ShareUrl"] = objPwaArticle.ShareUrl;
                            _logger.Error(String.Format("api/pwa/cms/id/{0}/page/", basicId));
                        }
                    }
                    return Ok(objPwaArticle);
                }

                else
                {
                    BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.CMS.CMSController");
               
                return InternalServerError();
            }
            return NotFound();
        }  //get News Details

        #endregion

        #region BikeList based on BasicId
        /// <summary>        
        /// Created By : Prasad Gawde on 04 Apr 2017
        /// Summary : 
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns>Two Lists of Bikes</returns>
        [ResponseType(typeof(List<PwaBikeNews>)), Route("api/pwa/cms/bikelists/id/{basicId}/page/")]
        public IHttpActionResult GetBikeListsForEditCms(string basicId)
        {
            int _basicId;
            IEnumerable<MostPopularBikesBase> bikes;
            List<PwaBikeNews> objPwaBikeNews = new List<PwaBikeNews>();
            try
            {
                if (int.TryParse(basicId, out _basicId))
                {
                    uint cityId = 0;
                    var currentCityArea = GlobalCityArea.GetGlobalCityArea();
                    if (currentCityArea != null)
                        cityId = currentCityArea.CityId;

                    if (_basicId > 0)
                    {
                        var articleDetails = _CMSCache.GetNewsDetails((uint)_basicId);

                        if (articleDetails != null)
                        {
                            var makeData = GetTaggedBikeListByMake(articleDetails);
                            var modelId = GetTaggedBikeListByModel(articleDetails);

                            uint makeId = makeData == null ? 0 : (uint)makeData.MakeId;

                            bikes = _bikeModelEntity.GetMostPopularBikes(EnumBikeType.All, 9, makeId, cityId);

                            PwaBikeNews popularBikes = new PwaBikeNews();
                            if (bikes != null)
                            {
                                popularBikes.BikesList = ConverterUtility.MapMostPopularBikesBaseToPwaBikeDetails(bikes, currentCityArea.City);


                                if (makeId > 0)
                                {
                                    popularBikes.Heading = string.Format("Popular {0} bikes", makeData.MakeName);
                                    popularBikes.CompleteListUrl = string.Format("/m/{0}-bikes/", makeData.MaskingName);
                                    popularBikes.CompleteListUrlAlternateLabel = string.Format("{0} Bikes", makeData.MakeName);
                                }
                                else
                                {
                                    popularBikes.Heading = "Popular bikes";
                                    popularBikes.CompleteListUrl = "/m/best-bikes-in-india/";
                                    popularBikes.CompleteListUrlAlternateLabel = "Best Bikes in India";
                                }
                                popularBikes.CompleteListUrlLabel = "View all";

                                objPwaBikeNews.Add(popularBikes);
                            }

                            popularBikes = null;
                            if (modelId > 0)
                            {

                                bikes = _objModelCache.GetMostPopularBikesByModelBodyStyle((int)modelId, 9, cityId);

                                if (bikes != null && bikes.Any())
                                {
                                    popularBikes = new PwaBikeNews();
                                    var bodyStyle = bikes.FirstOrDefault().BodyStyle;
                                    popularBikes.BikesList = ConverterUtility.MapMostPopularBikesBaseToPwaBikeDetails(bikes, currentCityArea.City);
                                    popularBikes.Heading = string.Format("Popular {0}", BodyStyleLinks.BodyStyleHeadingText(bodyStyle));
                                    popularBikes.CompleteListUrlAlternateLabel = string.Format("Best {0} in India", BodyStyleLinks.BodyStyleHeadingText(bodyStyle));
                                    popularBikes.CompleteListUrl = "/m" + UrlFormatter.FormatGenericPageUrl(bodyStyle);
                                    popularBikes.CompleteListUrlLabel = "View all";
                                    objPwaBikeNews.Add(popularBikes);
                                }
                            }
                            else
                            {
                                PwaBikeNews upcomingBikes = new PwaBikeNews();
                                UpcomingBikesListInputEntity filters = new UpcomingBikesListInputEntity();
                                filters.PageNo = 1;
                                filters.PageSize = 9;
                                if (makeId > 0)
                                    filters.MakeId = (int)makeId;

                                var tempbikes = _upcoming.GetModels(filters, EnumUpcomingBikesFilter.Default);
                                if (tempbikes != null && tempbikes.Any())
                                {
                                    upcomingBikes.BikesList = ConverterUtility.MapUpcomingBikeEntityToPwaBikeDetails(tempbikes, currentCityArea.City);
                                }

                                if (makeData != null)
                                {
                                    upcomingBikes.Heading = string.Format("Upcoming {0} bikes", makeData.MakeName);
                                    upcomingBikes.CompleteListUrl = string.Format("/m/{0}-bikes/upcoming/", makeData.MaskingName);
                                }
                                else
                                {
                                    upcomingBikes.Heading = "Upcoming bikes";
                                    upcomingBikes.CompleteListUrl = "/m/upcoming-bikes/";
                                }
                                upcomingBikes.CompleteListUrlAlternateLabel = "Upcoming Bikes in India";
                                upcomingBikes.CompleteListUrlLabel = "View all";
                                if (upcomingBikes.BikesList != null)
                                    objPwaBikeNews.Add(upcomingBikes);
                            }
                        }

                    }
                    else
                    {// negative or zero basic id means send popular Bikes and Upcoming Bikes
                     //get popular bikes

                        bikes = _bikeModelEntity.GetMostPopularBikes(EnumBikeType.All, 9, 0, cityId);
                        if (bikes != null && bikes.Any())
                        {
                            PwaBikeNews popularBikes = new PwaBikeNews();
                            popularBikes.Heading = "Popular bikes";
                            popularBikes.CompleteListUrl = "/m/best-bikes-in-india/";
                            popularBikes.CompleteListUrlAlternateLabel = "Best Bikes in India";
                            popularBikes.CompleteListUrlLabel = "View all";
                            popularBikes.BikesList = ConverterUtility.MapMostPopularBikesBaseToPwaBikeDetails(bikes, currentCityArea.City);
                            objPwaBikeNews.Add(popularBikes);
                        }

                        UpcomingBikesListInputEntity filters = new UpcomingBikesListInputEntity();
                        filters.PageNo = 1;
                        filters.PageSize = 9;
                        var tempbikes = _upcoming.GetModels(filters, EnumUpcomingBikesFilter.Default);
                        if (tempbikes != null && tempbikes.Any())
                        {
                            PwaBikeNews upcomingBikes = new PwaBikeNews();
                            upcomingBikes.Heading = "Upcoming bikes";
                            upcomingBikes.CompleteListUrl = "/m/upcoming-bikes/";
                            upcomingBikes.CompleteListUrlAlternateLabel = "Upcoming Bikes in India";
                            upcomingBikes.CompleteListUrlLabel = "View all";
                            upcomingBikes.BikesList = ConverterUtility.MapUpcomingBikeEntityToPwaBikeDetails(tempbikes, currentCityArea.City);
                            objPwaBikeNews.Add(upcomingBikes);
                        }

                    }

                    return Ok(objPwaBikeNews);
                }

                else
                {
                    BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.CMS.CMSController");
               
                return InternalServerError();
            }
            return NotFound();
        }  //get News Details

        /// <summary>
        /// Created by : Aditi Srivastava on 29 Mar 2017
        /// Summary    : Get tagged make in article
        /// </summary>
        private BikeMakeEntityBase GetTaggedBikeListByMake(ArticleDetails artDetails)
        {
            try
            {
                if (artDetails.VehiclTagsList != null && artDetails.VehiclTagsList.Count > 0)
                {
                    BikeMakeEntityBase make;
                    var taggedMakeObj = artDetails.VehiclTagsList.FirstOrDefault(m => !string.IsNullOrEmpty(m.MakeBase.MaskingName));
                    if (taggedMakeObj != null)
                    {
                        return taggedMakeObj.MakeBase;
                    }
                    else
                    {
                        make = artDetails.VehiclTagsList.FirstOrDefault().MakeBase;
                        if (make != null)
                            return _bikeMakesEntity.GetMakeDetails((uint)make.MakeId);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Pwa.Bikewale.Models.NewsDetailPage.GetTaggedBikeListByMake");
            }
            return null;
        }


        /// <summary>
        /// Created by : Aditi Srivastava on 29 Mar 2017
        /// Summary    : Get tagged model in article
        /// </summary>
        private uint GetTaggedBikeListByModel(ArticleDetails artDetails)
        {
            try
            {
                if (artDetails.VehiclTagsList != null && artDetails.VehiclTagsList.Count > 0)
                {

                    BikeModelEntityBase model;
                    var taggedModelObj = artDetails.VehiclTagsList.FirstOrDefault(m => !string.IsNullOrEmpty(m.ModelBase.MaskingName));
                    if (taggedModelObj != null)
                    {
                        return (uint)taggedModelObj.ModelBase.ModelId;
                    }
                    else
                    {
                        model = artDetails.VehiclTagsList.FirstOrDefault().ModelBase;
                        if (model != null)
                            return (uint)model.ModelId;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Pwa.Bikewale.Models.NewsDetailPage.GetTaggedBikeListByModel");
            }
            return 0;
        }

        #endregion

        #region BikeInfoSlug based on basicId Api
        /// <summary>
        /// Modified By : Ashish G. Kamble
        /// Summary : API to get details of article. This is api is used for the articles single page. e.g. News.
        /// Modified By : Sangram Nandkhile on 04 Mar 2016
        /// Summary : Utility function to fetch shareurl is used
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns>News Details</returns>
        [ResponseType(typeof(PwaBikeInfo)), Route("api/pwa/cms/bikeinfo/id/{basicId}/page/")]
        public IHttpActionResult GetBikeInfo(string basicId)
        {
            uint _basicId = default(uint);
            PwaBikeInfo outBikeInfo = null;
            GenericBikeInfo objBikeInfo = null;
            try
            {
                if (uint.TryParse(basicId, out _basicId))
                {
                    if (_basicId > 0)
                    {
                        var articleDetails = _CMSCache.GetNewsDetails((uint)_basicId);

                        if (articleDetails != null)
                        {
                            uint cityId = 0;
                            var currentCityArea = GlobalCityArea.GetGlobalCityArea();
                            if (currentCityArea != null)
                                cityId = currentCityArea.CityId;

                            var modelId = GetTaggedBikeListByModel(articleDetails);
                            objBikeInfo = _bikeInfo.GetBikeInfo(modelId, cityId);

                            if (objBikeInfo != null)
                            {
                                //modelid is not getting set
                                if (objBikeInfo.Model != null && modelId != 0 && objBikeInfo.Model.ModelId != modelId)
                                    objBikeInfo.Model.ModelId = (int)modelId;

                                CityEntityBase cityDetails = null;
                                if (cityId > 0)
                                {
                                    var objCityList = _cityCacheRepository.GetAllCities(EnumBikeType.All);
                                    cityDetails = objCityList.FirstOrDefault(c => c.CityId == cityId);
                                }

                                objBikeInfo.Tabs = BindInfoWidgetDatas(objBikeInfo, cityDetails, 3, BikeInfoTabType.News);

                                outBikeInfo = ConverterUtility.MapGenericBikeInfoToPwaBikeInfo(objBikeInfo, cityDetails);
                            }
                        }
                    }
                    return Ok(outBikeInfo);
                }
                else
                {
                    BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.CMS.CMSController");
               
                return InternalServerError();
            }
            return NotFound();
        }  //get News Details

        private ICollection<BikeInfoTab> BindInfoWidgetDatas(GenericBikeInfo _genericBikeInfo, CityEntityBase cityDetails,
            uint totalTabCount, BikeInfoTabType pageId)
        {
            ICollection<BikeInfoTab> tabs = null;
            try
            {
                tabs = new Collection<BikeInfoTab>();
                if (_genericBikeInfo.ExpertReviewsCount > 0)
                {
                    tabs.Add(new BikeInfoTab()
                    {
                        URL = Bikewale.Utility.UrlFormatter.FormatExpertReviewUrl(_genericBikeInfo.Make.MaskingName, _genericBikeInfo.Model.MaskingName),
                        Title = "Expert Reviews",
                        TabText = "Expert Reviews",
                        IconText = "reviews",
                        Count = _genericBikeInfo.ExpertReviewsCount,
                        Tab = BikeInfoTabType.ExpertReview
                    });
                }
                if (_genericBikeInfo.NewsCount > 0)
                {
                    tabs.Add(new BikeInfoTab()
                    {
                        URL = Bikewale.Utility.UrlFormatter.FormatNewsUrl(_genericBikeInfo.Make.MaskingName, _genericBikeInfo.Model.MaskingName),
                        Title = "News",
                        TabText = "News",
                        IconText = "reviews",
                        Count = _genericBikeInfo.NewsCount,
                        Tab = BikeInfoTabType.News
                    });
                }
                if (_genericBikeInfo.PhotosCount > 0)
                {
                    tabs.Add(new BikeInfoTab()
                    {
                        URL = Bikewale.Utility.UrlFormatter.FormatPhotoPageUrl(_genericBikeInfo.Make.MaskingName, _genericBikeInfo.Model.MaskingName),
                        Title = "Images",
                        TabText = "Images",
                        IconText = "photos",
                        Count = _genericBikeInfo.PhotosCount,
                        Tab = BikeInfoTabType.Image
                    });
                }
                if (_genericBikeInfo.VideosCount > 0)
                {
                    tabs.Add(new BikeInfoTab()
                    {
                        URL = Bikewale.Utility.UrlFormatter.FormatVideoPageUrl(_genericBikeInfo.Make.MaskingName, _genericBikeInfo.Model.MaskingName),
                        Title = "Videos",
                        TabText = "Videos",
                        IconText = "videos",
                        Count = _genericBikeInfo.VideosCount,
                        Tab = BikeInfoTabType.Videos
                    });
                }
                if (_genericBikeInfo.IsSpecsAvailable)
                {
                    tabs.Add(new BikeInfoTab()
                    {
                        URL = Bikewale.Utility.UrlFormatter.ViewAllFeatureSpecs(_genericBikeInfo.Make.MaskingName, _genericBikeInfo.Model.MaskingName),
                        Title = "Specification",
                        TabText = "Specs",
                        IconText = "specs",
                        IsVisible = _genericBikeInfo.IsSpecsAvailable,
                        Tab = BikeInfoTabType.Specs
                    });
                }
                if (_genericBikeInfo.UserReview > 0)
                {
                    tabs.Add(new BikeInfoTab()
                    {
                        URL = Bikewale.Utility.UrlFormatter.FormatUserReviewUrl(_genericBikeInfo.Make.MaskingName, _genericBikeInfo.Model.MaskingName),
                        Title = "User Reviews",
                        TabText = "User Reviews",
                        IconText = "user-reviews",
                        Count = _genericBikeInfo.UserReview,
                        Tab = BikeInfoTabType.UserReview
                    });
                }
                if (_genericBikeInfo.DealersCount > 0)
                {
                    tabs.Add(new BikeInfoTab()
                    {
                        URL = Bikewale.Utility.UrlFormatter.DealerLocatorUrl(_genericBikeInfo.Make.MaskingName, cityDetails != null ? cityDetails.CityMaskingName : "india"),
                        Title = string.Format("Dealers in {0}", cityDetails != null ? cityDetails.CityName : "India"),
                        TabText = "Dealers",
                        IconText = "dealers",
                        Count = _genericBikeInfo.DealersCount,
                        Tab = BikeInfoTabType.Dealers
                    });
                }
                if (tabs.Any())
                {
                    tabs = tabs.Where(m => (m.Count > 0 || m.IsVisible) && pageId != m.Tab).OrderBy(m => m.Tab).Take((int)totalTabCount).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "VideoDetailsHelper.BindInfoWidgetDatas");
            }
            return tabs;
        }

        #endregion
    }   // class
}   // namespace
