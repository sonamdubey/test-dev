
using Bikewale.Entities.BikeData;
using Bikewale.Entities.SEO;
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
