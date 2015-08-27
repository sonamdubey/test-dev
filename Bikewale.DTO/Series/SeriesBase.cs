using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bikewale.DTO.Series
{
    public class SeriesBase
    {
        [JsonProperty("seriesId")]
        public int SeriesId { get; set; }

        [JsonProperty("seriesName")]
        public string SeriesName { get; set; }

        [JsonProperty("maskingName")]
        public string MaskingName { get; set; }
    }
}
