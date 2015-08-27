using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.DTO
{
    public class ReviewDetails
    {
        private ReviewTaggedBike objBikes = new ReviewTaggedBike();
        private Review objReview = new Review();
        private ReviewRating objRating = new ReviewRating();

        public ReviewTaggedBike Bike { get; set; }
        public Review Review { get; set; }
        public string HostUrl { get; set; }
        public string LargePicUrl { get; set; }
        public string OriginalImagePath { get; set; }
        public bool New { get; set; }  
        public bool Used { get; set; }  
        public uint NextReviewId { get; set; }
        public uint PrevReviewId { get; set; }

    }
}
