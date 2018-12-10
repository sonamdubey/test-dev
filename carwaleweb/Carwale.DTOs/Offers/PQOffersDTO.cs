using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Offers
{
    public class PQOffersDTO
    {
        [JsonProperty("offerId")]
        public int OfferId { get; set; }
        [JsonProperty("dealerId")]
        public int DealerId { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("availabilityCount")]
        public int AvailabilityCount { get; set; }
        [JsonProperty("image")]
        public string Image { get; set; }
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        [JsonProperty("originalImage")]
        public string OriginalImg { get; set; }
        [JsonProperty("termsAndCondition")]
        public string TermsAndCondition { get; set; }
        [JsonProperty("expiryDate")]
        public string ExpiryDate { get; set; }
        [JsonProperty("offerType")]
        public int OfferType { get; set; }
        [JsonProperty("dispOnDesk")]
        public bool DispOnDesk { get; set; }
        [JsonProperty("dispOnMobile")]
        public bool DispOnMobile { get; set; }
        [JsonProperty("dealerName")]
        public string DealerName { get; set; }
        [JsonProperty("sourceCategory")]
        public int SourceCategory { get; set; }
        [JsonProperty("dispSnippetOnDesk")]
        public bool DispSnippetOnDesk { get; set; }
        [JsonProperty("dispSnippetOnMob")]
        public bool DispSnippetOnMob { get; set; }
        [JsonProperty("shortDescription")]
        public string ShortDescription { get; set; }
    }
}
