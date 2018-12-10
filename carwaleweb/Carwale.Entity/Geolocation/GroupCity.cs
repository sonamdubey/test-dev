using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Geolocation
{
    [Serializable]
    public class GroupCity
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public List<City> Cities { get; set; }
    }
}
