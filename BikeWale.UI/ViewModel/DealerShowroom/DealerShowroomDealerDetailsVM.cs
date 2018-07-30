
using Bikewale.Entities.BikeData;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Models.ServiceCenters;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by Subodh Jain on 30 March 2017
    /// Summary :- Dealer details view model
    /// Modified by : Aditi Srivastava on 25 May 2017
    /// Summary     : Added GALabel for GA triggers
    /// Modified by : Vivek Singh Tomar on 8th Sep 2017
    /// Summary     : Added RedirectUrl
    /// </summary>
    public class DealerShowroomDealerDetailsVM : ModelBase
    {
        public DealerCardVM DealersList { get; set; }
        public LeadCaptureEntity LeadCapture { get; set; }
        public BikeMakeEntityBase Make { get; set; }
        public CityEntityBase CityDetails { get; set; }
        public DealerBikesEntity DealerDetails { get; set; }
        public MostPopularBikeWidgetVM PopularBikes { get; set; }
        public ServiceCenterDetailsWidgetVM ServiceCenterDetails { get; set; }
        public uint PQCityId { get; set; }
        public uint PQAreaID { get; set; }
        public string PQAreaName { get; set; }
        public string CustomerAreaName { get; set; }
        public string GALabel { get; set; }
        public string RedirectUrl { get; set; }
    }
}
