using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.UserReviews
{
    /// <summary>
    /// created by sajal gupta on 14-07-2017
    /// description : Entity to hold question rating value of particular model.
    /// </summary>
    [Serializable, DataContract]
    public class QuestionsRatingValueByModel
    {
        public uint ModelId { get; set; }
        public IEnumerable<QuestionRatingsValueEntity> QuestionsList { get; set;} 
    }
}
