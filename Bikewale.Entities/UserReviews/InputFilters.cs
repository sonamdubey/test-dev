
namespace Bikewale.Entities.UserReviews.Search
{
    public class InputFilters
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public ushort SO { get; set; }
        public int PN { get; set; }
        public int PS { get; set; }
        public string CAT { get; set; }
        public bool Reviews { get; set; }
        public bool Ratings { get; set; }
        public uint SkipReviewId { get; set; }
    }
}
