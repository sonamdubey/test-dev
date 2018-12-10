using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Deals
{
    public class AdvantageSearchResultsDTO
    {
        [JsonProperty("deals")]
        public List<DealsSummaryDesktop_DTO> Deals { get; set; }

        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }

        [JsonProperty("filterCount")]
        public FilterCountDTO FilterCount { get; set; }

        [JsonProperty("visibleCarsCount")]
        public string VisibleCarsCount { get; set; }

        [JsonProperty("offerOfTheWeek")]
        public DealsStockDTO OfferOfTheWeek { get; set; }

        [JsonProperty("nextPageURL")]
        public string nextPageURL { get; set; }
    }
}
