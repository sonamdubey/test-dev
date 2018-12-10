using System;
using Carwale.Entity.Common;
using Newtonsoft.Json;

namespace Carwale.Entity.Classification
{
	[Serializable]
	public class BodyTypeBase: IdName
	{
		[JsonProperty("image")]
		public string Image { get; set; }
		[JsonIgnore]
		public string Icon { get; set; }
		[JsonIgnore]
		public string AppIcon { get; set; }
	}
}
