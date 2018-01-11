using Bikewale.Entities.SEO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Modified By : Lucky Rathore
    /// Summary : HostUrl and LogoUrl Added
    /// Modified by :   Sumit Kate on 03 Mar 2016
    /// Description :   Added PopularityIndex
    /// Modified by sajal Gupta on 14-11-2017
    /// Description : Added MakeCategoryId
    /// Modified by : Rajan Chauhan on 11 Jan 2018
    /// Description : Added PhotosCount
    /// </summary>
    [Serializable, DataContract]
    public class BikeMakeEntityBase
    {
        [JsonProperty(PropertyName = "makeId"), DataMember]
        public int MakeId { get; set; }

        [JsonProperty(PropertyName = "makeName"), DataMember]
        public string MakeName { get; set; }

        [JsonProperty(PropertyName = "maskingName"), DataMember]
        public string MaskingName { get; set; }

        [JsonProperty(PropertyName = "hostUrl"), DataMember]
        public string HostUrl { get; set; }

        [JsonProperty(PropertyName = "logoUrl"), DataMember]
        public string LogoUrl { get; set; }

        [JsonProperty("popularityIndex"), DataMember]
        public ushort PopularityIndex { get; set; }

        [JsonProperty("isScooterOnly"), DataMember]
        public bool IsScooterOnly { get; set; }

        [JsonProperty("totalCount"), DataMember]
        public uint TotalCount { get; set; }

        [JsonProperty("photosCount"), DataMember]
        public uint PhotosCount { get; set; }

        [JsonProperty("href"), DataMember]
        public string Href { get; set; }

        [JsonProperty("title"), DataMember]
        public string Title { get; set; }

        [JsonProperty("text"), DataMember]
        public string Text { get; set; }

        [JsonProperty("metas"), DataMember]
        public IEnumerable<CustomPageMetas> Metas { get; set; }

        [JsonProperty(PropertyName = "makeCategoryId"), DataMember]
        public ushort MakeCategoryId { get; set; }
    }
}
