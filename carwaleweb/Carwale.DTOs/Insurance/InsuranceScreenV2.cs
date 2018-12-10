using Carwale.Entity;
using Carwale.Entity.Insurance;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carwale.DTOs.Insurance
{
    public class InsuranceScreenV2
    {
        [JsonProperty("cities")]
        public List<InsuranceCityDTO> Cities { get; set; }
        [JsonProperty("makes")]
        public List<MakeEntity> Makes { get; set; }
        [JsonProperty("maskingNumber")]
        public string MaskingNumber { get; set; }
    }
}
