using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PriceQuote.v2
{
    /// <summary>
    /// Created By : Sushil Kumar
    /// Created On : 17th June 2016
    /// Description : Dealer Basics details with version prices.
    /// Modified by :   Sumit Kate on 03 Aug 2016
    /// Description :   Added new property SelectedVersionPrice
    /// Modified By : Sangram Nandkhile on 29 Dec 2016
    /// Description : Added DisplayTextLarge, DisplayTextSmall
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

        [JsonProperty("dealerPackageType"), DataMember]
        public DealerPackageTypes DealerPackageType { get; set; }

        [JsonProperty("isPremium"), DataMember]
        public bool IsPremiumDealer { get { return (DealerPackageType == DealerPackageTypes.Premium || DealerPackageType == DealerPackageTypes.CPS); } }

        [JsonProperty("versions"), DataMember]
        public IEnumerable<VersionPriceEntity> Versions { get; set; }

        [JsonProperty("offerCount"), DataMember]
        public UInt16 OfferCount { get; set; }

        [JsonProperty("distance"), DataMember]
        public double Distance { get; set; }

        [JsonProperty("selectedVersionPrice"), DataMember]
        public uint SelectedVersionPrice { get; set; }

        [JsonProperty("displayTextLarge"), DataMember]
        public string DisplayTextLarge { get; set; }

        [JsonProperty("displayTextSmall"), DataMember]
        public string DisplayTextSmall { get; set; }

        [JsonProperty("isDSA"), DataMember]
        public bool IsDSA { get; set; }
    }
}
