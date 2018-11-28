
using Bikewale.Entities.BikeData;
using Bikewale.Entities.AutoComplete;
using System.Collections.Generic;
namespace Bikewale.Models
{
    /// <summary>
    /// Modified By : Monika Korrapati on 22 Nov 2018
    /// Description : Added Payload PageData
    /// </summary>
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
        public Payload PageData { get; set; }
    }
}
