using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Dealer
{
    public class NewBikeDealerEntityList
    {
        public IEnumerable<NewBikeDealerEntityBase> Dealers { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string BikeMake { get; set; }
        public string MakeMaskingName { get; set; }
    }
}
