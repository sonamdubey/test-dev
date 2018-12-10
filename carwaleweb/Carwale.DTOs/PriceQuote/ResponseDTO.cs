using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carwale.DTOs.PriceQuote
{
    public class ResponseDTO
    {
        [JsonProperty("cityId")]
        public int CityId { get; set; }

        [JsonProperty("result")]
        public bool Result { get; set; }

        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }
    }
}
