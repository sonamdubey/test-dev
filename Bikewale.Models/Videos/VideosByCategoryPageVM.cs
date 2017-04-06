using Bikewale.Entities.Videos;

namespace Bikewale.Models.Videos
{
    public class VideosByCategoryPageVM : ModelBase
    {
        public BikeVideosListEntity Videos { get; set; }
        public string CanonicalTitle { get; set; }
        public string TitleName { get; set; }
        public string PageHeading { get; set; }
        public string CategoryId { get; set; }
    }
}
