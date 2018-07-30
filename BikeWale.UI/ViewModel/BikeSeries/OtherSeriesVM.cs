using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Models.BikeSeries
{
	public class OtherSeriesVM
	{
		public BikeMakeBase BikeMake { get; set; }
		public IEnumerable<BikeSeriesEntity> OtherSeriesList { get; set; }
	}
}
