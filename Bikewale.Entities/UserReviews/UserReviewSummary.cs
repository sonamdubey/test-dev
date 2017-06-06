using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;

namespace Bikewale.Entities.UserReviews
{
    [Serializable]
    public class UserReviewSummary //: BasicBikeEntityBase
    /// <summary>
    /// Modified by Sajal Gupta on 05-05-2017
    /// Descruioption : Added UpVotes, DownVotes, Views, EntryDate
    /// </summary>
    {
        public uint ReviewId { get; set; }
        public UserReviewOverallRating OverallRating { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string TipsDescriptionSmall { get; set; }
        public string Tips { get; set; }
        public ushort OverallRatingId { get; set; }
        public IEnumerable<UserReviewQuestion> Questions { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string ReturnUrl { get; set; }
        public uint UpVotes { get; set; }
        public ushort PlatformId { get; set; }
        public uint DownVotes { get; set; }
        public uint Views { get; set; }
        public DateTime EntryDate { get; set; }
        public uint OldReviewId { get; set; }

        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public string OriginalImagePath { get; set; }
        public string HostUrl { get; set; }
        public bool IsDiscontinued { get; set; }
        public bool IsUpcoming { get; set; }
    }
}
