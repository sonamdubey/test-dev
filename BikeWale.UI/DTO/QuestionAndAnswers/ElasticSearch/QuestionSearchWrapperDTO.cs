using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.DTO.QuestionAndAnswers.ElasticSearch
{
    public class QuestionSearchWrapperDTO
    {
        [JsonProperty("questions")]
        public IEnumerable<QuestionSearchDTO> Questions { get; set; }

        [JsonProperty("bikeinfo")]
        public BikewaleInfoDTO Bikeinfo { get; set; }
    }
}