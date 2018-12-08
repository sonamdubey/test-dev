using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.QuestionAndAnswers.ElasticSearch
{
    public class QuestionSearchWrapper
    {
        [JsonProperty("questions")]
        public IEnumerable<QuestionSearch> Questions { get; set; }

        [JsonProperty("bikeinfo")]
        public BikewaleInfo Bikeinfo { get; set; }
    }
}
