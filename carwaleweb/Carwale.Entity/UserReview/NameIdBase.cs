using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Carwale.Entity.UserReview
{
	public class NameIdBase
	{
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("id")]
		public int Id { get; set; }
		[JsonIgnore]
		public string MaskingName { get; set; }
	}
}
