using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.Dealer
{
    public class NewBikeDealerList
    {
        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("bikeMake")]
        public string BikeMake { get; set; }

        [JsonProperty("makeMaskingName")]
        public string MakeMaskingName { get; set; }

        [JsonProperty("dealers")]
        public IEnumerable<NewBikeDealerBase> Dealers { get; set; }

        [JsonProperty("totalDealers")]
        public int TotalDealers { get; set; }
    }
}
