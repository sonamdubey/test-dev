
namespace Bikewale.Entities.UserReviews.Search
{
    public class ProcessedInputFilters
    {
        public uint CityId { get; set; }
        public string[] Make { get; set; }
        public string[] Model { get; set; }
        public string[] Category { get; set; }

        public ushort SortOrder { get; set; }

        // for paging
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }

        public bool Reviews { get; set; }
        public bool Ratings { get; set; }
    }
}
