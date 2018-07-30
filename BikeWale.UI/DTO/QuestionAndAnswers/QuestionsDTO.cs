using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.QuestionAndAnswers
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 22 June 2018
    /// Description : DTO containing list of questions
    /// Modified by : Sanskar Gupta on 22 June 2018
    /// Description : Add properties `PrevPageURL` and `NextPageURL`
    /// </summary>
    public class QuestionsDTO
    {
        [JsonProperty("questions")]
        public IEnumerable<QuestionAnswerDTO> QuestionsAnswersList { get; set; }

        [JsonProperty("prevPageUrl")]
        public string PrevPageURL { get; set; }

        [JsonProperty("nextPageUrl")]
        public string NextPageURL { get; set; }
    }
}
