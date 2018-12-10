using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CMS.UserReviews
{
	public class UserReviewCustomerDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string CreatedOn { get; set; }
		public string UpdatedOn { get; set; }
	}
}
