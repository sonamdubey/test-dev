using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carwale.DTOs.CarData
{
	public class CarDetailsListDTOV2 
	{
        [JsonProperty("models")]
        public List<VersionDetailsDtoV2> Models { get; set; }
		[JsonProperty("orpText")]
		public string OrpText { get; set; }
	}
}
