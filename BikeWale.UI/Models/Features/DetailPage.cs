
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Memcache;
using Bikewale.Models.BestBikes;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Linq;
namespace Bikewale.Models.Features
{
    public class DetailPage
    {
        private readonly ICMSCacheContent _cache = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly IBikeModelsCacheRepository<int> _models = null;


        public uint BasicId;
        public StatusCodes status;

        public uint TopCount { get; set; }
        public DetailPage(uint basicId, ICMSCacheContent cache, IUpcoming upcoming, IBikeModels<BikeModelEntity, int> bikeModels, IBikeModelsCacheRepository<int> models)
        {
            _cache = cache;
            BasicId = basicId;
            _upcoming = upcoming;
            _bikeModels = bikeModels;
            _models = models;
            ProcessQuery(basicId);

        }
        /// <summary>
        /// Created by : SubodhJain  on 30 March 2017
        /// Summary    : Get all feature details
        /// </summary>
        public DetailFeatureVM GetData()
        {
            DetailFeatureVM objDetailsVM = new DetailFeatureVM();
            try
            {

                objDetailsVM.objFeature = _cache.GetArticlesDetails(BasicId);
                if (objDetailsVM.objFeature != null)
                {
                    status = Entities.StatusCodes.ContentFound;
                    GetTaggedBikeListByMake(objDetailsVM);
                    GetTaggedBikeListByModel(objDetailsVM);
                    GetWidgetData(objDetailsVM);
                    PopulatePhotoGallery(objDetailsVM);

                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Bikewale.Models.Features.GetData()");
            }
            return objDetailsVM;
        }
        private void GetWidgetData(DetailFeatureVM objData)
        {
            try
            {
                GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
                uint CityId = 0;
                if (currentCityArea != null)
                    CityId = currentCityArea.CityId;
                MostPopularBikesWidget objPopularBikes = new MostPopularBikesWidget(_bikeModels, EnumBikeType.All, false, false, 0, 0, (uint)objData.objMake.MakeId);
                objPopularBikes.TopCount = (int)TopCount;
                objPopularBikes.CityId = CityId;
                objData.MostPopularBikes = objPopularBikes.GetData();
                if (objData.objMake != null && objData.objMake.MakeId > 0)
                {
                    objData.MostPopularBikes.WidgetHeading = string.Format("Popular {0} bikes", objData.objMake.MakeName);
                    objData.MostPopularBikes.WidgetHref = string.Format("/{0}-bikes/", objData.objMake.MaskingName);
                    objData.MostPopularBikes.WidgetLinkTitle = string.Format("{0} Bikes", objData.objMake.MakeName);
                }
                else
                {
                    objData.MostPopularBikes.WidgetHeading = "Popular bikes";
                    objData.MostPopularBikes.WidgetHref = "/best-bikes-in-india/";
                    objData.MostPopularBikes.WidgetLinkTitle = "Best Bikes in India";
                }

                if (objData.objModel != null && objData.objModel.ModelId > 0)
                {
                    PopularBikesByBodyStyle objPopularStyle = new PopularBikesByBodyStyle(_models);
                    objPopularStyle.ModelId = (uint)objData.objModel.ModelId;
                    objPopularStyle.CityId = CityId;
                    objPopularStyle.TopCount = (int)TopCount;
                    objData.PopularBodyStyle = objPopularStyle.GetData();
                    if (objData.PopularBodyStyle != null)
                    {
                        objData.PopularBodyStyle.WidgetHeading = string.Format("Popular {0}", objData.PopularBodyStyle.BodyStyleText);
                        objData.PopularBodyStyle.WidgetLinkTitle = string.Format("Best {0} in India", objData.PopularBodyStyle.BodyStyleLinkTitle);
                        objData.PopularBodyStyle.WidgetHref = UrlFormatter.FormatGenericPageUrl(objData.PopularBodyStyle.BodyStyle);
                    }
                }
                else
                {
                    UpcomingBikesWidget objUpcomingBikes = new UpcomingBikesWidget(_upcoming);
                    objUpcomingBikes.Filters = new UpcomingBikesListInputEntity();
                    objUpcomingBikes.Filters.StartIndex = 1;
                    objUpcomingBikes.Filters.EndIndex = (int)TopCount;
                    if (objData.objMake.MakeId > 0)
                    {
                        objUpcomingBikes.Filters.MakeId = objData.objMake.MakeId;
                    }
                    objUpcomingBikes.SortBy = EnumUpcomingBikesFilter.Default;
                    objData.UpcomingBikes = objUpcomingBikes.GetData();

                    if (objData.objMake != null)
                    {
                        objData.UpcomingBikes.WidgetHeading = string.Format("Upcoming {0} bikes", objData.objMake.MakeName);
                        objData.UpcomingBikes.WidgetHref = string.Format("/{0}-bikes/upcoming/", objData.objMake.MaskingName);
                    }
                    else
                    {
                        objData.UpcomingBikes.WidgetHeading = "Upcoming bikes";
                        objData.UpcomingBikes.WidgetHref = "/upcoming-bikes/";
                    }
                    objData.UpcomingBikes.WidgetLinkTitle = "Upcoming Bikes in India";
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.ExpertReviewsDetailPage.GetWidgetData");
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
                objPage.PageMetaTags.Title = string.Format("{0} - Bikewale ", objPage.objFeature.Title);
                objPage.PageMetaTags.Description = string.Format("Read about {0}. Read through more bike care tips to learn more about your bike maintenance.", objPage.objFeature.Title);
                objPage.PageMetaTags.Keywords = string.Format("features, stories, travelogues, specials, drives.");

            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "DealerShowroomIndiaPage.BindPageMetas()");
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
                ErrorClass objErr = new ErrorClass(err, "Bikewale.Models.Features.GetTaggedBikeListByMake");

            }
        }
        private void PopulatePhotoGallery(DetailFeatureVM objData)
        {
            try
            {
                objData.PhotoGallery = new EditCMSPhotoGalleryVM();
                objData.PhotoGallery.Images = _cache.GetArticlePhotos(Convert.ToInt32(BasicId));
                if (objData.PhotoGallery.Images != null && objData.PhotoGallery.Images.Count() > 0)
                {
                    objData.PhotoGallery.ImageCount = objData.PhotoGallery.Images.Count();
                }
                if (objData.objMake != null && objData.objModel != null)
                    objData.PhotoGallery.BikeName = string.Format("{0} {1}", objData.objMake.MakeName, objData.objModel.ModelName);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.ExpertReviewsDetailPage.PopulatePhotoGallery");
            }
        }

        /// <summary>
        /// Created by : SubodhJain  on 30 March 2017
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
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.Features.GetTaggedBikeListByModel");
            }
        }
        private void ProcessQuery(uint basicId)
        {
            try
            {
                if (basicId > 0)
                {
                    string _basicId = BasicIdMapping.GetCWBasicId(Convert.ToString(basicId));

                    if (!_basicId.Equals(Convert.ToString(basicId)))
                    {

                        if (Convert.ToUInt32(_basicId) > 0)
                        {


                            status = Entities.StatusCodes.RedirectPermanent;
                        }

                    }
                }
                else
                {
                    status = Entities.StatusCodes.ContentNotFound;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.Features.ProcessQueryString()");
            }

        }

    }
}