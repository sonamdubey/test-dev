
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.BikeData
{
    public class MakeModelList
    {
		[JsonProperty("make")]
		public MakeBase MakeBase { get; set; }
		[JsonProperty("models")]
		public IEnumerable<ModelBase> ModelBase { get; set; }
    }
}
