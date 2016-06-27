
using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;
namespace Bikewale.Entities.Location
{

    /// Created By : Vivek Gupta 
    /// Date : 24 june 2016
    /// Desc: get dealer states
    /// </summary>

    [Serializable]
    public class DealerStateEntity : StateEntityBase
    {

        [JsonProperty("latitude"), DataMember]
        public string StateLatitude { get; set; }

        [JsonProperty("longitude"), DataMember]
        public string StateLongitude { get; set; }

        [JsonProperty("dealerCount"), DataMember]
        public uint StateCount { get; set; }

        [JsonProperty("link"), DataMember]
        public string Link { get; set; }
    }
}
