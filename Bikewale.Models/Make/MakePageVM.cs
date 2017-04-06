
using Bikewale.Entities.BikeData;
using Bikewale.Models.ServiceCenters;
using System.Collections.Generic;
namespace Bikewale.Models
{
    /// <summary>
    /// </summary>
    /// <seealso cref="Bikewale.Models.ModelBase" />
    /// <author>
    /// Create by: Sangram Nandkhile on 27-Mar-2017
    /// Summary:  View Model for Make page controller
    /// </author>
    public class MakePageVM : ModelBase
    {
        public string MakeName { get; set; }
        public string MakeMaskingName { get; set; }
        public string LocationMasking { get; set; }
        public string Location { get; set; }
        public string DealerServiceTitle { get; set; }

        public IEnumerable<MostPopularBikesBase> Bikes { get; set; }
        public UpcomingBikesWidgetVM UpcomingBikes { get; set; }
        public BikeDescriptionEntity BikeDescription { get; set; }
        public UsedBikeModelsWidgetVM UsedModels { get; set; }

        public RecentNewsVM News { get; set; }
        public RecentExpertReviewsVM ExpertReviews { get; set; }
        public RecentVideosVM Videos { get; set; }
        public IEnumerable<BikeVersionEntity> DiscontinuedBikes { get; set; }

        public bool IsUpComingBikesAvailable { get; set; }
        public bool IsNewsAvailable { get; set; }
        public bool IsExpertReviewsAvailable { get; set; }
        public bool IsVideosAvailable { get; set; }
        public bool IsDiscontinuedBikeAvailable { get; set; }
        public bool IsUsedModelsBikeAvailable { get; set; }

        public bool IsDealerServiceDataInIndiaAvailable { get; set; }
        public bool IsDealerServiceDataAvailable { get; set; }
        public bool IsDealerAvailable { get; set; }
        public bool IsServiceDataAvailable { get; set; }
        public bool IsMakeTabsDataAvailable { get; set; }

        public DealersServiceCentersIndiaWidgetVM DealersServiceCenter { get; set; }
        public ServiceCenterDetailsWidgetVM ServiceCenters { get; set; }
        public DealerCardVM Dealers { get; set; }

    }
}
