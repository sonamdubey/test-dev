using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Dealer
{
    public class DealersDTO
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("pinCode")]
        public string Pincode { get; set; }
        [JsonProperty("contactNo")]
        public string ContactNo { get; set; }
        [JsonProperty("emailId")]
        public string EMailId { get; set; }
        [JsonProperty("dealerMobile")]
        public string DealerMobileNo { get; set; }
        [JsonProperty("mobile")]
        public string Mobile { get; set; }
        [JsonProperty("area")]
        public string Area { get; set; }
    }
}
