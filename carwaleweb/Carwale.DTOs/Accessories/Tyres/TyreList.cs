using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carwale.DTOs.Accessories.Tyres
{
    public class TyreListDTO
    {
        [JsonProperty("count")]
        public int Count { get; set; }
        [JsonProperty("tyres")]
        public List<TyreSummaryDTO> Tyres { get; set; }
        [JsonProperty("loadAdslot")]
        public bool LoadAdslot { get; set; }
    }
}
