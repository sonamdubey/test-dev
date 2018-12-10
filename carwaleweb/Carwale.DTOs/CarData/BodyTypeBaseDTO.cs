using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.DTOs.Common;
using Newtonsoft.Json;

namespace Carwale.DTOs.CarData
{
	public class BodyTypeBaseDTO: IdNameDto
	{
		[JsonProperty("icon")]
		public string Icon { get; set; }
		[JsonProperty("appIcon")]
		public string AppIcon { get; set; }
        [JsonProperty("isSelected")]
        public bool IsSelected { get; set; }
	}
}
