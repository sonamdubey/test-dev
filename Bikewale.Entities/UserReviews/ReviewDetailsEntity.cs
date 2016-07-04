using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.UserReviews
{
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
        public bool New { get; set; }   //Added by Suresh Prajapati on 20 Aug 2014
        [DataMember]
        public bool Used { get; set; }  //Added by Suresh Prajapati on 20 Aug 2014
        [DataMember]
        public uint NextReviewId { get; set; }
        [DataMember]
        public uint PrevReviewId { get; set; }
        //public string NextReviewTitle { get; set; }
        //public string PrevReviewTitle { get; set; }
    }
}
