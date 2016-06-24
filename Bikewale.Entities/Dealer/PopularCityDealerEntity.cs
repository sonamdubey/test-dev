using Bikewale.Entities.Location;

namespace Bikewale.Entities.Dealer
{
    /// <summary>
    /// Created by  :   Sumit Kate on 21 Jun 2016
    /// Description :   Popular City Dealers
    /// </summary>
    public class PopularCityDealerEntity
    {
        public CityEntityBase CityBase { get; set; }
        public uint NumOfDealers { get; set; }
    }
}
