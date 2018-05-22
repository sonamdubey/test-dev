using Bikewale.Entities.BikeData;
using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.UserReviews
{
    /// <summary>
    /// modified By :- Subodh Jain on 17 jan 2017
    /// Summary :- added ModelSpecs,ModelHighendPrice,ModelBasePrice,IsFuturistic
    /// </summary>
    [Serializable, DataContract]
    public class ReviewDetailsEntity
    {
        [DataMember]
        private ReviewTaggedBikeEntity objBikes = new ReviewTaggedBikeEntity();
        [DataMember]
        private ReviewEntity objReview = new ReviewEntity();
        [DataMember]
        private ReviewRatingEntity objRating = new ReviewRatingEntity();

        [DataMember]
        public ReviewTaggedBikeEntity BikeEntity { get { return objBikes; } set { objBikes = value; } }
        [DataMember]
        public ReviewEntity ReviewEntity { get { return objReview; } set { objReview = value; } }
        [DataMember]
        public ReviewRatingEntity ReviewRatingEntity { get { return objRating; } set { objRating = value; } }

        [DataMember]
        public string HostUrl { get; set; }
        [DataMember]
        public string LargePicUrl { get; set; }
        [DataMember]
        public string OriginalImagePath { get; set; }
        [DataMember]
        public bool IsFuturistic { get; set; }
        [DataMember]
        public bool New { get; set; }
        [DataMember]
        public bool Used { get; set; }
        [DataMember]
        public uint NextReviewId { get; set; }
        [DataMember]
        public uint PrevReviewId { get; set; }
        [DataMember]
        public string ModelBasePrice { get; set; }
        [DataMember]
        public string ModelHighendPrice { get; set; }
    }
}
