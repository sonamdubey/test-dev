using System;
using Newtonsoft.Json;

namespace Bikewale.Entities.Schema
{
    /// <summary>
    /// Created by: Kumar Swapnil on 17-July-2018
    /// </summary>
    
    public class Answer
    {
        [JsonProperty("@type")]
        public string Type { get { return "Answer"; } }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("dateCreated")]
        public DateTime CreatedOn { get; set; }

        [JsonProperty("author")]
        public Author AuthorObj { get; set; }

        [JsonProperty("position")]
        public ushort Position { get; set; }
    }
}
