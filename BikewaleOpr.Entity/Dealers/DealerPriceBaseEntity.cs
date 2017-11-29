using System.Collections.Generic;

namespace BikewaleOpr.Entity.Dealers
{
    /// <summary>
    /// Created by  :   Vishnu Teja Yalakuntla on 28-Jul-2017
    /// Description :   Entity for holding dealer, version and price details in seperate lists.
    /// </summary>
    public class DealerPriceBaseEntity
    {
        public IEnumerable<DealerVersionEntity> DealerVersions { get; set; }
        public IEnumerable<VersionPriceEntity> VersionPrices { get; set; }
    }
}
