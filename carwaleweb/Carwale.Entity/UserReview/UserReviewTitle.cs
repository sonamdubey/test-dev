using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Carwale.Entity.UserReview
{
	public class UserReviewTitle
	{
		[JsonProperty("heading")]
		public string Heading { get; set; }
		[JsonProperty("description")]
		public string Description { get; set; }
		[JsonProperty("userResponse")]
		public string UserResponse { get; set; }

	}
}
