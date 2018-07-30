using Newtonsoft.Json;

namespace Bikewale.DTO.Finance
{
    /// <summary>
    /// Created by  :   Sumit Kate on 18 Sep 2017
    /// Description :   Capital First Lead Response DTO
    /// </summary>
    public class CapitalFirstLeadResponseDTO
    {
        [JsonProperty("cpId")]
        public uint CpId { get; set; }
        [JsonProperty("ctLeadId")]
        public uint CTleadId { get; set; }
        [JsonProperty("leadId")]
        public uint LeadId { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("status")]
        public ushort Status { get; set; }
    }
}
