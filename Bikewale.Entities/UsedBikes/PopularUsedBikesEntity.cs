using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.UsedBikes
{
    public class PopularUsedBikesEntity
    {
        
        public string MakeName { get; set; }
        public uint TotalBikes { get; set; }
        public double AvgPrice { get; set; }
        public string HostURL { get; set; }
        public string OriginalImagePath { get; set; }
    }
}
