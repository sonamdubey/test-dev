
using Bikewale.Entities.Pager;
namespace Bikewale.Models.Shared
{
    public class Pager
    {
        public int TotalPages { get; set; }
        public int CurrentPageNo { get; set; }
        public string MakeId { get; set; }
        public uint CityId { get; set; }
        public string ModelId { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public PagerOutputEntity PagerOutput { get; set; }
        public bool ShowHash { get; set; }
    }
}
