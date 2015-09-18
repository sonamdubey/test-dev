using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bikewale.DTO.BikeData
{
    public class SimilarBikeList
    {
        [JsonProperty("similarBike")]
        public IEnumerable<SimilarBike> SimilarBike { get; set; }
    }
}
