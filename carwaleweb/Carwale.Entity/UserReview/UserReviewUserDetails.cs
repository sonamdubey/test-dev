using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Carwale.Entity.UserReview
{
	public class UserReviewUserDetails
	{
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("email")]
		public string Email { get; set; }
	}
}
