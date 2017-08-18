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
        internal static List<DTO.AutoComplete.SuggestionList> Convert<T>(IEnumerable<Nest.SuggestOption<T>> objSuggestion) where T:class
        {
            Mapper.CreateMap<Nest.SuggestOption<T>, SuggestionList>();
            if (objSuggestion != null)
                return Mapper.Map<List<Nest.SuggestOption<T>>, List<SuggestionList>>(objSuggestion.ToList());
            else
                return null;
        }
    }
}