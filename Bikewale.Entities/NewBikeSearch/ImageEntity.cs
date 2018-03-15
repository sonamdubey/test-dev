using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.NewBikeSearch
{
    public class ImageEntity
    {
        [JsonProperty("hostURL")]
        public string HostURL { get; set; }
        [JsonProperty("imageURL")]
        public string ImageURL { get; set; }
    }
}
