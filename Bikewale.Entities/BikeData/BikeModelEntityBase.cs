using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{
    [Serializable, DataContract]
    public class BikeModelEntityBase
    {
        [DataMember, JsonProperty("ModelId")]
        public int ModelId { get; set; }
        [DataMember, JsonProperty("ModelName")]
        public string ModelName { get; set; }
        [DataMember, JsonProperty("MaskingName")]
        public string MaskingName { get; set; }
    }
}
