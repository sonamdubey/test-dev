using Bikewale.Entities.BikeData;
using System.Collections.Generic;
namespace Bikewale.Models
{
    /// <summary>
    /// </summary>
    /// <seealso cref="Bikewale.Models.ModelBase" />
    /// <author>
    /// Create by: Sangram Nandkhile on 01-Apr-2017
    /// Summary:  View Model for 
    /// Modified by : Aditi Srivastava on 27 Apr 2017
    /// Summary  : Added new viewModel for similar comparisons
    /// Modified by : Aditi Srivastava on 15 June 2017
    /// Summary     : Added editorial widgets (news, expert reviews, videos)
    /// Modified by : Snehal Dange on 21 August 2017
    /// Summary     : Added ScooterNewsUrl
    /// Modified by : Vivek Singh Tomar on 12th Oct 2017
    /// Summary : removed service center widget
    /// </author>
    public class ScootersMakePageVM : ModelBase
    {
        public BikeMakeEntityBase Make { get; set; }
        public BikeDescriptionEntity Description { get; set; }
        public IEnumerable<MostPopularBikesBase> Scooters { get; set; }
        public UpcomingBikesWidgetVM UpcomingScooters { get; set; }
        public PopularComparisonsVM SimilarCompareScooters { get; set; }
        public DealersServiceCentersIndiaWidgetVM DealersServiceCenter { get; set; }
        public DealerCardVM Dealers { get; set; }
        public IEnumerable<BikeMakeEntityBase> OtherBrands { get; set; }
        public RecentNewsVM News { get; set; }
        public RecentExpertReviewsVM ExpertReviews { get; set; }
        public RecentVideosVM Videos { get; set; }
        public string LocationMasking { get; set; }
        public string Location { get; set; }
        public ushort PageCatId { get; set; }
        public bool IsScooterDataAvailable { get; set; }
        public bool IsCompareDataAvailable { get; set; }
        public bool IsUpComingBikesAvailable { get; set; }
        public bool IsNewsAvailable { get; set; }
        public bool IsExpertReviewsAvailable { get; set; }
        public bool IsVideosAvailable { get; set; }
        public bool IsDealerServiceDataInIndiaAvailable { get; set; }
        public bool IsDealerServiceDataAvailable { get; set; }
        public bool IsDealerAvailable { get; set; }
        public bool IsMakeTabsDataAvailable { get; set; }

        public string ScooterNewsUrl { get; set; }


    }
}
