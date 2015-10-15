using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeData
{
    [Serializable]
    public class Overview
    {
        public List<Specs> OverviewList { get; set; }
        public string DisplayName { get; set; }
    }
}
