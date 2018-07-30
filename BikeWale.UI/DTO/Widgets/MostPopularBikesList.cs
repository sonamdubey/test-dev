using System.Collections.Generic;

namespace Bikewale.DTO.Widgets
{
    /// <summary>
    /// Modified by :   Sumit Kate on 01 Jul 2016
    /// Description :   Used IEnumerable
    /// </summary>
    public class MostPopularBikesList
    {
        public IEnumerable<MostPopularBikes> PopularBikes { get; set; }

    }
}
