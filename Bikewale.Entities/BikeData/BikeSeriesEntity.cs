using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeData
{
    public class BikeSeriesEntity : BikeSeriesEntityBase
    {
		public string HostUrl { get; set; }
		public string OriginalImagePath { get; set; }
		public uint ModelsCount { get; set; }
    }
}
