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
        public BikeMakeEntityBase objMake { get; set; }
        public BikeModelEntityBase objModel { get; set; }
        public BikeVersionEntityBase objVersion { get; set; }

        public List<string> Disclaimer { get; set; }
        public string HostUrl { get; set; }
        public string LargePicUrl { get; set; }
        public string SmallPicUrl { get; set; }
        public string OriginalImagePath { get; set; }

        [JsonProperty("primaryDealer")]
        public DealerQuotationEntity PrimaryDealer { get; set; }
        [JsonProperty("secondaryDealers")]
        public IEnumerable<NewBikeDealerBase> SecondaryDealers { get; set; }
        [JsonProperty("secondaryDealerCount")]
        public int SecondaryDealerCount { get; set; }
    }
}
