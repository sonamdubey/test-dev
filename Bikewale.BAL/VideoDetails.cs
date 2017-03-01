
using Bikewale.Entities.SEO;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.Videos;
using Bikewale.Models.Mobile.Videos;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
namespace Bikewale.BAL
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 01 Mar 2017
    /// Summary: Model for Video details Page
    /// </summary>
    public class VideoDetailsHelper
    {
        Bikewale.Models.Mobile.Videos.VideoDetails model = null;
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
                ErrorClass er = new ErrorClass(ex, string.Format(""));
            }
            return metas;
        }
    }
}
