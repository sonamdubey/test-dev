using AutoMapper;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.NewBikeSearch;
using Bikewale.DTO.Series;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.NewBikeSearch;

namespace Bikewale.BAL.BikeSearch
{
    public class SearchOutputMapper
    {
        internal static SearchOutput Convert(SearchOutputEntity objSuggestion)
        {
            Mapper.CreateMap<SearchOutputEntity, SearchOutput>();
            Mapper.CreateMap<PagingUrl, Bikewale.DTO.NewBikeSearch.Pager>();
            Mapper.CreateMap<SearchOutputEntityBase, SearchOutputBase>();
            Mapper.CreateMap<BikeModelEntity, ModelDetail>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeSeriesEntityBase, SeriesBase>();
            Mapper.CreateMap<Bikewale.Entities.NewBikeSearch.SearchBudgetLink, Bikewale.DTO.NewBikeSearch.SearchBudgetLink>();
            return Mapper.Map<SearchOutputEntity, SearchOutput>(objSuggestion);
        }
    }
}
