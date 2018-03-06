using AutoMapper;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.NewBikeSearch;
using Bikewale.DTO.Series;
using Bikewale.ElasticSearch.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.NewBikeSearch;
using System.Collections.Generic;
namespace Bikewale.Service.AutoMappers.NewBikeSearch
{
    public class SearchOutputMapper
    {
        internal static SearchOutput Convert(SearchOutputEntity objSuggestion)
        {
            Mapper.CreateMap<SearchOutputEntity, SearchOutput>();
            Mapper.CreateMap<PagingUrl, Pager>();
            Mapper.CreateMap<SearchOutputEntityBase, SearchOutputBase>();
            Mapper.CreateMap<BikeModelEntity, ModelDetail>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeSeriesEntityBase, SeriesBase>();

            return Mapper.Map<SearchOutputEntity, SearchOutput>(objSuggestion);
        }      

        internal static SearchFilters Convert(SearchFilterDTO input)
        {

            Mapper.CreateMap<SearchFilterDTO, SearchFilters>();
            Mapper.CreateMap<PriceRangeDTO, PriceRangeEntity>();
            Mapper.CreateMap<RangeDTO, RangeEntity>();
            return Mapper.Map<SearchFilterDTO, SearchFilters>(input);



        }
    }
}