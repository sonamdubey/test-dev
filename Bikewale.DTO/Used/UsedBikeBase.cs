using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.Used.Search
{
    /// <summary>
    /// Created By  : Sushil Kumar on 14th Sep 2016
    /// Description : Used bike search result page base 
    /// </summary>
    public class UsedBikeBase
    {
        [JsonProperty("inquiryId")]
        public uint InquiryId { get; set; }
        [JsonProperty("profileId")]
        public string ProfileId { set; get; }
        [JsonProperty("askingPrice")]
        public uint AskingPrice { get; set; }
        [JsonProperty("kmsDriven")]
        public uint KmsDriven { get; set; }
        [JsonProperty("modelYear")]
        public string ModelYear { get; set; }
        [JsonProperty("modelMonth")]
        public string ModelMonth { get; set; }
        [JsonProperty("noOfOwners")]
        public ushort NoOfOwners { get; set; }
        [JsonProperty("sellerType")]
        public ushort SellerType { get; set; }

        [JsonProperty("photo")]
        public BikePhoto Photo { get; set; }
        [JsonProperty("totalPhotos")]
        public ushort TotalPhotos { get; set; }

        [JsonProperty("city")]
        public string CityName { get; set; }
        [JsonProperty("cityMasking")]
        public string CityMaskingName { get; set; }

        [JsonProperty("make")]
        public string MakeName { get; set; }
        [JsonProperty("model")]
        public string ModelName { get; set; }
        [JsonProperty("makeMasking")]
        public string MakeMaskingName { get; set; }
        [JsonProperty("modelMasking")]
        public string ModelMaskingName { get; set; }
        [JsonProperty("version")]
        public string VersionName { get; set; }
        [JsonProperty("lastUpdated")]
        public DateTime LastUpdated { get; set; }
        [JsonProperty("strLastUpdated")]
        public string StrLastUpdated { get { return LastUpdated.ToString("dd MMMM yyyy"); } }
        [JsonProperty("bikeName")]
        public string BikeName { get { return string.Format("{0} {1} {2}", MakeName, ModelName, VersionName); } }
    }
}
