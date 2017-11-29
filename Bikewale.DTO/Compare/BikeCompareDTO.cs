using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.Compare
{
    /// <summary>
    /// Created By  :   Sumit Kate on 22 Jan 2016
    /// Description :   Bike Compare DTO
    /// </summary>    
    public class BikeCompareDTO
    {
        [JsonProperty("basicInfo")]
        public IEnumerable<BikeDTOBase> BasicInfo { get; set; }
        [JsonProperty("specifications")]
        public IEnumerable<BikeSpecificationDTO> Specifications { get; set; }
        [JsonProperty("features")]
        public IEnumerable<BikeFeatureDTO> Features { get; set; }
        [JsonProperty("colors")]
        public List<BikeColorDTO> Color { get; set; }
        [JsonProperty("compareSpecifications")]
        public CompareMainCategoryDTO CompareSpecifications { get; set; }
        [JsonProperty("compareFeatures")]
        public CompareMainCategoryDTO CompareFeatures { get; set; }
        [JsonProperty("compareColors")]
        public CompareBikeColorCategoryDTO CompareColors { get; set; }
    }
}
