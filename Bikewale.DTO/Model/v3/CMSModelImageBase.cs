using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.Model.v3
{
    /// <summary>
    /// Created by  :   Sangram Nandkhile on 29 Jan 2016
    /// Description :   This new DTO for model API photos
    /// </summary
    public class CMSModelImageBase
    {
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }
    }
}
