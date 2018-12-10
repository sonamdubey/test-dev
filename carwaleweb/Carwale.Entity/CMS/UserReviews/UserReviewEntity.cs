using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CMS.UserReviews
{
    [Serializable]
    public class UserReviewEntity
    {
        public int ReviewId { get; set; }
        public int Comments { get; set; }
        public int ThreadId { get; set; }
        public string Title { get; set; }
        public string HandleName { get; set; }
        public string CustomerName { get; set; }
        public DateTime ReviewDate { get; set; }
        public string ReviewRate { get; set; }
        public string Goods { get; set; }
        public string Bads { get; set; }
        public string Description { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string MaskingName { get; set; }
        public string VersionName { get; set; }
    }

    public class UserRatingEntity
    {
        public float OverallRatings { get; set; }
        public Dictionary<string, short> Questions { get; set; }
    }
	public class UserReviewCustomerInfo
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public bool IsEmailVerified { get; set; }
		public string CreatedOn { get; set; }
		public string UpdatedOn { get; set; }
	}
}
