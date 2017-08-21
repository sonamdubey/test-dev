using System.Collections.Generic;

namespace BikewaleOpr.Entity.Dealers
{
    /// <summary>
    /// Created by  :   Vishnu Teja Yalakuntla on 31-Jul-2017
    /// Description :   Entity holding dealer versions with item categories as list for each entry..
    /// </summary>
    public class DealerVersionPriceEntity : DealerVersionEntity
    {
        public IEnumerable<VersionPriceEntity> Categories { get; set; }
    }
}
