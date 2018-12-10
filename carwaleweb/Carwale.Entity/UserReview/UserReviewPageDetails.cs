using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Carwale.Entity.UserReview
{
	public class UserReviewPageDetails
	{
		[JsonProperty("carDetails")]
		public UserReviewCarDetails CarDetails { get; set; }
		[JsonProperty("review")]
		public UserReviewGuideLines Review { get; set; }
		[JsonProperty("title")]
		public UserReviewTitle Title { get; set; }
		[JsonProperty("moreRatings")]
		public UserReviewMoreRatings MoreRatings { get; set; }
		[JsonProperty("userRating")]
		public string UserRating { get; set; }
		[JsonProperty("userDetails")]
		public UserReviewUserDetails UserDetails { get; set; }
	}
}
