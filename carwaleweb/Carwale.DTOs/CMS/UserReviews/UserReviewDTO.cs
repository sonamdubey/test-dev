using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CMS.UserReviews
{
    public class UserReviewDTO
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
}
