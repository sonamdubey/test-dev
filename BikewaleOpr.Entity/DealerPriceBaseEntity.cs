using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Entity
{
    /// <summary>
    /// Created by  :   Vishnu Teja Yalakuntla on 28-Jul-2017
    /// Description :   Entity for holding dealer, version and price details.
    /// </summary>
    public class DealerPriceBaseEntity
    {
        public IEnumerable<DealerVersionEntity> DealerVersions { get; set; }
        public IEnumerable<VersionPriceEntity> VersionPrices { get; set; }
    }
}
