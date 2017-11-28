using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.UserReviews
{
    /// <summary>
    /// created by sajal gupta on 14-07-2017
    /// description : Entity to hold question rating value 
    /// </summary>
    [Serializable, DataContract]
    public class QuestionRatingsValueEntity
    {
        public uint VersionId { get; set; }
        public uint ModelId { get; set;}
        public UInt16 QuestionId { get; set;}
        public float AverageRatingValue { get; set;}
        public string QuestionHeading { get; set;}
        public string QuestionDescription { get; set; }
    }
}
