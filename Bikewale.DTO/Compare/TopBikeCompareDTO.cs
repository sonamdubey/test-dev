using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.Compare
{
    /// <summary>
    /// Created by  :   Sumit Kate on 22 Jan 2016
    /// Description :   Top Bike Compare DTO
    /// Modified by : Sumit Kate on 29 Jan 2016
    /// Description :   Added HostUrl1, HostUrl2, VersionImgUrl1, VersionImgUrl2 as string to Bikewale.Entities.Compare.TopBikeCompareBase entity.
    /// Modified By : Sushil Kumar on 27th Oct 2016
    /// Description : Removed unused methods for reviews and topcompare image
    /// </summary>
    public class TopBikeCompareDTO
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonProperty("versionId1")]
        public UInt16 VersionId1 { get; set; }
        [JsonProperty("versionId2")]
        public UInt16 VersionId2 { get; set; }
        [JsonProperty("modelId1")]
        public UInt16 ModelId1 { get; set; }
        [JsonProperty("modelId2")]
        public UInt16 ModelId2 { get; set; }
        [JsonProperty("bike1")]
        public string Bike1 { get; set; }
        [JsonProperty("bike2")]
        public string Bike2 { get; set; }
        [JsonProperty("makeMaskingName1")]
        public string MakeMaskingName1 { get; set; }
        [JsonProperty("makeMaskingName2")]
        public string MakeMaskingName2 { get; set; }
        [JsonProperty("modelMaskingName1")]
        public string ModelMaskingName1 { get; set; }
        [JsonProperty("modelMaskingName2")]
        public string ModelMaskingName2 { get; set; }
        [JsonProperty("price1")]
        public UInt32 Price1 { get; set; }
        [JsonProperty("price2")]
        public UInt32 Price2 { get; set; }
        [JsonProperty("hostUrl1")]
        public string HostUrl1 { get; set; }
        [JsonProperty("hostUrl2")]
        public string HostUrl2 { get; set; }
        [JsonProperty("versionImgUrl1")]
        public string VersionImgUrl1 { get; set; }
        [JsonProperty("versionImgUrl2")]
        public string VersionImgUrl2 { get; set; }
    }
}
