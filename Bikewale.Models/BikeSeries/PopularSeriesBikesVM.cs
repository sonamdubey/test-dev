using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Models.BikeSeries
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 11 Dec 2017
    /// Description : ViewModel for popular series bikes widget.
    /// </summary>
    public class PopularSeriesBikesVM
    {
        public IEnumerable<NewBikeEntityBase> BikesList { get; set; }
        public BikeSeriesEntityBase SeriesBase { get; set; }
        public string WidgetTitle { get; set; }
        public string WidgetViewAllUrl { get; set; }
        public string CityName { get; set; }
        public int PQSourceId { get; set; }
    }
}
