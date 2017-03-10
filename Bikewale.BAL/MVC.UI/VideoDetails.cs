
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.SEO;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Videos;
using Bikewale.Models.Mobile.Videos;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
namespace Bikewale.BAL.MVC.UI
{

    /* ***********************************************************************
     * This classes needs to be moved at appropriate place after the discussion 
     * ***********************************************************************/


    /// <summary>
    /// Created by : Sangram Nandkhile on 01 Mar 2017
    /// Summary: Model for Video details Page
    /// </summary>
    public class VideoDetailsHelper
    {
        private Bikewale.Models.Mobile.Videos.VideoDetails model = null;
        private uint _videoId;
        private readonly IVideosCacheRepository _IVideos;
        public VideoDetailsHelper(uint videoId, IVideosCacheRepository IVideos)
        {
            _videoId = videoId;
            _IVideos = IVideos;
        }

        public VideoDetails GetDetails()
        {
            model = new Models.Mobile.Videos.VideoDetails();
            model.VideoEntity = BindVideoDetails(_videoId, true);
            if (model.VideoEntity != null)
            {
                if (model.VideoEntity.MakeName != null || model.VideoEntity.ModelName != null)
                    model.IsMakeModelTag = true;
                model.DisplayDate = FormatDate.GetFormatDate(model.VideoEntity.DisplayDate, "MMMM dd, yyyy");
                model.Description = FormatDescription.SanitizeHtml(model.VideoEntity.Description);
                model.PageMetas = CreateDescriptionTag(model.VideoEntity);
            }
            return model;
        }
        private BikeVideoEntity BindVideoDetails(uint videoId, bool isMakeModelTag)
        {
            BikeVideoEntity videoModel = null;
            try
            {
                videoModel = _IVideos.GetVideoDetails(videoId);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BindVideoDetails: videoId {0}", videoId));
            }
            return videoModel;
        }

        private PageMetaTags CreateDescriptionTag(BikeVideoEntity videoModel)
        {
            PageMetaTags metas = null;
            try
            {
                metas = new PageMetaTags();
                metas.CanonicalUrl = string.Format("https://www.bikewale.com/bike-videos/{0}-{1}/", videoModel.VideoTitleUrl, videoModel.BasicId);
                metas.AlternateUrl = string.Format("https://www.bikewale.com/m/bike-videos/{0}-{1}/", videoModel.VideoTitleUrl, videoModel.BasicId);

                if (model.IsMakeModelTag)
                {
                    metas.Keywords = String.Format("{0},{1}, {0} {1}", videoModel.MakeName, videoModel.ModelName);
                    string yeh = EnumVideosCategory.DoitYourself.ToString();
                    switch (videoModel.SubCatId)
                    {
                        case "55":
                            metas.Description = String.Format("{0} {1} Video Review-Watch BikeWale Expert's Take on {0} {1}-Features, performance, price, fuel economy, handling and more.", videoModel.MakeName, videoModel.ModelName);
                            metas.Title = String.Format("Expert Video Review-{0} {1} - BikeWale", videoModel.MakeName, videoModel.ModelName);
                            videoModel.SubCatName = "Expert Reviews";
                            break;
                        case "57":
                            metas.Description = String.Format("First Ride Video Review of {0} {1}-Watch BikeWale Expert's Take on the First Ride of {0} {1}-Features, performance, price, fuel economy, handling and more.", videoModel.MakeName, videoModel.ModelName);
                            metas.Title = String.Format("First Ride Video Review-{0} {1} - BikeWale", videoModel.MakeName, videoModel.ModelName);
                            break;
                        case "59":
                            metas.Description = String.Format("Launch Video of {0} {1}-{0} {1} bike launched. Watch BikeWale's Expert's take on its Launch-Features, performance, price, fuel economy, handling and more.", videoModel.MakeName, videoModel.ModelName);
                            metas.Title = String.Format("Bike Launch Video Review-{0} {1} - BikeWale", videoModel.MakeName, videoModel.ModelName);
                            break;
                        case "53":
                            metas.Description = String.Format("Do It Yourself tips for {0} {1}.  Watch Do It Yourself tips for {0} {1} from BikeWale's Experts.", videoModel.MakeName, videoModel.ModelName);
                            metas.Title = String.Format("Do It Yourself-{0} {1} - BikeWale", videoModel.MakeName, videoModel.ModelName);
                            break;
                        default:
                            metas.Description = "Check latest bike and scooter videos, watch BikeWale expert's take on latest bikes and scooters-features, performance, price, fuel economy, handling and more.";
                            metas.Title = String.Format("{0} - BikeWale", videoModel.VideoTitle);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass er = new ErrorClass(ex, string.Format("VideoDetailsHelper.CreateDescriptionTag() => videoBasicId {0}", videoModel.BasicId));
            }
            return metas;
        }
    }

    public class GenericBikeInfoHelper
    {
        GenericBikeInfoModel model = new GenericBikeInfoModel();
        private readonly IBikeInfo _bikeInfo;
        private readonly ICityCacheRepository _city;
        public GenericBikeInfoHelper(IBikeInfo bikeInfo, ICityCacheRepository city)
        {
            _bikeInfo = bikeInfo;
            _city = city;
        }

        public GenericBikeInfoModel GetDetails(uint modelId, uint cityId, uint totalTabCount, BikeInfoTabType pageId)
        {
            model.BikeInfo = _bikeInfo.GetBikeInfo(modelId, cityId);
            if (cityId > 0)
            {
                var objCityList = _city.GetAllCities(EnumBikeType.All);
                model.CityDetails = objCityList.FirstOrDefault(c => c.CityId == cityId);
            }
            model.BikeInfo.Tabs = BindInfoWidgetDatas(model.BikeInfo, model.CityDetails, totalTabCount, pageId);
            model.IsUpcoming = model.BikeInfo.IsFuturistic;
            model.IsDiscontinued = model.BikeInfo.IsUsed && !model.BikeInfo.IsNew;
            model.BikeName = string.Format("{0} {1}", model.BikeInfo.Make.MakeName, model.BikeInfo.Model.ModelName);
            model.BikeUrl = string.Format("{0}", Bikewale.Utility.UrlFormatter.BikePageUrl(model.BikeInfo.Make.MaskingName, model.BikeInfo.Model.MaskingName));
            return model;
        }

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
