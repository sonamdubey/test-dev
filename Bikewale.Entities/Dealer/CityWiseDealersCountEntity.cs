using Bikewale.Entities.Location;

namespace Bikewale.Entities.Dealer
{
    /// <summary>
    /// Created By : Ashwini Todkar on 6 June 2014
    /// </summary>
    public class CityWiseDealersCountEntity : CityEntityBase
    {
        public int StateId { get; set; }
        public int DealersCount { get; set; }        
    }
}
