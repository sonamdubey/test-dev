﻿using Newtonsoft.Json;
using System.Collections.Generic;
namespace Bikewale.DTO.NewBikeSearch
{
    public class SearchFilterDTO
    {
       
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

     

   

        [JsonProperty("price")]
        public IEnumerable<PriceRangeDTO> Price { get; set; }

        [JsonProperty("mileage")]
        public IEnumerable<RangeDTO> Mileage { get; set; }

        [JsonProperty("displacement")]
        public IEnumerable<RangeDTO> Displacement { get; set; }

        [JsonProperty("power")]
        public IEnumerable<RangeDTO> Power { get; set; }
    }
    public class RangeDTO
    {
        [JsonProperty("min")]
        public double Min { get; set; }
        [JsonProperty("max")]
        public double Max { get; set; }
    }
    public class PriceRangeDTO
    {
        [JsonProperty("min")]
      public  int Min { get; set; }
        [JsonProperty("max")]
       public int Max { get; set; }
    }
}
