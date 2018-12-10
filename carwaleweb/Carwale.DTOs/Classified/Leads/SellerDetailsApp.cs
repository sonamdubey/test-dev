using Carwale.DTOs.Classified.CarDetails;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carwale.DTOs.Classified.Leads
{
    public class SellerDetailsApp
    {
        public string SourceId { get; set; }
        public string IMEICode { get; set; }
        public string DealerSellerMobile { get; set; } = string.Empty;
        public string DealerSellerEmail { get; set; } = string.Empty;
        public string CustomerSellerMobile { get; set; } = string.Empty;
        public string CustomerSellerEmail { get; set; } = string.Empty;
        public string MobileVerified { get; set; }
        public string NewCVID { get; set; }
        public string RatingText { get; set; }

        [JsonProperty("responseCode")]
        public string ResponseCode { get; set; }
        [JsonProperty("responseMessage")]
        public string ResponseMessage { get; set; }
        [JsonProperty("sellerName")]
        public string SellerName { get; set; }
        [JsonProperty("sellerEmail")]
        public string SellerEmail { get; set; }
        [JsonProperty("sellerContact")]
        public string SellerContact { get; set; }
        [JsonProperty("sellerAddress")]
        public string SellerAddress { get; set; }
        [JsonProperty("sellerContactPerson")]
        public string SellerContactPerson { get; set; }
        [JsonProperty("zipDialNum")]
        public string ZipDialNum { get; set; }
        [JsonProperty("deliveryText")]
        public string DeliveryText { get; set; } = string.Empty;
        [JsonProperty("deliveryCityId")]
        public int DeliveryCity { get; set; }
        public int UsedCarNotificationId { get; set; }
        [JsonProperty("alternativeCars")]
        public List<UsedCar> AlternativeCars { get; set; } = new List<UsedCar>();
    }

    public class ReportResponse
    {
        [JsonProperty("certificationReportUrl")]
        public string CertificationReportUrl { get; set; }
        [JsonProperty("zipDialNum")]
        public string ZipDialNum { get; set; }
        [JsonProperty("responseCode")]
        public string ResponseCode { get; set; }
        [JsonProperty("responseMessage")]
        public string ResponseMessage { get; set; }
    }
}