using System;

namespace Bikewale.Entities.DTO
{
    public class ReviewBase
    {
        public int ReviewId { get; set; }
        public string ReviewTitle { get; set; }
        public DateTime ReviewDate { get; set; }
        public string WrittenBy { get; set; }
    }
}
