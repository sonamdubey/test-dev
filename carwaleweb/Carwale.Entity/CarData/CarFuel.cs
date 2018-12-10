using System.Collections.Generic;
using Newtonsoft.Json;
using Carwale.Entity.Common;

namespace Carwale.Entity.CarData
{
	public class CarFuel:IdName
    {
        [JsonProperty("alternativeFuels")]
        public List<IdName> AlternativeFuels { get; set; }
    }
}
