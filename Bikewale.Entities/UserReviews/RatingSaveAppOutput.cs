using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.UserReviews
{
    public class RatingSaveAppOutput
    {     
        public IEnumerable<UserReviewQuestion> Questions { get; set; }       
        public string RatingQuestionAns { get; set; }        
        public string UserName { get; set; }
        public string EmailId { get; set; }        
        public uint MakeId { get; set; }
        public uint ModelId { get; set; }        
        public ushort PriceRangeId { get; set; }        
        public uint ReviewId { get; set; }        
        public string ReturnUrl { get; set; }        
        public ushort PlatformId { get; set; }        
        public ushort? SourceId { get; set; }        
        public ushort? ContestSrc { get; set; }        
        public string UtmzCookieValue { get; set; }
    }
}
