using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BikewaleOpr.DTO.QuestionsAnswers
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 14 June 2018
    /// Description : DTO for UpdateTags API
    /// </summary>
    public class UpdateQuestionTagsDTO
    {
        [JsonProperty("moderatorId")]
        public uint ModeratorId { get; set; }

        [JsonProperty("oldTags")]
        public List<uint> OldTags { get; set; }

        [JsonProperty("newTags")]
        public List<string> NewTags { get; set; }

        [JsonProperty("modelId")]
        public uint ModelId { get; set; }

    }
}
