using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Bikewale.DTO.PriceQuote.DealerPriceQuote
{
    /// <summary>
    /// Dealer Price Quote Bike Varient DTO
    /// Author  :   Sumit Kate
    /// Created :   08 Oct 2015
    /// Description :   DTO
    /// </summary>
    public class DPQ_BikeVarient
    {
        [JsonProperty("make")]
        public DPQMakeBase objMake { get; set; }
        [JsonProperty("model")]
        public DPQModelBase objModel { get; set; }
        [JsonProperty("version")]
        public DPQVersionBase objVersion { get; set; }
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        [JsonProperty("originalImagePath")]
        public string OriginalImagePath { get; set; }
        [JsonProperty("onRoadPrice")]
        public UInt32 OnRoadPrice { get; set; }
        [JsonProperty("bookingAmount")]
        public UInt32 BookingAmount { get; set; }
        [JsonProperty("priceList")]
        public IList<DPQ_Price> PriceList { get; set; }
    }
}
