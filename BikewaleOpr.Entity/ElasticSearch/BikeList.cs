using System.Collections.Generic;

namespace BikewaleOpr.Entity.ElasticSearch
{
    /// <summary>
    /// Created by  : Vivek Singh Tomar on 13th Dec 2017
    /// Description : Entity to hold elastic response for given make and model
    /// </summary>
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
    }
}
