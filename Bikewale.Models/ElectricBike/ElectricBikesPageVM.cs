
using Bikewale.Entities.BikeData;
using System.Collections.Generic;
namespace Bikewale.Models
{
    public class ElectricBikesPageVM : ModelBase
    {
        public IEnumerable<MostPopularBikesBase> ElectricBikes { get; set; }
        public RecentNewsVM News { get; set; }
        public RecentExpertReviewsVM ExpertReviews { get; set; }
        public RecentVideosVM Videos { get; set; }
        public BrandWidgetVM Brands { get; set; }

        public uint TabCount = 0;
        public bool IsNewsActive { get; set; }
        public bool IsExpertReviewActive { get; set; }
        public bool IsVideoActive { get; set; }
        public ushort PageCatId { get; set; }
    }
}
