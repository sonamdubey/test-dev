using Newtonsoft.Json;

namespace Bikewale.Entities.QuestionAndAnswers.ElasticSearch
{
    /// <summary>
    /// Created by : Snehal Dange on 19th Oct 2018
    /// Desc : Author entity 
    /// </summary>
    public class Author
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
