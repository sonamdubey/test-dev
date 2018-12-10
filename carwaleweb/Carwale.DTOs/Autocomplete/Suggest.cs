using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs
{

    
    public class BaseAutoCompleteDTO 
    {
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
    }

    public abstract class PayLoad : BaseAutoCompleteDTO { }
    
    public class Suggest
    {
        [JsonProperty("result")]
        public string Result { get; set; }
        [JsonProperty("payload")]
        public PayLoad Payload { get; set; }
    }

    public class AmpSuggest:Suggest
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class CityResultsDTO : PayLoad
    {
        [JsonProperty("cityId")]
        public int Id { get; set; }

        [JsonProperty("cityName")]
        public string Name { get; set; }
		[JsonProperty("cityMaskingName")]
		public string CityMaskingName { get; set; }
		[JsonProperty("isAreaAvailable")]
        public bool IsAreaAvailable { get; set; }

        [JsonProperty("isDuplicate")]
        public bool IsDuplicate { get; set; }
    }

    public class AreaResultsDTO : PayLoad
    {
        [JsonProperty("areaName")]
        public string Name { get; set; }

        [JsonProperty("areaId")]
        public int Id { get; set; }

        [JsonProperty("pinCode")]
        public string PinCode { get; set; }

        [JsonProperty("zoneId")]
        public int ZoneId { get; set; }

        [JsonProperty("zoneName")]
        public string ZoneName { get; set; }

        [JsonProperty("cityId")]
        public int CityId { get; set; }

        [JsonProperty("cityName")]
        public string CityName { get; set; }

        [JsonProperty("cityMaskingName")]
        public string CityMaskingName { get; set; }
    }

    public class CarResultsDTO : PayLoad
    {
        [JsonProperty("makeId")]
        public string MakeId { get; set; }
        [JsonProperty("makeName")]
        public string MakeName { get; set; }
        [JsonProperty("modelId")]
        public string ModelId { get; set; }
        [JsonProperty("modelName")]
        public string ModelName { get; set; }
    }
}
