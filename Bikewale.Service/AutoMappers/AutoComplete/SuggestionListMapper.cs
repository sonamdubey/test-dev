using AutoMapper;
using Bikewale.DTO.AutoComplete;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.AutoComplete
{
    public class SuggestionListMapper
    {
        internal static List<DTO.AutoComplete.SuggestionList> Convert(List<Nest.SuggestOption> objSuggestion)
        {
            Mapper.CreateMap<SuggestOption, SuggestionList>();
            return Mapper.Map<List<SuggestOption>, List<SuggestionList>>(objSuggestion);
        }
    }
}