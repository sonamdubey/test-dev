using Bikewale.Entities.PWA.Articles;
using Bikewale.Entities.Videos;
using Bikewale.Models.Images;
using System.Collections.Generic;
using System.Web;

namespace Bikewale.Models.Videos
{
    /// <summary>
    /// Created By Sajal Gupta on 23-03-2017
    /// Description : This View model has videos landing page.
    /// </summary>
    public class VideosLandingPageVM : ModelBase
    {
        public BikeVideoEntity LandingFirstVideoData { get; set; }
        public IEnumerable<BikeVideoEntity> LandingOtherVideosData { get; set; }
        public BrandWidgetVM Brands { get; set; }
        public VideosBySubcategoryVM ExpertReviewsWidgetData { get; set; }
        public VideosBySubcategoryVM FirstRideWidgetData { get; set; }
        public VideosBySubcategoryVM LaunchAlertWidgetData { get; set; }
        public VideosBySubcategoryVM FirstLookWidgetData { get; set; }
        public VideosBySubcategoryVM PowerDriftBlockbusterWidgetData { get; set; }
        public VideosBySubcategoryVM MotorSportsWidgetData { get; set; }
        public VideosBySubcategoryVM PowerDriftSpecialsWidgetData { get; set; }
        public VideosBySubcategoryVM PowerDriftTopMusicWidgetData { get; set; }
        public VideosBySubcategoryVM MiscellaneousWidgetData { get; set; }
        public PwaReduxStore Store { get; set; }
        public IHtmlString ServerRouterWrapper { get; set; }
        public string WindowState { get; set; }
        public ImageWidgetVM PopularSportsBikesWidget { get; set; }
    }
}
