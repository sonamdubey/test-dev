using AutoMapper;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.NewBikeSearch;
using Bikewale.DTO.Series;
using Bikewale.ElasticSearch.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.NewBikeSearch;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
namespace Bikewale.Service.AutoMappers.NewBikeSearch
{
    public class SearchOutputMapper
    {
        internal static SearchOutput Convert(SearchOutputEntity objSuggestion)
        {
            return Mapper.Map<SearchOutputEntity, SearchOutput>(objSuggestion);
        }

        internal static SearchFilters Convert(SearchFilterDTO input)
        {
           return Mapper.Map<SearchFilterDTO, SearchFilters>(input);
        }
    }
}