using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Carwale.Entity.Dealers
{
    [Serializable]
    public class NewCarDealerCountByMake
    {
        [JsonProperty("makeName")]
        public string MakeName { get; set; }

        [JsonProperty("makeCount")]
        public int Count { get; set; }

        [JsonProperty("makeId")]
        public int MakeId { get; set; }
    }
}
