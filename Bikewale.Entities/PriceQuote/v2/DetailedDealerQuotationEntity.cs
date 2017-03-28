﻿using Bikewale.Entities.BikeData;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.Entities.PriceQuote.v2
{
    /// <summary>
    /// Created By : Sushil Kumar
    /// Created On : 17th June 2016
    /// Description : To wrap Dealer Quotation page data with version prices.
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
        public IEnumerable<Bikewale.Entities.PriceQuote.v2.NewBikeDealerBase> SecondaryDealers { get; set; }

        [JsonProperty("secondaryDealerCount")]
        public int SecondaryDealerCount { get { return (this.SecondaryDealers != null ? this.SecondaryDealers.Count() : 0); } }
    }
}
