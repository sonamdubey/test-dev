
using Bikewale.Entities.SEO;
using Bikewale.Entities.Videos;
namespace Bikewale.Models.Mobile.Videos
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 01 Mar 2017
    /// Summary: Model for Video details Page
    /// </summary>
    public class VideoDetails
    {
        public PageMetaTags PageMetas { get; set; }
        public BikeVideoEntity VideoEntity { get; set; }
        public string Description { get; set; }
        public string DisplayDate { get; set; }
        public bool IsMakeModelTag { get; set; }
    }
}
