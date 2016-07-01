using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using BikeWale.Entities.AutoBiz;

namespace BikeWale.Entities.AutoBiz
{
    public class DealerPriceEntity
    {
        public PQ_Price Price { get; set; }
        public VersionEntityBase Version { get; set; }
        public CityEntityBase City { get; set; }
    }
}
