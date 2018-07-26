using System;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace Bikewale.Entities.Schema
{
    /// <summary>
    /// Created by: Kumar Swapnil on 17-July-2018
    /// </summary>
    
    public class AcceptedAnswer
    {
        [JsonProperty("@type")]
        public string Type { get { return "ItemList"; } }

        [JsonProperty("itemListElement")]
        public IEnumerable<Answer> AnswersList { get; set; }
    }
}
