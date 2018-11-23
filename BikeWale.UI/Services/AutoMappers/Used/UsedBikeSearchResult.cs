using AutoMapper;
using Bikewale.DTO.Used.Search;
using Bikewale.Entities.NewBikeSearch;

namespace Bikewale.Service.AutoMappers.Used
{
    public class UsedBikeSearchResult
    {
        internal static DTO.Used.Search.SearchResult Convert(Entities.Used.Search.SearchResult objSearchList)
        {
          
            return Mapper.Map<Bikewale.Entities.Used.Search.SearchResult, Bikewale.DTO.Used.Search.SearchResult>(objSearchList);
        }
    }
}