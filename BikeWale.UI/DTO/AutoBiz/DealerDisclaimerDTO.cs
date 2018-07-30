using Newtonsoft.Json;

namespace BikeWale.DTO.AutoBiz
{
    public class DealerDisclaimerEntityDTO
    {
        [JsonProperty("disclaimerId")]
        public uint DisclaimerId { get; set; }

        public MakeEntityBaseDTO objMake { get; set; }
        public ModelEntityBaseDTO objModel { get; set; }
        public VersionEntityBaseDTO objVersion { get; set; }

        [JsonProperty("disclaimerText")]
        public string DisclaimerText { get; set; }
    }
}
