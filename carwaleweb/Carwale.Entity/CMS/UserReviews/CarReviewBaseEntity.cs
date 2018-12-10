using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CMS.UserReviews
{
    [Serializable]
    public class CarReviewBaseEntity
    {
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string MaskingName { get; set; }
        public int TotalReviews { get; set; }
    }
}
