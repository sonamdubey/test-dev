using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.BAL.BikeData;
using Bikewale.BAL.Pager;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Pager;
using Microsoft.Practices.Unity;
using System;
using System.Collections.ObjectModel;
using System.Linq;
namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Created By : Sushil Kumar on 2nd Jan 2016
    /// Generic Bike Functions
	/// Modified By : Rajan Chauhan on 16 Apr 2018
	/// Description : Registered API Gateway in UnityContainer 
    /// </summary>
    public class BindBikeInfo
    {
        public uint ModelId { get; set; }
        public bool IsUpcoming { get; set; }
        public bool IsDiscontinued { get; set; }
        public uint CityId { get; set; }
        public BikeInfoTabType PageId { get; set; }
        private readonly IBikeInfo _objGenericBike = null;
        public uint TabCount { get; set; }
        public CityEntityBase cityDetails { get; set; }
        private GenericBikeInfo _genericBikeInfo;
        public float Rating { get; set; }
        public UInt16 RatingCount { get; set; }
        public UInt16 UserReviewCount { get; set; }
        public BindBikeInfo()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {

                    container.RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                        .RegisterType<ICacheManager, MemcacheManager>()
                        .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                        .RegisterType<IPager, Pager>()
                        .RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>()
						.RegisterType<IApiGatewayCaller, ApiGatewayCaller>()
						.RegisterType<IBikeInfo, BikeInfo>();


                    _objGenericBike = container.Resolve<IBikeInfo>();
                }

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "BindBikeInfo.BindBikeInfo");
            }

        }

        /// <summary>
        /// Created By : Sushil Kumar on 2nd Jan 2016
        /// Summary :  To get generic bike info by modelid
        /// Modified By : Aditi Srivastava on 23 Jan 2017
        /// Summary     : Added properties for checking upcoming and discontinued bikemodels
        ///Modified By :- Subodh Jain 30 jan 2017
        ///Summary:- Shifted generic to bikemodel repository
        /// </summary>
        public GenericBikeInfo GetBikeInfo()
        {
            try
            {

                _genericBikeInfo = _objGenericBike.GetBikeInfo(ModelId, CityId);
                BindInfoWidgetDatas();


                if (_genericBikeInfo != null)
                {
                    _genericBikeInfo.Tabs = _genericBikeInfo.Tabs.Where(m => (m.Count > 0 || m.IsVisible) && PageId != m.Tab).OrderBy(m => m.Tab).Take((int)TabCount).ToList();
                    IsUpcoming = _genericBikeInfo.IsFuturistic;
                    IsDiscontinued = _genericBikeInfo.IsUsed && !_genericBikeInfo.IsNew;
                }

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "BindBikeInfo.GetBikeInfo");
            }
            return _genericBikeInfo;
        }


        /// <summary>
        /// Created By :- subodh Jain 10 Feb 2017
        /// Summary :- BikeInfo Slug details
        /// </summary>
        private void BindInfoWidgetDatas()
        {
            if (_genericBikeInfo != null)
            {
                try
                {
                    Rating = _genericBikeInfo.Rating;
                    RatingCount = _genericBikeInfo.RatingCount;
                    UserReviewCount = _genericBikeInfo.UserReviewCount;

                    _genericBikeInfo.Tabs = new Collection<BikeInfoTab>();
                    if (_genericBikeInfo.ExpertReviewsCount > 0)
                    {
                        _genericBikeInfo.Tabs.Add(new BikeInfoTab()
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
                        _genericBikeInfo.Tabs.Add(new BikeInfoTab()
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
                        _genericBikeInfo.Tabs.Add(new BikeInfoTab()
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
                        _genericBikeInfo.Tabs.Add(new BikeInfoTab()
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
                        _genericBikeInfo.Tabs.Add(new BikeInfoTab()
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
                        _genericBikeInfo.Tabs.Add(new BikeInfoTab()
                        {

                            URL = (cityDetails != null) ? Bikewale.Utility.UrlFormatter.DealerLocatorUrl(_genericBikeInfo.Make.MaskingName, cityDetails.CityMaskingName) : Bikewale.Utility.UrlFormatter.DealerLocatorUrl(_genericBikeInfo.Make.MaskingName),
                            Title = string.Format("Dealers in {0}", cityDetails != null ? cityDetails.CityName : "India"),
                            TabText = "Dealers",
                            IconText = "dealers",
                            Count = _genericBikeInfo.DealersCount,
                            Tab = BikeInfoTabType.Dealers
                        });
                    }
                }
                catch (Exception ex)
                {
                    Bikewale.Notifications.ErrorClass.LogError(ex, "BindGenericBikeInfo.BindInfoWidgetDatas");


                }
            }
        }

    }
}