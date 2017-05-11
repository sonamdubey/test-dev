using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace BikewaleOpr.Entities.UserReviews
{
    [Serializable, DataContract]
    public class UserReviewQuestion
    {

        [DataMember]
        public uint Id { get; set; }
        [DataMember]
        public UserReviewQuestionType Type { get; set; }
        [DataMember]
        public string Heading { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public uint SelectedRatingId { get; set; }
        [DataMember]
        public UserReviewQuestionDisplayType DisplayType { get; set; }
        [DataMember]
        public IEnumerable<UserReviewRating> Rating { get; set; }
        [DataMember]
        public ushort Order { get; set; }


        private bool _isRequired = true;
        [DataMember]
        public bool IsRequired { get { return _isRequired; } set { _isRequired = value; } }


        private bool _isVisbile = true;
        [DataMember]
        public bool Visibility { get { return _isVisbile; } set { _isVisbile = value; } }


        [DataMember]
        public IEnumerable<uint> PriceRangeIds { get; set; }

        [DataMember]
        public uint SubQuestionId { get; set; }
    }

    public enum UserReviewQuestionDisplayType
    {
        Star = 1,
        Text = 2
    }

    [Serializable, DataContract]
    public class UserReviewRating
    {
        [DataMember]
        public uint Id { get; set; }
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public uint QuestionId { get; set; }
    }


}
