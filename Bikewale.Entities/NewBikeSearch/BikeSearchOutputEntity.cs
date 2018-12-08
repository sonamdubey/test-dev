using System.Collections.Generic;
namespace Bikewale.Entities.NewBikeSearch
{
    public class BikeSearchOutputEntity
    {
        public IEnumerable<BikeModelDocumentEntity> Bikes { get; set; }
        public int TotalCount { get; set; }
        public int CurrentPageNumber { get; set; }
        public SearchFilters NextFilters { get; set; }
        public SearchFilters PrevFilters { get; set; }

    }
}
