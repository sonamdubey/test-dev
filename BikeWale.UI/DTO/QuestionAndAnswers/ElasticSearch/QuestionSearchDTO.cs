using Bikewale.Entities.QuestionAndAnswers.ElasticSearch;
using Newtonsoft.Json;
using System.Collections.Generic;
using QnaElasticDTO = Bikewale.DTO.QuestionAndAnswers.ElasticSearch;
namespace Bikewale.DTO.QuestionAndAnswers.ElasticSearch
{
    /// <summary>
    /// Created By : Monika Korrapati on 17 Oct 2018
    /// Description : DTO to store result of QnA search
    /// </summary>
    public class QuestionSearchDTO
    {
        [JsonProperty("question")]
        public QnaElasticDTO.QuestionDTO Question { get; set; }

        [JsonProperty("pageUrl")]
        public string PageUrl { get; set; }

        [JsonProperty("answerCount")]
        public uint AnswerCount { get; set; }

        [JsonProperty("answer")]
        public QnaElasticDTO.AnswerDTO Answer { get; set; }

        [JsonProperty("answers")]
        public IEnumerable<Answer> Answers { get; set; }
    }
}