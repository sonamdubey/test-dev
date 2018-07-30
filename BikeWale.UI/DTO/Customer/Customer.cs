using Bikewale.DTO.Area;
using Bikewale.DTO.City;
using Bikewale.DTO.State;
using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.Customer
{
    public class Customer : CustomerBase
    {
        [JsonProperty("password")]
        public string Password { get; set; }
        
        [JsonProperty("passwordSalt")]
        public string PasswordSalt { get; set; }
        
        [JsonProperty("passwordHash")]
        public string PasswordHash { get; set; }
        
        [JsonProperty("isVerified")]
        public bool IsVerified { get; set; }

        [JsonProperty("clientIP")]
        public string ClientIP { get; set; }

        [JsonProperty("sourceId")]
        public UInt16 SourceId { get; set; }

        [JsonProperty("isExist")]
        public bool IsExist { get; set; }

        [JsonProperty("cityBase")]
        public CityBase cityBase { get; set; }

        [JsonProperty("stateBase")]
        public StateBase stateBase { get; set; }

        [JsonProperty("areaBase")]
        public AreaBase areaBase { get; set; }
    }
}
