using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.CMS.Photos
{
    public class CMSImageList
    {
        [JsonProperty("recordCount")]
        public uint RecordCount { get; set; }

        [JsonProperty("ImagesList")]
        public List<CMSModelImageBase> ModelImage { get; set; }
    }
}
