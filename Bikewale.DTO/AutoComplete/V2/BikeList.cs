using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.AutoComplete.V2
{
    /// <summary>
    /// Created By : Sajal Gupta
    /// Created On : 01/08/2016
    /// </summary>
    public class BikeList
    {
        [JsonProperty("suggestionListV2")]
        public List<SuggestionList> Bikes { get; set; }
    }
}
