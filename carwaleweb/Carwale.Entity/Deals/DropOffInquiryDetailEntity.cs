using Carwale.Entity.Classified.CarDetails;
using Carwale.Entity.Dealers;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Offers;
using Carwale.Entity.PaymentGateway;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Deals
{
    public class DropOffInquiryDetailEntity
    {
        [JsonProperty("customerDetail")]
        public TransactionDetails CustomerDetail { get; set; }

        [JsonProperty("dealerDetail")]
        public DealerSummary DealerDetail { get; set; }

        [JsonProperty("carDetail")]
        public CarEntity CarDetail { get; set; }

        [JsonProperty("stockDetail")]
        public BasicCarInfo StockDetail { get; set; }

        [JsonProperty("offer")]
        public OffersEntity Offer { get; set; }

        [JsonProperty("custLocation")]
        public CustLocation CustLocation { get; set; }

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
