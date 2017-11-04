using Newtonsoft.Json;

namespace BikewaleOpr.DTO.Dealers
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 02 Aug 2017
    /// Description :   Holds dealer primary information along with make id.
    /// </summary>
    public class DealerMakeDTO
    {
        [JsonProperty("dealerId")]
        public uint DealerId { get; set; }
        [JsonProperty("dealerName")]
        public string DealerName { get; set; }
        [JsonProperty("dealerMakeId")]
        public uint MakeId { get; set; }
    }
}
