using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.BikeData
{
    /// <summary>
    /// Created by  :   Sumit Kate on 13 Sep 2016
    /// Description :   Make and models DTO
    /// </summary>
    public class MakeModelBase
    {
        [JsonProperty("make")]
        public MakeBase Make { get; set; }
        [JsonProperty("models")]
        public IEnumerable<ModelBase> Models { get; set; }
    }
}
