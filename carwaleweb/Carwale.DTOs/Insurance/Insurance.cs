using Carwale.Entity;
using Carwale.Entity.Insurance;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carwale.DTOs.Insurance
{
    public class InsuranceScreen
    {
        [JsonProperty("cities")]
        public List<InsuranceCity> Cities { get; set; }
        [JsonProperty("makes")]
        public List<MakeEntity> Makes { get; set; }
        [JsonProperty("maskingNumber")]
        public string MaskingNumber { get; set; }
    }
}
