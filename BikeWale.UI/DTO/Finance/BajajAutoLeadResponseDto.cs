using Newtonsoft.Json;

namespace Bikewale.DTO.Finance.BajajAuto
{
    public class BajajAutoLeadResponseDto
    {
        [JsonProperty("bajaAutoId")]
        public uint BajaAutoId { get; set; }
        [JsonProperty("status")]
        public ushort Status { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("leadId")]
        public uint LeadId{ get; set; }
    }
}
