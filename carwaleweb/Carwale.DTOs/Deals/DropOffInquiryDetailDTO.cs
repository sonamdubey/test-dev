using Carwale.DTOs.CarData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Deals
{
    public class DropOffInquiryDetailDTO
    {
        [JsonProperty("customerDetail")]
        public CustomerBaseDTO CustomerDetail { get; set; }

        [JsonProperty("carDetail")]
        public CarDetailBase CarDetail { get; set; }

        [JsonProperty("custLocation")]
        public CustLocationDTO CustLocation { get; set; }

        [JsonProperty("dealerDetail")]
        public DealerSummaryDTO DealerDetail { get; set; }

        [JsonProperty("manufacturingMonth")]
        public string ManufacturingMonth { get; set; }

        [JsonProperty("createdOn")]
        public DateTime CreatedOn { get; set; }

        [JsonProperty("lastCallTime")]
        public DateTime LastCallTime { get; set; }

        [JsonProperty("followUpTime")]
        public DateTime FollowUpTime { get; set; }

        [JsonProperty("comments")]
        public string Comments { get; set; }

        [JsonProperty("offerPrice")]
        public int OfferPrice { get; set; }

        [JsonProperty("actualPrice")]
        public int ActualPrice { get; set; }

        [JsonProperty("interiorColor")]
        public string InteriorColor { get; set; }

        [JsonProperty("color")]
        public string Color { get;set; }

        [JsonProperty("tCStockId")]
        public int TCStockId { get; set; }

        [JsonProperty("shortDescription")]
        public string ShortDescription { get; set; }

        [JsonProperty("referenceId")]
        public int ReferenceId { get; set; }

        [JsonProperty("disposition")]
        public string Disposition { get; set; }

        [JsonProperty("dispositionid")]
        public int DispositionId { get; set; }

        [JsonProperty("pushStatus")]
        public int PushStatus { get; set; }
    }
}
