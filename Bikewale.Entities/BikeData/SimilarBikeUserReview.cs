
using Bikewale.Entities.BikeData;
using System;
namespace Bikewale.Entities
{
    /// <summary>
    /// created by Sajal Gupta on 08-05-2017
    /// descriptopn : Class to store similar bike user review widget 
    /// Modified by : Aditi Srivastava on 7 June 2017
    /// Summary     : Added NumberOfReviews
    /// </summary>
    [Serializable]
    public class SimilarBikeUserReview
    {
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public string HostUrl { get; set; }
        public string OriginalImagePath { get; set; }
        public double OverAllRating { get; set; }
        public uint NumberOfRating { get; set; }
        public uint NumberOfReviews { get; set; }
    }
}
