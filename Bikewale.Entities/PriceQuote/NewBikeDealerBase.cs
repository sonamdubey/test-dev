using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PriceQuote
{
    /// <summary>
    /// Modified By : Lucky Rathore
    /// Modified on : 15 March 2016
    /// Description : for Dealer Basics details.
    /// Modified By : Lucky Rathore
    /// Modified on : 21 March 2016
    /// Description : DealerPkgType added.
    /// Modified by :   Sumit Kate on 01 Aug 2016
    /// Description :   Added Offer Count and Distance
    /// </summary>
    [Serializable, DataContract]
    public class NewBikeDealerBase
    {
        [JsonProperty("id"), DataMember]
        public UInt32 DealerId { get; set; }

        [JsonProperty("name"), DataMember]
        public string Name { get; set; }

        [JsonProperty("area"), DataMember]
        public string Area { get; set; }

        [JsonProperty("maskingNumber"), DataMember]
        public string MaskingNumber { get; set; }

        [JsonProperty("dealerPackageType")]
        public DealerPackageTypes DealerPackageType { get; set; }

        [JsonProperty("versions")]
        public IEnumerable<VersionPriceEntity> Versions { get; set; }
    }
}
