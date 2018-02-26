
using Bikewale.ElasticSearch.Entities;
using Bikewale.Entities.NewBikeSearch;
using System.Collections.Generic;
namespace Bikewale.Interfaces.NewBikeSearch
{
    public interface IBikeSearch
    {
        IEnumerable<BikeModelDocument> GetBikeSearch(SearchFilters filters);
    }
}
