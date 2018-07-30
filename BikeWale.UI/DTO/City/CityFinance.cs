using Newtonsoft.Json;

namespace Bikewale.DTO.City
{
	/// <summary>
	/// Created By  : Pratibha Verma on 17 May 2018
	/// Description : city entity for finance city popup
	/// </summary>
	public class CityFinance : CityBase
	{
		[JsonProperty("popularityOrder")]
		public uint PopularityOrder { get; set; }
	}
}
