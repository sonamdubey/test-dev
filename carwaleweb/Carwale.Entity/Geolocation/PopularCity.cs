using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Geolocation
{
    [Serializable]
    public class PopularCity
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string DisplayText { get; set; }
    }
}
