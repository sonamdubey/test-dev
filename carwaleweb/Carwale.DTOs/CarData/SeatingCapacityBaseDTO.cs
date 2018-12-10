using Carwale.DTOs.Common;
using Newtonsoft.Json;

namespace Carwale.DTOs.CarData
{
	public class SeatingCapacityBaseDTO: MinMaxDTO
	{
        [JsonProperty("isSelected")]
        public bool IsSelected { get; set; }
	}
}
