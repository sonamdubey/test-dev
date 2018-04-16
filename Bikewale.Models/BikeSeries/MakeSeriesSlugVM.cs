using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.BikeData;

namespace Bikewale.Models.BikeSeries
{
    public class MakeSeriesSlugVM
    {
        public string MakeName { get; set; }
        public string MakeMaskingName { get; set; }
        public IEnumerable<BikeSeriesEntity> MakeSeriesList { get; set; }
    }
}
