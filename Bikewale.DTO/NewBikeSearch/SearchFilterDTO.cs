
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
namespace Bikewale.DTO.NewBikeSearch
{
    public class SearchFilterDTO
    {
        [JsonProperty("priceRange")]
        public IEnumerable<Tuple<int, int>> PriceRange { get; set; }
        [JsonProperty("mileage")]
        public IEnumerable<Tuple<double, double>> Mileage { get; set; }
        [JsonProperty("displacement")]
        public IEnumerable<Tuple<double, double>> Displacement { get; set; }
        [JsonProperty("power")]
        public IEnumerable<Tuple<double, double>> Power { get; set; }
        [JsonProperty("bodyStyle")]
        public ushort BodyStyle { get; set; }


        [JsonProperty("makeId")]
        public uint MakeId { get; set; }
        [JsonProperty("abs")]
        public bool ABS { get; set; }
        [JsonProperty("discBrake")]
        public bool DiscBrake { get; set; }
        [JsonProperty("drumBrake")]
        public bool DrumBrake { get; set; }
        [JsonProperty("alloyWheel")]
        public bool AlloyWheel { get; set; }
        [JsonProperty("spokeWheel")]
        public bool SpokeWheel { get; set; }
        [JsonProperty("electric")]
        public bool Electric { get; set; }
        [JsonProperty("manual")]
        public bool Manual { get; set; }
        [JsonProperty("pageNumber")]
        public int PageNumber { get; set; }
        [JsonProperty("pageSize")]
        public int PageSize { get; set; }
        [JsonProperty("excludeMake")]
        public bool ExcludeMake { get; set; }
        [JsonProperty("cityId")]
        public uint CityId { get; set; }
        [JsonProperty("modelStatus")]
        public byte ModelStatus { get; set; }
    }
}
