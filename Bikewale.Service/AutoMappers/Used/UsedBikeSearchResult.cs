using AutoMapper;
using Bikewale.DTO.Used.Search;
using Bikewale.Entities.NewBikeSearch;

namespace Bikewale.Service.AutoMappers.Used
{
    public class UsedBikeSearchResult
    {
        internal static DTO.Used.Search.SearchResult Convert(Entities.Used.Search.SearchResult objSearchList)
        {
            Mapper.CreateMap<Bikewale.Entities.Used.BikePhoto, BikePhoto>();
            Mapper.CreateMap<PagingUrl, Bikewale.DTO.BikeBooking.PagingUrl>();
            Mapper.CreateMap<Bikewale.Entities.Used.UsedBikeBase, UsedBikeBase>();
            Mapper.CreateMap<Bikewale.Entities.Used.Search.SearchResult, Bikewale.DTO.Used.Search.SearchResult>();
            return Mapper.Map<Bikewale.Entities.Used.Search.SearchResult, Bikewale.DTO.Used.Search.SearchResult>(objSearchList);
        }
    }
}