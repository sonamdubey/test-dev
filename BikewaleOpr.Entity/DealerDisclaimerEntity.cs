using Newtonsoft.Json;


namespace BikewaleOpr.Entities
{
    public class DealerDisclaimerEntity
    {
        [JsonProperty("disclaimerId")]
        public uint DisclaimerId { get; set; }

        public BikeMakeEntityBase objMake { get; set; }
        public BikeModelEntityBase objModel { get; set; }
        public BikeVersionEntityBase objVersion { get; set; }

        [JsonProperty("disclaimerText")]
        public string DisclaimerText { get; set; }
    }
}
