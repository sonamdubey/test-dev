using Bikewale.Entities.Videos;
using System.Collections.Generic;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 29 Mar 2017
    /// Description :   Similar Video Models View Model
    /// </summary>
    public class SimilarVideoModelsVM
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
