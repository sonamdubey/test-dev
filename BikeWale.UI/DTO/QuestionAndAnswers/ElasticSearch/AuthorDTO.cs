using Newtonsoft.Json;

namespace Bikewale.DTO.QuestionAndAnswers.ElasticSearch
{
    public class AuthorDTO
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}