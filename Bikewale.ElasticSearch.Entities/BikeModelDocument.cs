using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.ElasticSearch.Entities
{
    /// <summary>
    /// Created by: Dhruv Joshi
    /// Dated: 20th Feb 2018
    /// Description: Document for a bike model
    /// </summary>
    public class BikeModelDocument: WeightedDocument
    {
        public MakeEntity BikeMake { get; set; }
        public ModelEntity BikeModel { get; set; }
        public VersionEntity TopVersion { get; set; }
        public string BikeName { get; set; }
        public uint BodyStyleId { get; set; }
        public ImageEntity BikeImage { get; set; }
        public uint ImageCount { get; set; }
        public uint VideosCount { get; set; }
        public uint ExpertReviewsCount { get; set; }
        public uint UserReviewsCount { get; set; }
        public double ReviewRatings { get; set; }
        public uint RatingsCount { get; set; }
    }
}
