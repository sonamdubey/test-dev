using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.UserReviews
{
    /// <summary>
    /// created by sajal gupta on 14-07-2017
    /// description : Entity to hold question rating value 
    /// </summary>
    [Serializable, DataContract]
    public class QuestionRatingsValueEntity
    {
        public uint ModelId { get; set;}
        public UInt16 QuestionId { get; set;}
        public float AverageRatingValue { get; set;}
        public string QuestionHeading { get; set;}
        public string QuestionDescription { get; set; }
    }
}
