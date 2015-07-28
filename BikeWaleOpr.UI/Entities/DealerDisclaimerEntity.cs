using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DealerDisclaimerEntity
/// </summary>
namespace BikeWaleOpr.Entities
{
    [Serializable]
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