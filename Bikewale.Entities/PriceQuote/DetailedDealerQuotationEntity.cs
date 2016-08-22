using Bikewale.Entities.BikeData;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.Entities.PriceQuote
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created On : 15 march 2016
    /// Description : To wrap Dealer Quotation page data.
    /// </summary>
    public class DetailedDealerQuotationEntity
    {
        [JsonProperty("objMake")]
        public BikeMakeEntityBase objMake { get; set; }

        [JsonProperty("objModel")]
        public BikeModelEntityBase objModel { get; set; }

        [JsonProperty("objVersion")]
        public BikeVersionEntityBase objVersion { get; set; }

        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }

        [JsonProperty("originalImagePath")]
        public string OriginalImagePath { get; set; }

        [JsonProperty("primaryDealer")]
        public DealerQuotationEntity PrimaryDealer { get; set; }

        [JsonProperty("secondaryDealers")]
        public IEnumerable<NewBikeDealerBase> SecondaryDealers { get; set; }

        [JsonProperty("secondaryDealersV2")]
        public IEnumerable<Bikewale.Entities.PriceQuote.v2.NewBikeDealerBase> SecondaryDealersV2 { get; set; }

        [JsonProperty("secondaryDealerCount")]
        public int SecondaryDealerCount { get; set; }
    }
}