using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bikewale.Entities.Schema
{
    /// <summary>
    /// Created by: Kumar Swapnil on 18-July-2018
    /// </summary>
    
    public class QuestionAnswerWrapper
    {
        [JsonProperty("@type")]
        public string Type { get { return "Questions"; } }

        [JsonProperty("ItemList")]
        public IEnumerable<Question> QuestionList {get;set;}

    }
}
