using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Bikewale.Entities.BikeData;

namespace BikeWale.Entities.AutoBiz
{
    public class DealerDisclaimerEntity
    {
        [JsonProperty("disclaimerId")]
        public uint DisclaimerId { get; set; }

        public BikeMakeEntityBase objMake { get; set; }
        public BikeModelEntityBase objModel { get; set; }
        public BikeVersionEntityBase objVersion { get; set; }

        [JsonProperty("disclaimerText")]
        public string DisclaimerText { get; set; }
    }
}
