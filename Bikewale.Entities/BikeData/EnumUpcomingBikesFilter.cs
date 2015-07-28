using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// Summary : Enum for upcoming bikes filter clauses
    /// </summary>
    public enum EnumUpcomingBikesFilter
    {
        Default = 0,
        PriceLowToHigh = 1,
        PriceHighToLow = 2,
        LaunchDateSooner = 3,
        LaunchDateLater = 4
    }
}
