
using Bikewale.Entities.BikeData;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.BikeData
{
    public class MakeModelList
    {
		[JsonProperty("makeBase")]
		public BikeMakeEntityBase MakeBase { get; set; }
		[JsonProperty("modelBase")]
		public IEnumerable<BikeModelEntityBase> ModelBase { get; set; }
    }
}
