using System.Linq;
namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 30 Mar 2017
    /// Description :   NewLaunchedIndex View Model
    /// Modified By :Snehal Dange on 30th Oct 2017
    /// Description : Added news widget
    /// </summary>
    public class NewLaunchedIndexVM : ModelBase
    {
        public NewLaunchesBikesVM NewLaunched { get; set; }
        public UpcomingBikesWidgetVM Upcoming { get; set; }
        public uint CityId { get; set; }
        public bool HasUpcoming { get { return Upcoming != null && Upcoming.UpcomingBikes != null && Upcoming.UpcomingBikes.Any(); } }
        public RecentNewsVM News { get; set; }
    }
}
