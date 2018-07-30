using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Models.Shared
{
    /// <summary>
    /// Created by : Aditi Srivastava on 9 Mar 2017
    /// Summary    : Model for popular scooters widget
    /// </summary>
    public class PopularScootersList
    {
        public int PageCatId { get; set; }
        public int PQSourceId { get; set; }
        public IEnumerable<MostPopularBikesBase> PopularScooters { get; set; }
    }
}
