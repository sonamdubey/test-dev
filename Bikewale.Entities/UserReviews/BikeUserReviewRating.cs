
using System;
namespace Bikewale.Entities.UserReviews
{
    /// <summary>
    /// Created By :-Subodh Jain 17 Jan 2017
    /// Summary :- Bike user review rating entity
    /// </summary>
    [Serializable]
    public class BikeUserReviewRating
    {
        public string OriginalImagePath { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string HostUrl { get; set; }
        public string ModelMaskingName { get; set; }
        public string MakeMaksingName { get; set; }
        public double ReviewCounting { get; set; }
        public double OverAllRating { get; set; }
    }
}
