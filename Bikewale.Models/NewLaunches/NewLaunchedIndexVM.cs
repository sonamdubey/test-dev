using System.Linq;
namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 30 Mar 2017
    /// Description :   NewLaunchedIndex View Model
    /// </summary>
    public class NewLaunchedIndexVM : ModelBase
    {
        public NewLaunchesBikesVM NewLaunched { get; set; }
        public UpcomingBikesWidgetVM Upcoming { get; set; }
        public BrandWidgetVM Brands { get; set; }
        public uint CityId { get; set; }
        public bool HasUpcoming { get { return Upcoming != null && Upcoming.UpcomingBikes != null && Upcoming.UpcomingBikes.Count() > 0; } }
        public bool HasBrands { get { return Brands != null && Brands.TopBrands != null && Brands.TopBrands.Count() > 0; } }
    }
}
