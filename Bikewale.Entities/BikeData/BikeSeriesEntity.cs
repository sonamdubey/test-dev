using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{
    [Serializable, DataContract]
    public class BikeSeriesEntity : BikeSeriesEntityBase
    {

		public string HostUrl { get; set; }
		public string OriginalImagePath { get; set; }
		public uint ModelsCount { get; set; }
        public uint MinPrice { get; set; }
	}
}
