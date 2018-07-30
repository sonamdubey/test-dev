using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.BikeData;

namespace Bikewale.Models.BikeSeries
{
    /// <summary>
    /// Created By : Deepak Israni on 16 April 2018
    /// Description: ViewModel for the series linkage slug.
    /// </summary>
    public class MakeSeriesSlugVM
    {
        public string MakeName { get; set; }
        public string MakeMaskingName { get; set; }
        public IEnumerable<BikeSeriesEntity> MakeSeriesList { get; set; }
    }
}
