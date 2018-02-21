
using Bikewale.Entities.NewBikeSearch;
using System.Collections.Generic;
namespace Bikewale.Interfaces.NewBikeSearch
{
    public interface IBikeSearch
    {
        IEnumerable<Nest.SuggestOption<T>> GetBikeSearch<T>(SearchFilters filters, BikeSearchEnum source, int noOfRecords = 0) where T : class;
    }
}
