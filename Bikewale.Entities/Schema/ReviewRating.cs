using Newtonsoft.Json;

namespace Bikewale.Entities.Schema
{
    public class ReviewRating
    {
        [JsonProperty("@type")]
        public string Type { get { return "Rating"; } }
        public uint BestRating { get; set; }
        public double RatingValue { get; set; }
        public uint WorstRating { get; set; }
    }
}
