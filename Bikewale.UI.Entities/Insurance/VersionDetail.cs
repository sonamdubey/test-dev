using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.UI.Entities.Insurance
{
    /// <summary>
    /// Created BY : Lucky Rathore on 18 November 2015.
    /// Description : For Bike's model's List.
    /// </summary>
    public class VersionDetail
    {
        [JsonProperty("versionId")]
        public int VersionId { get; set; }

        [JsonProperty("versionName")]
        public string VersionName { get; set; }

        [JsonProperty("exShowroomPrice")]
        public int ExShowroomPrice { get; set; }
    }
}
