using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.AutoComplete.V2
{
    /// <summary>
    /// Created By : Sajal Gupta
    /// Created On : 01/08/2016
    /// Description : Dto for saving list of bike details.
    /// </summary>
    public class BikeList
    {
        [JsonProperty("bikes")]
        public List<SuggestionList> Bikes { get; set; }
    }
}
