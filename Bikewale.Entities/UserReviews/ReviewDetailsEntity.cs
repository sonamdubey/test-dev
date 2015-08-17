using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.UserReviews
{
    public class ReviewDetailsEntity
    {
        private ReviewTaggedBikeEntity objBikes = new ReviewTaggedBikeEntity();
        private ReviewEntity objReview = new ReviewEntity();
        private ReviewRatingEntity objRating = new ReviewRatingEntity();

        public ReviewTaggedBikeEntity BikeEntity { get { return objBikes; } set { objBikes = value ;} }
        public ReviewEntity ReviewEntity { get { return objReview; } set { objReview = value ;} }
        public ReviewRatingEntity ReviewRatingEntity { get { return objRating; } set { objRating = value ;} }

        public string HostUrl { get; set; }
        public string LargePicUrl { get; set; }
        public string OriginalImagePath { get; set; }
        public bool New { get; set; }   //Added by Suresh Prajapati on 20 Aug 2014
        public bool Used { get; set; }  //Added by Suresh Prajapati on 20 Aug 2014
        public uint NextReviewId { get; set; }
        public uint PrevReviewId { get; set; }
        //public string NextReviewTitle { get; set; }
        //public string PrevReviewTitle { get; set; }
    }
}
