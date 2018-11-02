using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Models.BikeSeries
{
    /// <summary>
    /// Created By : Deepak Israni on 16 April 2018
    /// Description: ViewModel for the series linkage slug.
    /// Modified by : Snehal Dange on 1st Nov 2018
    /// Desc : Added CardSlideCount which describe how many cards should slide on clicking `next , prev` in corousel. Default to 2 . Used in UI.
    /// </summary>
    public class MakeSeriesSlugVM
    {
        public string MakeName { get; set; }
        public string MakeMaskingName { get; set; }
        public IEnumerable<BikeSeriesEntity> MakeSeriesList { get; set; }
        public uint CardSlideCount { get; set; }
    }
}
