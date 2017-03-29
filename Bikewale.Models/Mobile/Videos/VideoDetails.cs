
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Videos;
using System.Collections.Generic;
namespace Bikewale.Models.Mobile.Videos
{

    public class GenericBikeInfoCard
    {
        public uint ModelId { get; set; }
        public uint CityId { get; set; }
        public BikeInfoTabType PageId { get; set; }
        public bool IsSmallSlug { get; set; }
    }

    public class SimilarModelsModel
    {
        public uint ModelId { get; set; }
        public string ModelName { get; set; }
        public string BikeName { get; set; }
        public IEnumerable<BikeVideoEntity> Videos { get; set; }
        public string ViewAllLinkText { get; set; }
        public string ViewAllLinkUrl { get; set; }

        public string ViewAllLinkTitle { get; set; }
    }
}
