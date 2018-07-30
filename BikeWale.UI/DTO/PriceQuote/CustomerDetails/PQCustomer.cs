using Newtonsoft.Json;

namespace Bikewale.DTO.PriceQuote.CustomerDetails
{
    /// <summary>
    /// Price Quote Customer
    /// Author  :   Sumit Kate
    /// Date    :   21 Aug 2015
    /// </summary>
    public class PQCustomer
    {
        [JsonProperty("customer")]
        public PQCustomerBase objCustomerBase { get; set; }
        [JsonProperty("bikeColor")]
        public PQColor objColor { get; set; }
        [JsonProperty("isTransactionCompleted")]
        public bool IsTransactionCompleted { get; set; }
        [JsonProperty("selectedVersionId")]
        public uint SelectedVersionId { get; set; }
    }
}
