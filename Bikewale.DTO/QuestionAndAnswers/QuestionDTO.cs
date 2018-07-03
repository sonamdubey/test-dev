using Bikewale.DTO.Customer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Bikewale.DTO.QuestionAndAnswers
{
    /// <summary>
    /// Created By : Deepak Israni on 11 June 2018
    /// Description: DTO to serve as input for saving questions.
    /// </summary>
    public class QuestionDTO
    {
        [JsonProperty("questionId")]
        public Guid? Id { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("askedBy")]
        public CustomerBase AskedBy { get; set; }
        [JsonProperty("tags")]
        public IEnumerable<Tag> Tags { get; set; }
        [JsonProperty("modelId")]
        public uint ModelId { get; set; }
        [JsonProperty("platformId")]
        public ushort PlatformId { get; set; }
        [JsonProperty("sourceId")]
        public ushort SourceId { get; set; }
        [JsonProperty("askedOn")]
        public DateTime AskedOn { get; set; }
        [JsonProperty("questionAge")]
        public string QuestionAge { get; set; }
    }
}
