using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Customers
{
    public class GoogleUserInfo
    {
        public string CustomerId { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("given_name")]
        public string First_name { get; set; }
        [JsonProperty("gender")]
        public string Gender { get; set; }
        [JsonProperty("family_name")]
        public string Last_name { get; set; }
        [JsonProperty("link")]
        public string Link { get; set; }
        [JsonProperty("picture")]
        public string Picture { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("hd")]
        public string Hd { get; set; }
        [JsonProperty("verified_email")]
        public bool VerifiedEmail { get; set; }
    }
}
