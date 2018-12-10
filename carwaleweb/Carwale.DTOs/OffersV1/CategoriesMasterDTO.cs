using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.OffersV1
{
    public class CategoriesMasterDto
    {
        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }
        [JsonProperty("hostUrl")]
        public string HostURL { get; set; }
    }
}
