using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 25 Mar 2017
    /// Description :   BikeInfo ViewModel
    /// Modified by : Snehal Dange on 25th Oct 2018
    /// Desc :  Added series data :BikeSeriesEntity
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
        public BikeInfoTabType Category { get; set; }
        public string ReviewLink { get; set; }
        public uint ReviewCount { get; set; }
        public BikeSeriesEntity Series { get; set; }
    }
}
