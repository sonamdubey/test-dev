
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.SEO;
using Bikewale.Entities.Videos;
using System.Collections.Generic;
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

    public class GenericBikeInfoModel
    {
        public string BikeName { get; set; }
        public string BikeUrl { get; set; }
        public GenericBikeInfo BikeInfo { get; set; }
        public CityEntityBase CityDetails { get; set; }
        public bool IsUpcoming { get; set; }
        public bool IsDiscontinued { get; set; }
        public bool IsSmallSlug { get; set; }
    }

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
    }
}
