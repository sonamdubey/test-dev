using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Carwale.Entity;
using Carwale.Entity.UserReview;

namespace Carwale.UI.ViewModels.UserReviews
{
	public class WriteReview
	{
		public UserReviewPageDetails ReviewDetails { get; set; }
		public string Hash { get; set; }
		public bool IsMobile { get; set; }
        // channel added to detect journey of user
        public string Channel { get; set; }
	}
}