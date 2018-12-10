using Carwale.DTOs.Common;
using Newtonsoft.Json;

namespace Carwale.DTOs.CarData
{
	public class TransmissionBaseDTO : IdNameDto
	{
		[JsonProperty("icon")]
		public string Icon { get; set; }
		[JsonProperty("appIcon")]
		public string AppIcon { get; set; }
	}
}
