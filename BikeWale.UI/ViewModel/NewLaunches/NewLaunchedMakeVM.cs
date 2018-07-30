using Bikewale.Entities.BikeData;
using System.Linq;
namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 30 Mar 2017
    /// Description :   View model for Make wise new launches page
    /// </summary>
    public class NewLaunchedMakeVM : ModelBase
    {
        public BikeMakeEntityBase Make { get; set; }
        public NewLaunchesBikesVM NewLaunched { get; set; }
        public UpcomingBikesWidgetVM Upcoming { get; set; }
        public BrandWidgetVM Brands { get; set; }
        public uint CityId { get; set; }
        public bool HasUpcoming { get { return Upcoming != null && Upcoming.UpcomingBikes != null && Upcoming.UpcomingBikes.Any(); } }
        public bool HasBrands { get { return Brands != null && Brands.TopBrands != null && Brands.TopBrands.Any(); } }
    }
}
