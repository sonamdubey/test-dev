using Bikewale.Entities.BikeData;
using Bikewale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.UserReviews
{
    /// <summary>
    /// Created by : Snehal Dange on 1st Sep 2017
    /// Summary     : Entity for Rate bike page (1st page)
    /// </summary>
    [Serializable, DataContract]
    public class UserReviewRatingData
    {
        public BikeModelEntity ObjModelEntity { get; set; }

        public IEnumerable<UserReviewQuestion> Questions { get; set; }

        public IEnumerable<UserReviewOverallRating> OverallRating { get; set; }

        public string ReviewsOverAllrating { get; set; }

        public string ErrorMessage { get; set; }
        public ushort PriceRangeId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public uint ReviewId { get; set; }
        public bool IsFake { get; set; }
        public ushort SelectedRating { get; set; }
        public string ReturnUrl { get; set; }
        public ushort? SourceId { get; set; }
        public ushort? ContestSrc { get; set; }
        public ushort PlatFormId { get; set; }

        public string UtmzCookieValue { get; set; }

    }
}
