using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.NewBikeSearch
{
    public class MakeEntity
    {
        [JsonProperty("makeId")]
        public uint MakeId { get; set; }
        [JsonProperty("makeName")]
        public string MakeName { get; set; }
        [JsonProperty("makeMaskingName")]
        public string MakeMaskingName { get; set; }
        [JsonProperty("makeStatus")]
        public BikeStatus MakeStatus { get; set; }
    }
}
