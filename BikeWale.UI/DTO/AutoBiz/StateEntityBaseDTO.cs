using Newtonsoft.Json;

namespace BikeWale.DTO.AutoBiz
{
    /// <summary>
    /// Created By : Chetan Navin on 16th Dec 2015
    /// Summary    : This class represents state entity
    /// </summary>
    public class StateEntityBaseDTO
    {
        [JsonProperty("stateId")]
        public uint StateId { get; set; }

        [JsonProperty("stateName")]
        public string StateName { get; set; }
    }
}
