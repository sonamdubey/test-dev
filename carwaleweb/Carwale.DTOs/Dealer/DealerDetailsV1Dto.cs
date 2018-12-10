﻿using Newtonsoft.Json;

namespace Carwale.DTOs.Dealer
{
    public class DealerDetailsV1Dto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("pincode")]
        public string PinCode { get; set; }

        [JsonProperty("contactNo")]
        public string ContactNo { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
