using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CarData
{
    [Serializable]
    public class UpcomingCarModel : CarMakeModelEntityBase
    {

        [JsonProperty("expectedLaunchId")]
        public int ExpectedLaunchId { get; set; }


        [JsonProperty("expectedLaunch")]
        public string ExpectedLaunch { get; set; }


        [JsonProperty("recordCount")]
        public int RecordCount { get; set; }


        [JsonProperty("cwConfidenceText")]
        public string CWConfidenceText { get; set; }


        [JsonProperty("cwConfidenceCSS")]
        public string CWConfidenceCSS { get; set; }
        [JsonIgnore]
        public int CWConfidence { get; set; }

        [JsonProperty("updatedDate")]
        public string UpdatedDate { get; set; }

        [JsonProperty("launchDate")]
        public DateTime LaunchDate { get; set; }

        [JsonProperty("price")]
        public CarPrice Price { get; set; }

        [JsonProperty("image")]
        public CarImageBase Image { get; set; }


    }
}
