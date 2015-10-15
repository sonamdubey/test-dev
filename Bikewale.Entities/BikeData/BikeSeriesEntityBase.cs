using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeData
{
    [Serializable]
    public class BikeSeriesEntityBase
    {
        public int SeriesId { get; set; }
        public string SeriesName { get; set; }
        public string MaskingName { get; set; }
    }
}
