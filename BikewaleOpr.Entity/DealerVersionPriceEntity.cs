using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Entity
{
    public class DealerVersionPriceEntity : DealerVersionEntity
    {
        public IEnumerable<VersionPriceEntity> Categories { get; set; }
    }
}
