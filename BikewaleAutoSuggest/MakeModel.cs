using System.Collections.Generic;

namespace BikewaleAutoSuggest
{   
    /// <summary>
    /// Modified by : Rajan Chauhan on 10 Jan 2017
    /// Description : Added PhotosCount to Payload and TempList
    /// </summary>
    /// <returns></returns>
    public class BikeList
    {
        public string Id { get; set; }
        public BikeSuggestion mm_suggest { get; set; }
        public string name { get; set; }
        public string output { get; set; }

        public PayLoad payload { get; set; }
    }

    public class BikeSuggestion
    {
        public IList<string> input { get; set; }
        public int Weight { get; set; }
        public Context contexts { get; set; }
    }
    public class Context
    {
        public IList<string> types { get; set; }                         //  Context For new pricequote and user review
    }
    public class PayLoad
    {
        public string MakeId { get; set; }
        public string ModelId { get; set; }
        public string MakeMaskingName { get; set; }
        public string ModelMaskingName { get; set; }
        public string Futuristic { get; set; }
        public string IsNew { get; set; }
        public string UserRatingsCount { get; set; }
        public string ExpertReviewsCount { get; set; }
        public string PhotosCount { get; set; }
    }

    public class TempList
    {
        public int MakeId { get; set; }
        public int ModelId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string MakeMaskingName { get; set; }
        public string ModelMaskingName { get; set; }
        public bool New { get; set; }
        public bool Futuristic { get; set; }

        public int UserRatingsCount { get; set; }
        public uint ExpertReviewsCount { get; set; }
        public uint PhotosCount { get; set; }
  }
}
