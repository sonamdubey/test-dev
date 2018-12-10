using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Dealers
{
    [Serializable]
    public class DealerStateEntity : DealerCityEntity
    {
        [JsonProperty("stateName")]
        public string StateName { get; set; }

        [JsonProperty("stateId")]
        public int StateId { get; set; }

        public List<DealerCityEntity> cities = new List<DealerCityEntity>();
    }
}
