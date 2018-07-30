using Bikewale.DTO.Widgets.v2;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.DealerLocator
{
    /// <summary>
    /// Created By : Lucky Rathore on 21 March 2016
    /// Description : For Detail of Dealer and avaible bikes detail.
    /// </summary>
    public class DealerBikes
    {
        [JsonProperty("dealerDetails")]
        public DealerDetail DealerDetails { get; set; }

        [JsonProperty("dealerBikes")]
        public IEnumerable<MostPopularBikes> Models { get; set; }
    }
}
