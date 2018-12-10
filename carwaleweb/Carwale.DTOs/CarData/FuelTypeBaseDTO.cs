using Carwale.DTOs.Common;
using Newtonsoft.Json;

namespace Carwale.DTOs.CarData
{
	public class FuelTypeBaseDTO: IdNameDto
	{
		[JsonProperty("icon")]
		public string Icon { get; set; }
		[JsonProperty("appIcon")]
		public string AppIcon { get; set; }
        [JsonProperty("isSelected")]
        public bool IsSelected { get; set; }
    }
}
