using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bikewale.DTO.AutoComplete
{
    public class BikeList
    {
        [JsonProperty("suggestionList")]
        public List<SuggestionList> Bikes { get; set; }
    }
}
