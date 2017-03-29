﻿using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Location;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 25 Mar 2017
    /// Description :   BikeInfo Widget Model
    /// </summary>
    public class BikeInfoWidget
    {
        private readonly IBikeInfo _bikeInfo;
        private readonly ICityCacheRepository _cityCacheRepo;
        private readonly BikeInfoTabType _pageId;
        private readonly uint _modelId, _cityId, _tabCount;

        /// <summary>
        /// Created by  :   Sumit Kate on 25 Mar 2017
        /// Description :   Constructor to initialize member variables
        /// </summary>
        /// <param name="bikeInfo"></param>
        /// <param name="cityCacheRepo"></param>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <param name="totalTabCount"></param>
        /// <param name="pageId"></param>
        public BikeInfoWidget(IBikeInfo bikeInfo, ICityCacheRepository cityCacheRepo, uint modelId, uint cityId, uint totalTabCount, BikeInfoTabType pageId)
        {
            _bikeInfo = bikeInfo;
            _cityCacheRepo = cityCacheRepo;
            _modelId = modelId;
            _cityId = cityId;
            _tabCount = totalTabCount;
            _pageId = pageId;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 25 Mar 2017
        /// Description :   Returns the View Model
        /// </summary>
        /// <returns></returns>
        public BikeInfoVM GetData()
        {
            BikeInfoVM objVM = null;
            try
            {
                objVM = new BikeInfoVM();
                objVM.BikeInfo = _bikeInfo.GetBikeInfo(_modelId, _cityId);
                if (_cityId > 0)
                {
                    var objCityList = _cityCacheRepo.GetAllCities(EnumBikeType.All);
                    objVM.CityDetails = objCityList.FirstOrDefault(c => c.CityId == _cityId);
                }
                objVM.BikeInfo.Tabs = BindInfoWidgetDatas(objVM.BikeInfo, objVM.CityDetails, _tabCount, _pageId);
                objVM.BikeName = string.Format("{0} {1}", objVM.BikeInfo.Make.MakeName, objVM.BikeInfo.Model.ModelName);
                objVM.BikeUrl = string.Format("{0}", Bikewale.Utility.UrlFormatter.BikePageUrl(objVM.BikeInfo.Make.MaskingName, objVM.BikeInfo.Model.MaskingName));
                objVM.IsDiscontinued = !objVM.BikeInfo.IsNew;
                objVM.IsUpcoming = objVM.BikeInfo.IsFuturistic;
            }
            catch (Exception ex)
            {
                ErrorClass err = new ErrorClass(ex, String.Format("BikeInfoWidget.GetData({0},{1},{2},{3})", _modelId, _cityId, _tabCount, _pageId));
            }
            return objVM;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 25 Mar 2017
        /// Description :   Returns Tabs for Bike Info widget
        /// </summary>
        /// <param name="_genericBikeInfo"></param>
        /// <param name="cityDetails"></param>
        /// <param name="totalTabCount"></param>
        /// <param name="pageId"></param>
        /// <returns></returns>
        private ICollection<BikeInfoTab> BindInfoWidgetDatas(GenericBikeInfo _genericBikeInfo, CityEntityBase cityDetails, uint totalTabCount, BikeInfoTabType pageId)
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
                if (tabs.Count() > 0)
                {
                    tabs = tabs.Where(m => (m.Count > 0 || m.IsVisible) && pageId != m.Tab).OrderBy(m => m.Tab).Take((int)totalTabCount).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "VideoDetailsHelper.BindInfoWidgetDatas");
            }
            return tabs;
        }
    }
}