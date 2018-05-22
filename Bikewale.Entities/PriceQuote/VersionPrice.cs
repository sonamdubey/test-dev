using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PriceQuote
{
    /// <summary>
    /// Created By :  Sushil Kumar
    /// Created On  : 17th June 2016
    /// Description : Version price for secondary dealers with version prices
    /// </summary>
    [Serializable, DataContract]
    public class VersionPriceEntity
    {
        [JsonProperty("dealerId"), DataMember]
        public uint DealerId { get; set; }

        [JsonProperty("versionId"), DataMember]
        public uint VersionId { get; set; }

        [JsonProperty("versionPrice"), DataMember]
        public uint VersionPrice { get; set; }
    }
}
