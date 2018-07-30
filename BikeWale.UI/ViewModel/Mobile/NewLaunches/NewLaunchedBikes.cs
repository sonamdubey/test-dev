
using Bikewale.Entities.BikeData;
namespace Bikewale.Models.Mobile.NewLaunches
{
    public class NewLaunchedBikes
    {
        public PageMetaTags PageMetas { get; set; }
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public string PageHeading { get; set; }
        public string PageDescription { get; set; }
    }
}
