using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CarData
{
    [Serializable]
    public class VersionAveragePrice
    {
        public int CarAveragePrice { get; set; }
        public int VersionId { get; set; }
        public string VersionName { get; set; }
    }
}
