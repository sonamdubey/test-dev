using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 25 Mar 2017
    /// Description :   BikeInfo ViewModel
    /// </summary>
    public class BikeInfoVM
    {
        public GenericBikeInfo BikeInfo { get; set; }
        public string BikeName { get; set; }
        public string BikeUrl { get; set; }
        public CityEntityBase CityDetails { get; set; }
        public bool IsUpcoming { get; set; }
        public bool IsDiscontinued { get; set; }
        public bool IsSmallSlug { get; set; }
    }
}
