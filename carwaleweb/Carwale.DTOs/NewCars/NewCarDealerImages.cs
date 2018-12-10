using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.NewCars
{
    [Serializable]
    public class NewCarDealerImages
    {
        [JsonProperty("imgThumbUrl")]
        public string ImgThumbUrl { get; set; }

        [JsonProperty("imgLargeUrl")]
        public string ImgLargeUrl { get; set; }
    }
}
