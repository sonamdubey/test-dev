using Bikewale.Entities.BikeData;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
namespace Bikewale.Entities.PriceQuote.v2
{
    /// <summary>
    /// Created By : Sushil Kumar
    /// Created On : 17th June 2016
    /// Description : To wrap Dealer Quotation page data with version prices.
    /// </summary>
    [System.Serializable, DataContract]
    public class DetailedDealerQuotationEntity
    {
        [JsonProperty("objMake"), DataMember]
        public BikeMakeEntityBase objMake { get; set; }

        [JsonProperty("objModel"), DataMember]
        public BikeModelEntityBase objModel { get; set; }

        [JsonProperty("objVersion"), DataMember]
        public BikeVersionEntityBase objVersion { get; set; }

        [JsonProperty("hostUrl"), DataMember]
        public string HostUrl { get; set; }

        [JsonProperty("originalImagePath"), DataMember]
        public string OriginalImagePath { get; set; }

        [JsonProperty("primaryDealer"), DataMember]
        public DealerQuotationEntity PrimaryDealer { get; set; }

        [JsonProperty("secondaryDealers"), DataMember]
        public IEnumerable<Bikewale.Entities.PriceQuote.v2.NewBikeDealerBase> SecondaryDealers { get; set; }

        [JsonProperty("secondaryDealerCount"), DataMember]
        public int SecondaryDealerCount { get { return (this.SecondaryDealers != null ? this.SecondaryDealers.Count() : 0); } }
    }
}
