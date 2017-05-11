using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Location;
using Bikewale.Memcache;
using Bikewale.Models.BestBikes;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by : Aditi Srivastava on 30 Mar 2017
    /// Summary    : Model to populate view model for expert review detail page
    /// </summary>
    public class ExpertReviewsDetailPage
    {
        #region Variables for dependency injection
        private readonly ICMSCacheContent _cmsCache = null;
        private IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private IBikeModelsCacheRepository<int> _models = null;
        private readonly IBikeInfo _bikeInfo;
        private readonly ICityCacheRepository _cityCacheRepo;
        private IUpcoming _upcoming = null;
        private string _basicId;
        #endregion

        #region Page level variables
        public StatusCodes status;
        public string mappedCWId;
        public string redirectUrl;
        private GlobalCityAreaEntity currentCityArea;
        private uint CityId, MakeId, ModelId, pageCatId = 0;
        private uint _totalTabCount = 3;
        private BikeInfoTabType _pageId = BikeInfoTabType.News;
        private EnumBikeType bikeType = EnumBikeType.All;
        private bool showCheckOnRoadCTA = false;
        private uint basicId;
        private PQSourceEnum pqSource = 0;
        #endregion

        #region Public properties
        public bool IsMobile { get; set; }
        #endregion

        #region Constructor
        public ExpertReviewsDetailPage(ICMSCacheContent cmsCache, IBikeModelsCacheRepository<int> models, IBikeModels<BikeModelEntity, int> bikeModels, IUpcoming upcoming, IBikeInfo bikeInfo, ICityCacheRepository cityCacheRepo, string basicId)
        {
            _cmsCache = cmsCache;
            _models = models;
            _bikeModels = bikeModels;
            _upcoming = upcoming;
            _bikeInfo = bikeInfo;
            _cityCacheRepo = cityCacheRepo;
            _basicId = basicId;
            ProcessQueryString();
        }
        #endregion

        #region Functions
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
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.ExpertReviewsDetailPage.ProcessQueryString");
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 31 Mar 2017
        /// Summary    : Get entire data for expert reviews detail page
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
                    GetWidgetData(objData, widgetTopCount);
                    PopulatePhotoGallery(objData);
                    SetBikeTested(objData);
                }
                else
                    status = StatusCodes.ContentNotFound;
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Bikewale.Models.ExpertReviewsDetailPage.GetData");
            }
            return objData;
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 31 Mar 2017
        /// Summary    : Set expert reviews details page metas
        /// </summary>
        private void SetPageMetas(ExpertReviewsDetailPageVM objData)
        {
            try
            {
                objData.PageMetaTags.CanonicalUrl = string.Format("{0}/expert-reviews/{1}-{2}.html", BWConfiguration.Instance.BwHostUrl, objData.ArticleDetails.ArticleUrl, objData.ArticleDetails.BasicId);
                objData.PageMetaTags.AmpUrl = string.Format("{0}/m/expert-reviews/{1}-{2}/amp/", BWConfiguration.Instance.BwHostUrl, objData.ArticleDetails.ArticleUrl, objData.ArticleDetails.BasicId);
                objData.PageMetaTags.AlternateUrl = string.Format("{0}/m/expert-reviews/{0}-{1}.html", BWConfiguration.Instance.BwHostUrl, objData.ArticleDetails.ArticleUrl, objData.ArticleDetails.BasicId);
                objData.PageMetaTags.Title = string.Format("{0}- BikeWale.", objData.ArticleDetails.Title);
                objData.PageMetaTags.Keywords = string.Format("{0},road test, road tests, roadtests, roadtest, bike reviews, expert bike reviews, detailed bike reviews, test-drives, comprehensive bike tests, bike preview, first drives", objData.Model.ModelName);
                if (IsMobile)
                    objData.PageMetaTags.Description = string.Format("BikeWale tests {0}, Read the complete road test report to know how it performed.", objData.Model.ModelName);
                else
                    objData.PageMetaTags.Description = "Learn about the trending stories related to bike and bike products. Know more about features, do's and dont's of different bike products exclusively on BikeWale";
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.ExpertReviewsDetailPage.SetPageMetas");
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 31 Mar 2017
        /// Summary    : Get tagged make in article
        /// </summary>
        private void GetTaggedBikeListByMake(ExpertReviewsDetailPageVM objData)
        {
            try
            {
                if (objData.ArticleDetails.VehiclTagsList != null && objData.ArticleDetails.VehiclTagsList.Count() > 0)
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
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.ExpertReviewsDetailPage.GetTaggedBikeListByMake");
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
                if (objData.ArticleDetails.VehiclTagsList != null && objData.ArticleDetails.VehiclTagsList.Count() > 0)
                {

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
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.ExpertReviewsDetailPage.GetTaggedBikeListByModel");
            }
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 31 Mar 2017
        /// Summary    : Get data for the page widgets
        ///  Modified By Sajal Gupta on 25-04-20187
        /// Descrition : Call most popular bike widget by body type
        /// </summary>
        private void GetWidgetData(ExpertReviewsDetailPageVM objData, int topCount)
        {
            try
            {
                currentCityArea = GlobalCityArea.GetGlobalCityArea();
                if (currentCityArea != null)
                    CityId = currentCityArea.CityId;

                if (ModelId > 0)
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
                        bikeType = objData.PopularBodyStyle.BodyStyle == EnumBikeBodyStyles.Scooter ? EnumBikeType.Scooters : EnumBikeType.All;
                    }

                    BikeInfoWidget objBikeInfo = new BikeInfoWidget(_bikeInfo, _cityCacheRepo, ModelId, CityId, _totalTabCount, _pageId);
                    objData.BikeInfo = objBikeInfo.GetData();
                    objData.BikeInfo.IsSmallSlug = true;
                }
                else
                {
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
                        objData.UpcomingBikes.WidgetHeading = string.Format("Upcoming {0} bikes", objData.Make.MakeName);
                        objData.UpcomingBikes.WidgetHref = string.Format("/{0}-bikes/upcoming/", objData.Make.MaskingName);
                    }
                    else
                    {
                        objData.UpcomingBikes.WidgetHeading = "Upcoming bikes";
                        objData.UpcomingBikes.WidgetHref = "/upcoming-bikes/";
                    }
                    objData.UpcomingBikes.WidgetLinkTitle = "Upcoming Bikes in India";
                }

                MostPopularBikesWidget objPopularBikes = new MostPopularBikesWidget(_bikeModels, bikeType, showCheckOnRoadCTA, false, pqSource, pageCatId, MakeId);
                objPopularBikes.TopCount = topCount;
                objPopularBikes.CityId = CityId;
                objData.MostPopularBikes = objPopularBikes.GetData();

                MostPopularBikeWidgetVM PopularBikesWidget = objData.MostPopularBikes;

                if (MakeId > 0 && objData.Make != null)
                {
                    if (bikeType == EnumBikeType.Scooters)
                    {
                        PopularBikesWidget.WidgetHeading = string.Format("Popular {0} scooters", objData.Make.MakeName);
                        if (objData.Make.IsScooterOnly)
                            PopularBikesWidget.WidgetHref = string.Format("/{0}-bikes/", objData.Make.MaskingName);
                        else
                            PopularBikesWidget.WidgetHref = string.Format("/{0}-scooters/", objData.Make.MaskingName);
                        PopularBikesWidget.WidgetLinkTitle = string.Format("{0} Scooters", objData.Make.MakeName);
                        PopularBikesWidget.CtaText = "View all scooters";
                    }
                    else
                    {
                        PopularBikesWidget.WidgetHeading = string.Format("Popular {0} bikes", objData.Make.MakeName);
                        PopularBikesWidget.WidgetHref = string.Format("/{0}-bikes/", objData.Make.MaskingName);
                        PopularBikesWidget.WidgetLinkTitle = string.Format("{0} Bikes", objData.Make.MakeName);
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
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.ExpertReviewsDetailPage.GetWidgetData");
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
                if (objData.PhotoGallery.Images != null && objData.PhotoGallery.Images.Count() > 0)
                {
                    objData.PhotoGallery.ImageCount = objData.PhotoGallery.Images.Count();
                }
                if (objData.Make != null && objData.Model != null)
                    objData.PhotoGallery.BikeName = string.Format("{0} {1}", objData.Make.MakeName, objData.Model.ModelName);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.ExpertReviewsDetailPage.PopulatePhotoGallery");
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
                        VehicleTag item = objData.ArticleDetails.VehiclTagsList.Where(e => e.ModelBase.ModelId == i).First();
                        if (!String.IsNullOrEmpty(item.MakeBase.MaskingName))
                        {
                            objData.BikeTested.Append(string.Format("<a title={0} {1} Bikes href=/m/{2}-bikes/{3}/>{4}</a>", item.MakeBase.MakeName, item.ModelBase.ModelName, item.MakeBase.MaskingName, item.ModelBase.MaskingName, item.ModelBase.ModelName));
                            if (iTemp < ids.Count()) { objData.BikeTested.Append(", "); }
                            iTemp++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.ExpertReviewsDetailPage.SetBikeTested");
            }
        }
        #endregion
    }
}