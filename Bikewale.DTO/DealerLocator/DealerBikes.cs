using Bikewale.DTO.BikeData;
using Bikewale.DTO.Widgets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.DealerLocator
{
    /// <summary>
    /// Created By : Lucky Rathore on 21 March 2016
    /// Description : For Detail of Dealer and avaible bikes detail.
    /// </summary>
    public class DealerBikes
    {
        [JsonProperty("dealerDetail")]
        public DealerDetail DealerDetail { get; set; }

        [JsonProperty("models")]
        public IEnumerable<MostPopularBikes> Models { get; set; }
    }
}
