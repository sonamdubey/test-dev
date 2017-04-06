using Bikewale.Entities.Videos;

namespace Bikewale.Models.Videos
{
    /// <summary>
    /// Created by : Aditi Srivastava on 25 Mar 2017
    /// Summary    : View model for videos by subcategory
    /// </summary>
    public class VideosBySubcategoryVM
    {
        public BikeVideosListEntity VideoList { get; set; }
        public string SectionTitle { get; set; }
        public string CategoryIdList { get; set; }
        public EnumVideosCategory CategoryId { get; set; }
        public string SectionBackgroundClass { get; set; }
    }
}
