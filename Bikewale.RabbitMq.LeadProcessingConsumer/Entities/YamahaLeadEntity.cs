
using Newtonsoft.Json;

namespace Bikewale.RabbitMq.LeadProcessingConsumer.Entities
{
    /// <summary>
    /// Created by  : Pratibha Verma on 18 June 2018
    /// Description : yamaha lead entity 
    /// </summary>
    public class YamahaLeadEntity
    {
        [JsonProperty("company_cd")]
        public string CompanyCode { get; set; }
        [JsonProperty("dealer_cd")]
        public string DealerCode { get; set; }
        [JsonProperty("customer_nm")]
        public string CustomerName { get; set; }
        [JsonProperty("mobile_no")]
        public string MobileNumber { get; set; }
        [JsonProperty("email_id")]
        public string EmailId { get; set; }
        [JsonProperty("pre_enquiry_status")]
        public string PreEnquiryStatus { get; set; }
        [JsonProperty("source_of_pre_enquiry")]
        public string PreEnquirySource { get; set; }
        [JsonProperty("product")]
        public string ModelName { get; set; }

    }
}
