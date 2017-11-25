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
            Mapper.CreateMap<Nest.SuggestOption<SuggestionOutput>, SuggestionList>().ForMember(d => d.Text, opt => opt.MapFrom(s => s.Source.output));
            Mapper.CreateMap<Nest.SuggestOption<SuggestionOutput>, SuggestionList>().ForMember(d => d.Payload, opt => opt.MapFrom(s => s.Source.Payload));
            if (objSuggestion != null)
                return Mapper.Map<IEnumerable<Nest.SuggestOption<SuggestionOutput>>, IEnumerable<SuggestionList>>(objSuggestion);
            else
                return null;
        }
    }
}