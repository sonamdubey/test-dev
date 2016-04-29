using Newtonsoft.Json;

namespace Bikewale.Entities.BikeBooking
{
    public class PQOutputEntity
    {
        [JsonProperty("pqId")]
        public ulong PQId { get; set; }
        [JsonProperty("dealerId")]
        public uint DealerId { get; set; }
        [JsonProperty("versionId")]
        public uint VersionId { get; set; }
        [JsonProperty("isDealerAvailable")]
        public bool IsDealerAvailable { get; set; }
    }
}
