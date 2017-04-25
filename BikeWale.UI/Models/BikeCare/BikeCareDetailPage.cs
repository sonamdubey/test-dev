using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Models.BestBikes;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Linq;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by : Aditi Srivastava on 1 Apr 2017
    /// Summary    : Model to fetch data for bike care details page
    /// </summary>
    public class BikeCareDetailPage
    {
        #region Variables for dependency injection
        private readonly ICMSCacheContent _cmsCache = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly IBikeModelsCacheRepository<int> _models = null;
        private string _basicId;
        #endregion

        #region Page level variables
        private uint basicId;
        public StatusCodes status;
        private GlobalCityAreaEntity currentCityArea;
        private uint CityId, MakeId, ModelId, pageCatId = 0;
        private EnumBikeType bikeType = EnumBikeType.All;
        private bool showCheckOnRoadCTA = false;
        private PQSourceEnum pqSource = 0;
        #endregion

        #region Constructor
        public BikeCareDetailPage(ICMSCacheContent cmsCache, IUpcoming upcoming, IBikeModels<BikeModelEntity, int> bikeModels, IBikeModelsCacheRepository<int> models, string basicId)
        {
            _cmsCache = cmsCache;
            _upcoming = upcoming;
            _bikeModels = bikeModels;
            _models = models;
            _basicId = basicId;
            ProcessQueryString();
        }
        #endregion

        #region Functions

        /// <summary>
        /// Created by : Aditi Srivastava on 1 Apr 2017
        /// Summary    : Process query string
        /// </summary>
        private void ProcessQueryString()
        {
            if (!string.IsNullOrEmpty(_basicId) && uint.TryParse(_basicId, out basicId) && basicId > 0)
            {
                status = StatusCodes.ContentFound;
            }
            else
            {
                status = StatusCodes.ContentNotFound;
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 1 Apr 2017
        /// Summary    : Get bike care detail page data
        /// </summary>
        public BikeCareDetailPageVM GetData(int widgetTopCount)
        {
            BikeCareDetailPageVM objData = new BikeCareDetailPageVM();
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
                }
                else
                {
                    status = StatusCodes.ContentNotFound;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.BikeCareDetailPage.GetData");
            }
            return objData;
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 1 Apr 2017
        /// Summary    : Set expert reviews details page metas
        /// </summary>
        private void SetPageMetas(BikeCareDetailPageVM objData)
        {
            try
            {
                objData.PageMetaTags.CanonicalUrl = string.Format("{0}/bike-care/{1}-{2}.html", BWConfiguration.Instance.BwHostUrl, objData.ArticleDetails.ArticleUrl, objData.ArticleDetails.BasicId);
                objData.PageMetaTags.AlternateUrl = string.Format("{0}/m/bike-care/{1}-{2}.html", BWConfiguration.Instance.BwHostUrl, objData.ArticleDetails.ArticleUrl, objData.ArticleDetails.BasicId);
                objData.PageMetaTags.Title = string.Format("{0} | Maintenance Tips from Bike Experts - BikeWale", objData.ArticleDetails.Title);
                objData.PageMetaTags.Keywords = "Bike maintenance, bike common issues, bike common problems, Maintaining bikes, bike care";
                objData.PageMetaTags.Description = string.Format("Read about {0}. Read through more bike care tips to learn more about your bike maintenance.", objData.ArticleDetails.Title);
                objData.Page_H1 = string.Format("{0} | Maintenance Tips from Bike Experts - BikeWale", objData.ArticleDetails.Title);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.BikeCareDetailPage.SetPageMetas");
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 1 Apr 2017
        /// Summary    : Get tagged make in article
        /// </summary>
        private void GetTaggedBikeListByMake(BikeCareDetailPageVM objData)
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
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.BikeCareDetailPage.GetTaggedBikeListByMake");
            }
        }


        /// <summary>
        /// Created by : Aditi Srivastava on 1 Apr 2017
        /// Summary    : Get tagged model in article
        /// </summary>
        private void GetTaggedBikeListByModel(BikeCareDetailPageVM objData)
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
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.BikeCareDetailPage.GetTaggedBikeListByModel");
            }
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 1 Apr 2017
        /// Summary    : Get data for the page widgets
        /// </summary>
        private void GetWidgetData(BikeCareDetailPageVM objData, int topCount)
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
                    if (bikeType.Equals(EnumBikeType.Scooters))
                    {
                        PopularBikesWidget.WidgetHeading = string.Format("Popular {0} scooters", objData.Make.MakeName);
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
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.BikeCareDetailPage.GetWidgetData");
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 1 Apr 2017
        /// Summary    : Populate view model for photo gallery
        /// </summary>
        private void PopulatePhotoGallery(BikeCareDetailPageVM objData)
        {
            try
            {
                objData.PhotoGallery = new EditCMSPhotoGalleryVM();
                objData.PhotoGallery.Images = _cmsCache.GetArticlePhotos(Convert.ToInt32(basicId));
                if (objData.PhotoGallery.Images != null && objData.PhotoGallery.Images.Count() > 0)
                {
                    objData.PhotoGallery.ImageCount = objData.PhotoGallery.Images.Count();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.BikeCareDetailPage.PopulatePhotoGallery");
            }
        }

        #endregion
    }
}