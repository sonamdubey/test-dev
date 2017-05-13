
using System;
namespace Bikewale.Entities.UserReviews.Search
{
    /// <summary>
    /// Created By : Sushil Kumar on 7th May 2017
    /// Description : Class for inut filters to be applied on user reviews listing page
    /// </summary>
    [Serializable]
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
