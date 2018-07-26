using System;
using Newtonsoft.Json;


namespace Bikewale.Entities.Schema
{
    /// <summary>
    /// Created by: Kumar Swapnil on 17-July-2018
    /// </summary>
    
    public class Question
    {
        [JsonProperty("@type")]
        public string Type { get { return "Question"; } }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("dateCreated")]
        public DateTime DateCreated { get; set; }

        [JsonProperty("author")]
        public Author AuthorObj { get; set; }

        [JsonProperty("answerCount")]
        public uint AnswersCount { get; set; }

        [JsonProperty("acceptedAnswer")]
        public AcceptedAnswer AcceptedAnswerObj { get; set; }

        [JsonProperty("position")]
        public ushort Position { get; set; }
    }
}
