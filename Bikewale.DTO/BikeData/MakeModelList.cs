
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.BikeData
{
    public class MakeModelList
    {
		[JsonProperty("makeBase")]
		public MakeBase MakeBase { get; set; }
		[JsonProperty("modelBase")]
		public IEnumerable<ModelBase> ModelBase { get; set; }
    }
}
