using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.AutoComplete
{
    public class BikeList
    {
        [JsonProperty("suggestionList")]
        public IEnumerable<SuggestionList> Bikes { get; set; }
    }
}
