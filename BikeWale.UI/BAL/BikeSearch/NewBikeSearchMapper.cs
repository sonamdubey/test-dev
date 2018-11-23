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
            return Mapper.Map<SearchOutputEntity, SearchOutput>(objSuggestion);
        }

    }
}
