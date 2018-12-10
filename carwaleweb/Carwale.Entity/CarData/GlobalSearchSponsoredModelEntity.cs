using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CarData
{
    [Serializable]
    public class GlobalSearchSponsoredModelEntity : CarMakeModelAdEntityBase
    {
        [DataMember, JsonProperty("startDate")]
        public DateTime StartDate { get; set; }
        [DataMember, JsonProperty("endDate")]
        public DateTime EndDate { get; set; }
        [DataMember, JsonProperty("adPosition")]
        public int AdPosition { get; set; }
        [DataMember, JsonProperty("priority")]
        public int Priority { get; set; }
    }
}
