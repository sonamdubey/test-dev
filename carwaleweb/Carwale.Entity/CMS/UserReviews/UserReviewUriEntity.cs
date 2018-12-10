using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CMS.UserReviews
{
    public class UserReviewUriEntity
    {
        public int ModelId { get; set; }
        public int VersionId { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int SortCriteria { get; set; }
        public int CityId { get; set; }
    }
}
