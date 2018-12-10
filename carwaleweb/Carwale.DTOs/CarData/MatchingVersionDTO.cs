using Carwale.DTOs.PriceQuote;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Search.Version
{
    public class NewCarSearchDTO
    {
        [JsonProperty("matchingVersions")]
        public List<NewCarSearchVersionDTO> MatchingVersions { get; set; }

        [JsonProperty("moreVersionText")]
        public string MoreVersionText { get; set; }
    }

    public class NewCarSearchVersionDTO
    {
        [JsonProperty("versionId")]
        public int VersionId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("carRating")]
        public string CarRating { get; set; }

        [JsonProperty("features")]
        public string SpecsSummary { get; set; }

        [JsonProperty("versionOffer")]
        public object VersionOffer { get; set; } //null if no offer

        [JsonProperty("priceOverview")]
        public PriceOverviewDTO PriceOverview { get; set; }
        
    }
}
