using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.CMS.Photos
{
    public class CMSImageList
    {
        [JsonProperty("recordCount")]
        public uint RecordCount { get; set; }

        [JsonProperty("ImagesList")]
        public List<CMSModelImageBase> Images { get; set; }
    }
}
