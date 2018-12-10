using Newtonsoft.Json;

namespace Carwale.DTOs.Common
{
	public class MinMaxDTO
	{
		[JsonProperty("min")]
		public int Min { get; set; }
		[JsonProperty("max")]
		public int Max { get; set; }
	}
}
