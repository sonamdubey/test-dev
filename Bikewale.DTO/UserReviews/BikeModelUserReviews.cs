using System;
using System.Collections.Generic;

namespace Bikewale.DTO.UserReviews
{
    public class BikeModelUserReviews
    {
        public Make.MakeBase Make { get; set; }
        public Model.ModelBase Model { get; set; }
        public uint Price { get; set; }
        public string OriginalImagePath { get; set; }
        public string HostUrl { get; set; }
        public bool IsDiscontinued { get; set; }
        public bool IsUpcoming { get; set; }

        public BikeReviewsData ReviewDetails { get; set; }
        public BikeRatingData RatingDetails { get; set; }
        public IEnumerable<UserReviewSummaryDto> UserReviews { get; set; }
    }
}
