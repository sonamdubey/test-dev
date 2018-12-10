using Carwale.DTOs.Common;
using Newtonsoft.Json;

namespace Carwale.DTOs.CarData
{
	public class EmiBaseDTO : MinMaxDTO
	{
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
	}
}
