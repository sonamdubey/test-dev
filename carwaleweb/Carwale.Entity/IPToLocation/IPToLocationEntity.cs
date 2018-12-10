using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.IPToLocation
{
    [Serializable]
    public class IPToLocationEntity
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
    }
}
