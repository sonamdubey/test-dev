using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.PWA.Articles;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Pager;
using Bikewale.Notifications;
using Bikewale.PWA.Entities.Photos;
using Bikewale.PWA.Utils;
using Bikewale.Service.Utilities;
using Bikewale.Utility;
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
    /// Modified by :   Rajan Chauhan on 24 Feb 2018
    /// Description :   Added IArticles in constructor and removed _logger
    /// Modified By : Sanskar Gupta on 20 April 2018
    /// Description : Added dictionary logic to populated widget View All link titles (IDictionary<EditorialWidgetCategory, string> viewAllTitles).
    /// </summary>
    public class PwaCMSController : CompressionApiController//ApiController
    {
        private readonly IBikeSeries _series;
        private readonly IBikeVersionCacheRepository<BikeVersionEntity, uint> _objBikeVersionsCache = null;
        private readonly IPager _pager = null;
        private readonly ICMSCacheContent _CMSCache = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModelEntity = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeMakesCacheRepository _bikeMakesEntity;
        private readonly IBikeModelsCacheRepository<int> _objModelCache;
        private readonly IBikeInfo _bikeInfo;
        private readonly ICityCacheRepository _cityCacheRepository;
        private readonly IArticles _articles;
        IDictionary<EditorialWidgetCategory, string> viewAllTitles = EditorialWidgetHelper.EditorialViewAllTitles;


        public PwaCMSController(IPager pager, ICMSCacheContent cmsCache,
            IBikeModelsCacheRepository<int> objModelCache,
            IBikeModels<BikeModelEntity, int> bikeModelEntity,
            IUpcoming upcoming,
            IBikeMakesCacheRepository bikeMakesEntity,
            IBikeInfo bikeInfo,
            ICityCacheRepository cityCacheRepository, IArticles articles,
            IBikeVersionCacheRepository<BikeVersionEntity, uint> objBikeVersionsCache,
            IBikeSeries series)
        {
            _pager = pager;
            _CMSCache = cmsCache;
            _bikeModelEntity = bikeModelEntity;
            _upcoming = upcoming;
            _bikeMakesEntity = bikeMakesEntity;
            _objModelCache = objModelCache;
            _bikeInfo = bikeInfo;
            _cityCacheRepository = cityCacheRepository;
            _articles = articles;
            _objBikeVersionsCache = objBikeVersionsCache;
            _series = series;
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
                    }
                    return Ok(objPwaArticle);
                }

                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.CMS.CMSController");

                return InternalServerError();
            }
        }  //get News Details

        #endregion

        #region Expert-Review Details Api
        /// <summary>
        /// Created By : Pratibha Verma on 24 Feb, 2018
        /// Summary : API to get expert-review details of article.
        /// Modified By : Rajan Chauhan on 27 Feb 2018
        /// Description : Added ImageGallery to objPwaArticle
        /// Modified by : Ashutosh Sharma on 01 Mar 2018.
        /// Description : Added logic to split article html content into two parts to insert bikeinfo card at 25% height of article content.
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns>Expert-Review Details</returns>
        [ResponseType(typeof(PwaArticleDetails)), Route("api/pwa/cms/id/{basicId}/pages/")]
        public IHttpActionResult GetArticleDetailsPages(string basicId)
        {
            uint _basicId = default(uint);
            PwaArticleDetails objPwaArticle = null;
            try
            {
                if (!String.IsNullOrEmpty(basicId) && uint.TryParse(basicId, out _basicId))
                {
                    ArticlePageDetails objExpertReviews = _CMSCache.GetArticlesDetails(_basicId);
                    if (objExpertReviews != null)
                    {
                        int matchedPage = InsertBikeInfoWidgetIntoContentPwa(objExpertReviews);
                        objPwaArticle = ConverterUtility.MapArticleDetailsToPwaExpertReviewDetails(objExpertReviews, matchedPage);
                    }
                    IEnumerable<ModelImage> modelImages = _articles.GetArticlePhotos((int)_basicId);
                    if (modelImages != null && modelImages.Any())
                    {
                        objPwaArticle.ImageGallery = new PwaImageList()
                        {
                            ModelImages = ConverterUtility.PwaConvert(modelImages),
                            RecordCount = modelImages.Count()
                        };
                    }
                    return Ok(objPwaArticle);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.CMS.CMSController");
                return InternalServerError();
            }
        }  //get Expert Review Details

        #endregion
        #region BikeList based on BasicId
        /// <summary>        
        /// Created By : Prasad Gawde on 04 Apr 2017
        /// Summary : 
        /// Modified By : Pratibha Verma on 27 Feb, 2018
        /// Summary : Fetch Series Bikes and Popular Scooter Brands
        /// Modified By : Sanskar Gupta on 20 April 2018
        /// Description : Added dictionary logic to populated widget View All link titles.
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns>Two Lists of Bikes</returns>
        [ResponseType(typeof(PwaNewBikesListData)), Route("api/pwa/cms/bikelists/id/{basicId}/page/")]
        public IHttpActionResult GetBikeListsForEditCms(string basicId)
        {
            int _basicId;
            PwaNewBikesListData objPwaBikeNews = new PwaNewBikesListData();
            try
            {
                if (int.TryParse(basicId, out _basicId))
                {
                    IEnumerable<MostPopularBikesBase> bikes = null;
                    objPwaBikeNews.NewBikesList = new List<PwaBikeNews>();
                    uint cityId = 0;
                    var currentCityArea = GlobalCityArea.GetGlobalCityArea();
                    if (currentCityArea != null)
                        cityId = currentCityArea.CityId;

                    if (_basicId > 0)
                    {
                        var articleDetails = _CMSCache.GetNewsDetails((uint)_basicId);

                        if (articleDetails != null)
                        {
                            bool isScooter = false;
                            bool isSeries = false;
                            BikeMakeEntityBase makeData = GetTaggedBikeListByMake(articleDetails);
                            uint modelId = GetTaggedBikeListByModel(articleDetails.VehiclTagsList);
                            uint makeId = makeData == null ? 0 : (uint)makeData.MakeId;

                            BikeSeriesEntityBase bikeSeriesEntityBase = null;
                            bikes = FetchMostPopularBikes(cityId, modelId, makeId, out bikeSeriesEntityBase, out isScooter, out isSeries);
                            PwaBikeNews popularBikes = new PwaBikeNews();

                            string heading = string.Empty;
                            string viewAllUrl = string.Empty;
                            string viewAllBtnLabel = string.Empty;
                            string viewAllBtnText = string.Empty;
                            if (bikes != null)
                            {
                                popularBikes.BikesList = ConverterUtility.MapMostPopularBikesBaseToPwaBikeDetails(bikes);
                                if (isSeries)
                                {
                                    if (isScooter)
                                    {
                                        heading = string.Format("Popular {0} Scooters", bikeSeriesEntityBase.SeriesName);
                                        viewAllUrl = string.Format("/m/{0}-bikes/{1}/", makeData.MaskingName, bikeSeriesEntityBase.MaskingName);
                                        viewAllBtnLabel = string.Format(viewAllTitles[EditorialWidgetCategory.Series_Scooters], bikeSeriesEntityBase.SeriesName);
                                    }
                                    else
                                    {
                                        heading = string.Format("Popular {0} Bikes", bikeSeriesEntityBase.SeriesName);
                                        viewAllUrl = string.Format("/m/{0}-bikes/{1}/", makeData.MaskingName, bikeSeriesEntityBase.MaskingName);
                                        viewAllBtnLabel = string.Format(viewAllTitles[EditorialWidgetCategory.Popular_Series], bikeSeriesEntityBase.SeriesName);
                                    }
                                }
                                else if (makeId > 0)
                                {
                                    if (isScooter)
                                    {
                                        heading = string.Format("Popular {0} Scooters", makeData.MakeName);
                                        if (makeData.IsScooterOnly)
                                            viewAllUrl = string.Format("/m/{0}-bikes/", makeData.MaskingName);
                                        else
                                            viewAllUrl = string.Format("/m/{0}-scooters/", makeData.MaskingName);
                                        viewAllBtnLabel = string.Format(viewAllTitles[EditorialWidgetCategory.Popular_Make_Scooters], makeData.MakeName);
                                    }
                                    else
                                    {
                                        heading = string.Format("Popular {0} Bikes", makeData.MakeName);
                                        viewAllUrl = string.Format("/m/{0}-bikes/", makeData.MaskingName);
                                        viewAllBtnLabel = string.Format(viewAllTitles[EditorialWidgetCategory.Popular_Make], makeData.MakeName);
                                    }
                                }
                                else
                                {// When Model is not tagged
                                    heading = "Popular Bikes";
                                    viewAllUrl = "/m/best-bikes-in-india/";
                                    viewAllBtnLabel = string.Format(viewAllTitles[EditorialWidgetCategory.Popular_All]);
                                }
                                viewAllBtnText = "View all";
                                BindPopularWidgetProperties(popularBikes, heading, viewAllUrl, viewAllBtnLabel, viewAllBtnText);
                                objPwaBikeNews.NewBikesList.Add(popularBikes);
                            }

                            if (modelId > 0)
                            {
                                popularBikes = null;
                                IEnumerable<BikeMakeEntityBase> bikeList = null;
                                if (isScooter)
                                {
                                    bikeList = _bikeMakesEntity.GetScooterMakes();
                                    PwaMakeBikeBase scooterMakeList = new PwaMakeBikeBase();
                                    scooterMakeList.MakeList = ConverterUtility.MapBikeMakeEntityBaseToPwaMakeBikeEntity(bikeList);

                                    heading = string.Format("Popular {0} Brands", BodyStyleLinks.BodyStyleHeadingText(EnumBikeBodyStyles.Scooter));
                                    viewAllUrl = "/m/scooters/";
                                    viewAllBtnLabel = string.Format(viewAllTitles[EditorialWidgetCategory.OtherBrands_All]);
                                    viewAllBtnText = "View all";
                                    BindBrandsWidgetProperties(scooterMakeList, heading, viewAllUrl, viewAllBtnLabel, viewAllBtnText);

                                    objPwaBikeNews.BikeMakeList = new List<PwaMakeBikeBase>();
                                    objPwaBikeNews.BikeMakeList.Add(scooterMakeList);
                                }
                                else
                                {
                                    bikes = _objModelCache.GetMostPopularBikesByModelBodyStyle((int)modelId, 9, cityId);

                                    if (bikes != null && bikes.Any())
                                    {
                                        popularBikes = new PwaBikeNews();
                                        var bodyStyle = bikes.FirstOrDefault().BodyStyle;
                                        popularBikes.BikesList = ConverterUtility.MapMostPopularBikesBaseToPwaBikeDetails(bikes);

                                        heading = string.Format("Popular {0}", BodyStyleLinks.BodyStyleHeadingText(bodyStyle));
                                        viewAllUrl = "/m" + UrlFormatter.FormatGenericPageUrl(bodyStyle);
                                        if(bodyStyle == EnumBikeBodyStyles.Cruiser)
                                        {
                                            viewAllBtnLabel = string.Format(viewAllTitles[EditorialWidgetCategory.Popular_Cruisers]);
                                        }
                                        else if(bodyStyle == EnumBikeBodyStyles.Sports)
                                        {
                                            viewAllBtnLabel = string.Format(viewAllTitles[EditorialWidgetCategory.Popular_Sports]);
                                        }
                                        else
                                        {
                                            viewAllBtnLabel = string.Format(viewAllTitles[EditorialWidgetCategory.Popular_All]);
                                        }
                                        viewAllBtnText = "View all";
                                        BindPopularWidgetProperties(popularBikes, heading, viewAllUrl, viewAllBtnLabel, viewAllBtnText);

                                        objPwaBikeNews.NewBikesList.Add(popularBikes);
                                    }
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
                                    heading = string.Format("Upcoming {0} Bikes", makeData.MakeName);
                                    viewAllUrl = string.Format("/m/{0}-bikes/upcoming/", makeData.MaskingName);
                                    viewAllBtnLabel = string.Format(viewAllTitles[EditorialWidgetCategory.Upcoming_Make], makeData.MakeName);
                                }
                                else
                                {
                                    heading = "Upcoming Bikes";
                                    viewAllUrl = "/m/upcoming-bikes/";
                                    viewAllBtnLabel = string.Format(viewAllTitles[EditorialWidgetCategory.Upcoming_All]);
                                }
                                viewAllBtnText = "View all";
                                BindPopularWidgetProperties(upcomingBikes, heading, viewAllUrl, viewAllBtnLabel, viewAllBtnText);
                                if (upcomingBikes.BikesList != null)
                                    objPwaBikeNews.NewBikesList.Add(upcomingBikes);
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
                            BindPopularWidgetProperties(popularBikes, "Popular bikes", "/m/best-bikes-in-india/", string.Format(viewAllTitles[EditorialWidgetCategory.Popular_All]), "View all");
                            popularBikes.BikesList = ConverterUtility.MapMostPopularBikesBaseToPwaBikeDetails(bikes);
                            objPwaBikeNews.NewBikesList.Add(popularBikes);
                        }

                        UpcomingBikesListInputEntity filters = new UpcomingBikesListInputEntity();
                        filters.PageNo = 1;
                        filters.PageSize = 9;
                        var tempbikes = _upcoming.GetModels(filters, EnumUpcomingBikesFilter.Default);
                        if (tempbikes != null && tempbikes.Any())
                        {
                            PwaBikeNews upcomingBikes = new PwaBikeNews();
                            BindPopularWidgetProperties(upcomingBikes, "Upcoming bikes", "/m/upcoming-bikes/", string.Format(viewAllTitles[EditorialWidgetCategory.Upcoming_All]), "View all");
                            upcomingBikes.BikesList = ConverterUtility.MapUpcomingBikeEntityToPwaBikeDetails(tempbikes, currentCityArea.City);
                            objPwaBikeNews.NewBikesList.Add(upcomingBikes);
                        }

                    }

                    return Ok(objPwaBikeNews);
                }

                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.CMS.CMSController");

                return InternalServerError();
            }
        }  //get News Details

        /// <summary>
        /// Created by : Pratibha Verma on 28 Feb, 2018
        /// Summmary   : Method to bind PopularWidget Properties
        /// </summary>
        private void BindPopularWidgetProperties(PwaBikeNews popularBikes, string heading, string viewAllUrl, string viewAllBtnLabel, string viewAllBtnText)
        {
            popularBikes.Heading = heading;
            popularBikes.CompleteListUrl = viewAllUrl;
            popularBikes.CompleteListUrlAlternateLabel = viewAllBtnLabel;
            popularBikes.CompleteListUrlLabel = viewAllBtnText;
        }
        /// <summary>
        /// Created by : Pratibha Verma on 28 Feb, 2018
        /// Summmary   : Method to bind BrandsWidget Properties
        /// </summary>
        private void BindBrandsWidgetProperties(PwaMakeBikeBase bransList, string heading, string viewAllUrl, string viewAllBtnLabel, string viewAllBtnText)
        {
            bransList.Heading = heading;
            bransList.CompleteListUrl = viewAllUrl;
            bransList.CompleteListUrlAlternateLabel = viewAllBtnLabel;
            bransList.CompleteListUrlLabel = viewAllBtnText;
        }
        /// <summary>
        /// Created by : Pratibha Verma on 28 Feb, 2018
        /// Summmary   : Method to bind PopularBikes Widget
        /// </summary>
        private IEnumerable<MostPopularBikesBase> FetchMostPopularBikes(uint cityId, uint modelId, uint makeId, out BikeSeriesEntityBase bikeSeriesEntityBase, out bool isScooter, out bool isSeries)
        {
            try
            {
                IEnumerable<MostPopularBikesBase> bikes = null;
                bikeSeriesEntityBase = _objModelCache.GetSeriesByModelId(modelId);
                isSeries = bikeSeriesEntityBase != null && bikeSeriesEntityBase.IsSeriesPageUrl;
                IEnumerable<BikeVersionMinSpecs> objVersionsList = _objBikeVersionsCache.GetVersionMinSpecs(modelId, false);
                EnumBikeType bikeType = EnumBikeType.All;
                if (objVersionsList != null)
                {
                    BikeVersionMinSpecs firstOrDefaultVersion = objVersionsList.FirstOrDefault();
                    bikeType = (firstOrDefaultVersion != null && firstOrDefaultVersion.BodyStyle.Equals(EnumBikeBodyStyles.Scooter)) ? EnumBikeType.Scooters : EnumBikeType.All;                    
                }
                isScooter = bikeType.Equals(EnumBikeType.Scooters);
                if (isSeries)
                {
                    bikes = _objModelCache.GetMostPopularBikesByMakeWithCityPrice((int)makeId, cityId);
                    string modelIds = string.Empty;
                    if (bikeSeriesEntityBase != null && bikeSeriesEntityBase.SeriesId > 0)
                        modelIds = _series.GetModelIdsBySeries(bikeSeriesEntityBase.SeriesId);
                    string[] modelArray = modelIds.Split(',');
                    if (bikes != null && modelArray != null && modelArray.Length > 0)
                    {
                        bikes = (from bike in bikes
                                 where modelArray.Contains(bike.objModel.ModelId.ToString())
                                 select bike
                                 );
                    }
                }
                else
                {
                    bikes = _bikeModelEntity.GetMostPopularBikes(bikeType, 9, makeId, cityId);
                }

                return bikes;
            }
            catch (Exception ex)
            {
                bikeSeriesEntityBase = null;
                isScooter = false;
                isSeries = false;
                ErrorClass.LogError(ex, "Pwa.Bikewale.Models.NewsDetailPage.FetchMostPopularBikes");
            }
            return null;
        }

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
        private uint GetTaggedBikeListByModel(IEnumerable<VehicleTag> vehiclTagsList)
        {
            try
            {
                if (vehiclTagsList != null && vehiclTagsList.Any())
                {

                    BikeModelEntityBase model;
                    var taggedModelObj = vehiclTagsList.FirstOrDefault(m => !string.IsNullOrEmpty(m.ModelBase.MaskingName));
                    if (taggedModelObj != null)
                    {
                        return (uint)taggedModelObj.ModelBase.ModelId;
                    }
                    else
                    {
                        model = vehiclTagsList.First().ModelBase;
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
                        var articleDetails = _CMSCache.GetArticlesDetails((uint)_basicId);

                        if (articleDetails != null)
                        {
                            uint cityId = 0;
                            var currentCityArea = GlobalCityArea.GetGlobalCityArea();
                            if (currentCityArea != null)
                                cityId = currentCityArea.CityId;

                            var modelId = GetTaggedBikeListByModel(articleDetails.VehiclTagsList);
                            objBikeInfo = _bikeInfo.GetBikeInfo(modelId, cityId, true);

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
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.CMS.CMSController");

                return InternalServerError();
            }
        }  //get News Details

        /// <summary>
        /// Modified By : Pratibha Verma on 26 Feb, 2018
        /// Summary : Removed User Reviews to make it consistent with non PWA
        /// </summary>
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
                if (_genericBikeInfo.DealersCount > 0)
                {
                    tabs.Add(new BikeInfoTab()
                    {
                        URL = (cityDetails != null) ? Bikewale.Utility.UrlFormatter.DealerLocatorUrl(_genericBikeInfo.Make.MaskingName, cityDetails.CityMaskingName) : Bikewale.Utility.UrlFormatter.DealerLocatorUrl(_genericBikeInfo.Make.MaskingName),
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


        #region Article Gallery Images Api
        /// <summary>
        /// Created by  : Rajan Chauhan on 24 Feb 2018
        /// Description : Returns the gallery images for article
        /// Modified by : Rajan Chauhan on 26 Feb 2018
        /// Description : Removed BikeName from PwaImageList
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns></returns>
        [ResponseType(typeof(PwaImageList)), Route("api/pwa/cms/images/articles/id/{basicId}/")]
        public IHttpActionResult GetArticleGalleryImages(string basicId)
        {
            int _basicId = default(int);
            PwaImageList objGalleryImages = null;
            try
            {
                if (int.TryParse(basicId, out _basicId))
                {
                    if (_basicId > 0)
                    {
                        IEnumerable<ModelImage> modelImages = _articles.GetArticlePhotos(_basicId);
                        if (modelImages != null && modelImages.Any())
                        {
                            objGalleryImages = new PwaImageList()
                            {
                                ModelImages = ConverterUtility.PwaConvert(modelImages),
                                RecordCount = modelImages.Count()
                            };
                        }
                    }
                }
                else
                {
                    return BadRequest();
                }
                return Ok(objGalleryImages);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.CMS.PwaCMSController.GetArticleGalleryImages");
                return InternalServerError();
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
                    else if (!string.IsNullOrEmpty(bottomContentInPage))
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
                ErrorClass.LogError(ex, string.Format("Bikewale.Service.Controller.PwaCMSController.InsertBikeInfoWidgetIntoContentPwa :{0}", articleDetails));
            }
            return matchedPage;
        }
        #endregion
    }   // class
}   // namespace
