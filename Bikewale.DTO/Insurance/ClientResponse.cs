using Newtonsoft.Json;

namespace Bikewale.DTO.Insurance
{
    public class ClientResponse
    {
        [JsonProperty("quotation")]
        public string Quotation { get; set; }

        [JsonProperty("confirmationStatus")]
        public string ConfirmationStatus { get; set; }

        [JsonProperty("uniqueId")]
        public int UniqueId { get; set; }
    }
}
