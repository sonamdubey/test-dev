using Bikewale.Entities.BikeData;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Videos;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models.Videos
{
    /// <summary>
    /// Created by Sajal Gupta on 31-03-2017
    /// Description : This model will bind data for videos landing page (Desktop + Mobile)
    /// </summary>
    public class VideosLandingPage
    {
        private readonly IVideosCacheRepository _videosCache = null;
        private readonly IVideos _videos = null;
        private readonly IBikeMakes<BikeMakeEntity, int> _bikeMakes = null;
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _objModelCache = null;

        public ushort LandingVideosTopCount { get; set; }
        public ushort ExpertReviewsTopCount { get; set; }
        public ushort FirstRideWidgetTopCount { get; set; }
        public ushort LaunchAlertWidgetTopCount { get; set; }
        public ushort FirstLookWidgetTopCount { get; set; }
        public ushort PowerDriftBlockbusterWidgetTopCount { get; set; }
        public ushort MotorSportsWidgetTopCount { get; set; }
        public ushort PowerDriftSpecialsWidgetTopCount { get; set; }
        public ushort PowerDriftTopMusicWidgetTopCount { get; set; }
        public ushort MiscellaneousWidgetTopCount { get; set; }
        public ushort BrandWidgetTopCount { get; set; }

        private ushort _pageNo = 1;

        public VideosLandingPage(IVideos videos, IVideosCacheRepository videosCache, IBikeMakes<BikeMakeEntity, int> bikeMakes, IBikeMaskingCacheRepository<BikeModelEntity, int> objModelCache)
        {
            _videos = videos;
            _videosCache = videosCache;
            _bikeMakes = bikeMakes;
            _objModelCache = objModelCache;
        }

        public VideosLandingPageVM GetData()
        {
            VideosLandingPageVM objVM = null;
            try
            {
                objVM = new VideosLandingPageVM();

                BindLandingVideos(objVM);

                VideosBySubcategory objSubCat = new VideosBySubcategory(_videos);

                objVM.ExpertReviewsWidgetData = objSubCat.GetData("", "55", _pageNo, ExpertReviewsTopCount);
                objVM.FirstRideWidgetData = objSubCat.GetData("", "57", _pageNo, FirstRideWidgetTopCount);
                objVM.LaunchAlertWidgetData = objSubCat.GetData("", "59", _pageNo, LaunchAlertWidgetTopCount);
                objVM.FirstLookWidgetData = objSubCat.GetData("", "61", _pageNo, FirstLookWidgetTopCount);
                objVM.PowerDriftBlockbusterWidgetData = objSubCat.GetData("", "62", _pageNo, PowerDriftBlockbusterWidgetTopCount);
                objVM.MotorSportsWidgetData = objSubCat.GetData("", "51", _pageNo, MotorSportsWidgetTopCount);
                objVM.PowerDriftSpecialsWidgetData = objSubCat.GetData("", "63", _pageNo, PowerDriftSpecialsWidgetTopCount);
                objVM.PowerDriftTopMusicWidgetData = objSubCat.GetData("", "60", _pageNo, PowerDriftTopMusicWidgetTopCount);
                objVM.MiscellaneousWidgetData = objSubCat.GetData("", "58", _pageNo, MiscellaneousWidgetTopCount);
                objVM.Brands = new BrandWidgetModel(BrandWidgetTopCount, _bikeMakes, _objModelCache).GetData(Entities.BikeData.EnumBikeType.Videos);
                BindPageMetas(objVM);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "VideosLandingPage.GetData");
            }
            return objVM;
        }

        private void BindPageMetas(VideosLandingPageVM objPageVM)
        {
            try
            {
                if (objPageVM != null && objPageVM.PageMetaTags != null)
                {
                    objPageVM.PageMetaTags.Title = "Bike Videos, Expert Video Reviews with Road Test & Bike Comparison - BikeWale";
                    objPageVM.PageMetaTags.Keywords = "bike videos, video reviews, expert video reviews, road test videos, bike comparison videos";
                    objPageVM.PageMetaTags.Description = "Check latest bike and scooter videos, watch BikeWale expert's take on latest bikes and scooters - features, performance, price, fuel economy, handling and more.";
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "VideosLandingPage.GetData()");
            }
        }

        private void BindLandingVideos(VideosLandingPageVM objVM)
        {
            try
            {
                IEnumerable<BikeVideoEntity> objLandingVideosList = _videosCache.GetVideosByCategory(EnumVideosCategory.FeaturedAndLatest, LandingVideosTopCount);

                if (objLandingVideosList != null)
                {
                    if (objLandingVideosList.Count() > 0)
                        objVM.LandingFirstVideoData = objLandingVideosList.FirstOrDefault();
                    if (objLandingVideosList.Count() > 1)
                        objVM.LandingOtherVideosData = (objLandingVideosList.Skip(1)).Take(LandingVideosTopCount - 1);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "VideosLandingPage.BindLandingVideos");
            }
        }
    }
}