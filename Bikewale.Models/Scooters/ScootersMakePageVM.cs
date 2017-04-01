using Bikewale.Entities.BikeData;
using Bikewale.Models.ServiceCenters;
using System.Collections.Generic;
namespace Bikewale.Models
{
    /// <summary>
    /// </summary>
    /// <seealso cref="Bikewale.Models.ModelBase" />
    /// <author>
    /// Create by: Sangram Nandkhile on 01-Apr-2017
    /// Summary:  View Model for 
    /// </author>
    public class ScootersMakePageVM : ModelBase
    {
        public BikeMakeEntityBase Make { get; set; }
        public BikeDescriptionEntity Description { get; set; }
        public IEnumerable<MostPopularBikesBase> Scooters { get; set; }
        public UpcomingBikesWidgetVM UpcomingScooters { get; set; }
        public ICollection<SimilarCompareBikeEntity> SimilarCompareScooters { get; set; }
        public DealersServiceCentersIndiaWidgetVM DealersServiceCenter { get; set; }
        public ServiceCenterDetailsWidgetVM ServiceCenters { get; set; }
        public DealerCardVM Dealers { get; set; }
        public IEnumerable<BikeMakeEntityBase> OtherBrands { get; set; }
        public string DealerServiceTitle { get; set; }
        public string LocationMasking { get; set; }
        public string Location { get; set; }
        public ushort PageCatId { get; set; }
        public bool IsScooterDataAvailable { get; set; }
        public bool IsCompareDataAvailable { get; set; }
        public bool IsUpComingBikesAvailable { get; set; }
        public bool IsDealerServiceDataInIndiaAvailable { get; set; }
        public bool IsDealerServiceDataAvailable { get; set; }
        public bool IsDealerAvailable { get; set; }
        public bool IsServiceDataAvailable { get; set; }
        public bool IsMakeTabsDataAvailable { get; set; }
    }
}
