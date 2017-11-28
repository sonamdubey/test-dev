using Newtonsoft.Json;

namespace Bikewale.DTO.State
{
    public class StateBase
    {
        [JsonProperty("stateId")]
        public uint StateId { get; set; }

        [JsonProperty("stateName")]
        public string StateName { get; set; }

        [JsonProperty("stateMaskingName")]
        public string StateMaskingName { get; set; }
    }
}
