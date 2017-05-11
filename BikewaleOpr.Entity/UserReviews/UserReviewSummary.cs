using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BikewaleOpr.Entities.UserReviews
{
        [Serializable, DataContract]
    public class UserReviewSummary
    {
        [DataMember]
        public UserReviewOverallRating OverallRating { get; set; }
        [DataMember]
        public BikeMakeEntityBase Make { get; set; }
        [DataMember]
        public BikeModelEntityBase Model { get; set; }
        [DataMember]
        public string OriginalImgPath { get; set; }
        [DataMember]
        public string HostUrl { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Tips { get; set; }
        [DataMember]
        public ushort OverallRatingId { get; set; }
        [DataMember]
        public IEnumerable<UserReviewQuestion> Questions { get; set; }
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public string CustomerEmail { get; set; }

    }
}
