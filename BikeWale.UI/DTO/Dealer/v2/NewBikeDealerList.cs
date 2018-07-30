using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.DTO.Dealer.v2
{
    /// <summary>
    /// Created by  :   Sumit Kate on 20 May 2016
    /// Description :   Dealer list DTO version 2
    /// </summary>
    public class NewBikeDealerList
    {
        [JsonProperty("dealers")]
        public IEnumerable<NewBikeDealerBase> Dealers { get; set; }

        [JsonProperty("totalDealers")]
        public int TotalDealers { get { return Dealers != null ? Dealers.Count() : 0; } }
    }
}
