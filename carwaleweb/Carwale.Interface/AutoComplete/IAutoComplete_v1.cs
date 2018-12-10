using Carwale.DTOs;
using Carwale.DTOs.Elastic.Autocomplete.Area;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Carwale.Interfaces.AutoComplete
{
    public interface IAutoComplete_v1
    {
        List<Suggest> GetAreaSuggestion(NameValueCollection nvc);
        List<Suggest> GetCitySuggestion(NameValueCollection nvc);
        List<Carwale.DTOs.Suggestion.Base> GetSearchSuggestion(string _searchTerm, List<Carwale.Entity.Enum.SuggestionTypeEnum> _types, int _size, bool isAmp);
    }
}
