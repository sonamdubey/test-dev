using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BikeWaleOpr.Entities
{
    
    public class DealerPriceEntity
    {
        public PQ_Price Price { get; set; }
        public VersionEntityBase Version { get; set; }
        public CityEntityBase City { get; set; }
    }
}
