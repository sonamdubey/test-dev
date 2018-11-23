using AutoMapper;
using Bikewale.DTO.AutoComplete;
using Bikewale.Entities.AutoComplete;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.AutoComplete
{
    public class SuggestionListMapper
    {
     
        internal static IEnumerable<DTO.AutoComplete.SuggestionList> Convert(IEnumerable<Nest.SuggestOption<SuggestionOutput>> objSuggestion)
        {
            if (objSuggestion != null)
                return Mapper.Map<IEnumerable<Nest.SuggestOption<SuggestionOutput>>, IEnumerable<SuggestionList>>(objSuggestion);
            else
                return null;
        }
    }
}