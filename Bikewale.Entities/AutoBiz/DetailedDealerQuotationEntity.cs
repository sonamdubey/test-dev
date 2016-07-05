using BikeWale.Entities.AutoBiz;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikeWale.Entities.AutoBiz
{
    /// <summary>
    /// Created by  :   Sumit Kate on 14 Mar 2016
    /// Description :   Dealer Quotation Entity
    /// </summary>
    public class DetailedDealerQuotationEntity
    {
        public MakeEntityBase objMake { get; set; }
        public ModelEntityBase objModel { get; set; }
        public VersionEntityBase objVersion { get; set; }        
        public string HostUrl { get; set; }
        public string OriginalImagePath { get; set; }

        [JsonProperty("primaryDealer")]
        public DealerQuotationEntity PrimaryDealer { get; set; }
        [JsonProperty("secondaryDealers")]
        public IEnumerable<NewBikeDealerBase> SecondaryDealers { get; set; }
        [JsonProperty("secondaryDealerCount")]
        public int SecondaryDealerCount { get { return (SecondaryDealers != null ? SecondaryDealers.Count() : 0); } }
    }
}