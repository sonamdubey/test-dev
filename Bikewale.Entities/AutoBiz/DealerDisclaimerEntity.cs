using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace BikeWale.Entities.AutoBiz
{
    public class DealerDisclaimerEntity
    {
        [JsonProperty("disclaimerId")]
        public uint DisclaimerId { get; set; }

        public MakeEntityBase objMake { get; set; }
        public ModelEntityBase objModel { get; set; }
        public VersionEntityBase objVersion { get; set; }

        [JsonProperty("disclaimerText")]
        public string DisclaimerText { get; set; }
    }
}
